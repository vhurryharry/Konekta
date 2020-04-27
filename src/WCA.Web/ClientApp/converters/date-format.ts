function tryFormatDate(value: string | Date, textIfNull: string, convertToStringCallback: Function) {
    if (value instanceof Date) {
        return convertToStringCallback(value);
    } else {
        try {
            let valueAsDate = new Date(value);
            if (valueAsDate.toString() !== 'Invalid Date') {
                return convertToStringCallback(valueAsDate);
            }
        } finally { }
    }

    // date parsing failed, so return text instead
    if (textIfNull === undefined || textIfNull === null) {
        return "Not set";
    } else {
        return textIfNull;
    }
}

export class DateFormatValueConverter {
    toView(value: Date, textIfNull) {
        return tryFormatDate(value, textIfNull, (parsedDate: Date) => parsedDate.toLocaleDateString('en-AU'));
    }
}

export class DateTimeFormatValueConverter {
    toView(value: Date, textIfNull) {
        return tryFormatDate(value, textIfNull, (parsedDate: Date) => parsedDate.toLocaleString('en-AU'));
    }
}