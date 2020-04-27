
import { rawGetRequest } from 'utils/request'
import { UISettings } from 'utils/wcaApiTypes';

export async function getUIDefinitions(): Promise<UISettings> {
    const response = await rawGetRequest(`/api/uidefinitions`);
    let uidefinitions: UISettings = UISettings.fromJS(response);

    return uidefinitions;
}