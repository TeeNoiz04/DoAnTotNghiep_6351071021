import { ThemeSharedModule, ToasterService } from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { InputNumberComponent } from '@app/shared/components/input-number/input-number.component';
import { NumberHelper } from '@app/shared/helpers/number-helper';
import { RequestStatusEnum } from '@app/shared/status/components/status-label.component';
import { DPOAddExtraFeeDto, DPOService } from '@proxy/dpos';
import { finalize } from 'rxjs';

export interface ExtraFeeResult {
  confirmed: boolean;
  extraFee?: number;
  note?: string;
}

@Component({
  selector: 'app-extra-fee-modal',
  standalone: true,
  imports: [CommonModule, FormsModule, ThemeSharedModule, InputNumberComponent, ReactiveFormsModule],
  templateUrl: './extra-fee-modal.component.html',
  styleUrls: ['./extra-fee-modal.component.scss'],
})
export class ExtraFeeModalComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly toasterService = inject(ToasterService);
  private readonly service = inject(DPOService);

  @Input() visible: boolean = false;
  @Input() selectedItemsCount: number = 0;
  @Input() selectedItems: any[] = [];
  @Input() dpoId: string = '';

  @Output() visibleChange = new EventEmitter<boolean>();
  @Output() modalResult = new EventEmitter<ExtraFeeResult>();

  protected loading = false;
  protected isBusy = false;
  protected form: FormGroup;

  ngOnInit(): void {
    this.buildForm();
  }

  buildForm(): void {
    this.form = this.fb.group({
      extraFee: [null, [Validators.required]],
      note: ['', Validators.maxLength(500)],
    });
  }

  get hasAnyFinalizedItems(): boolean {
    return (
      this.selectedItems?.some(
        item => item.status === RequestStatusEnum.CANCELLED || item.status === RequestStatusEnum.CLOSED,
      ) || false
    );
  }

  // get isValidExtraFee(): boolean {
  //   if (this.extraFee.trim().length === 0) return false;
  //   const numericExtraFee = NumberHelper.convertToNumber(this.extraFee, { defaultValue: -1, allowNegative: false });
  //   return numericExtraFee >= 0;
  // }

  // getNumericExtraFee(): number {
  //   return NumberHelper.convertToNumber(this.extraFee, { defaultValue: 0, allowNegative: false });
  // }

  onSubmit(): void {
    if (this.form.invalid) {
      Object.keys(this.form.controls).forEach(key => {
        const control = this.form.get(key);
        control.markAsTouched();
      });
      this.toasterService.warn('Please correct the form errors before saving', 'Validation Error');
      return;
    }

    this.isBusy = true;
    const formValue = this.form.getRawValue();

    const numericExtraFee = NumberHelper.convertToNumber(formValue?.extraFee, {
      defaultValue: 0,
      allowNegative: true,
      decimalPlaces: 0,
    });

    const dpoDetailIds = this.selectedItems?.map(item => item.id);

    const updateDto: DPOAddExtraFeeDto = {
      dpoDetailIds: dpoDetailIds,
      extraFee: numericExtraFee,
      extraFeeNote: formValue.note,
    };

    this.service
      .addExtraFee(updateDto)
      .pipe(finalize(() => (this.isBusy = false)))
      .subscribe({
        next: () => {
          this.toasterService.success('Extra fee successfully', 'Success');
          this.modalResult.emit();
          this.closeDialog();
        },
        error: error => {
          console.error('Error updating extra fee:', error);
          this.toasterService.error('Failed to update extra fee', 'Error');
        },
      });
  }

  closeDialog(): void {
    this.visible = false;
    this.form.reset();
    this.visibleChange.emit(false);
  }
}
