import { CoreModule } from '@abp/ng.core';
import { DateAdapter, ThemeSharedModule, TimeAdapter } from '@abp/ng.theme.shared';
import { ChangeDetectionStrategy, Component, inject, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { InputNumberComponent } from '@app/shared/components/input-number/input-number.component';
import {
  NgbDateAdapter,
  NgbDatepickerModule,
  NgbNavModule,
  NgbTimeAdapter,
  NgbTimepickerModule,
} from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { LookupService } from '@proxy/general-lookups';
import { LookupDto } from '@proxy/shared';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { DistributorTargetDetailViewService } from '../services/distributor-target-detail.service';
import { EscCloseModalDirective } from '@app/shared/esc-close-modal/esc-close-modal.directive';

@Component({
  selector: 'app-distributor-target-detail-modal',
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    ReactiveFormsModule,
    NgbDatepickerModule,
    NgbTimepickerModule,
    NgbNavModule,
    MatSlideToggleModule,
    NgSelectModule,
    InputNumberComponent,
    EscCloseModalDirective,
  ],
  providers: [
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './distributor-target-detail.component.html',
  styleUrls: ['./distributor-target-detail.component.scss'],
})
export class DistributorTargetDetailModalComponent implements OnInit {
  public readonly service = inject(DistributorTargetDetailViewService);
  public readonly lookupService = inject(LookupService);
  // protected readonly fb = inject(FormBuilder);

  loading = false;

  projectTypeOption: { value: string; label: string }[] = [
    { value: 'FA', label: 'FA' },
    { value: 'LVS', label: 'LVS' },
  ];
  buyerOptions: LookupDto<string>[] = [];
  buyerTypeOptions: LookupDto<string>[] = [];

  ngOnInit(): void {
    this.generateFiscalYears();
    this.getBuyerType();
  }

  isInvalid(controlName: string): boolean {
    const control = this.service.form?.get(controlName);
    return control ? control.invalid && (control.dirty || control.touched) : false;
  }

  hasError(controlName: string, errorName: string): boolean {
    const control = this.service.form?.get(controlName);
    return control ? control.hasError(errorName) && (control.dirty || control.touched) : false;
  }

  getControl(controlName: string) {
    return this.service.form?.get(controlName);
  }

  getBuyer() {
    this.lookupService
      .getBuyerLookup(false)
      // .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: result => {
          this.buyerOptions = result?.items || [];
        },
        error: error => {
          console.error('Error loading buyers:', error);
        },
      });
  }
  getBuyerType() {
    this.lookupService
      .getBuyerTypeLookup({})
      // .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: result => {
          this.buyerTypeOptions = result?.items || [];
        },
        error: error => {
          console.error('Error loading buyers:', error);
        },
      });
  }
  onBuyerTypeChange(selectedValue: any) {
    this.lookupService.getBuyerLookupByBuyerType(selectedValue.id).subscribe({
      next: result => {
        this.buyerOptions = result?.items || [];
        this.service.form.get('buyer').setValue('');
      },
      error: error => {
        console.error('Error loading buyers:', error);
      },
    });
  }
  onBuyerChange(selectedValue: any) {
    this.service.form.get('buyerName').setValue(selectedValue.displayName);
    this.service.form.get('buyerId').setValue(selectedValue.id);
    this.service.form.get('buyerCode').setValue(selectedValue.displayCode);
  }

  fiscalYears: number[] = [];
  generateFiscalYears(): void {
    this.fiscalYears = [];

    const now = new Date();
    const currentCalendarYear = now.getFullYear();
    const currentMonth = now.getMonth();

    const currentFiscalYear = currentMonth < 3 ? currentCalendarYear - 1 : currentCalendarYear;

    for (let year = currentFiscalYear - 5; year <= currentFiscalYear + 1; year++) {
      this.fiscalYears.push(year);
    }

    this.fiscalYears.sort((a, b) => b - a);
  }
}
