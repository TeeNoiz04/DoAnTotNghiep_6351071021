import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'appNumberFormat',
  standalone: true,
})
export class NumberFormatPipe implements PipeTransform {
  transform(value: number | string, decimals: number = 2, allowDecimal: boolean = true): string {
    if (value === null || value === undefined || value === '') {
      return '0';
    }
    const numberValue = Number(value);
    if (isNaN(numberValue)) {
      return '';
    }

    return numberValue.toLocaleString('en-US', {
      minimumFractionDigits: allowDecimal ? decimals : 0,
      maximumFractionDigits: allowDecimal ? decimals : 0,
      useGrouping: true,
    });
  }
}
