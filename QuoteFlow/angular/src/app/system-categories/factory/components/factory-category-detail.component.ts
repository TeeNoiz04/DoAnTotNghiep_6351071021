import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule, DateAdapter, TimeAdapter } from '@abp/ng.theme.shared';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { ChangeDetectionStrategy, Component, inject, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import {
  NgbNavModule,
  NgbDatepickerModule,
  NgbTimepickerModule,
  NgbDateAdapter,
  NgbTimeAdapter,
} from '@ng-bootstrap/ng-bootstrap';
import { SystemCategoryDetailViewService } from '@app/system-categories/system-category/services/system-category-detail.service';
import { CategoryTypes } from '@app/system-categories/system-category.model';
import { FactoryDetailViewService } from '../services/factory-detail.service';
import { LookupService } from '@proxy/general-lookups';
import { NgSelectModule } from '@ng-select/ng-select';

@Component({
  selector: 'app-factory-detail-modal',
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    ReactiveFormsModule,
    NgbDatepickerModule,
    NgbTimepickerModule,
    NgbNavModule,
    NgSelectModule,
    MatSlideToggleModule,
  ],
  providers: [
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './factory-category-detail.component.html',
  styles: [],
})
export class FactoryDetailModalComponent implements OnInit {
  public readonly service = inject(FactoryDetailViewService);
  public readonly proxyLookupService = inject(LookupService);

  supplierOptions: { value: string; code: string; label: string }[] = [];
  materialTypeOptions: { code: string; label: string }[] = [];
  currencyOptions: { code: string; label: string }[] = [];
  ngOnInit(): void {
    this.supplier();
    this.materialType();
    this.currency();
  }
  supplier() {
    this.proxyLookupService.getSupplierLookup().subscribe(result => {
      this.supplierOptions = result.items.map(item => ({
        value: item.id,
        code: item.displayCode,
        label: item.displayName,
      }));
    });
  }
  materialType() {
    this.proxyLookupService.getMaterialTypeLookup().subscribe(result => {
      this.materialTypeOptions = result.items.map(item => ({
        code: item.displayCode,
        label: item.displayName,
      }));
    });
  }
  currency() {
    this.proxyLookupService.getCurrencyCategoryLookup().subscribe(result => {
      this.currencyOptions = result.items.map(item => ({
        code: item.displayCode,
        label: item.displayName,
      }));
    });
  }
  onSupplierChange(event: any) {
    this.service.form.patchValue({
      supplierId: event?.id,
      supplierCode: event?.displayCode,
      supplierShortName: event?.displayName,
    });
  }
  submitForm() {
    this.service.submitForm();
  }
}
