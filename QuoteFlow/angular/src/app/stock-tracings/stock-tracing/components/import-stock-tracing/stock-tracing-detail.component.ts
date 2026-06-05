import { PageModule } from '@abp/ng.components/page';
import { CoreModule, ListService } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AppRoutes } from '@app/app.routes';
import { NgbNavModule, NgbTooltip } from '@ng-bootstrap/ng-bootstrap';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { TableFilterPipe } from '../../../../shared/pipes/table-filter.pipe';
import { StockTracingDetailViewService } from '../../services/stock-tracing-detail.service';
import { TitleService } from '@app/shared/services/title/title.service';
// import { TableFilterPipe } from '../../../../shared/pipes/table-filter.pipe';

@Component({
  selector: 'app-stock-tracing-detail',
  changeDetection: ChangeDetectionStrategy.Default,
  standalone: true,
  imports: [
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    CommonModule,
    PageModule,
    ReactiveFormsModule,
    NgbNavModule,
    TableFilterPipe,
    NgbTooltip,
  ],
  providers: [ListService, StockTracingDetailViewService],
  templateUrl: './stock-tracing-detail.component.html',
  styleUrls: ['./stock-tracing-detail.component.scss'],
})
export class StockTracingDetailComponent implements OnInit {
  protected readonly router = inject(Router);
  protected route = inject(ActivatedRoute);
  public readonly service = inject(StockTracingDetailViewService);
  public readonly titleService = inject(TitleService);

  protected readonly title = 'Import Stock Tracing Detail';
  requestId: string | null;
  public keyword: string;

  searchableColumns: string[] = [
    'checklistCode',
    'dateEntered',
    'stock',
    'bu',
    'customer',
    'category',
    'giv',
    'invoice',
    'skuCode',
    'skuName',
    'quality',
    'unit',
    'series',
    'originCode',
    'productionDate',
    'location',
  ];

  typeReport: string;
  ngOnInit() {
    this.titleService.setTitle('Import Stock Tracing Detail | Stock Tracing');

    this.service.buildForm();
    this.requestId = this.route.snapshot.paramMap.get('id');

    this.service.form?.get('reportType')?.valueChanges.subscribe(value => {
      this.typeReport = value;
    });

    this.service.getStockTracingDetails(this.requestId);
  }

  navigateBack(): void {
    this.router.navigate([AppRoutes.STOCK_TRACING.BASE, AppRoutes.STOCK_TRACING.IMPORT.BASE]);
  }
}
