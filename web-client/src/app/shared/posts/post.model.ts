import { User } from '../user/user.model';

export class Post {
    private name: string;
    private creationDate: Date;
    private content: string;
    private user: User;

    public fromJSON(obj) {
        this.name = obj.name;
        this.creationDate = obj.creationDate;
        this.content = obj.content;
        this.user = obj.user;
    }
}