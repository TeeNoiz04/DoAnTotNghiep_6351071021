import { inject, provideAppInitializer } from '@angular/core';
import { ABP, RoutesService } from '@abp/ng.core';
import { BUYER_BASE_ROUTES } from './buyer-base.routes';

export const BUYER_ROUTE_PROVIDER = [
  provideAppInitializer(() => {
    configureRoutes();
  }),
];

function configureRoutes() {
  const routesService = inject(RoutesService);
  const routes: ABP.Route[] = [...BUYER_BASE_ROUTES];
  routesService.add(routes);
}
