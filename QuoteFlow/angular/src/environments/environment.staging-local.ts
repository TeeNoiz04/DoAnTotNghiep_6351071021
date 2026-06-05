import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

const oAuthConfig = {
  issuer: 'https://192.168.200.52:44321/',
  redirectUri: baseUrl,
  clientId: 'QuoteFlow_App',
  responseType: 'code',
  scope: 'offline_access QuoteFlow',
  requireHttps: true,
};

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'QuoteFlow',
  },
  oAuthConfig,
  apis: {
    default: {
      url: 'https://192.168.200.52:44321',
      rootNamespace: 'QuoteFlow',
    },
    AbpAccountPublic: {
      url: 'https://192.168.200.52:44321',
      rootNamespace: 'AbpAccountPublic',
    },
  },
} as Environment;
