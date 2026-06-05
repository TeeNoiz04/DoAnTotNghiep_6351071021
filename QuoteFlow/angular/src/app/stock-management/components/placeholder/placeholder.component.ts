import { CommonModule, Location } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ListService } from '@abp/ng.core';
import { StockReportViewService } from '@app/stock-management/services/stock-report.service';

@Component({
  selector: 'app-report-stock-placeholder',
  standalone: true,
  imports: [CommonModule],
  providers: [StockReportViewService, ListService],
  template: `
    <div class="d-flex justify-content-center align-items-center p-5">
      <div class="text-center">
        <i class="bi bi-hourglass-split text-primary mb-3" style="font-size: 2rem"></i>
        <p class="text-muted">Preparing your report...</p>
      </div>
    </div>
  `,
})
export class ReportStockPlaceholderComponent implements OnInit {
  private readonly location = inject(Location);
  private readonly route = inject(ActivatedRoute);
  private stockReportService = inject(StockReportViewService);

  ngOnInit(): void {
    const reportType = this.route.snapshot.data['reportType'];

    if (reportType === 'inventory') {
      this.stockReportService.openInventoryReportDialog();
    }

    setTimeout(() => {
      this.location.back();
    }, 100);
  }
}
