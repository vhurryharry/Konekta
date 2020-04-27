import { HttpClient } from 'aurelia-fetch-client';
import { autoinject } from 'aurelia-framework';
import { Ibox } from '../components/ibox';
import { ErrorViewModel } from '../services/wca-api-types';
import * as toastr from 'toastr';
import { AppInfoService } from './app-info-service';

@autoinject
export class DataService {
    private static TrimStartingSlash(url: string) {
        if (url.startsWith('/')) {
            url = url.substr(1);
        }
        return url;
    }

    constructor(private httpClient: HttpClient) {
        this
            .httpClient
            .configure((config) => {
                config
                    .useStandardConfiguration()
                    .withBaseUrl('/api/')
                    .withDefaults({
                        credentials: 'same-origin',
                        headers: {
                            'Accept': 'application/json',
                            'Content-Type': 'application/json'
                        }
                    }).withInterceptor({
                        response(response) {
                            return response;
                        },
                        responseError(error) {
                            return error;
                        },
                        request(request) {
                            let latestCsrfToken = document.cookie.replace(/(?:(?:^|.*;\s*)XSRF-TOKEN\s*\=\s*([^;]*).*$)|^.*$/, "$1"); // Cookies.get('XSRF-TOKEN')
                            if (latestCsrfToken.length > 0) {
                                if (['GET', 'HEAD', 'OPTIONS', 'TRACE'].indexOf(request.method) < 0) { // only send xsrf token with non-safe requests
                                    request.headers.append('X-XSRF-Token', latestCsrfToken);
                                }
                            }
                            return request;
                        }
                    });
            });
    }

    /**
     *
     *
     * @param {string} endpoint - the endpoint (after /api/) without a starting slash.
     * @returns the unaltered fetch promise.
     * @memberof DataService
     */
    public getData(endpoint: string, ibox: Ibox) {
        endpoint = DataService.TrimStartingSlash(endpoint);

        return this
            .httpClient
            .fetch(endpoint)
            .then((response) => {
                const isWCA = location.hostname.includes("workcloud") || location.hostname.includes("appwca-test");
                const supportUrl = isWCA ? "https://www.workcloud.support" : "https://support.konekta.com.au";

                if (!response.ok) {
                    toastr.error('An unexpected server problem was encountered, please try again later or log a ticket '
                        + '<a href="' + supportUrl + '/support/tickets/new" target="_blank"><u>here</u></a>');
                }

                return response.json();

            })
            .catch((reason: Response) => {

                reason.json()
                    .then((reasonData: ErrorViewModel) => {
                        if (ibox !== null && ibox !== undefined) {
                            ibox.toggleLoading(false);
                        }
                        toastr.error(reasonData.message);
                    });
            });
    }

    /**
     * Post data to the specified endpoint.
     *
     * @param {string} endpoint - the endpoint (after /api/) without a starting slash.
     * @param {object} data - a JavaScript object to be submitted as JSON data. This will automatically be stringified.
     * @returns the unaltered fetch promise
     * @memberof DataService
     */
    public postData(endpoint: string, data: object, isJsonResponse: boolean, ibox: Ibox) {
        endpoint = DataService.TrimStartingSlash(endpoint);

        return this
            .httpClient
            .fetch(endpoint, {
                method: 'post',
                body: JSON.stringify(data)
            })
            .then((response) => { if (isJsonResponse) { return response.json(); } else { return response; } })
            .catch((reason: Response) => {
                if (ibox !== null && ibox !== undefined) {
                    ibox.toggleLoading(false);
                }
                if (isJsonResponse) {
                    reason.json()
                        .then((reasonData: ErrorViewModel) => {
                            toastr.error(reasonData.message);
                        });
                }
                else {
                    toastr.error(reason as any);
                }
            });
    }

    public simplePost(endpoint: string, data: object): Promise<Response> {
        endpoint = DataService.TrimStartingSlash(endpoint);

        return this
            .httpClient
            .fetch(endpoint, {
                method: 'post',
                body: JSON.stringify(data)
            });
    }

    public simpleGet(endpoint: string): Promise<Response> {
        endpoint = DataService.TrimStartingSlash(endpoint);

        return this
            .httpClient
            .fetch(endpoint, {
                method: 'get'
            });
    }
}
