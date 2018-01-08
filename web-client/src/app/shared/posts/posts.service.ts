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

    public getUserPostsByLoginWithPagination(login: string, pageNumber: number) {
        return this.http.get(this.userUrl() + 'by/' + login + '/page/' + pageNumber, this.tokenService.requestOptionsWithToken())
            .map((response: Response) => {
                let postsPage = new PostsPage();
                postsPage.fromJSON(response.json());
                return postsPage;
            });
    }

    public createPost(post: Post, targetPageNumber: number) {
        let body = new URLSearchParams();
        body.set('post', JSON.stringify(post));
        body.set('targetPage', targetPageNumber.toString());

        return this.http.post(this.userUrl() + 'create', body.toString(), this.tokenService.requestOptionsWithToken())
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
