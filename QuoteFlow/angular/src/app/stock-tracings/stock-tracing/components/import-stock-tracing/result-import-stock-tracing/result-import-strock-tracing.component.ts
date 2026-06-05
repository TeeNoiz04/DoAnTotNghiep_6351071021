import { Component, inject, Input, OnChanges, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CoreModule } from '@abp/ng.core';
import { NgSelectModule } from '@ng-select/ng-select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { PriceOfferImportDto } from '@proxy/price-offers';
import { ExcelValidationResult } from '@proxy/shared/excels';

import { PriceOfferDetailImportDto } from '@proxy/price-offers/price-offer-details';
import { ImportStockTracingType } from '../stock-tracing.abstract.component';
import { ImportStockTracingInformation } from '../stock-tracing.types';
import { StockTracingItemsComponent } from './stock-tracing-items/stock-tracing-items.component';

@Component({
  selector: 'app-result-import-stock-tracing',
  templateUrl: './result-import-stock-tracing.component.html',
  styleUrls: ['./result-import-stock-tracing.component.scss'],
  standalone: true,
  providers: [],
  imports: [
    FormsModule,
    NgbModule,
    NgSelectModule,
    MatCheckboxModule,
    CoreModule,
    StockTracingItemsComponent,
    // ProjectInformationFormComponent,
    // PriceOfferItemsComponent,
    // CustomerInformationTableComponent,
    // ImportInformationFormComponent,
  ],
})
export class ResultImportStockTracingComponent implements OnChanges {
  @Input() importMode: ImportStockTracingType | undefined;
  @Input() stockTracingInformation: ImportStockTracingInformation;
  @Input() resultImport: ExcelValidationResult<any> | undefined;
  @Input() resultImportItems: ExcelValidationResult<PriceOfferDetailImportDto> | undefined;
  @Input() useServiceData: boolean = true;

  private readonly fb = inject(FormBuilder);

  public fileImport: File | null = null;
  importPriceOfferType = ImportStockTracingType;

  isCardCollapsed: { [key: string]: boolean } = {
    importInformation: false,
    projectInformation: false,
    customerInformation: false,
  };
  isLoading = false;
  validationErrors: { [key: string]: boolean } = {};

  toggleCardCollapse(cardId: string): void {
    this.isCardCollapsed[cardId] = !this.isCardCollapsed[cardId];
  }
  importInfoForm: FormGroup;
  constructor() {
    this.importInfoForm = this.fb.group({
      fromDate: [''],
      toDate: [''],
      entered: [''],
      note: [''],
    });
  }
  isInventory = false;
  ngOnChanges(changes: SimpleChanges): void {
    this.isInventory = this.importMode === ImportStockTracingType.Inventory;
    this.importInfoForm.patchValue({
      fromDate: this.stockTracingInformation.fromDate,
      toDate: this.stockTracingInformation.toDate,
      entered: this.stockTracingInformation.entered,
      note: this.stockTracingInformation.note,
    });
  }
}
