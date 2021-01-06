import { ActionstepMatterInfo } from "utils/wcaApiTypes";

export default class Tools {
    public static PopupCenter(url: string, title: string, w: number, h: number): Window | null {
        var left = (window.screen.width / 2) - (w / 2);
        var top = (window.screen.height / 2) - (h / 2);
        return window.open(url, title, `toolbar=no, location=no, directories=no, menubar=no, resizable=no, copyhistory=no, width=${w}, height=${h}, top=${top}, left=${left}`);
    }

    public static PopupConnectToActionstep(callback: Function) {
        Tools.PopupCenter(`/integrations/actionstep/connect?returnUrl=/PopupConnectionSuccessful`, "Link Konekta to Actionstep", 1050, 700);
        window.addEventListener('message', event => {
            if (event.data === 'konekta_connection_successful') {
                callback();
            }
        });
    }

    public static PopupConnectToPexa(callback: Function) {
        Tools.PopupCenter(`/integrations/pexa/connect`, "Link Konekta to Pexa", 1050, 700);
        window.addEventListener('message', event => {
            if (event.data === 'konekta_connection_successful') {
                callback();
            }
        });
    }

    public static ParseBooleanPropertyValue(o: any, propertyName: string): boolean | undefined {
        const value = o[propertyName];
        if (value !== undefined && (typeof value === 'string' || value instanceof String)) {
            const lowerCaseValue = value.toLowerCase();
            if (lowerCaseValue === 'true' || lowerCaseValue === 'yes') {
                return true;
            } else if (lowerCaseValue === 'false' || lowerCaseValue === 'no') {
                return false;
            }
        }

        return undefined;
    }

    public static ParseStringPropertyValue(o: any, propertyName: string): string | undefined {
        const value = o ? o[propertyName] : undefined;
        if (value !== undefined && (typeof value === 'string' || value instanceof String)) {
            return value as string;
        }

        return undefined;
    }

    public static ParseNumberPropertyValue(o: any, propertyName: string): number | undefined {
        const value = o ? o[propertyName] : undefined;
        if (value !== undefined) {
            if (typeof value === 'number') {
                return value;
            } else if (value instanceof Number) {
                if (!isNaN(value as number)) {
                    return value as number;
                }
            } else if (typeof value === 'string' || value instanceof String) {
                const stringValue = (value as string).replace(/\$/g, '').replace(/,/g, '');
                const testValue = parseFloat(stringValue);
                if (!isNaN(testValue)) {
                    return testValue;
                }
            }
        }

        return undefined;
    }

    public static LowerCaseParams(params: any): any {
        const paramsWithKeysInLowerCase: any = [params.length];
        for (const param in params) {
            paramsWithKeysInLowerCase[param.toLowerCase()] = params[param];
        }

        return paramsWithKeysInLowerCase;
    }

    public static ParseActionstepMatterInfo(params: any): ActionstepMatterInfo | null {
        const lowerCaseParams = Tools.LowerCaseParams(params);

        if (lowerCaseParams['actionsteporg'] === undefined || lowerCaseParams['matterid'] === undefined) {
            return null;
        }

        return new ActionstepMatterInfo({
            matterId: lowerCaseParams['matterid'] ? lowerCaseParams['matterid'] : "*",
            orgKey: lowerCaseParams['actionsteporg'] ? lowerCaseParams['actionsteporg'] : "*",
        });
    }

    public static assign(object: any, path: string | string[], value: any) {
        if (typeof path === "string")
            path = path.split(".");

        while (path.length > 1) {
            object = object[path.shift()!];
        }

        object[path.shift()!] = value;
    }
}
