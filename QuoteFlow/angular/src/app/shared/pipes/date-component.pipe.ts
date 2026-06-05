import { Pipe, PipeTransform } from '@angular/core';
import { formatDate } from '@angular/common';
// <p>Formatted Date: {{ someDate | appDate:'fullDate' }}</p>

@Pipe({
  name: 'appDate',
  standalone: true,
})
export class DateComponentPipe implements PipeTransform {
  transform(
    value: Date | string | number,
    format: string = 'mediumDate',
    locale: string = 'en-US',
  ): string {
    if (!value) {
      return '';
    }

    try {
      return formatDate(value, format, locale);
    } catch (error) {
      console.error('Error formatting date:', error);
      return '';
    }
  }
}
