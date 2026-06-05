import { inject, provideAppInitializer } from '@angular/core';
import { ABP, RoutesService } from '@abp/ng.core';
import { APPLICATION_CATEGORY_BASE_ROUTES } from './application-category-base.routes';

export const APPLICATION_CATEGORIES_APPLICATION_CATEGORY_ROUTE_PROVIDER = [
  provideAppInitializer(() => {
    configureRoutes();
  }),
];

function configureRoutes() {
  const routesService = inject(RoutesService);
  const routes: ABP.Route[] = [...APPLICATION_CATEGORY_BASE_ROUTES];
  routesService.add(routes);
}
