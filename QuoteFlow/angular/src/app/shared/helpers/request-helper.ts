import { RequestStatusTextMap } from '@app/requests-subcomponent/request-status/request-status.model';
import {
  RequestTypeRouteToRequestTypeMap,
  RequestTypeToRouteMap,
  RequestView,
} from '@app/requests/request/components/my-request.model';
import { RequestType } from '@proxy/requests';

export class RequestHelper {
  /**
   * Get route from request type
   * @param requestType
   * @param view default is detail view
   * @returns the request type to use in route
   */
  static getRouteFromRequestType(
    requestType: RequestType,
    view: RequestView = RequestView.Detail,
  ): string {
    // this is due to settlement and payment are using the same list view route
    if (view === RequestView.List && requestType === RequestType.PaymentSettlement) {
      return RequestTypeToRouteMap[RequestType.Payment];
    }
    if (view === RequestView.List && requestType === RequestType.NonExpenditureSettlement) {
      return RequestTypeToRouteMap[RequestType.NonExpenditurePayment];
    }
    return RequestTypeToRouteMap[requestType];
  }

  static getRequestTypeFromRequestTypeRoute(requestTypeRoute: string): RequestType | undefined {
    return RequestTypeRouteToRequestTypeMap[requestTypeRoute];
  }

  static getRequestTypeInText(requestType: RequestType): string {
    switch (requestType) {
      case RequestType.Purchase:
        return 'Purchase Request';
      case RequestType.BusinessTrip:
        return 'Business Trip Request';
      case RequestType.Entertainment:
        return 'Entertainment Request';
      case RequestType.Payment:
        return 'Payment Request';
      case RequestType.AdvancePayment:
        return 'Advance Payment';
      case RequestType.NonExpenditurePurchase:
        return 'Non-Expenditure Purchase';
      case RequestType.NonExpenditurePayment:
        return 'Non-Expenditure Payment';
      case RequestType.PaymentSettlement:
        return 'Payment Request';
      case RequestType.Promotion:
        return 'Promotion';
      case RequestType.ITRequest:
        return 'IT Request';
      case RequestType.ITTakeout:
        return 'IT Takeout';
      case RequestType.NonExpenditureSettlement:
        return 'Non-Expenditure Settlement';
      case RequestType.CarRequest:
        return 'Car Request';
      default:
        return 'General Request';
    }
  }

  static getStatusLabel(status: string): string {
    const newStatus = status.toUpperCase();
    const normalizedStatus = RequestStatusTextMap[newStatus] || newStatus;

    return normalizedStatus;
  }

  private static formatKey(key, action) {
    return action === 6
      ? key.replace(/([A-Z])/g, ' $1').trim()
      : key.replace(/([A-Z][a-z])/g, ' $1').trim();
  }
}
