import { CoreModule } from '@abp/ng.core';
import { Component, OnInit, ViewChild, inject } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

interface ImportSpecialInputPriceInformation {
  file: File | null;
}

@Component({
  selector: 'app-import-special-input-price-modal',
  standalone: true,
  templateUrl: './import-special-input-price-modal.component.html',
  styleUrls: ['./import-special-input-price-modal.component.scss'],
  providers: [LoadingService],
  imports: [FormsModule, ReactiveFormsModule, CoreModule],
})
export class ImportSpecialInputPriceModalComponent implements OnInit {
  protected readonly loadingService = inject(LoadingService);
  protected readonly fb = inject(FormBuilder);

  @ViewChild('fileInput') fileInput: any;
  importForm: FormGroup = new FormGroup({});
  public fileImport: File | null = null;

  ngOnInit(): void {
    this.initializeForm();
  }

  get f() {
    return this.importForm.controls;
  }

  private initializeForm(): void {
    this.importForm = this.fb.group({
      file: [null, [Validators.required]],
    });
  }

  onFileChange(event: any): void {
    const file = event.target.files?.[0];
    if (file) {
      this.fileImport = file;
      this.importForm.patchValue({ file });
    }
  }

  validateForm(): boolean {
    if (this.importForm.valid) {
      return true;
    } else {
      // Mark all fields as touched to show validation errors
      Object.keys(this.importForm.controls).forEach(key => {
        this.importForm.get(key)?.markAsTouched();
      });
      return false;
    }
  }

  resetForm(): void {
    this.importForm.reset();
    this.fileImport = null;
  }

  resetFile() {
    if (this.fileInput) {
      this.fileInput.nativeElement.value = '';
      this.fileImport = null;
      this.importForm.patchValue({ file: null });
    }
  }

  getImportInformation(): ImportSpecialInputPriceInformation {
    return {
      file: this.fileImport,
    };
  }
}
