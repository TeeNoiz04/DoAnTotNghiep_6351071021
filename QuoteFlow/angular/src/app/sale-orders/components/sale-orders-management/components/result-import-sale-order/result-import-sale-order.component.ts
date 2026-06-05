import { Component, inject, Input, OnChanges, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CoreModule } from '@abp/ng.core';
import { NgSelectModule } from '@ng-select/ng-select';
import { MatCheckboxModule } from '@angular/material/checkbox';

import { SaleOrderItemsComponent } from './sale-order-items/sale-order-items.component';

@Component({
  selector: 'app-result-import-sale-order',
  templateUrl: './result-import-sale-order.component.html',
  styleUrls: ['./result-import-sale-order.component.scss'],
  standalone: true,
  providers: [],
  imports: [FormsModule, NgbModule, NgSelectModule, MatCheckboxModule, CoreModule, SaleOrderItemsComponent],
})
export class ResultImportSaleOrderComponent implements OnChanges {
  @Input() importInformation: any;
  @Input() resultImport!: any;
  @Input() useServiceData: boolean = true;

  private readonly fb = inject(FormBuilder);
  public fileImport: File | null = null;

  isCardCollapsed: { [key: string]: boolean } = {
    importInformation: false,
    projectInformation: false,
    customerInformation: false,
  };
  isLoading = false;
  validationErrors: { [key: string]: boolean } = {};
  importInfoForm: FormGroup;

  constructor() {
    this.importInfoForm = this.fb.group({
      note: [''],
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    const { note } = this.importInformation;

    this.importInfoForm.patchValue({
      note: note,
    });
  }

  toggleCardCollapse(cardId: string): void {
    this.isCardCollapsed[cardId] = !this.isCardCollapsed[cardId];
  }
}
