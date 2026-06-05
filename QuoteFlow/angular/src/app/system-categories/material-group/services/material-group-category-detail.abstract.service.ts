import { ListService, TrackByService } from '@abp/ng.core';
import { inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { finalize, tap } from 'rxjs/operators';

import { FormMode } from '@app/shared/enums/form-mode';
import { FormGroupHelper } from '@app/shared/helpers/form-group-helper';
import { LookupService } from '@proxy/general-lookups';
import { MaterialGroupService } from '@proxy/material-groups';
import {
  MaterialGroupCreateDto,
  MaterialGroupDto,
  MaterialGroupUpdateDto,
} from '@proxy/materials/material-groups';
import { LookupDto } from '@proxy/shared';

export abstract class AbstractMaterialGroupDetailViewService {
  protected readonly fb = inject(FormBuilder);
  protected readonly track = inject(TrackByService);
  protected readonly lookupService = inject(LookupService);

  public readonly proxyService = inject(MaterialGroupService);
  public readonly list = inject(ListService);

  isBusy = false;
  isVisible = false;
  selected = {} as any;
  form: FormGroup | undefined;
  materialTypeOptions = [
    { label: 'FA', value: 'FA' },
    { label: 'LVS', value: 'LVS' },
  ];
  materialGroupPSIOptions: LookupDto<string>[] = [];

  protected createRequest() {
    const formValues = {
      ...this.form.getRawValue(),
    };

    const item = {
      code: formValues.code,
      name: formValues.name,
      note: formValues.note,
      materialType: formValues.materialType,
      materialGroupPSI: formValues.materialGroupPSI,
      isDeActive: formValues.isDeActive,
      allowKeyAccount: formValues.allowKeyAccount,
      sortOrder: formValues.sortOrder,
    };

    if (this.selected) {
      const updateParam: MaterialGroupUpdateDto = {
        ...item,
        concurrencyStamp: this.selected.concurrencyStamp,
      };

      return this.proxyService.update(this.selected.id, updateParam);
    }

    const createParam: MaterialGroupCreateDto = {
      ...item,
    };
    return this.proxyService.create(createParam);
  }

  buildForm(mode: FormMode = FormMode.New) {
    this.materialGroupPSIOptions = [];
    const { code, name, materialType, materialGroupPSI, note, isDeActive, allowKeyAccount } =
      this.selected || {};

    if (materialType) {
      this.lookupService.getMaterialGroupPSILookup(materialType).subscribe({
        next: result => {
          this.materialGroupPSIOptions = result.items || [];
        },
        error: error => {
          console.error('Error loading suppliers:', error);
        },
      });
    }
    this.form = this.fb.group({
      code: [code ?? null, [Validators.required, Validators.maxLength(50)]],
      name: [name ?? null, [Validators.required, Validators.maxLength(500)]],
      materialType: [materialType ?? null, [Validators.required]],
      materialGroupPSI: [materialGroupPSI ?? null],
      note: [note ?? null, [Validators.maxLength(4000)]],
      isDeActive: [isDeActive ?? false, []],
      allowKeyAccount: [allowKeyAccount ?? false, []],
    });

    if (mode === FormMode.Edit) {
      FormGroupHelper.toggleFormControls(this.form.get('code'), true);
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

  update(record: MaterialGroupDto) {
    this.selected = record;
    this.showForm(FormMode.Edit);
  }

  hideForm() {
    this.isVisible = false;
  }

  submitForm() {
    if (this.form.invalid) return;

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

  onMaterialTypeChange(selectedValue: any) {
    this.form.get('materialGroupPSI').reset();
    if (!selectedValue || !selectedValue.value) {
      this.materialGroupPSIOptions = [];
      return;
    }
    this.lookupService.getMaterialGroupPSILookup(selectedValue.value).subscribe({
      next: result => {
        this.materialGroupPSIOptions = result.items || [];
      },
      error: error => {
        console.error('Error loading suppliers:', error);
      },
    });
  }
}
