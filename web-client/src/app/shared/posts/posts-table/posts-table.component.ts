import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Post } from '../post.model';
import { PostsService } from '../posts.service';
import { PostsPage } from '../index';

@Component({
    selector: 'posts-table',
    styleUrls: ['./posts-table.component.scss'],
    templateUrl: './posts-table.component.html'
})
export class PostsTableComponent implements OnInit {
    private postsPage: PostsPage = new PostsPage();
    private postsNav: number[];
    private paramsLogin: string;

    constructor(
        private postsService: PostsService,
        private route: ActivatedRoute) {
        this.postsNav = this.range(this.postsPage.minPageNumber, this.postsPage.maxPageNumber);
        this.paramsLogin = this.route.snapshot.params['login'];
    }

    ngOnInit() {
        this.updatePostsPage();
    }

    private updatePostsPage() {
        this.postsService.getUserPostsByLoginWithPagination(this.paramsLogin, this.postsPage.curentPageNumber).subscribe(data => {
            this.postsPage = data;
            this.postsNav = this.range(this.postsPage.minPageNumber, this.postsPage.maxPageNumber);
        }, error => {
            console.log(error);
        });
    }

    private navigateTo(pageNumber: number) {
        if (this.postsPage.curentPageNumber !== pageNumber) {
            this.postsPage.curentPageNumber = pageNumber;
            this.updatePostsPage();
        }
    }

    private range(start: number, end: number): number[] {
        let list = [];
        for (let i = start; i <= end; i++) {
            list.push(i);
        }

        return list;
    }

    private dateToString(date: Date) {
        if (!date) {
            return '';
        }
        return `${date.getDate()}.${date.getMonth()}.${date.getFullYear()}`;
    }
}