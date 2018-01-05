import { Inject, Injectable } from '@angular/core';
import { Http, RequestOptions, Response } from '@angular/http';

import { PostsPage } from './posts-page.model';
import { APP_CONFIG, IConfig } from './../../app.config';
import { TokenService } from '../authentication/token.service';

@Injectable()
export class PostsService {
    constructor(
        @Inject(APP_CONFIG) private config: IConfig,
        private http: Http,
        private tokenService: TokenService) { }

    public getUserPostsByLoginWithPagination(login: string, pageNumber: number) {
        return this.http.get(this.userUrl() + 'PostsBy/' + login + '/page/' + pageNumber, this.tokenService.requestOptionsWithToken()).map((response: Response) => {
            let postsPage = new PostsPage();
            postsPage.fromJSON(response.json());
            return postsPage;
        });
    }

    private userUrl() {
        return this.config.apiEndpoint + 'Posts/';
    }
}
