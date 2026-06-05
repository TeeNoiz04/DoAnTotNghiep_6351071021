import { CoreModule, ListService } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { ChangeDetectionStrategy, Component, inject, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { NgbNavModule, NgbTooltip } from '@ng-bootstrap/ng-bootstrap';
import { CommonModule } from '@angular/common';
import { PageModule } from '@abp/ng.components/page';
import { ActivatedRoute, Router } from '@angular/router';
import { ImportStockDetailViewService } from '../../services/import-stock-detail.service';
import { ImportStockColumns, ImportStockManagementType } from './import-stock.type';
import { finalize } from 'rxjs';
import { AppRoutes } from '@app/app.routes';
import { StatusLabelComponent } from '@app/shared/status/components/status-label.component';
import { ExpandablePanelV2Component } from '@app/shared/components/expandable-panel-v2/expandable-panel-v2.component';
import { TitleService } from '@app/shared/services/title/title.service';

@Component({
  selector: 'app-import-stock-detail-modal',
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
    StatusLabelComponent,
    ExpandablePanelV2Component,
    NgbTooltip,
  ],
  providers: [ListService, ImportStockDetailViewService],
  templateUrl: './import-stock-detail.component.html',
  styleUrls: ['./import-stock-detail.component.scss'],
})
export class ImportStockDetailComponent implements OnInit {
  protected readonly router = inject(Router);
  protected route = inject(ActivatedRoute);
  public readonly service = inject(ImportStockDetailViewService);
  public readonly titleService = inject(TitleService);

  protected readonly title = 'Stock Management - Stock Upload Detail';

  form: FormGroup = new FormGroup({});
  requestId: string | null;
  importType = ImportStockManagementType;
  columns = ImportStockColumns;

  @ViewChild('customCellTemplate', { static: true }) customCellTemplate: TemplateRef<any>;

  ngOnInit() {
    this.titleService.setTitle('Stock Upload Detail | Stock Management');

    this.requestId = this.route.snapshot.paramMap.get('id');
    if (this.requestId) {
      this.service.isBusy = true;

      this.service
        .loadDetailsAndImports(this.requestId)
        .pipe(finalize(() => (this.service.isBusy = false)))
        .subscribe({
          next: ({ details }) => {
            this.service.selected = details;
            this.service.buildForm();

            if (this.columns?.[details.importType]?.length > 0) {
              this.columns?.[details.importType]?.forEach(col => {
                col.cellTemplate = this.customCellTemplate;
              });
            }
          },
          error: error => {
            console.error('Error loading data', error);
          },
        });
    }
  }

  goBack(): void {
    const currentUrl = this.router.url;

    const routesMap = [
      {
        condition:
          currentUrl.includes(AppRoutes.STOCK_MANAGEMENT.BASE) &&
          currentUrl.includes(AppRoutes.STOCK_MANAGEMENT.DETAILS.BASE),
        route: [AppRoutes.STOCK_MANAGEMENT.BASE, AppRoutes.STOCK_MANAGEMENT.IMPORT_STOCK.BASE],
      },
    ];

    for (const route of routesMap) {
      if (route.condition) {
        this.router.navigate(route.route);
        break;
      }
    }
  }
}
