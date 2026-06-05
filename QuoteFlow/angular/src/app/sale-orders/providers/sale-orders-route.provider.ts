import { ABP, RoutesService } from '@abp/ng.core';
import { inject, provideAppInitializer } from '@angular/core';
import { SALE_ORDERS_MANAGEMENT_BASE_ROUTES } from './sale-orders-base.routes';

export const SALE_ORDERS_MANAGEMENT_ROUTE_PROVIDER = [
  provideAppInitializer(() => {
    configureRoutes();
  }),
];

function configureRoutes() {
  const routesService = inject(RoutesService);
  const routes: ABP.Route[] = [...SALE_ORDERS_MANAGEMENT_BASE_ROUTES];
  routesService.add(routes);
}
