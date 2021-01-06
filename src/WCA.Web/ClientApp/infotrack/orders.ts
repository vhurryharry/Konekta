import { DialogService } from 'aurelia-dialog';
import { autoinject, TaskQueue, computedFrom } from 'aurelia-framework';
import { Ibox } from '../components/ibox';
import { DataService } from '../services/data-service';
import { Tools } from '../tools/tools';

import {
    GetInfoTrackOrderHistoryQuery,
    IInfoTrackOrderResult,
} from '../services/wca-api-types';

import * as $ from 'jquery';
import * as toastr from 'toastr';
import 'footable';

@autoinject
export class Orders {
    public ibox1: Ibox;
    public infoTrackOrders: IInfoTrackOrderResult[] = new Array();
    public infoTrackOrderQuery: GetInfoTrackOrderHistoryQuery = new GetInfoTrackOrderHistoryQuery();
    public filterFromDatePickerValue: Date;
    public filterToDatePickerValue: Date;
    public paramsWithKeysInLowerCase: any;
    public footable: any;
    public currentPage: number;
    public itemsPerPage: number = 10;
    public pageFilters: Filters[] = [];

    constructor(
        private dataService: DataService,
        private dialogService: DialogService,
        private taskQueue: TaskQueue) {
    }

    @computedFrom('infoTrackOrders', 'itemsPerPage')
    get totalPages(): number {
        return Math.ceil(this.infoTrackOrders.length / this.itemsPerPage);
    }

    @computedFrom('infoTrackOrders', 'itemsPerPage', 'currentPage')
    get currentPageOffset(): number {
        return Math.min(this.currentPage * this.itemsPerPage - this.itemsPerPage, this.infoTrackOrders.length);
    }

    @computedFrom('infoTrackOrders', 'itemsPerPage', 'currentPage')
    get numberOfItemsMissingToMakeAFullPage(): number {
        let highestIndexOnCurrentPage = this.currentPage * this.itemsPerPage;
        if (highestIndexOnCurrentPage > this.infoTrackOrders.length) {
            return highestIndexOnCurrentPage - this.infoTrackOrders.length;
        }

        return 0;
    }

    public itemsPerPageChanged() {
        // Reset current page as the formulas regarding the currently
        // displayed items will have changed and we might end up
        // on a page greater than the number of total pages.
        this.currentPage = 1;
        this.redrawFooTable();
    }

    // Queuing a microtask ensures that the footable is
    // redrawn after Aurelia binding is complete.
    // See https://github.com/aurelia/templating/issues/192
    public redrawFooTable() {
        this.taskQueue.queueMicroTask(() => {
            this.footable.redraw();
        });
    }

    private GetLowerCaseQueryStrings(params: any) {
        this.paramsWithKeysInLowerCase = [params.length];
        for (const param in params) {
            if (params.hasOwnProperty(param)) {
                this.paramsWithKeysInLowerCase[param.toLowerCase()] = params[param];
            }
        }
    }

    public async RemoveFilter(filter) {
        // refresh results
        this.pageFilters = this.pageFilters.filter(f => f.key != filter.key);
        this.infoTrackOrderQuery[filter.key] = undefined;
        await this.GetOrderHistory();

        // update browser page URL
        var pageUrl = '/wca/infotrack/orders?';
        if (this.infoTrackOrderQuery.matterId) pageUrl = pageUrl + 'matterId=' + this.infoTrackOrderQuery.matterId + '&';
        if (this.infoTrackOrderQuery.orgKey) pageUrl = pageUrl + 'actionstepOrg=' + this.infoTrackOrderQuery.orgKey;
        history.pushState(undefined, 'InfoTrack Orders | WCA', pageUrl)
    }

    public async activate(params, routeConfig) {

        if (params) this.GetLowerCaseQueryStrings(params);

        this.infoTrackOrderQuery.matterId = Tools.ParseNumberPropertyValue(
            this.paramsWithKeysInLowerCase,
            `matterId`.toLowerCase())

        this.infoTrackOrderQuery.orgKey = Tools.ParseStringPropertyValue(
            this.paramsWithKeysInLowerCase,
            `actionstepOrg`.toLowerCase())

        if (this.infoTrackOrderQuery.matterId !== undefined)
            this.pageFilters.push({ key: 'MatterId', display: 'Matter', value: this.infoTrackOrderQuery.matterId });

        if (this.infoTrackOrderQuery.orgKey !== undefined)
            this.pageFilters.push({ key: 'OrgKey', display: 'Actionstep Organisation', value: this.infoTrackOrderQuery.orgKey });
    }

    public async attached(params: any) {
        let fromDate = new Date();
        let thisMonth = fromDate.getMonth();
        fromDate.setMonth(thisMonth - 1);

        this.filterFromDatePickerValue = fromDate;
        this.filterToDatePickerValue = new Date();
        this.footable = (<any>$('#infotrack-orders-table')).footable().data('footable');
        await this.GetOrderHistory();
    }

    // Can't use FooTable pagination because it attempts to insert rows
    // for next/previous controls. These conflict with Aurelia templating
    // and repeater.
    public pageNavFirst() {
        this.currentPage = 1;
        this.redrawFooTable();
    }

    public pageNavPrevious() {
        if (this.currentPage > 1) {
            this.currentPage--;
        }
        this.redrawFooTable();
    }

    public pageNavNext() {
        if (this.currentPage < this.totalPages) {
            this.currentPage++;
        }
        this.redrawFooTable();
    }

    public pageNavLast() {
        this.currentPage = this.totalPages;
        this.redrawFooTable();
    }

    // Inspired by https://stackoverflow.com/a/38021207
    private static objectToCSVRow(dataObject): string {
        const dataArray = new Array;
        for (var o in dataObject) {
            const innerValue = dataObject[o] === null ? '' : dataObject[o].toString();
            dataArray.push(`"${innerValue.replace(/"/g, '""')}"`);
        }
        return dataArray.join(',') + '\r\n';
    }

    private static getLocaleDateString(date: Date): string {
        return date.toISOString().substr(0, 10);
    }

    public async SaveAsCSV() {
        if (this.infoTrackOrders.length <= 0) {
            toastr.warning('No orders available, cannot save CSV');
        }

        let csvContent = "data:text/csv;charset=utf-8,";

        // Headers
        csvContent += Orders.objectToCSVRow(Object.keys(this.infoTrackOrders[0]));

        this.infoTrackOrders.forEach(function (item) {
            csvContent += Orders.objectToCSVRow(item);
        });

        let filename = 'InfoTrack Orders ';
        filename += Orders.getLocaleDateString(this.filterFromDatePickerValue);
        filename += ' to ';
        filename += Orders.getLocaleDateString(this.filterToDatePickerValue);
        filename += '.csv';

        const encodedUri = encodeURI(csvContent);
        const link = document.createElement('a');
        link.setAttribute('href', encodedUri);
        link.setAttribute('download', filename);
        document.body.appendChild(link); // Required for FF
        link.click();
        document.body.removeChild(link);
    }

    public async GetOrderHistory() {
        if (this.filterFromDatePickerValue === null) {
            toastr.error("Enter From Date");
        } else if (this.filterToDatePickerValue === null) {
            toastr.error("Enter To Date");
        } else if (this.filterFromDatePickerValue > this.filterToDatePickerValue) {
            toastr.error("From Date cannot be greater than To Date");
        } else {
            try {
                this.infoTrackOrderQuery.orderedFromDateUtc = new Date(this.filterFromDatePickerValue);
                this.infoTrackOrderQuery.orderedToDateUtc = new Date(this.filterToDatePickerValue);

                // Normalise dates
                this.infoTrackOrderQuery.orderedFromDateUtc.setHours(0, 0, 0, 0);
                this.infoTrackOrderQuery.orderedToDateUtc.setDate(this.infoTrackOrderQuery.orderedToDateUtc.getDate() + 1);
                this.infoTrackOrderQuery.orderedToDateUtc.setHours(0, 0, 0, 0)

                this.infoTrackOrders = await this.dataService
                    .postData('/infotrack/Orders/GetOrderHistory', this.infoTrackOrderQuery, true, null) as IInfoTrackOrderResult[];

                this.currentPage = 1;
                this.redrawFooTable();
            } catch (err) {
                toastr.error('Sorry, there was an unexpected error retrieving your orders.');
                console.error('Error fetching order history', err);
            }
        }
    }

    public async onReconcileChange(order: IInfoTrackOrderResult) {
        // First change the local value. This will immediately update
        // the UI to give the user feedback.
        order.reconciled = !order.reconciled;

        let serverUpdatedSuccessfully = false;

        if (order.reconciled) {
            serverUpdatedSuccessfully = await this.sendReconciliationUpdateToServer(`/infotrack/Orders/Reconcile?infoTrackOrderId=${order.infoTrackOrderId}`);
        } else {
            serverUpdatedSuccessfully = await this.sendReconciliationUpdateToServer(`/infotrack/Orders/UnReconcile?infoTrackOrderId=${order.infoTrackOrderId}`);
        }

        if (!serverUpdatedSuccessfully) {
            // Reverse the UI to reflect the failed server update
            order.reconciled = !order.reconciled;

            // Let the user know something went wrong
            toastr.error("We're sorry, there was a problem saving your reconciliation update to the server. Please try again or let us know if this keeps happening.");
        }
    }

    private async sendReconciliationUpdateToServer(url: string): Promise<boolean> {
        try {
            let result = await this.dataService.simplePost(url, null);
            if (result.ok) {
                toastr.success('Saved');
            } else {
                let resultJson = await result.json();
                console.error('There was a problem saving the reconciliation status:', resultJson);
                return false;
            }
        } catch (err) {
            console.error('There was a problem saving the reconciliation status:', err);
            return false;
        }

        return true;
    }
}

interface Filters {
    key: string;
    display: string;
    value: any;
}