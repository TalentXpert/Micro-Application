import { Injectable } from '@angular/core';

@Injectable()
export class PatternService {

    email(): string {
       return "^[a-zA-Z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,10}$";
    }

    phoneNumber(): string {
        return "^[0-9]{1}[0-9]{5,15}";
    }

    sharableLinkCode() : string{
        return "^[a-z0-9]{6}$";
    }
    alaphabet(): string {
        return "^[a-zA-Z ]*$";
    }
    // typeahedPatternWithoutDigit(): string {
    //     return "([^0-9\"#$%&'()*+,./:;<=>?@\^_`{|}~-]*$)|[A-Za-z]";
    // }
    typeahedPatternWithDigit(): string {
        return "([^\"#$%&'()*+,./:;<=>?@\^_`{|}~-]*$)|[A-Za-z]";
    }
  
    alphaNumeric(): string {
        return "^(?=.*[a-zA-Z])[a-zA-Z0-9 ]+$";
    }

    numeric(): string {
        return "^-?[0-9]\\d*(\\.\\d{1,2})?$";
    }


    customTime(): string {
        return "^([0-9]{0,3})(:?[0-9]{0,2})$";
    }

    customTimeHour(): string {
        return "^[0-9]{0,3}$";
    }
    
    customTimeMinute(): string {
        return "^[0-9]{0,2}$";
    }

    time(): string {
        return "^[1-9][0-9]{0,2}$"
    }

    minute(): string {
        return "^[0-5]?[0-9]$"
    }

    hour(): string {
        return "^([ 01]?[0-9]|2[0-3])(:[0-5][0-9])?$"
    }

    percentage(): string {
        return "^(?:[1-9][0-9]?|100)$"
    }

    digit(): string {
        return "^[0-9]*$";
    }
    expPattern():string{
        return "^[0-9]{1,4}(\.([0-9]|10|11))?$"
    }

    decimalTwoDigit():string{
        return "^\d*(\.\d{0,2})?$"
    }

    invoicePattern():string{
        return "/^[ A-Za-z0-9_.-]*$/" ;
    }

    timeStampPattern():string{
        return "^(2[0-3]|[01]?[0-9]):([0-5]?[0-9])$" ;
    }
}
