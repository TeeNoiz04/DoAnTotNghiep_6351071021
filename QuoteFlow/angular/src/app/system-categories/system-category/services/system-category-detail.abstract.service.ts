import { ListService, TrackByService } from '@abp/ng.core';
import { inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { finalize, tap } from 'rxjs/operators';

import { FormMode } from '@app/shared/enums/form-mode';
import { FormGroupHelper } from '@app/shared/helpers/form-group-helper';
import { CategoryTypes } from '@app/system-categories/system-category.model';
import type {
  SystemCategoryCreateDto,
  SystemCategoryDto,
  SystemCategoryUpdateDto,
} from '../../../proxy/system-categories/models';
import { SystemCategoryService } from '../../../proxy/system-categories/system-category.service';

export abstract class AbstractSystemCategoryDetailViewService {
  protected readonly fb = inject(FormBuilder);
  protected readonly track = inject(TrackByService);

  public readonly proxyService = inject(SystemCategoryService);
  public readonly list = inject(ListService);

  isBusy = false;
  isVisible = false;
  selected = {} as any;
  form: FormGroup | undefined;

  protected createRequest() {
    const formValues = {
      ...this.form.getRawValue(),
    };

    if (this.selected) {
      const updateParam: SystemCategoryUpdateDto = {
        parentId: formValues.parentId,
        description: formValues.description,
        categoryType: formValues.categoryType,
        value: parseFloat(formValues.value?.replace(/,/g, '')),
        note: formValues.note,
        isDeactive: formValues.isDeactive,
        concurrencyStamp: this.selected.concurrencyStamp,
      };

      return this.proxyService.update(this.selected.id, updateParam);
    }

    const createParam: SystemCategoryCreateDto = {
      parentId: formValues.parentId,
      code: formValues.code,
      description: formValues.description,
      value: parseFloat(formValues.value?.replace(/,/g, '')),
      note: formValues.note,
      categoryType: formValues.categoryType,
    };
    return this.proxyService.create(createParam);
  }

  private buildCommonFields(selected: any) {
    return {
      code: [selected?.code ?? null, [Validators.required, Validators.maxLength(50)]],
      description: [
        selected?.description ?? null,
        [Validators.required, Validators.maxLength(500)],
      ],
      note: [selected?.note ?? null, [Validators.maxLength(4000)]],
      isDeactive: [selected?.isDeactive ?? false, []],
      categoryType: [selected?.categoryType ?? null, []],
    };
  }

  buildForm(mode: FormMode = FormMode.New, type?: CategoryTypes) {
    const { parentId, value } = this.selected || {};

    const commonFields = this.buildCommonFields(this.selected);
    if (type === CategoryTypes.BuyerType) {
      this.form = this.fb.group({
        ...commonFields,
      });
    } else {
      this.form = this.fb.group({
        ...commonFields,
        parentId: [parentId ?? null, []],
        value: [value ?? null, []],
      });
    }

    if (mode === FormMode.Edit) {
      FormGroupHelper.toggleFormControls(this.form.get('code'), true);
    }
  }

  showForm(type?: CategoryTypes, mode: FormMode = FormMode.New) {
    this.buildForm(mode, type);
    this.isVisible = true;
  }

  create(type?: CategoryTypes) {
    this.selected = undefined;
    this.showForm(type);
  }

  update(record: SystemCategoryDto, type?: CategoryTypes) {
    this.selected = record;
    this.showForm(type, FormMode.Edit);
  }

  hideForm() {
    this.isVisible = false;
  }

  submitForm(type?: CategoryTypes) {
    if (this.form.invalid) return;

    this.form.patchValue({
      categoryType: type,
    });

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
