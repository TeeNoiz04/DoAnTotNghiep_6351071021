import { CoreModule } from '@abp/ng.core';
import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { ExpandablePanelV2Component } from '@app/shared/components/expandable-panel-v2/expandable-panel-v2.component';
import { ThreeStateButtonComponent } from '@app/shared/components/three-state-button';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { LookupService } from '@proxy/general-lookups';
import { LookupDto } from '@proxy/shared';

@Component({
  selector: 'app-sale-report-r06-filter',
  templateUrl: './sale-report-r06-filter.component.html',
  standalone: true,
  imports: [
    FormsModule,
    NgbModule,
    NgSelectModule,
    MatCheckboxModule,
    CoreModule,
    ReactiveFormsModule,
    ExpandablePanelV2Component,
    ThreeStateButtonComponent,
  ],
  providers: [LookupService, LoadingService],
})
export class SaleReportR06FilterComponent implements OnInit {
  protected readonly lookupService = inject(LookupService);
  protected readonly loadingService = inject(LoadingService);
  protected readonly fb = inject(FormBuilder);
  form: FormGroup = new FormGroup({});

  ngOnInit(): void {
    this.initializeForm();
  }

  get f() {
    return this.form.controls;
  }

  private initializeForm(): void {
    const today = new Date();
    const currentYear = today.getFullYear();
    const defaultFromDate = { year: currentYear, month: 1, day: 1 };
    const defaultToDate = { year: currentYear, month: 12, day: 31 };

    this.form = this.fb.group({
      fromDate: [null],
      toDate: [null],
      invoiceFromDate: [null],
      invoiceToDate: [null],
    });
  }
}
