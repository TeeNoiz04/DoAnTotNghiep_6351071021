import { ListService, TrackByService } from '@abp/ng.core';
import { inject } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { finalize, tap } from 'rxjs/operators';

import { FormMode } from '@app/shared/enums/form-mode';
import { FormGroupHelper } from '@app/shared/helpers/form-group-helper';
import {
  DistributorTargetCreateDto,
  DistributorTargetDto,
  DistributorTargetService,
} from '@proxy/distributor-targets';
import { DistributorTargetViewService } from './distributor-target.service';

export abstract class AbstractDistributorTargetDetailViewService {
  protected readonly fb = inject(FormBuilder);
  protected readonly track = inject(TrackByService);
  public readonly proxyService = inject(DistributorTargetService);
  public readonly list = inject(ListService);
  public readonly service = inject(DistributorTargetViewService);

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
    this.form = this.fb.group({
      buyerType: [{ value: '', disabled: this.selected }, [Validators.required]],
      buyer: [{ value: '', disabled: this.selected }, [Validators.required]],
      materialType: [{ value: '', disabled: this.selected }, [Validators.required]],
      firstFYTarget: ['', [Validators.required]],
      secondFYTarget: ['', [Validators.required]],
      financeYear: ['', [Validators.required]],
      note: [''],
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
        buyerTypeId: formValues.buyerType,
        buyerId: formValues.buyerId,
        buyerCode: formValues.buyerCode,
        buyerName: formValues.buyerName,
        materialType: formValues.materialType,
        firstFYTarget: toNumber(formValues.firstFYTarget),
        secondFYTarget: toNumber(formValues.secondFYTarget),
        financeYear: toNumber(formValues.financeYear),
        note: formValues.note,
        concurrencyStamp: this.selected.concurrencyStamp,
      });
    }

    const postData = {
      buyerTypeId: formValues.buyerType,
      buyerId: formValues.buyer?.id,
      buyerCode: formValues.buyer?.displayCode,
      buyerName: formValues.buyer?.displayName,
      materialType: formValues.materialType,
      firstFYTarget: toNumber(formValues.firstFYTarget),
      secondFYTarget: toNumber(formValues.secondFYTarget),
      financeYear: toNumber(formValues.financeYear),
      note: formValues.note,
    } as DistributorTargetCreateDto;

    return this.proxyService.create(postData);
  }

  buildForm(mode: FormMode = FormMode.New) {
    const {
      buyerTypeId,
      buyerCode,
      buyerId,
      buyerName,
      materialType,
      firstFYTarget,
      secondFYTarget,
      financeYear,
      note,
    } = this.selected || {};

    this.form = this.fb.group({
      buyerType: [{ value: buyerTypeId ?? null, disabled: this.selected }, [Validators.required]],
      buyerName: [{ value: buyerName ?? null, disabled: this.selected }, [Validators.required]],
      buyerCode: [{ value: buyerCode ?? null, disabled: this.selected }, [Validators.required]],
      buyerId: [{ value: buyerId ?? null, disabled: this.selected }, [Validators.required]],
      buyer: [{ value: buyerCode ?? null, disabled: this.selected }, [Validators.required]],
      materialType: [
        { value: materialType ?? null, disabled: this.selected },
        [Validators.required],
      ],
      firstFYTarget: [firstFYTarget ?? null, [Validators.required]],
      secondFYTarget: [secondFYTarget ?? null, [Validators.required]],
      financeYear: [financeYear ?? null, [Validators.required]],
      note: [note ?? null],
    });
  }

  showForm(mode: FormMode = FormMode.New) {
    this.buildForm(mode);
    this.isVisible = true;
  }

  create() {
    this.selected = undefined;
    this.showForm();
  }

  update(record: DistributorTargetDto) {
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

    this.isBusy = true;

    (this.createRequest() as Observable<any>)
      .pipe(
        finalize(() => {
          this.isBusy = false;
          this.isVisible = false;
        }),
        tap(() => {
          this.list.get();
          this.service.generateFiscalYears();
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
