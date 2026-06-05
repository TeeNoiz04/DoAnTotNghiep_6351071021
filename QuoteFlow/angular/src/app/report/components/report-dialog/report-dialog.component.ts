import { AbpWindowService, CoreModule } from '@abp/ng.core';
import { ThemeSharedModule, ToasterService } from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';
import { Component, inject, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { AppRoutes } from '@app/app.routes';
import { NgbActiveModal, NgbDatepickerModule, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { Observable, Subject, takeUntil } from 'rxjs';
import { ReportMenuService } from '../../services/report-menu.service';
import { LookupDto } from '@proxy/shared';
import { LookupService } from '@proxy/general-lookups/lookup.service';
import { NgSelectModule } from '@ng-select/ng-select';

@Component({
  selector: 'app-report-dialog',
  standalone: true,
  imports: [CommonModule, CoreModule, ThemeSharedModule, ReactiveFormsModule, NgbDatepickerModule, NgSelectModule],
  template: `
    <div class="container-fluid p-0">
      <form [formGroup]="form">
        <div class="row">
          <div class="col-md-6 mb-1">
            <label for="fromDate" class="form-label">From Date</label>
            <div class="input-group">
              <input
                id="fromDate"
                name="fromDate"
                readonly
                autocomplete="off"
                class="form-control"
                formControlName="fromDate"
                ngbDatepicker
                container="body"
                (click)="fromDatePicker.toggle()"
                #fromDatePicker="ngbDatepicker" />
              <button class="btn btn-outline-secondary calendar" (click)="fromDatePicker.toggle()" type="button">
                <i class="bi bi-calendar"></i>
              </button>
            </div>
            <!-- <div *ngIf="submitted && form.get('fromDate')?.invalid" class="text-danger small mt-1">
              From date is required
            </div> -->
          </div>

          <div class="col-md-6 mb-1">
            <label for="toDate" class="form-label">To Date</label>
            <div class="input-group">
              <input
                id="toDate"
                readonly
                autocomplete="off"
                name="toDate"
                class="form-control"
                formControlName="toDate"
                ngbDatepicker
                container="body"
                (click)="toDatePicker.toggle()"
                #toDatePicker="ngbDatepicker" />
              <button class="btn btn-outline-secondary calendar" (click)="toDatePicker.toggle()" type="button">
                <i class="bi bi-calendar"></i>
              </button>
            </div>
            <!-- <div *ngIf="submitted && form.get('toDate')?.invalid" class="text-danger small mt-1">
              To date is required
            </div> -->
          </div>
        </div>
        <!-- Date range error message -->
        <!-- <div class="row" *ngIf="submitted && form.errors?.['dateRangeExceeded']">
          <div class="col-12 mb-1">
            <div class="text-danger small">
              <i class="bi bi-exclamation-circle me-1"></i>
              Date range cannot exceed 1 year
            </div>
          </div>
        </div> -->
        <!-- if report R03 -->
        <div class="row" *ngIf="isR03Report">
          <div class="col-md-6 mb-1">
            <label for="Location" class="form-label">Location</label>
            <ng-select
              [items]="locationOptions"
              name="locationFilter"
              bindLabel="label"
              bindValue="value"
              formControlName="location">
            </ng-select>
          </div>
          <div class="col-md-6 mb-1">
            <label for="MaterialType" class="form-label">Material Type</label>
            <ng-select
              [items]="materialTypeOptions"
              name="materialTypeFilter"
              bindLabel="displayName"
              bindValue="displayCode"
              formControlName="materialType"
              appendTo="body"
              clearable="true">
            </ng-select>
          </div>
        </div>

        <div class="row">
          <div class="col-md-6 mb-1">
            <label for="BuyerCode" class="form-label">Buyer</label>
            <ng-select
              [items]="buyerOptions"
              name="materialTypeFilter"
              bindLabel="displayCode"
              bindValue="displayCode"
              formControlName="buyer"
              appendTo="body"
              clearable="true">
            </ng-select>
          </div>
          <!-- if report R03 -->
          <div class="col-md-6 mb-1" *ngIf="isR03Report">
            <label for="CustomerName" class="form-label">Customer Name <strong>(%)</strong></label>
            <input type="text" class="form-control" formControlName="customerName" />
          </div>
          <!-- R04 -->
          <div class="col-md-6 mb-1" *ngIf="isR04Report">
            <label for="MaterialGroup" class="form-label">Material Group</label>
            <ng-select
              [items]="materialGroupOptions"
              name="materialGroupFilter"
              bindLabel="displayName"
              bindValue="displayCode"
              formControlName="materialGroup"
              appendTo="body"
              clearable="true">
            </ng-select>
          </div>
        </div>
        <div class="row" *ngIf="isR04Report">
          <div class="col-md-6 mb-1">
            <label for="golfaCode" class="form-label">Material Code</label>
            <input type="text" class="form-control" formControlName="golfaCode" />
          </div>
          <div class="col-md-6 mb-1">
            <label for="modelName" class="form-label">Model Name <strong>(%)</strong></label>
            <input type="text" class="form-control" formControlName="modelName" />
          </div>
        </div>
        <div class="row">
          <div class="col-md-6 mb-1">
            <label for="PriceOfferCode" class="form-label">Price Offer Code</label>
            <input type="text" class="form-control" formControlName="priceOfferCode" />
          </div>
          <div class="col-md-6 mb-1">
            <label for="PriceOfferName" class="form-label">Price Offer Name <strong>(%)</strong></label>
            <input type="text" class="form-control" formControlName="priceOfferName" />
          </div>
        </div>

        <!-- if report R03 -->
        <div class="row" *ngIf="isR03Report">
          <div class="col-md-6 mb-1">
            <label for="Status" class="form-label">Status</label>
            <ng-select
              [items]="statusOptions"
              name="statusFilter"
              bindLabel="label"
              bindValue="value"
              formControlName="status">
            </ng-select>
          </div>

          <div class="col-md-3 mb-1">
            <label for="OrderMin" class="form-label">Order Min</label>
            <input type="number" class="form-control" formControlName="orderMin" min="0" />
          </div>
          <div class="col-md-3 mb-1">
            <label for="OrderMax" class="form-label">Order Max</label>
            <input type="number" class="form-control" formControlName="orderMax" min="0" />
          </div>
        </div>

        <div class="d-flex justify-content-end align-items-center">
          <button type="button" [disabled]="isGenerating" class="btn btn-primary" (click)="generateReport()">
            <i *ngIf="isGenerating" class="fa fa-spinner fa-spin me-1"></i>
            Export
          </button>
        </div>
      </form>
    </div>
  `,
})
export class ReportDialogComponent implements OnInit, OnChanges {
  @Input() reportType?: string;
  @Input() onGenerateReport?: (data: any) => Observable<any>;
  AppRoutes = AppRoutes;

  form: FormGroup;
  submitted = false;
  isGenerating = false;
  lastGenerationResult?: string;
  generationError?: string;

  private fb = inject(FormBuilder);
  private activeModal = inject(NgbActiveModal);
  private abpWindowService = inject(AbpWindowService);
  private reportMenuService = inject(ReportMenuService);
  private toasterService = inject(ToasterService);
  private readonly lookupService = inject(LookupService);
  protected readonly destroy$ = new Subject<void>();
  buyerOptions: LookupDto<string>[] = [];
  materialTypeOptions: LookupDto<string>[] = [];
  materialGroupOptions: LookupDto<string>[] = [];
  //   Location: North - South
  // Status: APPROVED - CLOSED - IN_PROGRESS
  locationOptions: { label: string; value: string }[] = [
    { label: 'North', value: 'North' },
    { label: 'South', value: 'South' },
  ];
  statusOptions: { label: string; value: string }[] = [
    { label: 'APPROVED', value: 'APPROVED' },
    { label: 'CLOSED', value: 'CLOSED' },
    { label: 'IN PROGRESS', value: 'IN_PROGRESS' },
  ];

  get isR03Report(): boolean {
    return this.reportType === AppRoutes.SPECIAL_PRICE_OFFERS.GENERAL_REPORT.BASE;
  }

  get isR04Report(): boolean {
    return this.reportType === AppRoutes.SPECIAL_PRICE_OFFERS.SPO_DETAILED_REPORT.BASE;
  }

  constructor() {
    this.form = this.fb.group(
      {
        fromDate: [null],
        toDate: [null],
        buyer: [''],
        customerName: [''],
        priceOfferCode: [''],
        priceOfferName: [''],
        location: [''],
        status: [''],
        materialType: [''],
        orderMin: [''],
        orderMax: [''],
        keepModalOpen: [true],
        // R04
        materialGroup: [''],
        golfaCode: [''],
        modelName: [''],
      },
      {
        validators: [this.dateRangeValidator],
      },
    );
  }

  ngOnInit(): void {
    this.loadFilterOptions();
    // this.initializeForm();
  }
  private loadFilterOptions() {
    this.lookupService
      .getMaterialTypeLookup()
      .pipe()
      .subscribe({
        next: result => {
          this.materialTypeOptions = result?.items || [];
        },
        error: error => {
          console.error('Error loading material types:', error);
        },
      });

    // Load buyers
    this.lookupService
      .getBuyerLookup(false)
      .pipe()
      .subscribe({
        next: result => {
          this.buyerOptions = result?.items || [];
        },
        error: error => {
          console.error('Error loading buyers:', error);
        },
      });
    this.lookupService
      .getMaterialGroupLookup()
      .pipe()
      .subscribe({
        next: result => {
          this.materialGroupOptions = result?.items || [];
        },
        error: error => {
          console.error('Error loading material groups:', error);
        },
      });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['reportType'] && changes['reportType'].currentValue) {
      this.reportType = changes['reportType'].currentValue;
      if (!this.onGenerateReport) {
        this.onGenerateReport = this.reportMenuService.getReportGenerationFunctionForType(this.reportType);
      }
    }
  }

  private dateRangeValidator(control: AbstractControl): ValidationErrors | null {
    const fromDate = control.get('fromDate')?.value;
    const toDate = control.get('toDate')?.value;

    if (!fromDate || !toDate) {
      return null;
    }

    const fromJsDate = new Date(fromDate);
    const toJsDate = new Date(toDate);
    const diffTime = Math.abs(toJsDate.getTime() - fromJsDate.getTime());
    const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));

    if (diffDays > 365) {
      return { dateRangeExceeded: true };
    }

    return null;
  }

  private initializeForm(): void {
    const today = new Date();
    const oneYearAgo = new Date(today);
    oneYearAgo.setFullYear(today.getFullYear() - 1);

    const fromDateStruct: NgbDateStruct = {
      year: oneYearAgo.getFullYear(),
      month: oneYearAgo.getMonth() + 1,
      day: oneYearAgo.getDate(),
    };

    const toDateStruct: NgbDateStruct = {
      year: today.getFullYear(),
      month: today.getMonth() + 1,
      day: today.getDate(),
    };

    setTimeout(() => {
      this.form.patchValue({
        fromDate: fromDateStruct,
        toDate: toDateStruct,
      });
      this.form.markAsPristine();
    }, 400);
  }

  generateReport(): void {
    this.submitted = true;
    this.clearMessages();

    // if (!this.form.valid) {
    //   if (this.form.errors?.['dateRangeExceeded']) {
    //     this.toasterService.error('Date range cannot exceed 1 year', 'Validation Error');
    //   }
    //   return;
    // }

    this.isGenerating = true;
    const formValue = {
      ...this.form.value,
      reportType: this.reportType,
    };
    // console.log(formValue);

    const keepModalOpen = formValue.keepModalOpen;

    if (this.onGenerateReport) {
      // Use the async report generation function
      this.onGenerateReport(formValue).subscribe({
        next: result => {
          this.isGenerating = false;

          if (result instanceof Blob) {
            const filename = this.getReportFileName(this.reportType);
            this.abpWindowService.downloadBlob(result, filename);
            this.lastGenerationResult = 'Report general and downloaded successfully!';
          } else {
            this.lastGenerationResult = 'Report general successfully!';
          }

          if (!keepModalOpen) {
            // Close modal after showing success message briefly
            setTimeout(() => {
              this.activeModal.close(result);
            }, 1500);
          }
        },
        error: error => {
          this.isGenerating = false;
          this.generationError = 'Failed to general report. Please try again.';
          this.toasterService.error('Failed to general report', 'Error');
        },
      });
    } else {
      // Fallback to original behavior
      this.activeModal.close(formValue);
    }
  }

  private clearMessages(): void {
    this.lastGenerationResult = undefined;
    this.generationError = undefined;
  }

  private getReportFileName(reportType?: string): string {
    if (!reportType) return 'Report.xlsx';

    switch (reportType) {
      case AppRoutes.SPECIAL_PRICE_OFFERS.GENERAL_REPORT.BASE:
        return `SPOGeneralReport(R03)_${this.getTimeStamp()}.xlsx`;
      case AppRoutes.SPECIAL_PRICE_OFFERS.SPO_DETAILED_REPORT.BASE:
        return `SPODetailedReport(R04)_${this.getTimeStamp()}.xlsx`;
      case AppRoutes.REPORT.DPO_REPORT.DPO_2023.BASE:
        return `DPOReport_${this.getTimeStamp()}.xlsx`;
      default:
        return `Report_${this.getTimeStamp()}.xlsx`;
    }
  }
  private getTimeStamp(): string {
    const now = new Date();
    const pad = (n: number) => n.toString().padStart(2, '0');

    const yyyy = now.getFullYear();
    const MM = pad(now.getMonth() + 1);
    const dd = pad(now.getDate());
    const hh = pad(now.getHours());
    const mm = pad(now.getMinutes());
    const ss = pad(now.getSeconds());

    return `${yyyy}${MM}${dd}_${hh}${mm}${ss}`;
  }
}
