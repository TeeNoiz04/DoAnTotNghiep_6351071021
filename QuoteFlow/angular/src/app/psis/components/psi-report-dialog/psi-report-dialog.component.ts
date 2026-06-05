import { AbpWindowService, CoreModule } from '@abp/ng.core';
import { ThemeSharedModule, ToasterService } from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';
import { Component, inject, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { PSIReportDialogService } from '@app/psis/services/psi-report-dialog.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { LookupService } from '@proxy/general-lookups';
import { LookupDto } from '@proxy/shared';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-psi-report-dialog',
  standalone: true,
  imports: [CommonModule, CoreModule, ThemeSharedModule, ReactiveFormsModule, NgSelectModule],
  template: `
    <div class="container-fluid p-0">
      <form [formGroup]="form">
        <div class="row">
          <div class="col-md-6 mb-3">
            <label for="materialType" class="form-label label-required">Material Type</label>
            <ng-select
              formControlName="materialType"
              id="materialType"
              [items]="materialTypes"
              bindLabel="displayCode"
              bindValue="displayCode"
              placeholder="-- Select Material Type --"
              [class.is-invalid]="submitted && form.get('materialType')?.invalid">
            </ng-select>
            <div class="invalid-feedback" *ngIf="submitted && form.get('materialType')?.invalid">
              <div *ngIf="form.get('materialType')?.errors?.['required']">Material Type is required</div>
            </div>
          </div>

          <div class="col-md-6 mb-3">
            <label for="fiscalYear" class="form-label label-required">Fiscal Year</label>
            <ng-select
              formControlName="fiscalYear"
              id="fiscalYear"
              [items]="years"
              bindLabel="displayName"
              bindValue="displayCode"
              placeholder="-- Select Fiscal Year --"
              [class.is-invalid]="submitted && form.get('fiscalYear')?.invalid">
            </ng-select>
            <div class="invalid-feedback" *ngIf="submitted && form.get('fiscalYear')?.invalid">
              <div *ngIf="form.get('fiscalYear')?.errors?.['required']">Fiscal Year is required</div>
              <div *ngIf="form.get('fiscalYear')?.errors?.['min']">Fiscal Year must be at least 2000</div>
              <div *ngIf="form.get('fiscalYear')?.errors?.['max']">Fiscal Year cannot exceed 2100</div>
            </div>
          </div>
        </div>

        <div class="d-flex justify-content-end align-items-center">
          <button type="button" [disabled]="isGenerating" class="btn btn-primary" (click)="generateReport()">
            <i *ngIf="isGenerating" class="fa fa-spinner fa-spin me-1"></i>
            Generate Report
          </button>
        </div>
      </form>
    </div>
  `,
})
export class PSIReportDialogComponent implements OnInit, OnChanges {
  @Input() onGenerateReport?: (data: any) => Observable<any>;

  form: FormGroup;
  submitted = false;
  isGenerating = false;
  materialTypes: any = [];
  years: { displayName: string; displayCode: number }[] = [];

  private fb = inject(FormBuilder);
  private activeModal = inject(NgbActiveModal);
  private abpWindowService = inject(AbpWindowService);
  private toasterService = inject(ToasterService);
  private lookupService = inject(LookupService);
  private reportService = inject(PSIReportDialogService);

  constructor() {
    this.getFY();
    this.form = this.fb.group({
      materialType: [null, Validators.required],
      fiscalYear: [new Date().getFullYear(), [Validators.required, Validators.min(2000), Validators.max(2100)]],
    });
  }

  ngOnInit(): void {
    // this.loadMaterialTypes();
    this.materialTypes = [
      { displayCode: 'FA', displayName: 'FA' },
      { displayCode: 'LVS', displayName: 'LVS' },
    ];
    //get lookup Fiscal year PSI
  }

  getFY() {
    this.lookupService.getYearDistinctPSI().subscribe({
      next: response => {
        if (response && response.items) {
          this.years = response.items.map((year: number) => ({
            displayName: year.toString(),
            displayCode: year,
          }));

          const currentYear = new Date().getFullYear();
          const existsCurrent = this.years.some(y => y.displayCode === currentYear);

          if (existsCurrent) {
            this.form.get('fiscalYear')?.setValue(currentYear);
          } else {
            const maxYear = Math.max(...this.years.map(y => y.displayCode));
            this.form.get('fiscalYear')?.setValue(maxYear);
          }
        }
      },
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (!this.onGenerateReport) {
      this.onGenerateReport = this.reportService.getReportGenerationFunction();
    }
  }

  // private loadMaterialTypes(): void {
  //   this.lookupService.getMaterialTypeLookup().subscribe({
  //     next: response => {
  //       if (response && response.items) {
  //         this.materialTypes = response.items;
  //       }
  //     },
  //     error: error => {
  //       this.toasterService.error('Failed to load material types', 'Error');
  //     },
  //   });
  // }

  generateReport(): void {
    this.submitted = true;
    if (!this.form.valid) {
      return;
    }

    this.isGenerating = true;
    const formValue = this.form.value;

    if (this.onGenerateReport) {
      this.onGenerateReport(formValue).subscribe({
        next: result => {
          this.isGenerating = false;

          if (result instanceof Blob) {
            // get fileName = "$"PSI by product ({DateTime.Now:dd-MM-yyyy})-{input.MaterialType}-FY<yyyy>.xlsx";"
            const materialType = formValue.materialType || '';
            const fiscalYear = formValue.fiscalYear || '';
            const fileName = `PSI by product (${new Date().toISOString().split('T')[0]})-${materialType}-FY${fiscalYear}.xlsx`;
            this.abpWindowService.downloadBlob(result, fileName);
          } else {
            this.activeModal.close(result);
          }
        },
        error: error => {
          this.isGenerating = false;
          this.toasterService.error('Failed to generate PSI report', 'Error');
        },
      });
    } else {
      this.activeModal.close(formValue);
    }
  }
}
