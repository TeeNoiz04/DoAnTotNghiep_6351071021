import { ListService, TrackByService } from '@abp/ng.core';
import { inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { finalize, tap } from 'rxjs/operators';

import { FormMode } from '@app/shared/enums/form-mode';
import { FormGroupHelper } from '@app/shared/helpers/form-group-helper';

import {
  SupplierCreateDto,
  SupplierDto,
  SupplierService,
  SupplierUpdateDto,
} from '@proxy/suppliers';

export abstract class AbstractVendorCategoryDetailViewService {
  protected readonly fb = inject(FormBuilder);
  protected readonly track = inject(TrackByService);

  public readonly proxyService = inject(SupplierService);
  public readonly list = inject(ListService);

  isBusy = false;
  isVisible = false;
  selected = {} as SupplierDto;
  form: FormGroup | undefined;

  protected createRequest() {
    const formValues = {
      ...this.form.getRawValue(),
    };

    if (this.selected) {
      const updateParam: SupplierUpdateDto = {
        supplierCode: formValues.supplierCode,
        sapCode: formValues.sapCode,
        shortName: formValues.shortName,
        fullName: formValues.fullName,
        taxCode: formValues.taxCode,
        address: formValues.address,
        isDeactive: formValues.isDeactive,
        concurrencyStamp: this.selected.concurrencyStamp,
      };

      return this.proxyService.update(this.selected.id, updateParam);
    }

    const createParam: SupplierCreateDto = {
      supplierCode: formValues.supplierCode,
      sapCode: formValues.sapCode,
      shortName: formValues.shortName,
      fullName: formValues.fullName,
      taxCode: formValues.taxCode,
      address: formValues.address,
      isDeactive: formValues.isDeactive,
    };
    return this.proxyService.create(createParam);
  }

  // private buildCommonFields(selected: any) {
  //   return {
  //     supplierCode: [selected?.supplierCode ?? null, [Validators.required, Validators.maxLength(50)]],
  //     shortName: [selected?.shortName ?? null, [Validators.required, Validators.maxLength(250)]],
  //     fullName: [selected?.fullName ?? null],
  //     taxCode: [selected?.taxCode ?? null, [Validators.maxLength(20)]],
  //     address: [selected?.address ?? null],
  //   };
  // }

  buildForm(mode: FormMode = FormMode.New) {
    const { id, supplierCode, sapCode, shortName, fullName, taxCode, address, isDeactive } =
      this.selected || {};

    // const commonFields = this.buildCommonFields(this.selected);
    // if (type === CategoryTypes.BuyerType) {
    // this.form = this.fb.group({
    //   ...commonFields,
    // });
    // } else {
    this.form = this.fb.group({
      // ...commonFields,
      supplierCode: [supplierCode ?? null, [Validators.required, Validators.maxLength(50)]],
      sapCode: [sapCode ?? null, [Validators.maxLength(50)]],
      shortName: [shortName ?? null, [Validators.required, Validators.maxLength(250)]],
      fullName: [fullName ?? null, [Validators.required, Validators.maxLength(250)]],
      taxCode: [taxCode ?? null, [Validators.maxLength(20)]],
      address: [address ?? null],
      isDeactive: [isDeactive ?? false],
    });
    // }

    if (mode === FormMode.Edit) {
      FormGroupHelper.toggleFormControls(this.form.get('supplierCode'), true);
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

  update(record: SupplierDto) {
    this.selected = record;
    this.showForm(FormMode.Edit);
  }

  hideForm() {
    this.isVisible = false;
  }

  submitForm() {
    if (this.form.invalid) return;

    this.form.patchValue({});

    this.isBusy = true;

    const request = this.createRequest().pipe(
      finalize(() => (this.isBusy = false)),
      tap(() => this.hideForm()),
    );

    request.subscribe(this.list.get);
  }

  changeVisible($event: boolean) {
    this.isVisible = $event;
  }
}
