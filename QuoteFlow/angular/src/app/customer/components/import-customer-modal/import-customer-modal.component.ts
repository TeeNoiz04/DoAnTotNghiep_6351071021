import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CoreModule } from '@abp/ng.core';
import { NgSelectModule } from '@ng-select/ng-select';

@Component({
  selector: 'app-import-customer-modal',
  templateUrl: './import-customer-modal.component.html',
  styleUrls: ['./import-customer-modal.component.scss'],
  standalone: true,
  imports: [FormsModule, NgbModule, NgSelectModule, CoreModule],
})
export class ImportCustomerModalComponent {
  public fileImport: File | null = null;

  onFileChange(event: any): void {
    const files = event.target.files;
    if (files.length > 0) {
      this.fileImport = files[0];
    } else {
      this.fileImport = null;
    }
  }

  resetForm(): void {
    this.fileImport = null;
    const fileInput = document.querySelector('input[type="file"]') as HTMLInputElement;
    if (fileInput) {
      fileInput.value = '';
    }
  }
}
