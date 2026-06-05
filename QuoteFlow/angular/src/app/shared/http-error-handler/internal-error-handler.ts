import {
  Confirmation,
  ConfirmationService,
  CUSTOM_HTTP_ERROR_HANDLER_PRIORITY,
  CustomHttpErrorHandlerService,
  Toaster,
  ToasterService,
} from '@abp/ng.theme.shared';
import { HttpErrorResponse } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { from, of } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class InternalServerErrorHandlerService implements CustomHttpErrorHandlerService {
  readonly priority = CUSTOM_HTTP_ERROR_HANDLER_PRIORITY.veryHigh;

  protected readonly toasterService: ToasterService = inject(ToasterService);
  protected readonly router: Router = inject(Router);
  protected readonly confirmationService: ConfirmationService = inject(ConfirmationService);
  private error: HttpErrorResponse | undefined = undefined;

  canHandle(error: any): boolean {
    if (error instanceof HttpErrorResponse) {
      this.error = error;

      return [400, 404, 500, 403].includes(error.status);
    }
    return false;
  }

  execute() {
    if (!this.error) return;

    const options: Partial<Toaster.ToastOptions> = {
      life: 7000,
      sticky: false,
      closable: true,
      tapToDismiss: true,
    };

    const warningOptions: Partial<Confirmation.Options> = {
      yesText: 'OK',
      cancelText: '',
      dismissible: true,
      hideCancelBtn: true,
      hideYesBtn: false,
    };

    // Handle error body parsing (including Blob responses)
    this.parseErrorBody(this.error.error)
      .pipe(
        catchError(parseError => {
          console.error('Failed to parse error response:', parseError);
          return of(null);
        }),
      )
      .subscribe(errorBody => {
        this.handleParsedError(errorBody, options, warningOptions);
      });
  }

  private parseErrorBody(errorBody: any) {
    if (errorBody instanceof Blob) {
      return from(errorBody.text()).pipe(
        switchMap(text => {
          try {
            return of(JSON.parse(text));
          } catch (e) {
            console.error('Failed to parse JSON from Blob:', e);
            return of(null);
          }
        }),
      );
    }
    return of(errorBody);
  }

  private handleParsedError(
    errorBody: any,
    options: Partial<Toaster.ToastOptions>,
    warningOptions: Partial<Confirmation.Options>,
  ) {
    // Try current approach first
    let errorMessage = this.error?.error?.error?.message;
    let errorCode = this.error?.error?.error?.code;
    let errorDetails = this.error?.error?.error?.details;
    let message: string = '';
    let title: string = '';
    // Fallback to parsed errorBody if current approach didn't work
    if (!errorMessage && !errorCode && !errorDetails) {
      errorMessage = errorBody?.error?.message || errorBody?.message;
      errorCode = errorBody?.error?.code || errorBody?.code;
      errorDetails = errorBody?.error?.details || errorBody?.details;
    }

    let fallBackError: string = '';
    if (errorCode && this.isBypassed(errorCode)) {
      // If the error code is bypassed, we do not handle it here.
      return;
    }

    switch (this.error.status) {
      case 400:
        fallBackError =
          'No worries—it’s a temporary issue. Just close this modal and give it another try.';
        /* 
        The following errors were detected during validation.
 - Close date cannot exceed one year from today.
 */
        if (errorDetails?.length > 0) {
          errorDetails = errorDetails
            ?.replace('The following errors were detected during validation.', '')
            ?.replace(
              'AbpValidationException: ModelState is not valid! See ValidationErrors for details.',
              '',
            )
            ?.trim();

          this.confirmationService.warn(errorDetails, 'Please review', warningOptions);
        } else if (errorMessage?.length > 0) {
          this.confirmationService.warn(errorMessage, 'Please review', warningOptions);
        } else {
          this.confirmationService.warn(fallBackError, 'Please review', warningOptions);
        }
        break;
      case 403:
        fallBackError = 'You do not have permission to perform this action.';
        if (errorMessage == 'You do not have permission') {
          this.router.navigate(['error/', this.error.status]);
        } else if (errorMessage?.length > 0) {
          this.confirmationService.warn(errorMessage, 'Please review', warningOptions);
        } else {
          this.confirmationService.warn(fallBackError, 'Please review', warningOptions);
        }
        break;
      case 404:
        fallBackError = 'The requested resource was not found.';
        if (errorMessage?.length > 0) {
          this.confirmationService.warn(errorMessage, 'Please review', warningOptions);
        } else {
          this.confirmationService.warn(fallBackError, 'Please review', warningOptions);
        }
        break;
      case 500:
        message = 'Please retry or contact support if the problem persists.';
        title = 'Oops! Something went wrong';
        this.toasterService.error(message, title, options);
        break;
      default:
        message = this.error.error?.error?.details ?? this.error.error?.error?.message;
        title = 'Error';
        this.toasterService.error(message, title, options);
        break;
    }
  }

  private isBypassed(errorCode: string): boolean {
    // Define the error codes that should be bypassed
    const bypassedErrorCodes = [
      'QuoteFlow:10703001',
      'QuoteFlow:10603007', // Price mismatch warning for DPO import
      'QuoteFlow:10603008', // Material discontinued warning for DPO import
      // Add more error codes as needed
    ];

    return bypassedErrorCodes.includes(errorCode);
  }
}
