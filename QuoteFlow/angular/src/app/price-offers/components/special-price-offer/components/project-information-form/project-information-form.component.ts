import { CoreModule } from '@abp/ng.core';
import { CommonModule } from '@angular/common';
import { Component, inject, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { PriceOfferDetailViewService } from '@app/price-offers/services/price-offer-detail.service';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { LookupService } from '@proxy/general-lookups';
import { PriceOfferImportDto } from '@proxy/price-offers';
import { LookupDto } from '@proxy/shared';
import { ExcelValidationResult } from '@proxy/shared/excels';

@Component({
  selector: 'app-project-information-form',
  templateUrl: './project-information-form.component.html',
  styleUrls: ['./project-information-form.component.scss'],
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, NgbModule, NgSelectModule, MatCheckboxModule, CoreModule],
})
export class ProjectInformationFormComponent implements OnInit, OnChanges {
  @Input() resultImport: ExcelValidationResult<PriceOfferImportDto> | undefined;
  @Input() useServiceData: boolean = true;
  @Input() isReadonly: boolean = true;
  @Input() parentDataLoaded: boolean = false;

  public readonly service = inject(PriceOfferDetailViewService);

  readonly fb = inject(FormBuilder);
  private readonly lookupService = inject(LookupService);

  projectForm: FormGroup;
  formSubmitted = false;
  isFormReadonly = false;

  // Options for dropdowns
  countryOptions: LookupDto<string>[] = [];

  constructor() {
    this.projectForm = this.fb.group({
      projectCode: [''],
      projectName: [''],
      projectType: [''],
      euIndustry: [''],
      application: [''],
      country: [null],
      province: [''],
      detailedAddress: [''],
      competitorBrand: [''],
      priceGap: [''],
      decisionRight: [''],
      poPlannedDate: [null],
      deliveryDate: [null],
      upcomingPotentialProjects: [''],
      otherInformation: [''],
    });
  }

  ngOnInit(): void {
    // Load lookups
    this.loadCountryLookup();

    // Apply readonly state if needed
    if (this.isReadonly) {
      this.setReadonlyState(true);
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    // Handle import data changes
    if (!this.useServiceData && changes['resultImport'] && this.resultImport) {
      if (this.resultImport.singleRow) {
        this.projectForm.patchValue({
          projectName: this.resultImport?.listData[0].rowData.projectName,
          projectType: this.resultImport?.listData[0].rowData.projectTypeDescription,
          application: this.resultImport?.listData[0].rowData.application,
          country: this.resultImport?.listData[0].rowData.country,
          province: this.resultImport?.listData[0].rowData.province,
          detailedAddress: this.resultImport?.listData[0].rowData.detailedAddress,
          competitorBrand: this.resultImport?.listData[0].rowData.competitorBrand,
          priceGap: this.resultImport?.listData[0].rowData.priceGapWithCompetitor,
          decisionRight: this.resultImport?.listData[0].rowData.decisionRight,
          poPlannedDate: this.resultImport?.listData[0].rowData.poPlannedDate,
          deliveryDate: this.resultImport?.listData[0].rowData.deliveryDate,
          upcomingPotentialProjects: this.resultImport?.listData[0].rowData.upcomingPotentialProjects,
          otherInformation: this.resultImport?.listData[0].rowData.otherPJInformation,
        });

        this.setReadonlyState(true);
      }
    }

    if (changes['isReadonly'] && this.isReadonly !== changes['isReadonly'].previousValue) {
      this.setReadonlyState(this.isReadonly);
    }

    if (changes['parentDataLoaded'] && changes['parentDataLoaded'].currentValue === true) {
      // Ensure form is updated when parent data is loaded
      if (this.useServiceData) {
        this.service.buildDetailForm();

        // Sync with service form if using service data
        if (this.service.projectInfoDetailForm) {
          const serviceFormValue = this.service.projectInfoDetailForm.getRawValue();

          this.projectForm.patchValue(serviceFormValue);

          // Subscribe to service form changes
          this.service.projectInfoDetailForm.valueChanges.subscribe(values => {
            this.projectForm.patchValue(values, { emitEvent: false });
          });

          // Sync back to service
          this.projectForm.valueChanges.subscribe(values => {
            if (this.service.projectInfoDetailForm && !this.isFormReadonly) {
              this.service.projectInfoDetailForm.patchValue(values, { emitEvent: false });
            }
          });
        }
      }
    }
  }

  private setReadonlyState(readonly: boolean): void {
    this.isFormReadonly = readonly;

    if (readonly) {
      // Disable editing
      this.projectForm.disable();
    } else {
      // Enable editing
      this.projectForm.enable();
    }
  }

  loadCountryLookup(): void {
    // this.lookupService.getCountryLookup().subscribe(result => {
    //   this.countryOptions = result.items || [];
    // });
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.projectForm.get(fieldName);
    return field ? field.invalid && (field.touched || this.formSubmitted) : false;
  }

  validateAllFields(): void {
    this.formSubmitted = true;
    Object.keys(this.projectForm.controls).forEach(field => {
      const control = this.projectForm.get(field);
      control?.markAsTouched();
    });
  }

  getForm(): FormGroup {
    return this.projectForm;
  }
}
