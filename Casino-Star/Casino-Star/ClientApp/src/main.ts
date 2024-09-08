import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

export function getBaseUrl() {
  return document.getElementsByTagName('base')[0].href;
}

export function getBaseUrlUser() {
  return "http://localhost:5077/api/user";
}

export function getBaseUrlGames() {
  return "http://localhost:5077/api/games";
}

export function getBaseUrlAdmin() {
  return "http://localhost:5077/api/admin";
}

const providers = [
  { provide: 'BASE_URL', useFactory: getBaseUrl, deps: [] },
  { provide: 'BASE_URL_API_USER', useFactory: getBaseUrlUser, deps: [] },
  { provide: 'BASE_URL_API_GAME', useFactory: getBaseUrlGames, deps: [] }
];

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic(providers).bootstrapModule(AppModule)
  .catch(err => console.log(err));
