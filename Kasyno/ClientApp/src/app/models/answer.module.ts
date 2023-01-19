import { PostUserFull } from "./user/userFull.module";

export interface PostAnswer {
    statusCode: boolean;
    message: string;
}

export interface PostAnswerWithAngle {
    statusCode: boolean;
    message: string;
    number: number | null;
}

export interface PostAnswerArrayOfUsers {
    statusCode: boolean;
    message: Array<PostUserFull>;
}