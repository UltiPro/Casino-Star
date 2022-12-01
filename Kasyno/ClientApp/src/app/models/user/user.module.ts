export class User {
  constructor(private id: number, private login: string, private email: string, private money: number, private admin: boolean) { }
  GetId = (): number => this.id;
  GetName = (): string => this.login;
  GetEmail = (): string => this.email;
  SetEmail = (email: string): any => this.email = email;
  GetMoney = (): number => this.money;
  GetAdmin = (): boolean => this.admin;
}