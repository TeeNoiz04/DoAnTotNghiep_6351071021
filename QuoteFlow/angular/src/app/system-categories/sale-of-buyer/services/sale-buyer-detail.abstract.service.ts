import { ListService, TrackByService } from '@abp/ng.core';
import { inject } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { finalize, tap } from 'rxjs/operators';

import {
  SalesAssignmentCreateDto,
  SalesAssignmentDto,
  SalesAssignmentService,
} from '@proxy/sales-assignments';

import { FormMode } from '@app/shared/enums/form-mode';
import { FormGroupHelper } from '@app/shared/helpers/form-group-helper';

export abstract class AbstractSaleBuyerDetailViewService {
  protected readonly fb = inject(FormBuilder);
  protected readonly track = inject(TrackByService);
  public readonly proxyService = inject(SalesAssignmentService);
  public readonly list = inject(ListService);

  isBusy = false;
  isVisible = false;
  selected = {} as any;
  form: FormGroup | undefined;
  public buyerShortName: string = '';

  selectedMaterials = {
    fa: false,
    lvs: false,
  };

  constructor() {}

  private materialTypeRequiredValidator: ValidatorFn = (group: AbstractControl) => {
    const { fa, lvs } = this.selectedMaterials;
    return fa || lvs ? null : { materialTypeRequired: true };
  };

  protected createRequest() {
    const { fa, lvs } = this.selectedMaterials || { fa: false, lvs: false };

    const materialTypesArray = [];
    if (fa) materialTypesArray.push('FA');
    if (lvs) materialTypesArray.push('LVS');

    const materialType = materialTypesArray.length ? materialTypesArray.join(',') : null;

    const formValues = {
      ...this.form.getRawValue(),
      materialType, // use singular
    };

    if (this.selected) {
      return this.proxyService.update(this.selected.id, {
        ...formValues,
        buyerShortName: this.buyerShortName,
        materialType, // use singular
        concurrencyStamp: this.selected.concurrencyStamp,
      });
    }

    const postData = {
      buyerId: this.form.getRawValue().buyerId,
      locationId: this.form.getRawValue().locationId,
      materialType: materialType,
      buyerShortName: this.buyerShortName,
      buyerTypeId: this.form.getRawValue().buyerTypeId,
      users: this.form.getRawValue().saleUserName,
    } as SalesAssignmentCreateDto;

    return this.proxyService.create(postData);
  }

  buildForm(mode: FormMode = FormMode.New) {
    const { buyerId, saleUserName, locationId, buyerTypeId } = this.selected || {};

    this.form = this.fb.group(
      {
        buyerId: [buyerId ?? null, [Validators.required, Validators.maxLength(50)]],
        locationId: [locationId ?? null, [Validators.required, Validators.maxLength(50)]],
        saleUserName: [saleUserName ?? null, [Validators.required, Validators.maxLength(500)]],
        buyerTypeId: [buyerTypeId ?? null, [Validators.required, Validators.maxLength(50)]],
      },
      { validators: this.materialTypeRequiredValidator },
    );

    if (mode === FormMode.Edit) {
      FormGroupHelper.toggleFormControls(this.form.get('saleUserName'), true);
    }
  }

  showForm(mode: FormMode = FormMode.New) {
    this.buildForm(mode);
    this.isVisible = true;
  }

  create() {
    this.selected = undefined;
    this.showForm();
  }

  update(record: SalesAssignmentDto) {
    this.selected = record;

    // Parse and set material types from the existing record
    const materialTypes = record.materialType?.split(',') || [];

    this.selectedMaterials = {
      fa: materialTypes.includes('FA'),
      lvs: materialTypes.includes('LVS'),
    };

    this.showForm(FormMode.Edit);
  }

  hideForm() {
    this.isVisible = false;
  }

  submitForm() {
    if (this.form.invalid) return;

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
        next: () => {
          this.buyerShortName = null;
        },
        error: err => {
          console.error('Error submitting form:', err);
        },
      });
  }

  changeVisible($event: boolean) {
    this.isVisible = $event;
  }

  onMaterialChange(materialType: string, event: Event) {
    const checkbox = event.target as HTMLInputElement;
    this.selectedMaterials[materialType as keyof typeof this.selectedMaterials] = checkbox.checked;

    if (this.form) {
      this.form.updateValueAndValidity();
    }
  }

  get materialTypeInvalid() {
    return this.form?.errors?.['materialTypeRequired'] && (this.form?.touched || this.form?.dirty);
  }
}
