import { CoreModule } from '@abp/ng.core';
import { DateAdapter, ThemeSharedModule, TimeAdapter } from '@abp/ng.theme.shared';
import { ChangeDetectionStrategy, Component, inject, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { EscCloseModalDirective } from '@app/shared/esc-close-modal/esc-close-modal.directive';
import { AngularEditorConfig, AngularEditorModule } from '@kolkov/angular-editor';
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
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { CustomerDetailViewService } from '../services/customer-detail.service';

@Component({
  selector: 'app-customer-detail-modal',
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
    EscCloseModalDirective,
  ],
  providers: [
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbTimeAdapter, useClass: TimeAdapter },
  ],
  templateUrl: './customer-detail.component.html',
  styleUrls: ['./customer-detail.component.scss'],
})
export class CustomerDetailModalComponent implements OnInit {
  public readonly service = inject(CustomerDetailViewService);
  protected proxyLookupService = inject(LookupService);
  nationalityOptions: { value: string; label: string }[] = [];
  customerTypeOptions: LookupDto<string>[];
  customerIndustryOptions: LookupDto<string>[];
  editorConfig: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '200px',
    minHeight: '150px',
    maxHeight: '400px',
    width: 'auto',
    minWidth: '0',
    translate: 'yes',
    enableToolbar: true,
    showToolbar: true,
    placeholder: 'Enter text here...',
    defaultParagraphSeparator: '',
    defaultFontName: '',
    defaultFontSize: '',
    fonts: [
      { class: 'arial', name: 'Arial' },
      { class: 'times-new-roman', name: 'Times New Roman' },
      { class: 'calibri', name: 'Calibri' },
    ],
    sanitize: true,
    toolbarPosition: 'top',
    toolbarHiddenButtons: [
      ['subscript', 'superscript'],
      ['customClasses', 'insertImage', 'insertVideo'],
    ],
  };

  provinceOptions = [];

  ngOnInit(): void {
    this.loadDropdownData();
  }
  loadDropdownData() {
    this.proxyLookupService.getNationalityLookup().subscribe(result => {
      this.nationalityOptions = result.items.map(item => ({
        value: item.displayCode,
        label: item.displayName,
      }));
    });

    this.proxyLookupService.getCustomerTypeLookup().subscribe(result => {
      this.customerTypeOptions = result.items;
    });

    this.proxyLookupService.getEUIndustryLookup().subscribe(result => {
      this.customerIndustryOptions = result.items;
    });
  }
}
