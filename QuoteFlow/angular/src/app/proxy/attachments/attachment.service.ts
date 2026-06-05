import type { FilesInput } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { IActionResult } from '../microsoft/asp-net-core/mvc/models';

@Injectable({
  providedIn: 'root',
})
export class AttachmentService {
  apiName = 'Default';

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/attachments/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  downloadFile = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, IActionResult>(
      {
        method: 'GET',
        url: '/api/app/attachments/download',
        params: { id },
      },
      { apiName: this.apiName, ...config },
    );

  uploadFile = (
    input: FilesInput,
    requestId: string,
    attachmentCode: string,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/attachments/upload',
        params: { requestId, attachmentCode },
        body: input.files,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
