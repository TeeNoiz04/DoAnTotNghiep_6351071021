import { CoreModule } from '@abp/ng.core';
import { NgbDateParserFormatter, NgbDateStruct, NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { Injectable, NgModule } from '@angular/core';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { provideNgxMask, NgxMaskConfig } from 'ngx-mask';
import { TotalAmountComponent } from '../total-amount/total-amount.component';
import { TableActionComponent } from './action/table-action.component';
import { ActionCellComponent } from './cell/actionCell.component';
import { ClickCellComponent } from './cell/clickCell.component';
import { AppTextboxCellComponent } from './cell/textboxCell.component';
import { TextCellComponent } from './cell/textCell.component';
import { ColumnComponent } from './column/column.component';
import { DataTableComponent } from './data-table.component';
import { HeaderTableComponent } from './header/header.component';

export const maskConfig: Partial<NgxMaskConfig> = {
  validation: false,
  allowNegativeNumbers: false,
  thousandSeparator: ',',
  decimalMarker: '.',
  dropSpecialCharacters: false,
};

@Injectable()
export class NgbDateCustomParserFormatter extends NgbDateParserFormatter {
  parse(value: string): NgbDateStruct {
    if (value) {
      const dateParts = value.trim().split('/');
      if (dateParts.length === 1 && isNumber(dateParts[0])) {
        return { day: toInteger(dateParts[0]), month: null, year: null };
      } else if (dateParts.length === 2 && isNumber(dateParts[0]) && isNumber(dateParts[1])) {
        return {
          day: toInteger(dateParts[0]),
          month: toInteger(dateParts[1]),
          year: null,
        };
      } else if (dateParts.length === 3 && isNumber(dateParts[0]) && isNumber(dateParts[1]) && isNumber(dateParts[2])) {
        return {
          day: toInteger(dateParts[0]),
          month: toInteger(dateParts[1]),
          year: toInteger(dateParts[2]),
        };
      }
    }
    return null;
  }

  format(date: NgbDateStruct): string {
    return date
      ? `${isNumber(date.day) ? padNumber(date.day) : ''}/${
          isNumber(date.month) ? padNumber(date.month) : ''
        }/${date.year}`
      : '';
  }
}
export function toInteger(value: any): number {
  return parseInt(`${value}`, 10);
}

export function isNumber(value: any): value is number {
  return !isNaN(toInteger(value));
}

export function padNumber(value: number) {
  if (isNumber(value)) {
    return `0${value}`.slice(-2);
  } else {
    return '';
  }
}

@NgModule({
  declarations: [],
  imports: [
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    NgbDropdownModule,
    NgxValidateCoreModule,
    DataTableComponent,
    HeaderTableComponent,
    ColumnComponent,
    TableActionComponent,
    ActionCellComponent,
    TextCellComponent,
    ClickCellComponent,
    TotalAmountComponent,
    AppTextboxCellComponent,
  ],
  exports: [
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    NgbDropdownModule,
    NgxValidateCoreModule,
    DataTableComponent,
    HeaderTableComponent,
    ColumnComponent,
    TableActionComponent,
    ActionCellComponent,
    TextCellComponent,
    ClickCellComponent,
    TotalAmountComponent,
    AppTextboxCellComponent,
  ],
  providers: [{ provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }, provideNgxMask(maskConfig)],
})
export class DataTableModule {}
