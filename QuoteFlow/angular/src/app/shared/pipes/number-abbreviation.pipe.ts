import { Pipe, PipeTransform } from '@angular/core';
import { NumberHelper } from '../helpers/number-helper';

@Pipe({
  name: 'numberAbbreviation',
  standalone: true,
})
export class NumberAbbreviationPipe implements PipeTransform {
  transform(
    value: number | string | undefined,
    decimalPlaces: number = 1,
    threshold: number = 1000,
    useFullWords: boolean = false,
  ): string {
    return NumberHelper.abbreviateNumber(value, {
      decimalPlaces,
      threshold,
      useFullWords,
    });
  }
}
