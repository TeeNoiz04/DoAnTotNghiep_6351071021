import { CoreModule } from '@abp/ng.core';
import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { ExpandablePanelV2Component } from '@app/shared/components/expandable-panel-v2/expandable-panel-v2.component';
import { ThreeStateButtonComponent } from '@app/shared/components/three-state-button';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { LookupService } from '@proxy/general-lookups';
import { LookupDto } from '@proxy/shared';

@Component({
  selector: 'app-dpo-processing-report-filter',
  templateUrl: './dpo-processing-report-filter.component.html',
  standalone: true,
  styleUrls: ['./dpo-processing-report-filter.component.scss'],
  imports: [
    FormsModule,
    NgbModule,
    NgSelectModule,
    MatCheckboxModule,
    CoreModule,
    FormsModule,
    ReactiveFormsModule,
    ExpandablePanelV2Component,
    ThreeStateButtonComponent,
  ],
  providers: [LookupService, LoadingService],
})
export class DPOProcessingReportFilterComponent implements OnInit {
  protected readonly lookupService = inject(LookupService);
  protected readonly loadingService = inject(LoadingService);
  protected readonly fb = inject(FormBuilder);

  form: FormGroup = new FormGroup({});
  buyerTypeOptions: LookupDto<string>[] = [];
  buyerOptions: LookupDto<string>[] = [];

  ngOnInit(): void {
    this.initializeForm();
    this.loadDropdownData();
  }

  get f() {
    return this.form.controls;
  }

  private initializeForm(): void {
    const today = new Date();
    const currentYear = today.getFullYear();

    const defaultFromDate = `${currentYear}-01-01`; // "2025-01-01"
    const defaultToDate = `${currentYear}-12-31`; // "2025-12-31"

    this.form = this.fb.group({
      buyerTypeId: [''],
      buyerId: [null], // null instead of 'none'
      fromdate: [defaultFromDate, [Validators.required]],
      todate: [defaultToDate, [Validators.required]],
    });
  }

  private async loadDropdownData(): Promise<void> {
    try {
      this.loadingService.show();
      const buyerTypes = await this.lookupService.getBuyerTypeLookup({}).toPromise();
      this.buyerTypeOptions = buyerTypes?.items || [];

      if (this.buyerTypeOptions.length > 0) {
        const firstBuyerType = this.buyerTypeOptions[0];
        this.form.patchValue({ buyerTypeId: firstBuyerType.id });

        // Load buyers for this buyer type
        await this.loadBuyersByType(firstBuyerType.id);
      } else {
        this.initializeBuyerOptions();
      }
    } catch (error) {
      console.error('Error loading dropdown data:', error);
    } finally {
      this.loadingService.hide();
    }
  }

  onBuyerTypeChange(event: LookupDto<string>): void {
    const buyerTypeId = event?.id;
    this.form.patchValue({ buyerId: 'none' });

    if (buyerTypeId) {
      this.loadBuyersByType(buyerTypeId);
    } else {
      this.initializeBuyerOptions();
    }
  }

  onConfirmDateChange(date: any): void {}

  onNoteChange(note: string): void {}

  validateForm(): boolean {
    if (this.form.valid) {
      return true;
    } else {
      Object.keys(this.form.controls).forEach(key => {
        this.form.get(key)?.markAsTouched();
      });
      return false;
    }
  }

  /**
   * Custom validator for buyer selection
   */
  private validateBuyerSelection = (control: any) => {
    const value = control.value;
    if (!value || value === 'none') {
      return { buyerRequired: true };
    }
    return null;
  };

  /**
   * Initialize buyer options with default 'none' option
   */
  private initializeBuyerOptions(): void {
    this.buyerOptions = [
      {
        // id: 'none',
        // displayCode: '-- Please select buyer --',
        // displayName: '-- Please select buyer --',
      } as LookupDto<string>,
    ];
  }

  /**
   * Load buyers filtered by buyer type
   */
  private async loadBuyersByType(buyerTypeId: string): Promise<void> {
    try {
      this.loadingService.show();

      // Load buyers filtered by buyer type
      const buyers = await this.lookupService.getBuyerLookupByBuyerType(buyerTypeId).toPromise();
      const buyerItems = buyers?.items || [];

      // Add default "none" option at the beginning
      this.buyerOptions = [
        {
          // id: 'none',
          // displayCode: '-- Please select buyer --',
          // displayName: '-- Please select buyer --',
        } as LookupDto<string>,
        ...buyerItems,
      ];
    } catch (error) {
      console.error('Error loading buyers by type:', error);
      // Fallback to default option only
      this.initializeBuyerOptions();
    } finally {
      this.loadingService.hide();
    }
  }
}
