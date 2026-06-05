import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, Validators } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CoreModule } from '@abp/ng.core';
import { NgSelectModule } from '@ng-select/ng-select';

@Component({
  selector: 'app-buyer-import-modal',
  templateUrl: './buyer-import-modal.component.html',
  styleUrls: ['./buyer-import-modal.component.scss'],
  standalone: true,
  imports: [FormsModule, NgbModule, NgSelectModule, CoreModule],
})
export class BuyerImportModalComponent {
  public fileImport: File | null = null;

  importCustomerForm: FormGroup = new FormGroup({});

  constructor(private fb: FormBuilder) {
    this.importCustomerForm = this.fb.group({
      file: [null, Validators.required],
      // financeYear: [null, Validators.required],
      // note: [null, [Validators.required]],
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

  downloadTemplate() {
    // Implement the logic to download the template file
  }
}
