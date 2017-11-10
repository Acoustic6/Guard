export class User {
    id: string;
    firstName: string;
    lastName: string;
    givenName: string;
    email: string;
    birthday: Date;

    constructor(fields?: Partial<User>) {
        if (fields) {
            Object.assign(this, fields);
        }
    }

    fromJSON(obj) {
        this.id = obj.Id;
        this.firstName = obj.FirstName;
        this.lastName = obj.LastName;
        this.givenName = obj.GivenName;
        this.email = obj.Email;
        this.birthday = obj.Birthday;
    }
}