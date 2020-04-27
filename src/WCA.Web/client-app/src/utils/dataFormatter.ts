
export const _onFormatDate = (date: Date | undefined): string => {
    return (date && !isNaN(date.getTime())) ? (date.toLocaleString('en-au', { month: 'long' }) + ' ' + date.getDate() + " " + date.getFullYear()) : "Please select...";
}

export const convertDateToUTC = (date: Date): Date => {
    return convertStringToDateUTC(date.toISOString());
}

export const convertStringToDateUTC = (dateString: string): Date => {
    let date = new Date(dateString);
    const offset = date.getTimezoneOffset();

    date = new Date(date.getTime() - offset * 60000);

    date.setUTCHours(0);
    date.setUTCMinutes(0);
    date.setUTCSeconds(0);
    date.setUTCMilliseconds(0);

    return date;
}

export const formatTime12HourString = (date: Date): string => {
    return date.toLocaleString('en-au', { hour: 'numeric', minute: 'numeric', hour12: true }).toUpperCase();
}

export const formatDateTime12HourString = (date: Date): string => {
    return _onFormatDate(date) + " " + formatTime12HourString(date);
}

export const formatISODateString = (date: Date): string => {
    date = new Date(date);
    const month = date.getMonth() + 1;
    const monthString = month < 10 ? "0" + month : `${month}`;
    const days = date.getDate();
    const daysString = days < 10 ? "0" + days : `${days}`;

    return `${date.getFullYear()}-${monthString}-${daysString}`;
}