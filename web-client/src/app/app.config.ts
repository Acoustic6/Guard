import { OpaqueToken } from '@angular/core';

export let APP_CONFIG = new OpaqueToken('app.config');

export interface IConfig {
    apiEndpoint: string;
    title: string;
}

export const CONFIG: IConfig = {
    apiEndpoint: 'api/',
    title: 'Api Endpoint'
};
