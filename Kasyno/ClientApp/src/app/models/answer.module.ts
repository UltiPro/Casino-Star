export interface PostAnswer {
    statusCode: boolean;
    message: string;
}

export interface PostAnswerWithAngle {
    statusCode: boolean;
    message: string;
    number: number | null;
}