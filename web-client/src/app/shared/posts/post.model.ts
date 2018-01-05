import { Account } from '../account/account.model';

export class Post {
    private id: string;
    private creationDate: Date;
    private content: string;
    private account: Account;

    public fromJSON(obj) {
        this.id = obj.Id;
        this.creationDate = obj.CreationDate;
        this.content = obj.Content;
        this.account = obj.Account;
    }
}