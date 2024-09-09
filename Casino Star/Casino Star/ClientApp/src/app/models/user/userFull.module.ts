import { User } from "./user.module";

export class UserFull extends User {
    constructor(protected id: number, protected login: string, protected email: string, protected money: number, protected admin: boolean, private active: boolean, private banned: boolean) {
        super(id, login, email, money, admin);
    }
    GetActive = (): boolean => this.active;
    SetActive = (active: boolean): any => this.active = active;
    GetBanned = (): boolean => this.banned;
    SetBanned = (banned: boolean): any => this.banned = banned;
}

export interface PostUserFull {
    id: number,
    login: string,
    email: string,
    money: number,
    admin: boolean,
    active: boolean,
    banned: boolean
}