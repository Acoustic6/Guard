import { Post } from './post.model';

export class PostsPage {
    public posts: Post[] = new Array<Post>();
    public curentPageNumber: number = 0;
    public minPageNumber: number = 0;
    public maxPageNumber: number = 8;

    public fromJSON(obj) {
        this.posts = obj.posts;
        this.curentPageNumber = obj.curentPageNumber;
        this.minPageNumber = obj.minPageNumber;
        this.maxPageNumber = obj.maxPageNumber;
    }
}