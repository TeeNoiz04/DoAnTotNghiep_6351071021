import { Component, inject, Input, OnChanges, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CoreModule } from '@abp/ng.core';
import { NgSelectModule } from '@ng-select/ng-select';
import { MatCheckboxModule } from '@angular/material/checkbox';

import { PAIItemsComponent } from './psi-items/psi-items.component';
import { ImportPSIInformation, ImportResultMap } from '../psi.types';

@Component({
  selector: 'app-result-import-psi',
  templateUrl: './result-import-psi.component.html',
  styleUrls: ['./result-import-psi.component.scss'],
  standalone: true,
  providers: [],
  imports: [FormsModule, NgbModule, NgSelectModule, MatCheckboxModule, CoreModule, PAIItemsComponent],
})
export class ResultImportPSIComponent implements OnChanges {
  @Input() importMode: any | undefined;
  @Input() importInformation: ImportPSIInformation;
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
      financeYear: [''],
      materialType: [''],
      note: [''],
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    const { note, financeYear, materialType } = this.importInformation;

    this.importInfoForm.patchValue({
      financeYear: financeYear,
      materialType: materialType,
      note: note,
    });
  }

  toggleCardCollapse(cardId: string): void {
    this.isCardCollapsed[cardId] = !this.isCardCollapsed[cardId];
  }
}
