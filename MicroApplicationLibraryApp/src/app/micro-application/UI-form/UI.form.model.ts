import { ControlValue, SmartAction } from "../application-page-container/application.page.model";

export class UIFormClientVM{
    SmartAction: SmartAction = new SmartAction();
    DataKey: string="";
    globalControls: ControlValue[] = [];
}