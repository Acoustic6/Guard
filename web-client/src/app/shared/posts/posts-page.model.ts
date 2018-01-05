import { Post } from './post.model';

export class PostsPage {
    public posts: Post[] = new Array<Post>();
    public curentPageNumber: number = 0;
    public minPageNumber: number = 0;
    public maxPageNumber: number = 0;

    public fromJSON(obj) {
        this.posts = obj.Posts ? obj.Posts.map(p => { let r = new Post(); r.fromJSON(p); return r; }) : obj.Posts;
        this.curentPageNumber = obj.CurentPageNumber;
        this.minPageNumber = obj.MinPageNumber;
        this.maxPageNumber = obj.MaxPageNumber;
    }
}