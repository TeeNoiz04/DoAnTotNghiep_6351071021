import { Environment } from '@abp/ng.core';

const baseUrl = 'https://192.168.200.52:44323';

const oAuthConfig = {
  issuer: 'https://192.168.200.52:44323/',
  redirectUri: baseUrl,
  clientId: 'QuoteFlow_App',
  responseType: 'code',
  scope: 'offline_access QuoteFlow',
  requireHttps: true,
};

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'QuoteFlow',
  },
  oAuthConfig,
  apis: {
    default: {
      url: '',
      rootNamespace: 'QuoteFlow',
    },
    AbpAccountPublic: {
      url: '',
      rootNamespace: 'AbpAccountPublic',
    },
  },

  features: {
    hideMenuItemNames: [],
  },
} as Environment;
