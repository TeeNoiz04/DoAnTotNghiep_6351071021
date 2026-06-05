import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { ChangeDetectionStrategy, Component, inject, OnInit } from '@angular/core';
import { FormArray, ReactiveFormsModule } from '@angular/forms';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MaterialGroupBuyerDetailViewService } from '@app/buyer/services/material-group-buyer-detail.service';
import { NgSelectModule } from '@ng-select/ng-select';
import { LookupService } from '@proxy/general-lookups';
import { LookupDto } from '@proxy/shared';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';

@Component({
  selector: 'app-material-group-buyer-modal',
  changeDetection: ChangeDetectionStrategy.Default,
  imports: [
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    ReactiveFormsModule,
    MatSlideToggleModule,
    NgSelectModule,
  ],
  providers: [],
  templateUrl: './material-group-buyer-modal.component.html',
  styleUrls: ['./material-group-buyer-modal.component.scss'],
})
export class MaterialGroupBuyerModalComponent implements OnInit {
  public readonly service = inject(MaterialGroupBuyerDetailViewService);
  private readonly lookupService = inject(LookupService);

  buyerOptions: LookupDto<string>[] = [];
  materialGroupLVSOptions: LookupDto<string>[] = [];
  materialGroupFAOptions: LookupDto<string>[] = [];

  ngOnInit(): void {
    this.loadBuyers();
    this.loadMaterialGroup();
  }

  private loadBuyers(): void {
    this.lookupService.getBuyersNotAssignedToMaterialGroup().subscribe({
      next: result => {
        const sortedBuyers = (result.items || []).sort((a, b) => a.displayCode?.localeCompare(b.displayCode));
        this.buyerOptions = sortedBuyers;
        this.service.buyerOptions = sortedBuyers;
      },
      error: error => {
        console.error('Error loading buyers:', error);
      },
    });
  }

  private loadMaterialGroup() {
    this.lookupService.getMaterialGroupByType('FA').subscribe({
      next: result => {
        const sortedBuyers = (result.items || []).sort((a, b) => a.displayCode?.localeCompare(b.displayCode));
        this.materialGroupFAOptions = sortedBuyers || [];
        this.service.materialGroupOptions = [...this.service.materialGroupOptions, ...sortedBuyers];
      },
      error: error => {
        console.error('Error loading material groups:', error);
      },
    });

    this.lookupService.getMaterialGroupByType('LVS').subscribe({
      next: result => {
        const sortedBuyers = (result.items || []).sort((a, b) => a.displayCode?.localeCompare(b.displayCode));
        this.materialGroupLVSOptions = sortedBuyers || [];
        this.service.materialGroupOptions = [...this.service.materialGroupOptions, ...sortedBuyers];
      },
      error: error => {
        console.error('Error loading material groups:', error);
      },
    });
  }

  // helper to access materialGroups FormArray
  get materialGroupsFormArray() {
    return this.service.form.get('materialGroups') as FormArray;
  }

  // toggle checkbox
  onCheckboxChange(event: any, id: string) {
    const formArray: FormArray = this.materialGroupsFormArray;

    if (event.target.checked) {
      formArray.push(this.service.fb.control(id));
    } else {
      const index = formArray.controls.findIndex(x => x.value === id);
      formArray.removeAt(index);
    }
  }

  submitForm() {
    this.service.submitForm();
  }

  get f() {
    return this.service.form.controls;
  }
}
