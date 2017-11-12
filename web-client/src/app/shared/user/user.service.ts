import { Inject, Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';

import { User } from './user.model';
import { APP_CONFIG, IConfig } from './../../app.config';
import { TokenService } from '../authentication/token.service';

@Injectable()
export class UserService {
    constructor(
        @Inject(APP_CONFIG) private config: IConfig,
        private http: Http,
        private tokenService: TokenService) { }

    private userUrl() {
        return this.config.apiEndpoint + 'User/';
    }
}
