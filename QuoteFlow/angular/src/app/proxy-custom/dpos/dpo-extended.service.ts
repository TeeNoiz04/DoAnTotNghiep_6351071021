import { EnvironmentService, Rest, RestService } from '@abp/ng.core';
import { inject, Injectable } from '@angular/core';
import { BatchAutoUnlockStockDto, DPOService, ImportDPODto, ImportDPOInput } from '@proxy/dpos';
import { ExcelValidationResult } from '@proxy/shared/excels';
import { Observable } from 'rxjs';
import { BatchUnlockProgressEventDto } from './dpo-extended.models';

@Injectable({
  providedIn: 'root',
})
export class DPOExtendedService extends DPOService {
  private readonly environment = inject(EnvironmentService);

  constructor(private myRestService: RestService) {
    super(myRestService);
  }

  validateAndParseDPOManual(file: File, input: ImportDPOInput, config?: Partial<Rest.Config>) {
    const formData = new FormData();

    formData.append('file', file);
    formData.append('materialType', input.materialType);
    formData.append('buyerId', input.buyerId);
    formData.append('buyerTypeId', input.buyerTypeId);
    formData.append('confirmDate', input.confirmDate);

    return this.myRestService.request<any, ExcelValidationResult<ImportDPODto>>(
      {
        method: 'POST',
        url: '/api/app/dpos/validate-and-parse/dpo',
        body: formData,
      },
      { apiName: this.apiName, ...config },
    );
  }

  /**
   * Batch auto-unlock stock with real-time progress tracking via SSE
   * @param request The batch unlock request with DPO detail IDs
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

      // Build headers with XSRF token for antiforgery validation
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
      fetch(`${apiUrl}/api/app/dpos/details/batch-auto-unlock`, {
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

      // Build headers with XSRF token for antiforgery validation
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
      fetch(`${apiUrl}/api/app/dpos/details/batch-auto-unlock-on-order-stock`, {
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
