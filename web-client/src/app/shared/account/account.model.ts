import { User } from '../user/user.model';

export class Account {
    id: string;
    login: string;
    password: string;
    confirmationPassword: string;
    user: User;

    constructor(fields?: Partial<Account>) {
        this.user = new User();

        if (fields) {
            Object.assign(this, fields);
        }
    }

    fromJSON(obj) {
        this.id = obj.Id;
        this.login = obj.Login;

        let user = new User();
        user.fromJSON(obj.User);

        this.user = user;
    }
}
