import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-skeleton-loader',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div
      class="skeleton-loader"
      [ngClass]="{ 'skeleton-animation': !noAnimation }"
      [ngStyle]="{
        height: height,
        width: width,
        'border-radius': rounded ? '4px' : '0',
      }"></div>
  `,
  styles: [
    `
      .skeleton-loader {
        background: linear-gradient(90deg, #f0f0f0 25%, #e0e0e0 50%, #f0f0f0 75%);
        background-size: 200% 100%;
        margin-bottom: 8px;
      }

      .skeleton-animation {
        animation: loading 1.5s infinite;
      }

      @keyframes loading {
        0% {
          background-position: 200% 0;
        }
        100% {
          background-position: -200% 0;
        }
      }
    `,
  ],
})
export class SkeletonLoaderComponent {
  @Input() height: string = '20px';
  @Input() width: string = '100%';
  @Input() rounded: boolean = true;
  @Input() noAnimation: boolean = false;
}
