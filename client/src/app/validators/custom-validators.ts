import { AbstractControl, FormGroup } from "@angular/forms";

export class CustomValidators {

    static passwordsMatch(control: FormGroup) {
        let password: any = control.get('password')?.value;
        const confirmPassword: any = control.get('confirmPassword')?.value;
    
        if(confirmPassword !== '' && password !== confirmPassword) {
            control.get('confirmPassword')?.setErrors({
                passwordsMatch: true,
            });
            return {
                passwordsMatch: true,
            };
        }
        return null;
    }
}