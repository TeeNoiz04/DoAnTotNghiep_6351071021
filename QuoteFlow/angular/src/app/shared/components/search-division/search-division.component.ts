import { PageModule } from '@abp/ng.components/page';
import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { Component, EventEmitter, Input, OnChanges, OnDestroy, Output, SimpleChanges } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { DateHelper } from '@app/shared/helpers/date-helper';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { BudgetPlanFiltersDto, FiscalYearLookupDto } from '@proxy/budget-plans';
import { DepartmentLookupDto, DivisionLookupDto, RegionLookupDto, SectionLookupDto } from '@proxy/shared/org-chart';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { Subscription } from 'rxjs';
import { TSearchDivision } from './search-division.model';

@Component({
  selector: 'app-search-division',
  standalone: true,
  imports: [
    CoreModule,
    ThemeSharedModule,
    PageModule,
    CommercialUiModule,
    NgbDropdownModule,
    NgSelectModule,
    ReactiveFormsModule,
  ],
  templateUrl: './search-division.component.html',
  styleUrls: ['./search-division.component.scss'],
})
export class SearchDivisionComponent implements OnDestroy, OnChanges {
  @Input({ required: true }) disableFiscalYearFilter: boolean;
  @Input({ required: true }) budgetPlanFilters: BudgetPlanFiltersDto | undefined;
  @Input() hideFiscal: boolean = false;
  @Input() submitLabel: string = 'Search';
  @Input() preSelectedFiscalYear: number;
  @Input() showSearchActions: boolean = true;
  @Input() manualFiscalYear: boolean = false;
  @Input() disableFilters: boolean = false;
  @Input() allowPreload: boolean = true;

  @Output() searchData: EventEmitter<TSearchDivision> = new EventEmitter<TSearchDivision>();
  @Output() clearFilters: EventEmitter<void> = new EventEmitter<void>();

  fiscalYearLookups: FiscalYearLookupDto[] = [];
  divisionLookups: DivisionLookupDto[] = [];
  departmentLookups: DepartmentLookupDto[] = [];
  sectionLookups: SectionLookupDto[] = [];
  regionLookups: RegionLookupDto[] = [];

  private subscriptions: Subscription[] = [];

  protected initialized = false;
  protected selectedFiscalYear: string;
  protected selectedDivisionCode: string;
  protected selectedDepartmentCode: string | null;
  protected selectedSectionCode: string | null;
  protected selectedRegionCode: string | null;

  protected isDepartmentClearable = false;
  protected isSectionClearable = false;
  protected isRegionClearable = false;

  // initialize
  // for this component, we have division, department, section, region, fiscal year
  // we need to initialize the division, department, section, region, fiscal year, using the first value we've got from the filter

  constructor() {}

  ngOnChanges(changes: SimpleChanges): void {
    let shouldSearch = false;

    if (changes?.budgetPlanFilters?.currentValue) {
      const budgetPlanFilters = changes.budgetPlanFilters.currentValue as BudgetPlanFiltersDto;
      this.fiscalYearLookups = budgetPlanFilters?.fiscalYears;
      this.divisionLookups = budgetPlanFilters?.divisions;

      const currentFiscalYear = DateHelper.getFiscalYearFromDate(new Date());
      const firstFiscalYear = this.fiscalYearLookups?.[0];
      this.selectedFiscalYear =
        this.fiscalYearLookups.find(f => f.displayName === currentFiscalYear.toString())?.displayName ??
        firstFiscalYear?.displayName;

      const firstDivision = budgetPlanFilters?.divisions[0];
      this.selectedDivisionCode = firstDivision?.displayName;

      this.onDivisionChange(firstDivision);

      this.initialized = true;

      if (this.allowPreload) {
        shouldSearch = true;
      }
    }

    if (
      changes?.preSelectedFiscalYear?.currentValue &&
      this.fiscalYearLookups?.length &&
      changes.manualFiscalYear?.currentValue
    ) {
      const preSelectedFiscalYear = changes.preSelectedFiscalYear.currentValue as number;
      const manualFiscalYear = changes.manualFiscalYear.currentValue as boolean;

      if (!manualFiscalYear) {
        this.selectedFiscalYear =
          this.fiscalYearLookups.find(f => f.displayName === preSelectedFiscalYear.toString())?.displayName ??
          preSelectedFiscalYear.toString();
      } else {
        this.selectedFiscalYear = preSelectedFiscalYear.toString();
      }

      if (this.allowPreload) {
        shouldSearch = true;
      }
    }

    if (shouldSearch) {
      this.onSearchClick();
    }
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  //#region On Clear Events
  // onDepartmentClear(): void {
  //   this.selectedDepartment = null;
  //   this.onDepartmentChange(this.selectedDepartment);
  // }

  // onSectionClear(): void {
  //   this.selectedSection = null;
  //   this.onSectionChange(this.selectedSection);
  // }

  // onRegionClear(): void {
  //   this.selectedRegion = null;
  // }

  //#region On Change Events
  onDivisionChange(division: DivisionLookupDto): void {
    this.isDepartmentClearable = !division?.requireDepartmentSelected;
    this.departmentLookups = division?.departments;

    if (division?.requireDepartmentSelected === true) {
      const firstDepartment = division?.departments?.[0];
      this.selectedDepartmentCode = firstDepartment?.displayName;
      this.onDepartmentChange(firstDepartment);
    } else {
      this.selectedDepartmentCode = null;
      this.onDepartmentChange(null);
    }
  }

  onDepartmentChange(department: DepartmentLookupDto | null): void {
    this.isSectionClearable = !department?.requireSectionSelected;
    this.sectionLookups = department?.sections;

    if (department?.requireSectionSelected === true) {
      const firstSection = department?.sections?.[0];
      this.selectedSectionCode = firstSection?.displayName;
      this.onSectionChange(firstSection);
    } else {
      this.selectedSectionCode = null;
      this.onSectionChange(null);
    }
  }

  onSectionChange(section: SectionLookupDto | null): void {
    this.isRegionClearable = !section?.requireRegionSelected;
    this.regionLookups = section?.regions;

    if (section?.requireRegionSelected === true) {
      const firstRegion = section?.regions?.[0];
      this.selectedRegionCode = firstRegion?.displayName;
    } else {
      this.selectedRegionCode = null;
    }
  }
  //#endregion

  onClearFiltersClick(): void {
    const firstDivision = this.divisionLookups?.[0];
    this.selectedDivisionCode = firstDivision?.displayName;
    this.departmentLookups = firstDivision?.departments;

    if (firstDivision.requireDepartmentSelected) {
      const firstDepartment = firstDivision.departments?.[0];
      this.selectedDepartmentCode = firstDepartment?.displayName;
      this.onDepartmentChange(firstDepartment);
    } else {
      this.selectedDepartmentCode = null;
      this.onDepartmentChange(null);
    }

    this.clearFilters.emit();
  }

  onSearchClick(): void {
    this.searchData.emit({
      // fy: convert this.selectedFiscalYear.displayName from string to number
      fy: Number(this.selectedFiscalYear),
      divisionCode: this.selectedDivisionCode == 'All' ? 'All' : this.selectedDivisionCode,
      departmentCode: this.selectedDepartmentCode,
      sectionCode: this.selectedSectionCode,
      regionCode: this.selectedRegionCode,
    });
  }

  getValues(): TSearchDivision {
    return {
      fy: Number(this.selectedFiscalYear),
      divisionCode: this.selectedDivisionCode == 'All' ? 'All' : this.selectedDivisionCode,
      departmentCode: this.selectedDepartmentCode,
      sectionCode: this.selectedSectionCode,
      regionCode: this.selectedRegionCode,
    };
  }
}
