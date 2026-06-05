import { ParamMap } from '@angular/router';
import { StringHelper } from './string-helper';

export class UrlHelper {
  static getIdFromRouteParamMap(paramMap: ParamMap): string | null {
    const idFromUrl = paramMap.get('id');
    return StringHelper.isGuid(idFromUrl) ? idFromUrl : null;
  }
}
