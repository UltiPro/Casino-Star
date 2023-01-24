export class UserRegistration {
  constructor(public login: string, public email: string, public password: string, public c_password: string) { }
}

export interface PostUserRegistration {
  statusCode: boolean;
  message: string;
}