import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, Validators } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CoreModule } from '@abp/ng.core';
import { NgSelectModule } from '@ng-select/ng-select';

@Component({
  selector: 'app-import-sale-order-modal',
  templateUrl: './import-sale-order-modal.component.html',
  styleUrls: ['./import-sale-order-modal.component.scss'],
  standalone: true,
  imports: [FormsModule, NgbModule, NgSelectModule, CoreModule],
})
export class ImportSaleOrderModalComponent {
  public fileImport: File | null = null;

  importForm: FormGroup = new FormGroup({});

  constructor(private fb: FormBuilder) {
    this.importForm = this.fb.group({
      file: [null, Validators.required],
      // note: [null, []],
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

  clearSelectedFile(): void {
    this.fileImport = null;
    this.importForm.patchValue({ file: null });

    // Reset the file input element
    const fileInput = document.querySelector('input[type="file"]') as HTMLInputElement;
    if (fileInput) {
      fileInput.value = '';
    }
  }
}
