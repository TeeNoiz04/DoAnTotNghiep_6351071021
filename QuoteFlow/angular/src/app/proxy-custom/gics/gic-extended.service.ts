import { EnvironmentService, Rest, RestService } from '@abp/ng.core';
import { inject, Injectable } from '@angular/core';
import { BatchAutoUnlockStockDto } from '@proxy/dpos';
import { GICImportDto, GICImportInput, GICService } from '@proxy/gics';
import { ExcelValidationResult } from '@proxy/shared/excels';
import { Observable } from 'rxjs';
import { BatchUnlockProgressEventDto } from '../dpos/dpo-extended.models';

@Injectable({
  providedIn: 'root',
})
export class GICImportExtendedService extends GICService {
  private readonly environment = inject(EnvironmentService);

  constructor(private myRestService: RestService) {
    super(myRestService);
  }

  private buildFormData(file: File, input: GICImportInput): FormData {
    const formData = new FormData();

    formData.append('file', file);

    const defaultConfirmDate = new Date(Date.UTC(1970, 0, 1)).toISOString();

    formData.append('materialType', input.materialType ?? '');
    formData.append('confirmDate', input.confirmDate ?? defaultConfirmDate);
    formData.append('buyerId', input.buyerId ?? '');
    formData.append('buyerShortName', input.buyerShortName ?? '');
    formData.append('buyerTypeId', input.buyerTypeId ?? '');
    formData.append('buyerTypeDescription', input.buyerTypeDescription ?? '');
    formData.append('gicProcessDescription', input.gicProcessDescription ?? '');
    formData.append('gicTypeDescription', input.gicTypeDescription ?? '');
    formData.append('costCenter', input.costCenter ?? '');
    formData.append('refDoc', input.refDoc ?? '');
    formData.append('referenceDocDate', input.referenceDocDate ?? defaultConfirmDate);
    formData.append('note', input.note ?? '');

    return formData;
  }

  validateAndParseInternalManual(file: File, input: GICImportInput, config?: Partial<Rest.Config>) {
    const formData = this.buildFormData(file, input);

    return this.myRestService.request<any, ExcelValidationResult<GICImportDto>>(
      {
        method: 'POST',
        url: '/api/app/gic/validation-parse/internal',
        body: formData,
      },
      { apiName: 'Default', ...config },
    );
  }

  validateAndParseSponsorManual(file: File, input: GICImportInput, config?: Partial<Rest.Config>) {
    const formData = this.buildFormData(file, input);

    return this.myRestService.request<any, ExcelValidationResult<GICImportDto>>(
      {
        method: 'POST',
        url: '/api/app/gic/validation-parse/sponsor',
        body: formData,
      },
      { apiName: 'Default', ...config },
    );
  }

  validateAndParseWarrantyManual(file: File, input: GICImportInput, config?: Partial<Rest.Config>) {
    const formData = this.buildFormData(file, input);

    return this.myRestService.request<any, ExcelValidationResult<GICImportDto>>(
      {
        method: 'POST',
        url: '/api/app/gic/validation-parse/warranty',
        body: formData,
      },
      { apiName: 'Default', ...config },
    );
  }

  validateAndParseWriteOffManual(file: File, input: GICImportInput, config?: Partial<Rest.Config>) {
    const formData = this.buildFormData(file, input);
    return this.myRestService.request<any, ExcelValidationResult<GICImportDto>>(
      {
        method: 'POST',
        url: '/api/app/gic/validation-parse/write-off',
        body: formData,
      },
      { apiName: 'Default', ...config },
    );
  }

  /**
   * Batch auto-unlock stock with real-time progress tracking via SSE
   * @param request The batch unlock request with GIC detail IDs
   * @returns Observable stream of progress events
   */
  batchAutoUnlockStockWithProgress(
    request: BatchAutoUnlockStockDto,
  ): Observable<BatchUnlockProgressEventDto> {
    return new Observable(observer => {
      const apiUrl = this.environment.getApiUrl('Default');
      const token = localStorage.getItem('access_token');
      const xsrfToken = this.getXsrfToken();
      if (!token) {
        observer.error(new Error('Authentication token not found'));
        return;
      }

      const headers: Record<string, string> = {
        'Content-Type': 'application/json',
        Accept: 'text/event-stream',
        Authorization: `Bearer ${token}`,
      };

      // Add XSRF token if available (required by ABP antiforgery validation)
      if (xsrfToken) {
        headers['RequestVerificationToken'] = xsrfToken;
      }

      // Use fetch API for SSE support
      fetch(`${apiUrl}/api/app/gic/details/batch-auto-unlock`, {
        method: 'POST',
        headers,
        body: JSON.stringify(request),
      })
        .then(response => {
          if (!response.ok) {
            response
              .json()
              .then(errorData => {
                observer.error(errorData);
              })
              .catch(() => {
                observer.error(new Error(`HTTP ${response.status}: ${response.statusText}`));
              });
            return;
          }

          const reader = response.body!.getReader();
          const decoder = new TextDecoder();
          let buffer = '';

          const processStream = () => {
            reader
              .read()
              .then(({ done, value }) => {
                if (done) {
                  observer.complete();
                  return;
                }

                buffer += decoder.decode(value, { stream: true });
                const lines = buffer.split('\n');

                // Keep the last incomplete line in the buffer
                buffer = lines.pop() || '';

                for (const line of lines) {
                  if (line.startsWith('data: ')) {
                    try {
                      const data = JSON.parse(line.substring(6));
                      observer.next(data);
                    } catch (error) {
                      console.error('Error parsing SSE data:', error, 'Line:', line);
                    }
                  }
                }

                processStream();
              })
              .catch(error => {
                observer.error(error);
              });
          };

          processStream();
        })
        .catch(error => {
          observer.error(error);
        });

      // Cleanup on unsubscribe
      return () => {
        // Connection will close when component unsubscribes
      };
    });
  }

  batchAutoUnlockOnOrderStockWithProgress(
    request: BatchAutoUnlockStockDto,
  ): Observable<BatchUnlockProgressEventDto> {
    return new Observable(observer => {
      const apiUrl = this.environment.getApiUrl('Default');
      const token = localStorage.getItem('access_token');
      const xsrfToken = this.getXsrfToken();
      if (!token) {
        observer.error(new Error('Authentication token not found'));
        return;
      }

      const headers: Record<string, string> = {
        'Content-Type': 'application/json',
        Accept: 'text/event-stream',
        Authorization: `Bearer ${token}`,
      };

      // Add XSRF token if available (required by ABP antiforgery validation)
      if (xsrfToken) {
        headers['RequestVerificationToken'] = xsrfToken;
      }

      // Use fetch API for SSE support
      fetch(`${apiUrl}/api/app/gic/details/batch-auto-unlock-on-order-stock`, {
        method: 'POST',
        headers,
        body: JSON.stringify(request),
      })
        .then(response => {
          if (!response.ok) {
            response
              .json()
              .then(errorData => {
                observer.error(errorData);
              })
              .catch(() => {
                observer.error(new Error(`HTTP ${response.status}: ${response.statusText}`));
              });
            return;
          }

          const reader = response.body!.getReader();
          const decoder = new TextDecoder();
          let buffer = '';

          const processStream = () => {
            reader
              .read()
              .then(({ done, value }) => {
                if (done) {
                  observer.complete();
                  return;
                }

                buffer += decoder.decode(value, { stream: true });
                const lines = buffer.split('\n');

                // Keep the last incomplete line in the buffer
                buffer = lines.pop() || '';

                for (const line of lines) {
                  if (line.startsWith('data: ')) {
                    try {
                      const data = JSON.parse(line.substring(6));
                      observer.next(data);
                    } catch (error) {
                      console.error('Error parsing SSE data:', error, 'Line:', line);
                    }
                  }
                }

                processStream();
              })
              .catch(error => {
                observer.error(error);
              });
          };

          processStream();
        })
        .catch(error => {
          observer.error(error);
        });

      // Cleanup on unsubscribe
      return () => {
        // Connection will close when component unsubscribes
      };
    });
  }

  /**
   * Reads XSRF token from browser cookie
   * ABP uses 'XSRF-TOKEN' cookie for antiforgery validation
   * @returns XSRF token value or null if not found
   */
  private getXsrfToken(): string | null {
    const name = 'XSRF-TOKEN=';
    const decodedCookie = decodeURIComponent(document.cookie);
    const cookieArray = decodedCookie.split(';');

    for (let cookie of cookieArray) {
      cookie = cookie.trim();
      if (cookie.indexOf(name) === 0) {
        return cookie.substring(name.length);
      }
    }
    return null;
  }
}
