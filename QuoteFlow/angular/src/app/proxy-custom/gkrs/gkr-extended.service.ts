import { EnvironmentService, Rest, RestService } from '@abp/ng.core';
import { inject, Injectable } from '@angular/core';
import { BatchAutoUnlockStockDto } from '@proxy/dpos/models';
import { GKRImportDto, GKRImportInput, GKRService } from '@proxy/gkrs';
import { ExcelValidationResult } from '@proxy/shared/excels';
import { Observable } from 'rxjs';
import { BatchUnlockProgressEventDto } from '../dpos/dpo-extended.models';

@Injectable({
  providedIn: 'root',
})
export class GKRExtendedService extends GKRService {
  private readonly environment = inject(EnvironmentService);

  constructor(private myRestService: RestService) {
    super(myRestService);
  }

  validateAndParseGKRManual(file: File, input: GKRImportInput, config?: Partial<Rest.Config>) {
    const formData = new FormData();

    formData.append('file', file);
    formData.append('materialType', input.materialType);
    if (input.buyerId) formData.append('buyerId', input.buyerId);
    if (input.buyerTypeId) formData.append('buyerTypeId', input.buyerTypeId);
    formData.append('confirmDate', input.confirmDate);
    formData.append('expirationDate', input.expirationDate);
    if (input.salePicUsername) formData.append('salePicUsername', input.salePicUsername);
    if (input.reason) formData.append('reason', input.reason);

    return this.myRestService.request<any, ExcelValidationResult<GKRImportDto>>(
      {
        method: 'POST',
        url: '/api/app/gkr/validation-parse/gkr',
        body: formData,
      },
      { apiName: this.apiName, ...config },
    );
  }

  /**
   * Batch auto-unlock stock with real-time progress tracking via SSE
   * @param request The batch unlock request with GKR detail IDs
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
      fetch(`${apiUrl}/api/app/gkr/details/batch-auto-unlock`, {
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
      fetch(`${apiUrl}/api/app/gkr/details/batch-auto-unlock-on-order-stock`, {
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
