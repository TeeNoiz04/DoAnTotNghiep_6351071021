import { Directive, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ToasterService } from '@abp/ng.theme.shared';
import { LookupService } from '@proxy/general-lookups';
import { LookupDto } from '@proxy/shared';
import { InventoryReportViewService } from '@app/stock-management/services/inventory-report.service';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';

export const ChildTabDependencies = [];

export const ChildComponentDependencies = [];

@Directive()
export abstract class AbstractInventoryReportComponent implements OnInit {
  protected readonly service = inject(InventoryReportViewService);
  protected readonly toast = inject(ToasterService);
  protected readonly fb = inject(FormBuilder);
  protected readonly lookupService = inject(LookupService);

  title = 'Inventory Report';

  filterForm: FormGroup;
  materialGroupOptions: LookupDto<string>[] = [];

  get data() {
    return this.service.data;
  }

  constructor() {
    this.buildForm();
  }

  ngOnInit(): void {
    this.service.list.maxResultCount = DEFAULT_PAGE_SIZE;
    this.service.hookToQuery();
    this.loadMaterialGroups();
  }

  private buildForm(): void {
    this.filterForm = this.fb.group({
      materialCode: [''],
      inventoryCategory: [''],
      materialGroup: [''],
    });
  }

  private loadMaterialGroups(): void {
    this.lookupService.getMaterialGroupLookup().subscribe({
      next: result => {
        this.materialGroupOptions = result.items || [];
      },
      error: () => {
        this.toast.error('Failed to load material groups');
      },
    });
  }

  onSearch(): void {
    const formValue = this.filterForm.value;
    this.service.search({
      materialCode: formValue.materialCode || undefined,
      inventoryCategory: formValue.inventoryCategory || undefined,
      materialGroup: formValue.materialGroup || undefined,
    });
  }

  onClear(): void {
    this.filterForm.reset();
    this.service.clearFilters();
  }

  onExport(): void {
    const formValue = this.filterForm.value;
    this.service.exportToExcel({
      materialCode: formValue.materialCode || undefined,
      inventoryCategory: formValue.inventoryCategory || undefined,
      materialGroup: formValue.materialGroup || undefined,
    });
  }
}
