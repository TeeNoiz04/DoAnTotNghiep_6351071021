import { Component, inject, Input, OnChanges, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CoreModule } from '@abp/ng.core';
import { NgSelectModule } from '@ng-select/ng-select';
import { MatCheckboxModule } from '@angular/material/checkbox';

import { MaterialItemsComponent } from './stock-items/stock-items.component';
import { ImportMaterialInformation, ImportResultMap } from '../import-stock.type';

@Component({
  selector: 'app-result-import-stock',
  templateUrl: './result-import-stock.component.html',
  styleUrls: ['./result-import-stock.component.scss'],
  standalone: true,
  providers: [],
  imports: [FormsModule, NgbModule, NgSelectModule, MatCheckboxModule, CoreModule, MaterialItemsComponent],
})
export class ResultImportStockComponent implements OnChanges {
  @Input() importMode: any | undefined;
  @Input() importMaterialInformation: ImportMaterialInformation;
  @Input() resultImport!: ImportResultMap[keyof ImportResultMap];
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
    const { note } = this.importMaterialInformation;

    this.importInfoForm.patchValue({
      note: note,
    });
  }

  toggleCardCollapse(cardId: string): void {
    this.isCardCollapsed[cardId] = !this.isCardCollapsed[cardId];
  }
}
