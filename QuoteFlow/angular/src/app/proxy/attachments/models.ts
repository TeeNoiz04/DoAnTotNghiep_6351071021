import type { ExtendedAuditedEntityDto } from '../shared/models';
import type { IRemoteStreamContent } from '../volo/abp/content/models';

export interface AttachmentDto extends ExtendedAuditedEntityDto<string> {
  requestPart?: string;
  attachCode?: string;
  attachName?: string;
  fileName?: string;
  fileNameDB?: string;
  filePath?: string;
  offlineAttachment: boolean;
  description?: string;
  concurrencyStamp?: string;
}

export interface FilesInput {
  files: IRemoteStreamContent[];
}
