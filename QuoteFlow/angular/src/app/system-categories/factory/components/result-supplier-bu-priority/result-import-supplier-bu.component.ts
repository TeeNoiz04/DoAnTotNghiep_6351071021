import { Component, inject, Input } from '@angular/core';
import { FormBuilder, FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CoreModule } from '@abp/ng.core';
import { NgSelectModule } from '@ng-select/ng-select';
import { MatCheckboxModule } from '@angular/material/checkbox';

import { SupplierBUItemsComponent } from './supplier-bu-items/supplier-bu-items.component';

@Component({
  selector: 'app-result-import-supplier-bu',
  templateUrl: './result-import-supplier-bu.component.html',
  styleUrls: ['./result-import-supplier-bu.component.scss'],
  standalone: true,
  providers: [],
  imports: [FormsModule, NgbModule, NgSelectModule, MatCheckboxModule, CoreModule, SupplierBUItemsComponent],
})
export class ResultImportSupplierBUComponent {
  @Input() resultImport!: any;
  @Input() useServiceData: boolean = true;

  private readonly fb = inject(FormBuilder);
  public fileImport: File | null = null;

  isCardCollapsed: { [key: string]: boolean } = {
    projectInformation: false,
    customerInformation: false,
  };
  isLoading = false;
  validationErrors: { [key: string]: boolean } = {};

  toggleCardCollapse(cardId: string): void {
    this.isCardCollapsed[cardId] = !this.isCardCollapsed[cardId];
  }
}
