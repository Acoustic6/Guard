import { Inject, Injectable } from '@angular/core';
import { Http, RequestOptions, Response } from '@angular/http';

import { User } from './user.model';
import { APP_CONFIG, IConfig } from './../../app.config';
import { TokenService } from '../authentication/token.service';

@Injectable()
export class UserService {
    constructor(
        @Inject(APP_CONFIG) private config: IConfig,
        private http: Http,
        private tokenService: TokenService) { }

    public getUserByLogin() {
        return this.http.get(this.userUrl() + 'UserByLogin/' + this.tokenService.userLogin(), this.tokenService.requestOptionsWithToken()).map((response: Response) => {
            let user = new User();
            user.fromJSON(response.json());
            return user;
        });
    }

    private userUrl() {
        return this.config.apiEndpoint + 'User/';
    }
}
