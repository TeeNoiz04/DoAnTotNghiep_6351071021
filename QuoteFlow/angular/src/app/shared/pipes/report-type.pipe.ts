import { Pipe, PipeTransform } from '@angular/core';
import { ReportType } from '@proxy/stock-tracings';

@Pipe({
  name: 'reportType',
  standalone: true,
})
export class ReportTypePipe implements PipeTransform {
  transform(val: ReportType): string {
    switch (val) {
      case ReportType.Delivery:
        return 'Delivery';
      case ReportType.Inventory:
        return 'Inventory';
      case ReportType.Receipt:
        return 'Receipt';
      case ReportType.None:
        return 'None';
      default:
        return '';
    }
  }
}
