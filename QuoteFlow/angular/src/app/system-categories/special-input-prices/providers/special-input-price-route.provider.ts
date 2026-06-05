import { inject, provideAppInitializer } from '@angular/core';
import { ABP, RoutesService } from '@abp/ng.core';
import { SPECIAL_INPUT_PRICE_BASE_ROUTES } from './special-input-price-base.routes';

export const SPECIAL_INPUT_PRICE_ROUTE_PROVIDER = [
  provideAppInitializer(() => {
    configureRoutes();
  }),
];

function configureRoutes() {
  const routesService = inject(RoutesService);
  const routes: ABP.Route[] = [...SPECIAL_INPUT_PRICE_BASE_ROUTES];
  routesService.add(routes);
  routesService.remove(['File Templates']);
}
