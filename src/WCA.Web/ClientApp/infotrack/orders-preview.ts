import { autoinject, TaskQueue, computedFrom } from 'aurelia-framework';
import { DataService } from '../services/data-service';
import { Tools } from '../tools/tools';
import { MultiSelectOption } from '../components/multiselect';

import {
    GetInfoTrackOrderHistoryPreviewQuery,
    IOrderHistoryResult,
    IInfoTrackOrderResult,
    ActionstepMatterInfo,
} from '../services/wca-api-types';

import * as $ from 'jquery';
import * as toastr from 'toastr';
import 'footable';
import { AppInfoService } from '../services/app-info-service';

@autoinject
export class OrdersPreview {
    public infoTrackOrdersPreview: IOrderHistoryResult[] = new Array();
    public filteredInfoTrackOrders: IOrderHistoryResult[] = new Array();
    public displayedOrders: IOrderHistoryResult[] = new Array();

    public viewedOrder: IInfoTrackOrderResult;
    public infoTrackOrderQuery: GetInfoTrackOrderHistoryPreviewQuery = new GetInfoTrackOrderHistoryPreviewQuery();
    public filterFromDatePickerValue: Date;
    public filterToDatePickerValue: Date;
    public footable: any;
    public currentPage: number;
    public itemsPerPage: number = 10;
    public pageFilters: Filters[] = [];
    public sortedAsc: boolean = true;
    public showResults = true;
    public statusOptions: MultiSelectOption[] = null;
    public selectedStatusOptions: Array<string>;
    public fooTableLoading: boolean = true;

    constructor(
        private dataService: DataService,
        private taskQueue: TaskQueue,
        private appInfoService: AppInfoService
    ) {
    }

    @computedFrom('filteredInfoTrackOrders', 'itemsPerPage')
    get totalPages(): number {
        return Math.ceil(this.filteredInfoTrackOrders.length / this.itemsPerPage);
    }

    @computedFrom('filteredInfoTrackOrders', 'itemsPerPage', 'currentPage')
    get currentPageOffset(): number {
        return Math.min(this.currentPage * this.itemsPerPage - this.itemsPerPage, this.filteredInfoTrackOrders.length);
    }

    @computedFrom('filteredInfoTrackOrders', 'itemsPerPage', 'currentPage')
    get numberOfItemsMissingToMakeAFullPage(): number {
        let highestIndexOnCurrentPage = this.currentPage * this.itemsPerPage;
        if (highestIndexOnCurrentPage > this.filteredInfoTrackOrders.length) {
            return highestIndexOnCurrentPage - this.filteredInfoTrackOrders.length;
        }

        return 0;
    }

    public disbursementStatusIcon(status): string {
        switch (status) {
            case 'Not Yet Created':
            case 'Not Yet Uploaded':
            case 'Not Applicable':
            case 'Creation In Progress':
            case 'Upload In Progress':
                return 'fa fa-clock-o';
            case 'Created Successfully':
            case 'Uploaded Successfully':
                return 'fa fa-check-circle';
            case 'Creation Failed':
            case 'Upload Failed':
                return 'fa fa-times-circle';
            default:
                return 'fa fa-question-circle-o';
        }
    }

    public itemsPerPageChanged() {
        this.currentPage = 1;
        this.redrawFooTable();
    }

    // Queuing a microtask ensures that the footable is
    // redrawn after Aurelia binding is complete.
    // See https://github.com/aurelia/templating/issues/192
    public redrawFooTable() {
        var pageOffset = (this.currentPage - 1) * this.itemsPerPage;
        this.displayedOrders = this.filteredInfoTrackOrders.slice(pageOffset, pageOffset + this.itemsPerPage);
        this.taskQueue.queueMicroTask(() => {
            if (this.footable) {
                this.footable.redraw();
                this.fooTableLoading = false;
            }
        });
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
        var paramsWithKeysInLowerCase = Tools.LowerCaseParams(params);

        var matterId = Tools.ParseNumberPropertyValue(paramsWithKeysInLowerCase, `matterId`.toLowerCase());
        var orgKey = Tools.ParseStringPropertyValue(paramsWithKeysInLowerCase, `actionstepOrg`.toLowerCase());

        this.infoTrackOrderQuery.orgKey = orgKey;

        if (matterId !== undefined) {
            this.infoTrackOrderQuery.matterId = matterId;
            this.pageFilters.push({ key: 'MatterId', display: 'Matter', value: this.infoTrackOrderQuery.matterId });
        }

        if (orgKey !== undefined) {
            this.infoTrackOrderQuery.orgKey = orgKey;
            this.pageFilters.push({ key: 'OrgKey', display: 'Actionstep Organisation', value: this.infoTrackOrderQuery.orgKey });
        }
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

    public sortData() {
        this.filteredInfoTrackOrders.reverse();
        this.sortedAsc = !this.sortedAsc;
        this.redrawFooTable();
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
        if (this.filteredInfoTrackOrders.length <= 0) {
            toastr.warning('No orders available, cannot save CSV');
        }

        let csvContent = "data:text/csv;charset=utf-8,";

        // Headers
        csvContent += OrdersPreview.objectToCSVRow(Object.keys(this.filteredInfoTrackOrders[0]));

        this.filteredInfoTrackOrders.forEach(function (item) {
            csvContent += OrdersPreview.objectToCSVRow(item);
        });

        let filename = 'InfoTrack Orders ';
        filename += OrdersPreview.getLocaleDateString(this.filterFromDatePickerValue);
        filename += ' to ';
        filename += OrdersPreview.getLocaleDateString(this.filterToDatePickerValue);
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

                this.fooTableLoading = true
                this.infoTrackOrderQuery.orderedFromDateUtc = new Date(this.filterFromDatePickerValue);
                this.infoTrackOrderQuery.orderedToDateUtc = new Date(this.filterToDatePickerValue);

                // Normalise dates
                this.infoTrackOrderQuery.orderedFromDateUtc.setHours(0, 0, 0, 0);
                this.infoTrackOrderQuery.orderedToDateUtc.setDate(this.infoTrackOrderQuery.orderedToDateUtc.getDate() + 1);
                this.infoTrackOrderQuery.orderedToDateUtc.setHours(0, 0, 0, 0)

                this.infoTrackOrdersPreview = await this.dataService.postData('/infotrack/Orders/GetOrderHistoryPreview', this.infoTrackOrderQuery, true, null) as IOrderHistoryResult[];
                this.filteredInfoTrackOrders = $.extend(true, [], this.infoTrackOrdersPreview);

                // Get unique list of order status
                let statusList = [];
                this.infoTrackOrdersPreview.map(m => {
                    return m.orders.map(o => statusList.push(o.infoTrackStatus));
                });
                let uniqueStatus = statusList.filter((value, index, self) => { return self.indexOf(value) === index; });
                this.statusOptions = uniqueStatus.map(s => { return { value: s, label: s } });
                this.selectedStatusOptions = this.statusOptions
                    .filter(status => ['Complete', 'Refund'].includes(status.label))
                    .map(statusOption => { return statusOption.label; });

                this.showResults = this.filteredInfoTrackOrders.length > 0;

                $('.toggle').click(function () {
                    $('.toggle').toggle();
                    (<any>$('#infotrack-orders-table')).trigger($(this).data('trigger')).trigger('footable_redraw');
                });

                this.currentPage = 1;
                this.redrawFooTable();

            } catch (err) {
                toastr.error('Sorry, there was an unexpected error retrieving your orders.');
                console.error('Error fetching order history', err);
            }
        }
    }


    public async getDocumentStatusIcon() {
        console.log('hello');
        return "fa fa-clock-o";
    }


    public filterByStatus($event) {
        if ($event.length == 0) {
            this.filteredInfoTrackOrders = $.extend(true, [], this.infoTrackOrdersPreview);


        } else {
            //filter by matter
            this.filteredInfoTrackOrders = $.extend(true, [], this.infoTrackOrdersPreview);

            this.filteredInfoTrackOrders = this.filteredInfoTrackOrders.filter(m => {
                var statusExist = false;
                m.orders.map(o => {
                    if ($event.includes(o.infoTrackStatus)) {
                        statusExist = true;
                    }
                })
                return statusExist;
            });

            this.filteredInfoTrackOrders.map(m => {
                m.orders = m.orders.filter(o => {
                    return $event.includes(o.infoTrackStatus);
                });
            });
        }
        this.redrawFooTable();
    }

    public ViewOrderDetails(order: any) {
        this.viewedOrder = order;
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

    public async connectToActionstepOrg() {
        window.location.href = '/integrations/actionstep/connect?returnUrl=/infotrack/orders-preview';
    }
}

interface Filters {
    key: string;
    display: string;
    value: any;
}