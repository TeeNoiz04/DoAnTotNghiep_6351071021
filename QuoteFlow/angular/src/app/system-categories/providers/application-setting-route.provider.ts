import { inject, provideAppInitializer } from '@angular/core';
import { ABP, RoutesService } from '@abp/ng.core';
import { APPLICATION_SETTING_BASE_ROUTES } from './application-setting-base.routes';

export const APPLICATION_SETTING_ROUTE_PROVIDER = [
  provideAppInitializer(() => {
    configureRoutes();
  }),
];

function configureRoutes() {
  const routesService = inject(RoutesService);
  const routes: ABP.Route[] = [...APPLICATION_SETTING_BASE_ROUTES];
  routesService.add(routes);
  routesService.remove(['File Templates']);
}
