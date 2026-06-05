import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'formatterPercent',
  standalone: true,
})
export class FormatterPercentPipe implements PipeTransform {
  transform(value: number | string): string {
    if (value === null || value === undefined || value === '') {
      return '';
    }

    const numberValue = Number(value);
    if (isNaN(numberValue)) {
      return '';
    }

    return `${numberValue.toFixed(2)}%`;
  }
}
