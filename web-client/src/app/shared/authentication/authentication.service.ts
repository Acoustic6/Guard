import { Inject, Injectable } from '@angular/core';
import { Headers, Http, Response, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import { APP_CONFIG, IConfig } from '../../app.config';
import { TokenService } from './token.service';

@Injectable()
export class AuthenticationService {
    constructor(
        @Inject(APP_CONFIG) private config: IConfig,
        private http: Http,
        private tokenService: TokenService) { }

    public login(login: string, password: string) {
        let body = new URLSearchParams();
        body.set('login', login);
        body.set('password', password);

        return this.http.post(this.authenticationUrl() + 'token', body.toString(), this.tokenService.requestOptions())
            .map((response: Response) => this.tokenService.updateToken(response.json()));
    }

    public logout() {
        this.tokenService.removeToken();
    }

    private authenticationUrl() {
        return this.config.apiEndpoint + 'Account/';
    }
}
