import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SkeletonLoaderComponent } from '../skeleton-loader/skeleton-loader.component';

@Component({
  selector: 'app-skeleton-form-field',
  standalone: true,
  imports: [CommonModule, SkeletonLoaderComponent],
  template: `
    <div class="mb-3">
      <app-skeleton-loader height="16px" width="100px"></app-skeleton-loader>
      <app-skeleton-loader height="38px" width="100%"></app-skeleton-loader>
    </div>
  `,
})
export class SkeletonFormFieldComponent {}

@Component({
  selector: 'app-skeleton-table',
  standalone: true,
  imports: [CommonModule, SkeletonLoaderComponent],
  template: `
    <div class="table-skeleton">
      <app-skeleton-loader height="50px" width="100%"></app-skeleton-loader>
      <ng-container *ngFor="let i of [].constructor(rows)">
        <app-skeleton-loader height="40px" width="100%"></app-skeleton-loader>
      </ng-container>
    </div>
  `,
})
export class SkeletonTableComponent {
  @Input() rows: number = 5;
}
