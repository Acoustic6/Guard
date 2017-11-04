import { OpaqueToken } from '@angular/core';

export let APP_CONFIG = new OpaqueToken('app.config');

export interface IConfig {
    apiEndpoint: string;
    endpoint: string;
    title: string;
}

export const CONFIG: IConfig = {
    apiEndpoint: 'api/',
    endpoint: 'localhost:5000/',
    title: 'Api Endpoint'
};
