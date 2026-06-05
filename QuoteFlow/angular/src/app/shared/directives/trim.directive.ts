import { Directive, ElementRef, HostListener, Optional } from '@angular/core';
import { NgControl } from '@angular/forms';

@Directive({
  selector: '[appTrim]',
  standalone: true,
})
export class TrimDirective {
  constructor(
    private element: ElementRef,
    @Optional() private control: NgControl,
  ) {}

  @HostListener('blur') onBlur() {
    const previousValue = this.element.nativeElement.value;
    if (!previousValue || typeof previousValue !== 'string') {
      return;
    }
    const trimmedValue = previousValue.trim();
    if (previousValue !== trimmedValue) {
      this.element.nativeElement.value = trimmedValue;

      // Update the form control value if NgControl is present
      if (this.control && this.control.control) {
        this.control.control.setValue(trimmedValue, { emitEvent: false });
      }
    }
  }
}
