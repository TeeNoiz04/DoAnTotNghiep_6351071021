import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule, ToasterService } from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output, inject } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { DPOViewService } from '@app/dpos/services/dpo.service';

import { InputNumberComponent } from '@app/shared/components/input-number/input-number.component';
import { EscCloseModalDirective } from '@app/shared/esc-close-modal/esc-close-modal.directive';
import { NumberHelper } from '@app/shared/helpers/number-helper';
import { StatusLabelComponent } from '@app/shared/status/components/status-label.component';
import { NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { DPOService } from '@proxy/dpos';
import { DPODetailDto } from '@proxy/dpos/dpodetails';

import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-edit-dpo-item-dialog',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgbModalModule,
    CoreModule,
    ThemeSharedModule,
    StatusLabelComponent,
    InputNumberComponent,
    EscCloseModalDirective,
  ],
  templateUrl: './edit-dpo-item-dialog.component.html',
  styleUrls: ['./edit-dpo-item-dialog.component.scss'],
})
export class EditDPOItemDialogComponent implements OnInit {
  @Input() isVisible = false;
  @Input() DPODetail: DPODetailDto;

  @Output() visibleChange = new EventEmitter<boolean>();
  @Output() itemUpdated = new EventEmitter<void>();

  private readonly proxyService = inject(DPOService);
  private readonly toasterService = inject(ToasterService);
  private readonly fb = inject(FormBuilder);

  isBusy = false;
  form: FormGroup;

  ngOnInit(): void {
    this.buildForm();
    this.patchForm();
  }

  buildForm(): void {
    this.form = this.fb.group({
      qty: [{ value: '' }, Validators.required],
      materialCode: [{ value: '', disabled: true }, []],
      model: [{ value: null, disabled: true }, []],
      confirmNote: ['', Validators.maxLength(500)],
      note: ['', Validators.maxLength(500)],
    });
  }

  patchForm(): void {
    if (!this.DPODetail) return;

    // let price = this.DPODetail.price || 0;
    // if (typeof price === 'string') {
    //   price = parseFloat((price as string).replace(/,/g, ''));
    // }

    this.form?.patchValue({
      qty: this.DPODetail?.qty || '0',
      materialCode: this.DPODetail?.golfaCode || '',
      model: this.DPODetail?.model || '',

      confirmNote: this.DPODetail?.confirmNoted || '',
      note: this.DPODetail?.note || '',
    });
  }

  closeDialog(): void {
    this.isVisible = false;
    this.visibleChange.emit(false);
    this.form.reset();
  }

  onSave(): void {
    if (this.form.invalid) {
      Object.keys(this.form.controls).forEach(key => {
        const control = this.form.get(key);
        control.markAsTouched();
      });
      this.toasterService.warn('Please correct the form errors before saving', 'Validation Error');
      return;
    }

    this.isBusy = true;

    const formValue = this.form.value;

    const numericQty = NumberHelper.convertToNumber(formValue.qty, {
      defaultValue: 0,
      allowNegative: false,
      decimalPlaces: 0,
    });

    if (numericQty <= 0) {
      this.toasterService.warn('Quantity must be greater than 0', 'Validation Error');
      this.isBusy = false;
      return;
    }

    this.proxyService
      .updateDPODetail(this.DPODetail.id, numericQty, formValue.confirmNote, formValue.note)
      .pipe(finalize(() => (this.isBusy = false)))
      .subscribe({
        next: result => {
          this.toasterService.success('DPO item updated successfully', 'Success');
          this.itemUpdated.emit();
          this.closeDialog();
        },
        error: error => {
          console.error('Error updating dpo item:', error);
          this.toasterService.error('Failed to update dpo item', 'Error');
        },
      });
  }
}
