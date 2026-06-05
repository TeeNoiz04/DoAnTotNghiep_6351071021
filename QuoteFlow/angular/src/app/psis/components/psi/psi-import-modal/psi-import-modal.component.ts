import { CoreModule } from '@abp/ng.core';
import { Component, inject, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, Validators } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { LookupService } from '@proxy/general-lookups';
import { LookupDto } from '@proxy/shared';

@Component({
  selector: 'app-psi-import-modal',
  templateUrl: './psi-import-modal.component.html',
  styleUrls: ['./psi-import-modal.component.scss'],
  standalone: true,
  imports: [FormsModule, NgbModule, NgSelectModule, CoreModule],
})
export class PSIImportModalComponent implements OnInit {
  private readonly lookupService = inject(LookupService);

  @Input() importMode: any | undefined;
  public fileImport: File | null = null;

  materialTypeOptions: LookupDto<string>[] = [];
  fyOptions: any[] = [];
  importForm: FormGroup = new FormGroup({});

  constructor(private fb: FormBuilder) {
    this.initializeForm();
  }

  ngOnInit() {
    this.loadDataOptions();
  }

  private initializeForm(): void {
    this.importForm = this.fb.group({
      financeYear: [null, Validators.required],
      // materialType: [this.type, Validators.required],
      file: [null, Validators.required],
      note: [null, []],
    });
  }

  private loadDataOptions() {
    this.lookupService.getMaterialTypeLookup().subscribe({
      next: result => {
        this.materialTypeOptions = result?.items || [];
      },
      error: error => {
        console.error('Error loading material types:', error);
      },
    });

    this.lookupService.getYearLookup().subscribe({
      next: result => {
        const arr = result?.items?.map(item => {
          return {
            displayName: item,
            displayCode: item,
          };
        });
        this.fyOptions = arr || [];

        // set default value for financeYear
        const currentYear = new Date().getFullYear();
        if (result?.items?.includes(currentYear)) {
          this.importForm.patchValue({ financeYear: currentYear });
        }
      },
      error: error => {
        console.error('Error loading material types:', error);
      },
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
