import { Rest, RestService } from '@abp/ng.core';
import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { AttachmentDto, AttachmentService } from '@proxy/attachments';

export enum AttachmentSource {
  KeyAccount = 'KeyAccount',
  PriceOffer = 'PriceOffer',
}

@Injectable({
  providedIn: 'root',
})
export class UploadProxyService extends AttachmentService {
  protected myRestService: RestService = inject(RestService);
  protected readonly http: HttpClient = inject(HttpClient);

  uploadFilewithData = (
    files: File[],
    requestId: string,
    attachmentCode: AttachmentSource,
    config?: Partial<Rest.Config>,
  ) =>
    this.myRestService.request<any, AttachmentDto[]>(
      {
        method: 'POST',
        url: '/api/app/attachments/upload',
        body: this.createFormData(files, requestId, attachmentCode),
      },
      { apiName: this.apiName, ...config },
    );

  private createFormData(
    files: File[],
    requestId: string,
    attachmentCode: AttachmentSource,
  ): FormData {
    const formData = new FormData();
    files.forEach(file => formData.append('input.files', file));
    formData.append('requestId', requestId);
    formData.append('attachmentCode', attachmentCode.toString());
    return formData;
  }

  deleteFile = (id: string, config?: Partial<Rest.Config>) =>
    this.myRestService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/attachments/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  downloadFile = (id: string, config?: Partial<Rest.Config>) =>
    this.myRestService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/attachments/download',
        params: { id },
      },
      { apiName: this.apiName, ...config },
    );

  downloadFileExtended = (id: string, config?: Partial<Rest.Config>) =>
    this.myRestService.request<Blob, any>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/attachments/download',
        params: { id },
      },
      { apiName: this.apiName, ...config },
    );
}
