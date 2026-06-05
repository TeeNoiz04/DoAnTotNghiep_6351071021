import { AbpWindowService, CoreModule } from '@abp/ng.core';
import { ThemeSharedModule, ToasterService } from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';
import { Component, inject, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgbActiveModal, NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { Observable } from 'rxjs';
import { LookupService } from '@proxy/general-lookups';
import { LookupDto } from '@proxy/shared';
import { StockReportDialogService } from '@app/stock-management/services/stock-report-dialog.service';
import { SalesAssignmentService } from '@proxy/sales-assignments';
import { SaleReportDialogService } from '@app/stock-management/services/sale-report-dialog.service';

@Component({
  selector: 'app-sale-report-dialog',
  standalone: true,
  imports: [CommonModule, CoreModule, ThemeSharedModule, ReactiveFormsModule, NgbDatepickerModule],
  template: `
    <div class="container-fluid p-0">
      <form [formGroup]="form">
        <div class="row">
          <div class="col-md-6 mb-3">
            <label class="form-label">{{ '::FromDate' | abpLocalization }}</label>
            <div class="input-group">
              <input
                autocomplete="off"
                id="fromDate"
                class="form-control"
                name="fromDate"
                #minFromDateInput="ngbDatepicker"
                ngbDatepicker
                format="dd/MM/yyyy"
                formControlName="fromDate"
                (click)="minFromDateInput.toggle()"
                container="body"
                readonly />
              <button class="btn btn-light bi bi-calendar3" (click)="minFromDateInput?.toggle()" type="button"></button>
            </div>
          </div>

          <div class="col-md-6 mb-3">
            <label for="toDate" class="form-label">{{ '::ToDate' | abpLocalization }}</label>
            <div class="input-group">
              <input
                autocomplete="off"
                id="toDate"
                class="form-control"
                name="toDate"
                #minToDateInput="ngbDatepicker"
                ngbDatepicker
                format="dd/MM/yyyy"
                formControlName="toDate"
                (click)="minToDateInput.toggle()"
                container="body"
                readonly />
              <button class="btn btn-light bi bi-calendar3" (click)="minToDateInput?.toggle()" type="button"></button>
            </div>
          </div>
        </div>
        <div class="row">
          <div class="col-md-6 mb-3">
            <label class="form-label">{{ '::Invoice From Date' | abpLocalization }}</label>
            <div class="input-group">
              <input
                autocomplete="off"
                id="invoiceFromDate"
                class="form-control"
                name="invoiceFromDate"
                #minInvoiceFrom="ngbDatepicker"
                ngbDatepicker
                format="dd/MM/yyyy"
                formControlName="invoiceFromDate"
                (click)="minInvoiceFrom.toggle()"
                container="body"
                readonly />
              <button class="btn btn-light bi bi-calendar3" (click)="minInvoiceFrom?.toggle()" type="button"></button>
            </div>
          </div>

          <div class="col-md-6 mb-3">
            <label for="toDate" class="form-label">{{ '::Invoice To Date' | abpLocalization }}</label>
            <div class="input-group">
              <input
                autocomplete="off"
                id="invoiceToDate"
                class="form-control"
                name="invoiceToDate"
                #maxInvoiceTo="ngbDatepicker"
                ngbDatepicker
                format="dd/MM/yyyy"
                formControlName="invoiceToDate"
                (click)="maxInvoiceTo.toggle()"
                container="body"
                readonly />
              <button class="btn btn-light bi bi-calendar3" (click)="maxInvoiceTo?.toggle()" type="button"></button>
            </div>
          </div>
        </div>

        <div class="d-flex justify-content-end align-items-center">
          <button type="button" [disabled]="isGenerating" class="btn btn-primary" (click)="generateReport()">
            <i *ngIf="isGenerating" class="fa fa-spinner fa-spin me-1"></i>
            Export to Excel
          </button>
        </div>
      </form>
    </div>
  `,
})
export class SaleReportDialogComponent implements OnInit, OnChanges {
  @Input() onGenerateReport?: (data: any) => Observable<any>;

  form: FormGroup;
  submitted = false;
  isGenerating = false;
  materialGroups: LookupDto<string>[] = [];

  private fb = inject(FormBuilder);
  private activeModal = inject(NgbActiveModal);
  private abpWindowService = inject(AbpWindowService);
  private toasterService = inject(ToasterService);
  private reportService = inject(SaleReportDialogService);

  constructor() {
    this.form = this.fb.group({
      fromDate: [null],
      toDate: [null],
      invoiceFromDate: [null],
      invoiceToDate: [null],
    });
  }

  ngOnInit(): void {
    // this.loadMaterialGroups();
    console.log();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (!this.onGenerateReport) {
      const values = this.form.getRawValue();
      this.onGenerateReport = this.reportService.getReportGenerationFunction();
    }
  }

  // private loadMaterialGroups(): void {
  //   this.lookupService.getMaterialGroupLookup().subscribe({
  //     next: response => {
  //       if (response && response.items) {
  //         this.materialGroups = response.items;
  //       }
  //     },
  //     error: error => {
  //       this.toasterService.error('Failed to load material groups', 'Error');
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
            const now = new Date();
            const dd = String(now.getDate()).padStart(2, '0');
            const mm = String(now.getMonth() + 1).padStart(2, '0'); // Months start at 0
            const yy = String(now.getFullYear()).slice(-2);

            const fileName = `R06-SaleReport-${dd}${mm}${yy}.xlsx`;

            this.abpWindowService.downloadBlob(result, fileName);

            setTimeout(() => {
              this.activeModal.close(result);
            }, 500);
          } else {
            this.activeModal.close(result);
          }
        },
        error: error => {
          this.isGenerating = false;
          this.toasterService.error('Failed to generate sale report', 'Error');
        },
      });
    } else {
      this.activeModal.close(formValue);
    }
  }
}
