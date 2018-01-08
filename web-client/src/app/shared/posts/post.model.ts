import { Account } from '../account/account.model';
import { User } from '../user/index';

export class Post {
    public creationDate: Date;
    public content: string;
    public ownerLogin: string;
    public creatorLogin: string;
    public ownerUser: User;
    public creatorUser: User;

    public fromJSON(obj) {
        this.creationDate = obj.CreationDate && new Date(obj.CreationDate);
        this.content = obj.Content;
        this.ownerLogin = obj.ownerLogin;
        this.creatorLogin = obj.creatorLogin;

        let ownerUser = new User();
        obj.OwnerUser && ownerUser.fromJSON(obj.OwnerUser);

        let creatorUser = new User();
        obj.CreatorUser && creatorUser.fromJSON(obj.CreatorUser);

        this.ownerUser = ownerUser;
        this.creatorUser = creatorUser;
    }
}