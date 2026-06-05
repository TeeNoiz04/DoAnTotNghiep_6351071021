import { ChangeDetectionStrategy, Component } from '@angular/core';
import { NgbCollapseModule, NgbDropdownModule, NgbTooltip } from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { ListService, CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { PageModule } from '@abp/ng.components/page';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { StorageLocationCategoryDetailModalComponent } from './storage-location-category-detail.component';
import {
  AbstractStorageLocationCategoryComponent,
  ChildTabDependencies,
  ChildComponentDependencies,
} from './storage-location-category.abstract.component';
import { StorageLocationDetailViewService } from '../services/storage-location-category-detail.service';
import { StorageLocationViewService } from '../services/storage-location-category.service';
import { TrimDirective } from '@app/shared/directives/trim.directive';

@Component({
  selector: 'app-storage-location-category',
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [
    ...ChildTabDependencies,
    NgbCollapseModule,
    NgbDropdownModule,
    NgxValidateCoreModule,
    PageModule,
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    TrimDirective,
    StorageLocationCategoryDetailModalComponent,
    NgbTooltip,
    ...ChildComponentDependencies,
  ],
  providers: [ListService, StorageLocationViewService, StorageLocationDetailViewService],
  templateUrl: './storage-location-category.component.html',
  styleUrls: [`storage-location-category.component.scss`],
})
export class StorageLocationCategoryComponent extends AbstractStorageLocationCategoryComponent {}
