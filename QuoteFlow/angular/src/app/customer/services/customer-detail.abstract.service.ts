import { inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ListService, PagedResultDto, TrackByService } from '@abp/ng.core';

import { finalize, map, tap } from 'rxjs/operators';

import { FormMode } from '@app/shared/enums/form-mode';
import { FormGroupHelper } from '@app/shared/helpers/form-group-helper';
import { GetPriceFrom, GetPriceFromOptions } from './get-price-from';
import { CustomerDto, CustomerService } from '@proxy/customers';
import {
  GetSystemCategoriesInput,
  SystemCategoryDto,
  SystemCategoryService,
} from '@proxy/system-categories';
import { SystemCategories } from '@proxy';
import { CategoryTypes } from '@app/system-categories/system-category.model';
import { Observable } from 'rxjs';

export abstract class AbstractCustomerDetailViewService {
  protected readonly fb = inject(FormBuilder);
  protected readonly track = inject(TrackByService);

  public readonly proxyService = inject(CustomerService);
  protected readonly proxySystemCategoryService = inject(SystemCategoryService);
  public readonly list = inject(ListService);

  isBusy = false;
  isVisible = false;
  selected = {} as any;
  form: FormGroup | undefined;

  public getPriceFromOptions = GetPriceFromOptions;

  provinceOptions = [
    { label: 'An Giang', value: 'An Giang' },
    { label: 'Bắc Ninh', value: 'Bắc Ninh' },
    { label: 'Cần Thơ', value: 'Cần Thơ' },
    { label: 'Cao Bằng', value: 'Cao Bằng' },
    { label: 'Cà Mau', value: 'Cà Mau' },
    { label: 'Đà Nẵng', value: 'Đà Nẵng' },
    { label: 'Đắk Lắk', value: 'Đắk Lắk' },
    { label: 'Điện Biên', value: 'Điện Biên' },
    { label: 'Đồng Nai', value: 'Đồng Nai' },
    { label: 'Đồng Tháp', value: 'Đồng Tháp' },
    { label: 'Gia Lai', value: 'Gia Lai' },
    { label: 'Hà Tĩnh', value: 'Hà Tĩnh' },
    { label: 'Hải Phòng', value: 'Hải Phòng' },
    { label: 'Hà Nội', value: 'Hà Nội' },
    { label: 'TP. Hồ Chí Minh', value: 'TP. Hồ Chí Minh' },
    { label: 'Huế', value: 'Huế' },
    { label: 'Hưng Yên', value: 'Hưng Yên' },
    { label: 'Khánh Hòa', value: 'Khánh Hoà' },
    { label: 'Lai Châu', value: 'Lai Châu' },
    { label: 'Lâm Đồng', value: 'Lâm Đồng' },
    { label: 'Lạng Sơn', value: 'Lạng Sơn' },
    { label: 'Lào Cai', value: 'Lào Cai' },
    { label: 'Nghệ An', value: 'Nghệ An' },
    { label: 'Ninh Bình', value: 'Ninh Bình' },
    { label: 'Phú Thọ', value: 'Phú Thọ' },
    { label: 'Quảng Ngãi', value: 'Quảng Ngãi' },
    { label: 'Quảng Ninh', value: 'Quảng Ninh' },
    { label: 'Quảng Trị', value: 'Quảng Trị' },
    { label: 'Sơn La', value: 'Sơn La' },
    { label: 'Tây Ninh', value: 'Tây Ninh' },
    { label: 'Thanh Hóa', value: 'Thanh Hóa' },
    { label: 'Thái Nguyên', value: 'Thái Nguyên' },
    { label: 'Tuyên Quang', value: 'Tuyên Quang' },
    { label: 'Vĩnh Long', value: 'Vĩnh Long' },
  ];

  protected createRequest() {
    const formValues = {
      ...this.form.getRawValue(),
    };

    if (this.selected) {
      return this.proxyService.update(this.selected.id, {
        ...formValues,

        concurrencyStamp: this.selected.concurrencyStamp,
      });
    }
    return this.proxyService.create(formValues);
  }

  buildForm(mode: FormMode = FormMode.New) {
    const {
      taxCode,
      customerName,
      customerShortName,
      country,
      isDeactive,
      address,
      website,
      phone,
      province,
      customerType,
      customerIndustry,
      note,
    } = this.selected || {};

    this.form = this.fb.group({
      taxCode: [taxCode ?? null, Validators.required],
      customerName: [customerName ?? null, Validators.required],
      // customerShortName: [customerShortName ?? null, Validators.required],
      country: [country ?? null, Validators.required],
      isDeactive: [isDeactive ?? false], // checkbox boolean
      address: [address ?? null],
      website: [website ?? null],
      phone: [phone ?? null],
      province: [province ?? null],
      customerType: [customerType ?? null],
      customerIndustry: [customerIndustry ?? null],
      note: [note ?? null],
    });
    if (this.selected) {
      this.form.get('taxCode')?.disable();
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

  update(record: CustomerDto) {
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
}
