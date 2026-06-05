import { Component, ElementRef, Input, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, Validators } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CoreModule } from '@abp/ng.core';
import { NgSelectModule } from '@ng-select/ng-select';

@Component({
  selector: 'app-import-material-modal',
  templateUrl: './import-material-modal.component.html',
  styleUrls: ['./import-material-modal.component.scss'],
  standalone: true,
  imports: [FormsModule, NgbModule, NgSelectModule, CoreModule],
})
export class ImportMaterialModalComponent {
  @Input() importMode: any | undefined;
  public fileImport: File | null = null;

  importForm: FormGroup = new FormGroup({});

  constructor(private fb: FormBuilder) {
    this.initializeForm();
  }

  private initializeForm(): void {
    this.importForm = this.fb.group({
      file: [null],
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
