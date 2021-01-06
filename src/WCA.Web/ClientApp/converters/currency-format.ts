import * as numeral from 'numeral';

export class CurrencyFormatValueConverter {
    public toView(value) {
        return numeral(value).format('($0,0.00)');
    }
}
