import { Component, Input, SimpleChanges, OnChanges } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, Validators } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CoreModule } from '@abp/ng.core';
import { NgSelectModule } from '@ng-select/ng-select';
import { ImportStockTracingType } from '../stock-tracing.abstract.component';
import { ImportStockTracingInformation } from '../stock-tracing.types';

@Component({
  selector: 'app-import-stock',
  templateUrl: './import-stock.component.html',
  styleUrls: ['./import-stock.component.scss'],
  standalone: true,
  imports: [FormsModule, NgbModule, NgSelectModule, CoreModule],
})
export class ImportStockComponent implements OnChanges {
  @Input() importMode: ImportStockTracingType | undefined;
  public fileImport: File | null = null;
  importForm: FormGroup = new FormGroup({});
  importPriceOfferType = ImportStockTracingType;

  importInformation: ImportStockTracingInformation = {
    file: null,
    fromDate: null,
    toDate: null,
    entered: null,
    note: '',
  };

  isInventory: boolean = false;
  constructor(private fb: FormBuilder) {
    // this.importStockForm = this.fb.group({
    //   file: [null, Validators.required],
    //   fromDate: [null, Validators.required],
    //   toDate: [null, Validators.required],
    //   note: [null, [Validators.required]],
    // });
    this.initializeForm();
  }
  ngOnChanges(changes: SimpleChanges): void {
    this.isInventory = this.importMode === ImportStockTracingType.Inventory;
  }
  private initializeForm(): void {
    this.importForm = this.fb.group({
      file: [null],
      fromDate: [null],
      toDate: [null],
      entered: [null],
      note: [null],
    });
  }

  onFileChange(event: any): void {
    const files = event.target.files;
    if (files.length > 0) {
      this.fileImport = files[0];
    } else {
      this.fileImport = null;
    }
  }
  resetForm(): void {
    this.importForm.reset();
    this.fileImport = null;
    const fileInput = document.querySelector('input[type="file"]') as HTMLInputElement;
    if (fileInput) {
      fileInput.value = '';
    }
  }
}
