import { inject, provideAppInitializer } from '@angular/core';
import { ABP, RoutesService } from '@abp/ng.core';
import { PRICE_OFFER_BASE_ROUTES } from './price-offer-base.routes';

export const PRICE_OFFERS_PRICE_OFFER_ROUTE_PROVIDER = [
  provideAppInitializer(() => {
    configureRoutes();
  }),
];

function configureRoutes() {
  const routesService = inject(RoutesService);
  const routes: ABP.Route[] = [...PRICE_OFFER_BASE_ROUTES];
  routesService.add(routes);
}
