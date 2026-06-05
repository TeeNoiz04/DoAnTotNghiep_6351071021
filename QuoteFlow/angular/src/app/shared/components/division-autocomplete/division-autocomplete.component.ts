import { PageModule } from '@abp/ng.components/page';
import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  inject,
  Input,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';
import { ControlContainer, FormGroup, FormGroupDirective, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { NgbTypeahead, NgbTypeaheadModule, NgbTypeaheadSelectItemEvent } from '@ng-bootstrap/ng-bootstrap';
import { ValidationGroupDirective } from '@ngx-validate/core';
import { BudgetPlanService } from '@proxy/budget-plans';
import { LookupDto } from '@proxy/shared';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-division-autocomplete',
  changeDetection: ChangeDetectionStrategy.Default,
  standalone: true,
  imports: [
    CoreModule,
    ThemeSharedModule,
    PageModule,
    CommercialUiModule,
    ReactiveFormsModule,
    ThemeSharedModule,
    MatInputModule,
    NgbTypeaheadModule,
  ],
  providers: [ValidationGroupDirective, { provide: ControlContainer, useExisting: FormGroupDirective }],
  templateUrl: './division-autocomplete.component.html',
  styleUrls: ['./division-autocomplete.component.scss'],
})
export class DivisionAutoCompleteComponent implements OnInit {
  @ViewChild('instance', { static: true }) instance: NgbTypeahead;
  protected readonly budgetPlanService = inject(BudgetPlanService);
  @Input() initFormGroup: FormGroup;
  @Input() isReadOnly: boolean;
  divisionSelected: LookupDto<string>;

  focus$ = new Subject<string>();
  click$ = new Subject<string>();

  @Output() changedDivision: EventEmitter<LookupDto<string>> = new EventEmitter<LookupDto<string>>();

  constructor() {}

  ngOnInit(): void {
    if (this.initFormGroup) {
      this.divisionSelected = {
        id: this.initFormGroup.get('divisionId').value,
        displayName: this.initFormGroup.get('divisionCode').value,
        displayDescription: this.initFormGroup.get('divisionName').value,
      };
    }
  }

  formatter = (val: { displayDescription: string }) => val.displayDescription;

  selectItem(event: NgbTypeaheadSelectItemEvent): void {
    if (event?.item) {
      this.divisionSelected = event.item;
      this.changedDivision.emit(event.item);
    }
  }
}
