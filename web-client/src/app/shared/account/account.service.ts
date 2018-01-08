import { Inject, Injectable } from '@angular/core';
import { Headers, Http, Response, RequestOptions } from '@angular/http';

import { APP_CONFIG, IConfig } from './../../app.config';
import { TokenService } from '../authentication/token.service';
import { Account } from './account.model';

@Injectable()
export class AccountService {
    constructor(
        @Inject(APP_CONFIG) private config: IConfig,
        private http: Http,
        private tokenService: TokenService) { }

    public create(account: Account) {
        let body = JSON.stringify(account);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        return this.http.post(this.userUrl() + 'create', body, options).map((response: Response) => response.json());
    }

    private userUrl() {
        return this.config.apiEndpoint + 'Account/';
    }
}
