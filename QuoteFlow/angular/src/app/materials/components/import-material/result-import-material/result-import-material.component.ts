import { CommonModule } from '@angular/common';
import { Component, inject, Input, OnChanges, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CoreModule } from '@abp/ng.core';
import { NgSelectModule } from '@ng-select/ng-select';
import { MatCheckboxModule } from '@angular/material/checkbox';

import { MaterialItemsComponent } from './material-items/material-items.component';
import { ImportMaterialInformation, ImportResultMap } from '../import-material.type';
import { WarningDisplayComponent } from '@app/shared/components/warning-display/warning-display.component';

@Component({
  selector: 'app-result-import-material',
  templateUrl: './result-import-material.component.html',
  styleUrls: ['./result-import-material.component.scss'],
  standalone: true,
  providers: [],
  imports: [
    CommonModule,
    FormsModule,
    NgbModule,
    NgSelectModule,
    MatCheckboxModule,
    CoreModule,
    MaterialItemsComponent,
    WarningDisplayComponent,
  ],
})
export class ResultImportMaterialComponent implements OnChanges {
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

  get hasWarnings(): boolean {
    return (this.resultImport as any)?.hasNotFoundWarnings === true;
  }

  get warningMessages(): string[] {
    return (this.resultImport as any)?.warnings ?? [];
  }

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
