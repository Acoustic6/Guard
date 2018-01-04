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

    constructor(private postsService: PostsService, private route: ActivatedRoute) {
        this.postsNav = this.range(this.postsPage.minPageNumber, this.postsPage.maxPageNumber);
        this.paramsLogin = this.route.snapshot.params['login'];
    }

    ngOnInit() {
        this.postsService.getUserPostsByLoginWithPagination(this.paramsLogin, 0).subscribe(
            data => {
                this.postsPage = data;
            },
            error => {
                console.log(error);
            });
    }

    private navigateTo(pageNumber: number) {
        this.postsPage.curentPageNumber = pageNumber;
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