import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root'
})
export class ValidationMessage {
    
    readonly NameMinMax: string = "Name must be 3 - 64 alphabets.";
    readonly EmailRequired: string = "Email is required.";
    readonly EmailInvalid: string = "Enter valid email.";
    readonly PasswordRequired: string = "Password is required.";
    readonly ContactRequired: string = "Contact number is required.";
    readonly ContactNumberRange: string = "Contact Number must be 6 to 16 digits long.";
    readonly CodeRequired: string = "Code is required.";
    readonly NameRequired: string = "Name is required.";
    readonly RoleRequired: string = "Role is required.";
    readonly LoginIdRequired: string ="Login id is required.";
    readonly min2Required: number = 3;
    readonly PositionRequired: string = "Position required.";
    readonly MenuRequired: string = "Menu required.";
    readonly TitleRequired: string = "Title is required.";
    readonly ContentTypeRequired: string = "Content type is required.";
    readonly DescriptionRange: string = "Description must be 6 to 16 digits long.";
    readonly HeightRequired: string = "Height is required.";
    readonly RowRequired: string = "Row is required.";
    readonly RowsCountRequired: string = "Row is required.";
    readonly EditPermissionRequired: string = "Edit Permission required.";
    readonly DeletePermissionRequired: string = "Delete Permission required.";
    readonly ViewPermissionRequired: string = "View Permission required.";


}

