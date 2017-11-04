import { Inject, Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';

import { APP_CONFIG, IConfig } from '../app.config';

@Injectable()
export class AccountService {
    constructor(
        @Inject(APP_CONFIG) private config: IConfig,
        private http: Http) { }

    private accountUrl() {
        return this.config.apiEndpoint + 'Account/';
    }
}
