import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { SystemConfigurationDetailViewService } from '../services/system-configuration-category-detail.service';
import { EscCloseModalDirective } from '@app/shared/esc-close-modal/esc-close-modal.directive';

@Component({
  selector: 'app-system-configuration-category-detail-modal',
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
  templateUrl: './system-configuration-category-detail.component.html',
  styleUrls: ['./system-configuration-category-detail.component.scss'],
})
export class SystemConfigurationCategoryDetailModalComponent {
  public readonly service = inject(SystemConfigurationDetailViewService);

  submitForm() {
    this.service.submitForm();
  }
  get f() {
    return this.service.form.controls;
  }
}
