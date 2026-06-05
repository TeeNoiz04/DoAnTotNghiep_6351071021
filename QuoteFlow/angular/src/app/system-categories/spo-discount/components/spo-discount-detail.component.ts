import { CoreModule, TrackCapsLockDirective } from '@abp/ng.core';
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
import { EscCloseModalDirective } from '@app/shared/esc-close-modal/esc-close-modal.directive';
import { SPODiscountDetailViewService } from '../services/spo-discount-detail.service';

@Component({
  selector: 'app-spo-discount-detail-modal',
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
    TrackCapsLockDirective,
  ],
  providers: [
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './spo-discount-detail.component.html',
  styleUrls: ['./spo-discount-detail.component.scss'],
})
export class SpoDiscountDetailModalComponent implements OnInit {
  public readonly service = inject(SPODiscountDetailViewService);
  public readonly lookupService = inject(LookupService);
  // protected readonly fb = inject(FormBuilder);

  loading = false;

  // projectTypeOption: { value: string; label: string }[] = [
  //   { value: 'FA', label: 'FA' },
  //   { value: 'LVS', label: 'LVS' },
  // ];
  // buyerOptions: LookupDto<string>[] = [];
  // buyerTypeOptions: LookupDto<string>[] = [];

  ngOnInit(): void {
    console.log();
  }

  // isInvalid(controlName: string): boolean {
  //   const control = this.service.form?.get(controlName);
  //   return control ? control.invalid && (control.dirty || control.touched) : false;
  // }

  // hasError(controlName: string, errorName: string): boolean {
  //   const control = this.service.form?.get(controlName);
  //   return control ? control.hasError(errorName) && (control.dirty || control.touched) : false;
  // }

  // getControl(controlName: string) {
  //   return this.service.form?.get(controlName);
  // }

  validateFromTo(): boolean {
    const toNumber = (value: any) => (value != null && value !== '' ? Number(String(value).replace(/,/g, '')) : null);
    const min = toNumber(this.service.form?.get('value_Min')?.value);
    const max = toNumber(this.service.form?.get('value_Max')?.value);

    if (max != null && min != null && min >= max) {
      return true;
    }
    return false;
  }
  // getBuyer() {
  //   this.lookupService
  //     .getBuyerLookup(false)
  //     // .pipe(takeUntil(this.destroy$))
  //     .subscribe({
  //       next: result => {
  //         this.buyerOptions = result?.items || [];
  //       },
  //       error: error => {
  //         console.error('Error loading buyers:', error);
  //       },
  //     });
  // }
  // getBuyerType() {
  //   this.lookupService
  //     .getBuyerTypeLookup()
  //     // .pipe(takeUntil(this.destroy$))
  //     .subscribe({
  //       next: result => {
  //         this.buyerTypeOptions = result?.items || [];
  //       },
  //       error: error => {
  //         console.error('Error loading buyers:', error);
  //       },
  //     });
  // }
  // onBuyerTypeChange(selectedValue: any) {
  //   this.lookupService.getBuyerLookupByBuyerType(selectedValue.id).subscribe({
  //     next: result => {
  //       this.buyerOptions = result?.items || [];
  //       this.service.form.get('buyer').setValue('');
  //     },
  //     error: error => {
  //       console.error('Error loading buyers:', error);
  //     },
  //   });
  // }
  // onBuyerChange(selectedValue: any) {
  //   this.service.form.get('buyerName').setValue(selectedValue.displayName);
  //   this.service.form.get('buyerId').setValue(selectedValue.id);
  //   this.service.form.get('buyerCode').setValue(selectedValue.displayCode);
  // }

  // fiscalYears: number[] = [];
  // generateFiscalYears(): void {
  //   const currentYear = new Date().getFullYear();
  //   for (let year = currentYear - 1; year <= currentYear + 5; year++) {
  //     this.fiscalYears.push(year);
  //   }
  //   this.fiscalYears.sort((a, b) => b - a);
  // }
}
