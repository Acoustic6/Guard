import { Inject, Injectable } from '@angular/core';
import { Http, RequestOptions, Response } from '@angular/http';

import { PostsPage } from './posts-page.model';
import { APP_CONFIG, IConfig } from './../../app.config';
import { TokenService } from '../authentication/token.service';
import { Post } from './index';

@Injectable()
export class PostsService {
    constructor(
        @Inject(APP_CONFIG) private config: IConfig,
        private http: Http,
        private tokenService: TokenService) { }

    public getPosts(ownerLogin: string, pageNumber: number, filter: string = '') {
        let body = new URLSearchParams();
        body.set('login', ownerLogin);
        body.set('pageNumber', pageNumber.toString());
        body.set('filter', filter);

        return this.http.post(this.userUrl(), body.toString(), this.tokenService.requestOptionsWithToken())
            .map((response: Response) => {
                let postsPage = new PostsPage();
                postsPage.fromJSON(response.json());
                return postsPage;
            });
    }

    public createPost(post: Post, targetPageNumber: number, filter: string = '') {
        let body = new URLSearchParams();
        body.set('post', JSON.stringify(post));
        body.set('targetPage', targetPageNumber.toString());
        body.set('filter', filter);

        return this.http.put(this.userUrl(), body.toString(), this.tokenService.requestOptionsWithToken())
            .map((response: Response) => {
                let postsPage = new PostsPage();
                postsPage.fromJSON(response.json());
                return postsPage;
            });
    }

    private userUrl() {
        return this.config.apiEndpoint + 'Posts/';
    }
}
