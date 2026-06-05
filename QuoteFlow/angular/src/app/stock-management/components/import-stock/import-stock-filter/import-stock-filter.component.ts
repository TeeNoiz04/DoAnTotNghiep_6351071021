import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CoreModule } from '@abp/ng.core';
import { NgSelectModule } from '@ng-select/ng-select';
import { ImportStockViewService } from '@app/stock-management/services/import-stock.service';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { LookupService } from '@proxy/general-lookups';
import { LookupDto } from '@proxy/shared';
import { RequestStatusEnum } from '@app/shared/status/components/status-label.component';
import { ImportMaterialOptions } from '../import-stock.type';
import { ExpandablePanelV2Component } from '@app/shared/components/expandable-panel-v2/expandable-panel-v2.component';
import { TrimDirective } from '@app/shared/directives/trim.directive';

@Component({
  selector: 'app-import-stock-filter',
  templateUrl: './import-stock-filter.component.html',
  standalone: true,
  styleUrls: ['./import-stock-filter.component.scss'],
  imports: [
    FormsModule,
    NgbModule,
    NgSelectModule,
    CoreModule,
    MatCheckboxModule,
    ExpandablePanelV2Component,
    TrimDirective,
  ],
})
export class ImportStockFilterComponent implements OnInit {
  public readonly service = inject(ImportStockViewService);
  public readonly lookupService = inject(LookupService);

  supplierOptions: LookupDto<string>[] = [];
  supplierBUOptions: LookupDto<string>[] = [];
  materialGroupOptions: LookupDto<string>[] = [];
  materialTypeOptions: LookupDto<string>[] = [];

  statusOptions = [{ value: RequestStatusEnum.DRAFT, label: 'Draft' }];

  importTypeOptions = [...ImportMaterialOptions];

  ngOnInit(): void {
    // Initialize filter component
  }
}
