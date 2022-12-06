export class UserLogin {
  constructor(public login: string, public password: string) { }
}
export interface PostUserLogin {
  statusCode: boolean;
  message: string;
}