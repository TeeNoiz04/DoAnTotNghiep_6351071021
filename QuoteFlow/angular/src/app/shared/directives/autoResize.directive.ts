import { Directive, ElementRef, HostListener, Input, OnInit } from '@angular/core';

@Directive({
  selector: '[appAutoResize]',
  standalone: true,
})
export class AutoResizeDirective implements OnInit {
  @Input() minRows: number = 3;
  @Input() maxRows: number = 10;

  constructor(private el: ElementRef) {}

  ngOnInit(): void {
    this.adjustRows();
  }

  @HostListener('input')
  onInput(): void {
    this.adjustRows();
  }

  private adjustRows(): void {
    const textarea = this.el.nativeElement as HTMLTextAreaElement;
    textarea.rows = this.minRows;
    const rows = Math.min(this.maxRows, Math.max(this.minRows, textarea.scrollHeight / 24));
    textarea.rows = rows;
  }
}
