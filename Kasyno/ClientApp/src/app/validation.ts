import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function RegexLogin(): RegExp {
    return /^[A-Za-z][A-Za-z0-9_-]{1,13}[A-Za-z0-9]$/;
}

export function RegexEmail(): RegExp {
    return /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/;
}

export function RegexPassword(): RegExp {
    return /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,30}$/;
}

export function CheckPasswords(passwordControl: string, password2Control: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        return control.get(passwordControl)?.value == control.get(password2Control)?.value ? null : { error: "Passwords do not match!" };
    }
}