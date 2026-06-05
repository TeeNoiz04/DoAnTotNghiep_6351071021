import { ListService, TrackByService } from '@abp/ng.core';
import { inject } from '@angular/core';

import { FormMode } from '@app/shared/enums/form-mode';
import { FormGroupHelper } from '@app/shared/helpers/form-group-helper';
import { SpoDiscountViewService } from './spo-discount.service';
import { CfgDiscountRatioDto, CfgDiscountRatioService } from '@proxy/cfg-discount-ratios';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { finalize, Observable, tap } from 'rxjs';
import { ToasterService } from '@abp/ng.theme.shared';

export abstract class AbstractSpoDiscountDetailViewService {
  protected readonly fb = inject(FormBuilder);
  protected readonly track = inject(TrackByService);
  public readonly proxyService = inject(CfgDiscountRatioService);
  public readonly list = inject(ListService);
  public readonly service = inject(SpoDiscountViewService);
  public readonly toast = inject(ToasterService);

  isBusy = false;
  isVisible = false;
  selected = {} as any;

  form: FormGroup | undefined;
  public buyerShortName: string = '';

  selectedMaterials = {
    fa: false,
    lvs: false,
  };

  public initializeForm(): void {
    const isDisabled = !!this.selected;

    this.form = this.fb.group({
      kaType: [{ value: null, disabled: isDisabled }],

      accountClassify: [{ value: null, disabled: isDisabled }],
      value_Min: [null],
      value_Max: [null],
      discountRatio: [null, [Validators.required]],
      note: [null],
    });
  }

  protected createRequest() {
    const formValues = {
      ...this.form.getRawValue(),
    };

    const toNumber = (value: any) =>
      value != null && value !== '' ? Number(String(value).replace(/,/g, '')) : null;

    if (this.selected) {
      return this.proxyService.update(this.selected.id, {
        approval_Type: formValues.approval_Type ?? null,
        product_Type: formValues.product_Type ?? null,
        accountClassify: formValues.accountClassify ?? null,
        value_Min: toNumber(formValues.value_Min) ?? null,
        value_Max: toNumber(formValues.value_Max) ?? null,
        discountRatio: toNumber(formValues.discountRatio),
        note: formValues.note,
        concurrencyStamp: this.selected.concurrencyStamp,
      });
    }

    // const postData = {
    //   buyerTypeId: formValues.buyerType,
    //   buyerId: formValues.buyer?.id,
    //   buyerCode: formValues.buyer?.displayCode,
    //   buyerName: formValues.buyer?.displayName,
    //   materialType: formValues.materialType,
    //   firstFYTarget: toNumber(formValues.firstFYTarget),
    //   secondFYTarget: toNumber(formValues.secondFYTarget),
    //   financeYear: toNumber(formValues.financeYear),
    //   note: formValues.note,
    // } as DistributorTargetCreateDto;

    // return this.proxyService.create(postData);
  }

  buildForm(mode: FormMode = FormMode.New) {
    const { kaType, accountClassify, value_Min, value_Max, discountRatio, note } =
      this.selected || {};

    const isDisabled = !!this.selected;
    const isRequired = !this.isAP;

    this.form = this.fb.group({
      kaType: [{ value: kaType ?? null, disabled: isDisabled }],

      accountClassify: [{ value: accountClassify ?? null, disabled: isDisabled }],
      value_Min: [value_Min ?? null, isRequired ? [] : [Validators.required]],
      value_Max: [value_Max ?? null],
      discountRatio: [discountRatio ?? null, [Validators.required]],
      note: [note ?? null],
    });
  }

  get isAP(): boolean {
    return this.selected?.approval_Type !== 'AP';
  }

  showForm(mode: FormMode = FormMode.New) {
    this.buildForm(mode);
    this.isVisible = true;
  }

  create() {
    this.selected = undefined;
    this.showForm();
  }

  update(record: CfgDiscountRatioDto) {
    this.selected = record;

    this.showForm(FormMode.Edit);
  }

  hideForm() {
    this.isVisible = false;
  }

  submitForm() {
    if (this.form.invalid) {
      FormGroupHelper.markFormGroupTouched(this.form);

      return;
    }
    const toNumber = (value: any) =>
      value != null && value !== '' ? Number(String(value).replace(/,/g, '')) : null;
    const minValue = toNumber(this.form.get('value_Min')?.value);
    const maxValue = toNumber(this.form.get('value_Max')?.value);

    if (minValue >= maxValue && maxValue != null) {
      return;
    }

    this.isBusy = true;

    (this.createRequest() as Observable<any>)
      .pipe(
        finalize(() => {
          this.isBusy = false;
          this.isVisible = false;
        }),
        tap(() => {
          this.list.get();
        }),
      )
      .subscribe({
        next: () => {},
        error: err => {
          console.error('Error submitting form:', err);
        },
      });
  }

  changeVisible($event: boolean) {
    this.isVisible = $event;
  }

  get materialTypeInvalid() {
    return this.form?.errors?.['materialTypeRequired'] && (this.form?.touched || this.form?.dirty);
  }
}
