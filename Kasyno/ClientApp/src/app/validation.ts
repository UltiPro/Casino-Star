export function CheckLogin(login: string) {
    const loginRegex = /^[A-Za-z][A-Za-z0-9_-]{1,13}[A-Za-z0-9]$/;
    if ((login.length < 3 || login.length > 15) || (!(loginRegex.test(login)))) return false;
    return true;
}

export function CheckEmail(email: string) {
    const emailRegex = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/;
    if ((email.length < 3 || email.length > 320) || (!(emailRegex.test(email)))) return false;
    return true;
}

export function CheckPassword(password: string) {
    const passwordRegex = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,30}$/;
    if ((password.length < 8 || password.length > 30) || (!(passwordRegex.test(password)))) return false;
    return true;
}

export function CheckPasswords(password: string, password2: string) {
    if ((password != password2) || !CheckPassword(password) || !CheckPassword(password2)) return false;
    return true;
}