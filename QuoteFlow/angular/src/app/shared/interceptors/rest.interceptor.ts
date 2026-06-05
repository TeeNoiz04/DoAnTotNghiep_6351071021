import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { finalize, tap } from 'rxjs/operators';
import { LoadingService } from '../services/loading/loading.service';

export const exportPatterns = [
  '/key-account-evaluations/as-excel-file',
  '/stock-imports/export',
  '/stock-imports/as-list-excel/export-stock-import-allocation',
  '/stock-imports/invoice-sap',
  '/stock-imports/import',
  '/attachments/download',
  '/key-accounts/report-keyAccount-classification',
  '/templates/stock-import-priority-template',
  '/cargos/cargo-report/export',
  '/templates/cargo-template',
  '/templates/dpo-template',
  '/templates/po-template',
  '/templates/price-offers/add-more-items-template',
  '/templates/price-offers/change-item-properties-template',
  '/templates/price-offers/import-template',
  '/templates/so-template',
  '/templates/stock-import-priority-template',
  '/templates/special-input-price',
];

@Injectable()
export class RestInterceptor implements HttpInterceptor {
  constructor(private loadingService: LoadingService) {}

  intercept(req: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    if (this.shouldShowLoading(req.url) || this.isExportRequest(req)) {
      this.loadingService.show();
    }

    return next.handle(req).pipe(
      tap(() => {
        // if (event instanceof HttpResponse) {}
      }),
      finalize(() => {
        if (this.shouldShowLoading(req.url) || this.isExportRequest(req)) {
          this.loadingService.hide();
        }
      }),
    );
  }
  private shouldShowLoading(url: string): boolean {
    return false;
  }

  private isExportRequest(req: HttpRequest<unknown>): boolean {
    const url = req.url.toLowerCase();
    const hasExportPattern = exportPatterns.some(pattern => url.includes(pattern));
    const acceptHeader = req.headers.get('Accept');
    const isFileRequest =
      acceptHeader &&
      (acceptHeader.includes('application/vnd.ms-excel') ||
        acceptHeader.includes('application/vnd.openxmlformats-officedocument') ||
        acceptHeader.includes('application/pdf') ||
        acceptHeader.includes('application/octet-stream'));

    return hasExportPattern || isFileRequest;
  }
}
