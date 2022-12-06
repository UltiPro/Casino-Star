export class User {
  constructor(private id: number, private login: string, private email: string, private money: number, private admin: boolean) { }
  GetId = (): number => this.id;
  SetId = (id: number): any => this.id = id;
  GetName = (): string => this.login;
  SetName = (login: string): any => this.login = login;
  GetEmail = (): string => this.email;
  SetEmail = (email: string): any => this.email = email;
  GetMoney = (): number => this.money;
  SetMoney = (money: number): any => this.money = money;
  GetAdmin = (): boolean => this.admin;
  SetAdmin = (admin: boolean): any => this.admin = admin;
}
export interface PostUser {
  id: number,
  login: string,
  email: string,
  money: number,
  admin: boolean
}