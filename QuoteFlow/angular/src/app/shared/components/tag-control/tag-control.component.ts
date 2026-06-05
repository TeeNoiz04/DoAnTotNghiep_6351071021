import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-tag-control',
  standalone: true,
  templateUrl: './tag-control.component.html',
  styleUrls: ['./tag-control.component.scss'],
})
export class TagControlComponent {
  @Input() value: number | string;
}
