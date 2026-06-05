import { Component, inject, Input, OnChanges, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CoreModule } from '@abp/ng.core';
import { NgSelectModule } from '@ng-select/ng-select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { ImportBatchRequestInformation, ImportResultMap } from '../batch-request.types';
import { BatchRequestItemsComponent } from './batch-request-items/batch-request-items.component';

@Component({
  selector: 'app-result-import-batch-request',
  templateUrl: './result-import-batch-request.component.html',
  styleUrls: ['./result-import-batch-request.component.scss'],
  standalone: true,
  providers: [],
  imports: [FormsModule, NgbModule, NgSelectModule, MatCheckboxModule, CoreModule, BatchRequestItemsComponent],
})
export class ResultImportBatchRequestComponent implements OnChanges {
  @Input() importMode: any | undefined;
  @Input() importBatchRequestInformation: ImportBatchRequestInformation;
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
    const { note } = this.importBatchRequestInformation;

    this.importInfoForm.patchValue({
      note: note,
    });
  }

  toggleCardCollapse(cardId: string): void {
    this.isCardCollapsed[cardId] = !this.isCardCollapsed[cardId];
  }
}
