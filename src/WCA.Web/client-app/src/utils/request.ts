import axios from 'utils/axios'
import rawAxios from 'axios'

/**
 * Requests a URL, returning a promise
 *
 * @param  {string} url       The URL we want to request
 * @param  {object} [options] The options we want to pass to “fetch”
 *
 * @return {object}           The response data
 */
export function getRequest(url: string, options?: any) {
    return axios.get(url, options).then(response => response.data);
}

export function rawGetRequest(url: string, options?: any) {
    return rawAxios.get(url, options).then(response => response.data);
}

export function postRequest(url: string, options?: any, config?: any) {
    return axios.post(url, options, config).then(response => response.data);
}

export function rawRequest(url: string, options?: any, config?: any) {
    return axios.post(url, options, config);
}

export function deleteRequest(url: string, options?: any) {
    return axios.delete(url, options).then(response => response.data);
}