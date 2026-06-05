import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ExpandablePanelV2Component } from '@app/shared/components/expandable-panel-v2/expandable-panel-v2.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { CoreModule } from '@abp/ng.core';
import { ThreeStateButtonComponent } from '@app/shared/components/three-state-button';
@Component({
  selector: 'app-sale-report-r05-filter',
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
  templateUrl: './sale-report-r05-filter.component.html',
})
export class SaleReportR05FilterComponent implements OnInit {
  form!: FormGroup;

  constructor(private fb: FormBuilder) {}

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
