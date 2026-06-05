import { PermissionService } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { AppPermissions } from '@app/app.permissions';
import {
  AppAdvancedDataTableComponent,
  AppTableColumnDirective,
  AppTableColumnGroupDirective,
} from '@app/shared/components/advanced-data-table';
import { InputNumberComponent } from '@app/shared/components/input-number/input-number.component';
import { MetricProgressComponent } from '@app/shared/components/metrics/metric-progress/metric-progress.component';
import { EscCloseModalDirective } from '@app/shared/esc-close-modal/esc-close-modal.directive';
import { RequestStatusEnum } from '@app/shared/status/components/status-label.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { SaleOrderService } from '@proxy/sale-orders';
import { SODetailExtrafeeUpdateInput } from '@proxy/sale-orders/models';
import { SaleOrderDetailDto } from '@proxy/sale-orders/sale-order-details';

@Component({
  selector: 'app-extra-fees-info-modal',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ThemeSharedModule,
    NgSelectModule,
    AppAdvancedDataTableComponent,
    AppTableColumnDirective,
    AppTableColumnGroupDirective,
    InputNumberComponent,
    MetricProgressComponent,
    ReactiveFormsModule,
    EscCloseModalDirective,
  ],
  templateUrl: './extra-fees-info-modal.component.html',
  styleUrls: ['./extra-fees-info-modal.component.scss'],
})
export class ExtraFeesInfoModalSOComponent implements OnInit {
  protected readonly permissionService = inject(PermissionService);
  @Input() visible: boolean = false;
  @Input() soDetail: SaleOrderDetailDto | null = null;
  @Input() soId: string = '';
  @Input() soDetailId: string = '';

  @Output() visibleChange = new EventEmitter<boolean>();
  @Output() itemUpdated = new EventEmitter<any>();

  loading = false;
  isBusy = false;

  // Table data
  deliveredData: any = [];
  tableLoading = false;

  get isItemFinalized(): boolean {
    return (
      this.soDetail?.statusCode === RequestStatusEnum.CANCELLED ||
      this.soDetail?.statusCode === RequestStatusEnum.CLOSED
    );
  }
  private readonly fb = inject(FormBuilder);
  private readonly service = inject(SaleOrderService);
  form: FormGroup;

  ngOnInit(): void {
    this.buildForm();
    this.patchForm();
  }

  buildForm(): void {
    this.form = this.fb.group(
      {
        soExtraFee: [{ value: null, disabled: !this.canEditExtraFee }, Validators.required],
        soCurrentExtraFee: [{ value: null, disabled: true }],
        noteSO: [{ value: '', disabled: !this.canEditExtraFee }, Validators.required],

        extrafeeDPO: [{ value: null, disabled: true }],
        extrafeeAvailable: [{ value: null, disabled: true }],
        noteDPO: [{ value: '', disabled: true }],
      },
      {
        validators: this.validateExtraFee(this.soDetail),
      },
    );
  }

  validateExtraFee(soDetail: SaleOrderDetailDto | null): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!soDetail) return null;

      const soExtraFeeControl = control.get('soExtraFee');
      if (!soExtraFeeControl) return null;

      // Normalize input: remove commas, trim
      let rawValue = soExtraFeeControl.value;
      if (typeof rawValue === 'string') {
        rawValue = rawValue.replace(/,/g, '').trim();
      }

      const soExtraFee = parseFloat(rawValue) || 0;

      const existingExtraFee = parseFloat((soDetail.extrafee ?? '0').toString().replace(/,/g, '')) || 0;

      const dpoExtraFee = parseFloat((soDetail.dpoDetail?.extrafee ?? '0').toString().replace(/,/g, '')) || 0;

      const totalSoExtraFees = soExtraFee + existingExtraFee;

      if (totalSoExtraFees > dpoExtraFee) {
        return {
          extraFeeExceeded: {
            total: totalSoExtraFees,
            max: soDetail.dpoDetail?.extrafeeAvailable ?? 0,
            current: soExtraFee,
            existing: existingExtraFee,
          },
        };
      }

      return null;
    };
  }

  patchForm(): void {
    if (!this.soDetail) return;

    this.form?.patchValue({
      // SO Information
      soExtraFee: null,
      soCurrentExtraFee: this.soDetail?.extrafee,
      noteSO: this.soDetail?.extrafee_Note || '',

      // DPO Information
      extrafeeDPO: this.soDetail?.dpoDetail?.extrafee || 0,
      extrafeeAvailable: this.soDetail?.dpoDetail?.extrafeeAvailable || 0,
      noteDPO: this.soDetail?.dpoDetail?.extrafeeNote || '',
    });
  }

  onCancel(): void {
    this.closeModal();
  }

  closeModal(): void {
    this.visible = false;
    this.visibleChange.emit(false);
  }

  onClose(): void {
    this.closeModal();
  }

  onSave(): void {
    const input = {} as SODetailExtrafeeUpdateInput;
    input.soDetailId = this.soDetail?.id;
    var inputExtrafee = parseFloat((this.form.get('soExtraFee')?.value ?? '0').toString().replace(/,/g, '')) || 0;
    input.extrafee = inputExtrafee;
    input.extrafeeNode = this.form.get('noteSO')?.value || '';

    this.service.updateSODetailExtrafee(input).subscribe({
      next: resp => {
        this.itemUpdated.emit(input);
        this.closeModal();
      },
      error: () => {},
    });
  }

  get canEditExtraFee(): boolean {
    return (
      this.permissionService.getGrantedPolicy(AppPermissions.SaleOrders.AdjustDetailExtraFee) && !this.isItemFinalized
    );
  }
}
