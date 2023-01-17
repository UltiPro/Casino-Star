import { User } from "oidc-client";

export interface PostAnswer {
    statusCode: boolean;
    message: string;
}

export interface PostAnswerArrayOfUsers {
    statusCode: boolean;
    message: Array<User>;
}

export interface PostAnswerWithAngle {
    statusCode: boolean;
    message: string;
    number: number | null;
}