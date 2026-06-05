import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { StorageLocationDetailViewService } from '../services/storage-location-category-detail.service';
import { EscCloseModalDirective } from '@app/shared/esc-close-modal/esc-close-modal.directive';

@Component({
  selector: 'app-storage-location-category-detail-modal',
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    ReactiveFormsModule,
    MatSlideToggleModule,
    EscCloseModalDirective,
  ],
  providers: [],
  templateUrl: './storage-location-category-detail.component.html',
  styleUrls: ['./storage-location-category-detail.component.scss'],
})
export class StorageLocationCategoryDetailModalComponent {
  public readonly service = inject(StorageLocationDetailViewService);

  submitForm() {
    this.service.submitForm();
  }

  get f() {
    return this.service.form.controls;
  }
}
