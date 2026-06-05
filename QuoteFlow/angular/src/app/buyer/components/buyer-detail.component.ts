import { CoreModule } from '@abp/ng.core';
import { HttpClientModule } from '@angular/common/http';
import { ThemeSharedModule, DateAdapter, TimeAdapter } from '@abp/ng.theme.shared';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { ChangeDetectionStrategy, Component, HostListener, inject, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { InputNumberComponent } from '@app/shared/components/input-number/input-number.component';
import { AngularEditorModule } from '@kolkov/angular-editor';
import {
  NgbDateAdapter,
  NgbDatepickerModule,
  NgbNavModule,
  NgbTimeAdapter,
  NgbTimepickerModule,
} from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { LookupService } from '@proxy/general-lookups';
import { LookupDto } from '@proxy/shared';
import { SystemCategoryDto } from '@proxy/system-categories';
import { BuyerDetailViewService } from '../services/buyer-detail.service';
import { EscCloseModalDirective } from '@app/shared/esc-close-modal/esc-close-modal.directive';

@Component({
  selector: 'app-buyer-detail-modal',
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    ReactiveFormsModule,
    NgbDatepickerModule,
    NgbTimepickerModule,
    NgbNavModule,
    MatSlideToggleModule,
    NgSelectModule,
    AngularEditorModule,
    InputNumberComponent,
    HttpClientModule,
    EscCloseModalDirective,
  ],
  providers: [
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './buyer-detail.component.html',
  styleUrls: ['./buyer-detail.component.scss'],
})
export class BuyerDetailModalComponent implements OnInit {
  public readonly service = inject(BuyerDetailViewService);
  categories: SystemCategoryDto[] = [];

  protected proxyLookupService = inject(LookupService);
  buyerTypeOptions: LookupDto<string>[] = [];
  ngOnInit(): void {
    this.getBuyerType();
  }

  getBuyerType() {
    this.proxyLookupService.getBuyerTypeLookup({}).subscribe(result => {
      this.buyerTypeOptions = result.items;
    });
  }

  buyerTypeChange(event: any) {
    this.service.form.get('buyerTypeCode')?.setValue(event?.displayCode);
  }
}
