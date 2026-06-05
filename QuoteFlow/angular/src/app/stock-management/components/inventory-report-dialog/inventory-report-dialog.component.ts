import { AbpWindowService, CoreModule } from '@abp/ng.core';
import { ThemeSharedModule, ToasterService } from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';
import { Component, inject, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Observable } from 'rxjs';
import { NgSelectModule } from '@ng-select/ng-select'; // Import này
import { LookupService } from '@proxy/general-lookups';
import { LookupDto } from '@proxy/shared';
import { StockReportDialogService } from '@app/stock-management/services/stock-report-dialog.service';

@Component({
  selector: 'app-inventory-report-dialog',
  standalone: true,
  imports: [CommonModule, CoreModule, ThemeSharedModule, ReactiveFormsModule, NgSelectModule],
  template: `
    <div class="container-fluid p-0">
      <form [formGroup]="form">
        <div class="row">
          <div class="col-md-6 mb-3">
            <label for="materialCode" class="form-label">Material Code <strong>(%)</strong></label>
            <input type="text" id="materialCode" formControlName="materialCode" class="form-control" />
          </div>

          <div class="col-md-6 mb-3">
            <label for="inventoryCategory" class="form-label">Inventory Category <strong>(%)</strong></label>
            <input type="text" id="inventoryCategory" formControlName="inventoryCategory" class="form-control" />
          </div>
        </div>

        <div class="row">
          <div class="col-md-12 mb-3">
            <label for="materialGroup" class="form-label">Material Group</label>
            <ng-select
              [items]="materialGroups"
              bindLabel="displayName"
              bindValue="id"
              formControlName="materialGroup"
              [searchable]="true"
              [clearable]="true"
              notFoundText="No material groups found"
              class="custom-ng-select">
            </ng-select>
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
export class InventoryReportDialogComponent implements OnInit, OnChanges {
  @Input() onGenerateReport?: (data: any) => Observable<any>;

  form: FormGroup;
  submitted = false;
  isGenerating = false;
  materialGroups: LookupDto<string>[] = [];

  private fb = inject(FormBuilder);
  private activeModal = inject(NgbActiveModal);
  private abpWindowService = inject(AbpWindowService);
  private toasterService = inject(ToasterService);
  private lookupService = inject(LookupService);
  private reportService = inject(StockReportDialogService);

  constructor() {
    this.form = this.fb.group({
      materialCode: [null],
      inventoryCategory: [null],
      materialGroup: [null],
    });
  }

  ngOnInit(): void {
    this.loadMaterialGroups();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (!this.onGenerateReport) {
      const values = this.form.getRawValue();
      this.onGenerateReport = this.reportService.getReportGenerationFunction();
    }
  }

  private loadMaterialGroups(): void {
    this.lookupService.getMaterialGroupLookup().subscribe({
      next: response => {
        if (response && response.items) {
          this.materialGroups = response.items;
        }
      },
      error: error => {
        this.toasterService.error('Failed to load material groups', 'Error');
      },
    });
  }

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
            this.abpWindowService.downloadBlob(result, 'InventoryReport.xlsx');
            setTimeout(() => {
              this.activeModal.close(result);
            }, 500);
          } else {
            this.activeModal.close(result);
          }
        },
        error: error => {
          this.isGenerating = false;
          this.toasterService.error('Failed to generate inventory report', 'Error');
        },
      });
    } else {
      this.activeModal.close(formValue);
    }
  }
}
