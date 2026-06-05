import { Directive, ElementRef, HostListener, Input } from '@angular/core';

@Directive({
  selector: '[appMaxLength]',
  standalone: true,
})
export class MaxLengthDirective {
  @Input('appMaxLength') maxLength: number | string = 0;

  constructor(private el: ElementRef<HTMLInputElement>) {}

  @HostListener('keypress', ['$event'])
  onKeyPress(event: KeyboardEvent): boolean {
    const input = this.el.nativeElement;
    const maxLengthParsed = parseInt(`${this.maxLength}`, 10);
    return input.value.length < maxLengthParsed;
  }

  @HostListener('paste', ['$event'])
  onPaste(event: ClipboardEvent): void {
    event.preventDefault();
    const pastedText = event.clipboardData?.getData('text') || '';
    const input = this.el.nativeElement;
    const maxLengthParsed = parseInt(`${this.maxLength}`, 10);
    const result = (input.value + pastedText).slice(0, maxLengthParsed);
    input.value = result;
    input.dispatchEvent(new Event('input'));
  }
}
