import type { GetMessagesInput, MessageCreateDto, MessageDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  apiName = 'Default';

  create = (input: MessageCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, MessageDto>(
      {
        method: 'POST',
        url: '/api/app/discussions',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/discussions/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, MessageDto>(
      {
        method: 'GET',
        url: `/api/app/discussions/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetMessagesInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<MessageDto>>(
      {
        method: 'GET',
        url: '/api/app/discussions',
        params: {
          filterText: input.filterText,
          userName: input.userName,
          fullName: input.fullName,
          sendTo: input.sendTo,
          comment: input.comment,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
