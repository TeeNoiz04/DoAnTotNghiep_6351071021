import { ListService } from '@abp/ng.core';
import { CommonModule, DatePipe, NgClass } from '@angular/common';
import { Component, computed, inject } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaterialDto } from '@app/proxy/materials/models';
import { NgbAccordionModule, NgbModalModule, NgbNavModule, NgbToastModule } from '@ng-bootstrap/ng-bootstrap';
import { StockManagementViewService } from '../services/stock-management.service';

export const ChildTabDependencies = [
  NgbNavModule,
  NgbModalModule,
  CommonModule,
  ReactiveFormsModule,
  FormsModule,
  DatePipe,
  NgbToastModule,
  NgbAccordionModule,
  NgClass,
];

export const ChildComponentDependencies = [
  NgbNavModule,
  NgbModalModule,
  CommonModule,
  ReactiveFormsModule,
  FormsModule,
  DatePipe,
  NgbToastModule,
  NgbAccordionModule,
];

@Component({
  standalone: true,
  template: '',
})
export class AbstractStockManagementComponent {
  service = inject(StockManagementViewService);
  list = inject(ListService);

  currentPage = computed(() => {
    const currentSkipCount = this.service.filters?.skipCount || 0;
    const currentMaxResultCount = this.service.filters?.maxResultCount || 10;
    return Math.floor(currentSkipCount / currentMaxResultCount) + 1;
  });

  onSelectedNavigate(data: MaterialDto): void {
    // Implement if needed
  }

  onPageChange(page: number): void {
    this.service.filters.skipCount = (page - 1) * (this.service.filters?.maxResultCount || 10);
    this.list.get();
  }

  openDetail(selected?: MaterialDto): void {
    // Implement if needed
  }
}
