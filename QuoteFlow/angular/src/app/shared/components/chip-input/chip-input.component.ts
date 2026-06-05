import { CommonModule } from '@angular/common';
import { Component, ElementRef, EventEmitter, Input, Output, ViewChild, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-chip-input',
  standalone: true,
  imports: [CommonModule, NgbTooltipModule],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => ChipInputComponent),
      multi: true,
    },
  ],
  templateUrl: './chip-input.component.html',
  styleUrls: ['./chip-input.component.scss'],
})
export class ChipInputComponent implements ControlValueAccessor {
  @Input() label = '';
  @Input() placeholder = '';
  @Input() inputTooltip = 'Press Enter or comma to add value';
  @Input() allowDuplicates = false;
  @Input() disabled = false;
  @Input() maxLength: number | null = null;
  @Input() validateInput: (value: string) => boolean = () => true;
  @Input() processInput: (value: string) => string = value => value.trim();

  @Output() chipsChanged = new EventEmitter<string[]>();
  @ViewChild('chipInput') chipInput: ElementRef<HTMLInputElement>;

  chips: string[] = [];

  // Generate unique ID for the input
  inputId = `chip-input-${Math.random().toString(36).substr(2, 9)}`;

  // ControlValueAccessor implementation
  private onChange = (value: string[]) => {};
  private onTouched = () => {};

  onKeyDown(event: KeyboardEvent): void {
    const input = event.target as HTMLInputElement;
    const value = input.value.trim();

    if (event.key === 'Enter' || event.key === ',') {
      event.preventDefault();
      this.addChip(value);
      input.value = '';
    }
  }

  addChip(value: string): void {
    if (value) {
      const processedValue = this.processInput(value);

      if (processedValue && this.validateInput(processedValue)) {
        // Check for duplicates if not allowed
        if (this.allowDuplicates || !this.chips.includes(processedValue)) {
          // Check max length if specified
          if (!this.maxLength || this.chips.length < this.maxLength) {
            this.chips.push(processedValue);
            this.updateValue();
          }
        }
      }
    }
  }

  removeChip(chip: string): void {
    const index = this.chips.indexOf(chip);
    if (index >= 0) {
      this.chips.splice(index, 1);
      this.updateValue();
    }
  }

  private updateValue(): void {
    this.onChange(this.chips);
    this.chipsChanged.emit(this.chips);
  }

  // ControlValueAccessor methods
  writeValue(value: string[]): void {
    this.chips = value || [];
  }

  registerOnChange(fn: (value: string[]) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }
}
