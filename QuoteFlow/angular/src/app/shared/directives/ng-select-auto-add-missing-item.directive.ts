import { Directive, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { NgControl } from '@angular/forms';
import { NgSelectComponent } from '@ng-select/ng-select';
import { Subject, merge } from 'rxjs';
import { debounceTime, startWith, takeUntil } from 'rxjs/operators';

@Directive({
  selector: '[appAutoAddMissing]',
  standalone: true,
})
export class AutoAddMissingItemDirective implements OnInit, OnDestroy {
  @Input() autoAddMissing = true;
  @Input() missingItemLabel = '{value}';
  @Output() missingItemDetected = new EventEmitter<any>();

  private destroy$ = new Subject<void>();
  private itemsChange$ = new Subject<void>();

  constructor(
    private ngSelect: NgSelectComponent,
    private ngControl: NgControl,
  ) {}

  ngOnInit() {
    if (!this.autoAddMissing) return;

    setTimeout(() => {
      this.setupWatchers();
      this.checkAndAddMissingValue();
    });
  }

  private setupWatchers(): void {
    const control = this.ngControl.control;
    if (!control || !control.disabled) return;

    merge(control.valueChanges.pipe(startWith(control.value)))
      .pipe(debounceTime(0), takeUntil(this.destroy$))
      .subscribe(() => {
        this.checkAndAddMissingValue();
      });
  }

  private checkAndAddMissingValue(): void {
    const control = this.ngControl.control;
    if (!control || !control.disabled) return;

    const value = control?.getRawValue();
    if (value === null || value === undefined || !this.ngSelect.items) return;

    const bindValue = this.ngSelect.bindValue;
    const valueExists = this.ngSelect.items?.some((item: any) => item[bindValue] === value);

    if (!valueExists) {
      const missingItem = {
        [bindValue]: value,
        [this.ngSelect.bindLabel || 'label']: this.missingItemLabel.replace(
          '{value}',
          String(value),
        ),
      };
      this.missingItemDetected.emit(missingItem);
    }
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
