import axios from 'axios'

import * as ACTIONS from 'redux/common/actions'
import { store } from 'redux/store'

const AxiosInstance = axios.create({
    baseURL: "",
})

AxiosInstance.interceptors.response.use((response) => {

    store.dispatch(ACTIONS.actionstepOrgConnected());
    store.dispatch(ACTIONS.pexaConnected());
    store.dispatch(ACTIONS.firstTitleConnected());

    return response;
}, (error) => {

    if (!error.response) {
        return Promise.reject('Network Error')
    }

    if (error.response.status === 401) {
        const authenticateHeaderValue = error.response.headers["www-authenticate"];
        if (authenticateHeaderValue && authenticateHeaderValue.startsWith("ActionstepConnection OrgKey=")) {
            const warningHeaderValue = error.response.headers["warning"];
            if (warningHeaderValue && warningHeaderValue === "InvalidCredentials") {
                store.dispatch(ACTIONS.actionstepOrgNotConnected(true));
            } else {
                store.dispatch(ACTIONS.actionstepOrgNotConnected(false));
            }
        } else if (authenticateHeaderValue && authenticateHeaderValue.startsWith("PexaConnection")) {
            store.dispatch(ACTIONS.pexaNotConnected());
        } else if (authenticateHeaderValue && authenticateHeaderValue.startsWith("FirstTitleConnection")) {
            const warningHeaderValue = error.response.headers["warning"];
            if (warningHeaderValue && warningHeaderValue === "MissingCredentials") {
                store.dispatch(ACTIONS.firstTitleNotConnected(true));
            } else {
                store.dispatch(ACTIONS.firstTitleNotConnected(false));
            }
        }
    }

    if (!error.response.data) {
        return Promise.reject("Unknown Error");
    }

    return Promise.reject(error.response.data);
})

export default AxiosInstance