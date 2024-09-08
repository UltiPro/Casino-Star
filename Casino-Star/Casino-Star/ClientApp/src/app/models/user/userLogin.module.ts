export class UserLogin {
  constructor(public login: string, public password: string) { }
}

export interface PostUserLogin {
  statusCode: boolean;
  message: string;
}

export interface PostUserLoginSuccess {
  statusCode: boolean;
  message: {
    token: string,
    user: {
      id: number,
      login: string,
      email: string,
      money: number,
      admin: boolean
    }
  }
}