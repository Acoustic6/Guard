import { Component, Input, OnInit } from '@angular/core';

import { Post } from '../post.model';
import { PostsService } from '../posts.service';
import { PostsPage } from '../index';
import { Account } from '../../account/index';

@Component({
    selector: 'posts-table',
    styleUrls: ['./posts-table.component.scss'],
    templateUrl: './posts-table.component.html'
})
export class PostsTableComponent implements OnInit {
    @Input() monitoredUserLogin: string;

    private filter: string = '';
    private isSearchEnabled: boolean = false;
    private postsNav: number[];
    private newPost: Post = new Post();
    private postsPage: PostsPage = new PostsPage();

    constructor(private postsService: PostsService) {
        this.postsNav = this.range(this.postsPage.minPageNumber, this.postsPage.maxPageNumber);
    }

    ngOnInit() {
        this.newPost.ownerLogin = this.monitoredUserLogin;
        this.updatePostsPage();
    }

    private updatePostsPage() {
        this.postsService.getPosts(this.monitoredUserLogin, this.postsPage.curentPageNumber, this.filter)
            .subscribe(
            data => {
                this.postsPage = data;
                this.postsNav = this.range(this.postsPage.minPageNumber, this.postsPage.maxPageNumber);
            },
            error => {
                console.log(error);
            });
    }

    private createPost() {
        if (this.newPost.content) {
            this.postsService.createPost(this.newPost, this.postsPage.curentPageNumber, this.filter)
                .subscribe(
                data => {
                    this.postsPage = data;
                    this.postsNav = this.range(this.postsPage.minPageNumber, this.postsPage.maxPageNumber);
                    this.newPost.content = '';
                },
                error => {
                    console.log(error);
                    this.newPost.content = '';
                });
        }
    }

    private search() {
        this.isSearchEnabled = true;
        this.updatePostsPage();
    }

    private clearSearch() {
        this.filter = '';
        this.updatePostsPage();
        this.isSearchEnabled = false;
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
}