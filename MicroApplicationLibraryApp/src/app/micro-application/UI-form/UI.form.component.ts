import { Component, OnInit, OnDestroy, Output, EventEmitter, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ApplicationPageService } from '../services/application.page.service';
import { DateVM, UIControl, UIForm, SmartFormGenerateRequest, SmartFormTemplateRequest, SmartControlOption } from '../application-page-container/application.page.model';
import { UtilityService } from '../library/utility.service';
import { UIFormClientVM } from './UI.form.model';

@Component({
    selector: 'app-UIForm',
    templateUrl: './UI.Form.component.html',
    standalone:false
})

export class UIFormComponent implements OnInit, OnDestroy {
      observableSubscription: Subscription | null = null;;
    smartFb: FormGroup = new FormGroup({});
    @Input() UIFormClientVM: UIFormClientVM = new UIFormClientVM();
    uIForm: UIForm= new UIForm();
    @Output() IsFormResponseSaved: EventEmitter<boolean> = new EventEmitter();
    selectedDate: string = "";
    date = new Date();
    uIControl: UIControl[] = [];

    constructor(public ngbActiveModal: NgbActiveModal,  private applicationPageService: ApplicationPageService,
        public utilsService: UtilityService) {

    }


    ngOnInit() {
        this.processFormGenerateRequest();
    }

    processFormGenerateRequest() {
        if (this.UIFormClientVM !=undefined && this.UIFormClientVM.SmartAction) {
        let smartFormGenerateRequest = new SmartFormGenerateRequest(this.UIFormClientVM.SmartAction, this.UIFormClientVM.DataKey);
        if (this.UIFormClientVM.globalControls.length > 0) {
            this.UIFormClientVM.globalControls.forEach(element => {
                smartFormGenerateRequest.GlobalControls.push(element);
            })
        }
        this.observableSubscription = this.applicationPageService.processFormGenerateRequest(smartFormGenerateRequest).subscribe(UIForm => {
            this.uIForm = UIForm;
            this.uIForm.UIControls.forEach(e => { this.uIControl.push(new UIControl(e)) })
            this.setValueOfMultipleSelection();

        },
            error => {
                this.close();
                throw error;
            });
        }
    }


    createForm() {
        const group: any = {};
        for (var control of this.uIControl) {
            if (control.IsMandatory && (control.Minimum || control.Maximum)) {
                group[control.DisplayLabel] = new FormControl(control.Value || '', [Validators.required, Validators.minLength(control.Minimum? control.Minimum:0), Validators.maxLength(control.Maximum)]);
            } else if (!control.IsMandatory && (control.Minimum || control.Maximum)) {
                group[control.DisplayLabel] = new FormControl(control.Value, [Validators.minLength(control.Minimum? control.Minimum:0), Validators.maxLength(control.Maximum)]);
            }
            else if (control.ControlType == 'DatePicker') {
                let value;
                if (control.Value) {
                    value = this.toDateVM(control.Value);
                };
                if (control.IsMandatory) group[control.DisplayLabel] = new FormControl(value, Validators.required);
                else group[control.DisplayLabel] = new FormControl(value);
            }
            else if (control.IsMandatory) group[control.DisplayLabel] = new FormControl(control.Value, Validators.required);
            else group[control.DisplayLabel] = new FormControl(control.Value);

        }

        this.smartFb = new FormGroup(group);
    };


    toDateVM(calenderFormatDate: any) {
        let dateVM = new DateVM();
        let dateString = calenderFormatDate.split("-");
        dateVM.Year = parseInt(dateString[0]);
        dateVM.Month = parseInt(dateString[1]);
        dateVM.Day = parseInt(dateString[2]);
        this.selectedDate = this.utilsService.convertDateVMToReadableFormat(dateVM);
        return {
            date: {
                year: dateVM.Year,
                month: dateVM.Month,
                day: dateVM.Day
            }
        }
    }

    setValueOfMultipleSelection() {
        this.uIControl.forEach(x => {
            if (x.ControlType == "MultipleSelection" && x.Value) {
                let options = x.Value.split(',');
                for (var i = 0; i < options.length; i++) {
                    x.ListOfSelectedValue.push(options[i]);
                }
            }
            this.createForm();
        })

    }

    save() {
        let formValues = this.smartFb.value;
        let smartFormTemplateRequest = new SmartFormTemplateRequest(this.UIFormClientVM.SmartAction, this.uIControl, formValues);
        if (this.UIFormClientVM.DataKey) smartFormTemplateRequest.DataKey = this.UIFormClientVM.DataKey;
        this.observableSubscription = this.applicationPageService.processFormSaveRequest(smartFormTemplateRequest).subscribe(dataSaved => {
            if (dataSaved) {
                this.IsFormResponseSaved.emit(dataSaved);
            }

        },
            error => {
                throw error;
            });
    }


    getSearchItems(event:any , control: UIControl) {
        if (control && event.term) {
            let parentControl = this.uIControl.find(c => c.ControlIdentifier == control.ParentControlIdentifier);
            let parentControlId=  parentControl ? parentControl.ControlId: "";
            if (event.term.length > 3) {
                this.observableSubscription = this.applicationPageService.getSearchOptions(control.ControlId, parentControlId, event.term).subscribe(options => {
                    this.addSmartSelectionOption(options, control);
                },
                    error => {
                        throw error;
                    });
            }
        }
    }

    loadChildOptions(control:UIControl) {
        let value = this.smartFb.controls[control.DisplayLabel].value;
        if (value) this.loadChildrenValue(control);

    }

    loadChildrenValue(control: UIControl) {
        if (control) {
            let childControl = this.uIControl.find(c => c.ParentControlIdentifier == control.ControlIdentifier);
            if (childControl) {
                let formValues = this.smartFb.value;
                let parentValue = formValues[control.DisplayLabel];
                this.observableSubscription = this.applicationPageService.getChildren(childControl.ControlId, parentValue).subscribe(options => {
                    this.addSmartSelectionOption(options, childControl);

                },
                    error => {
                        throw error;
                    });
            }
        }
    }


    // onSelectedDate(event: IMyDateModel) {
    //     event ? this.selectedDate = this.utilsService.convertDateToReadableFormat(event.date) : this.selectedDate = "";
    // }

    addSmartSelectionOption(Options:SmartControlOption[], childControl:UIControl) {
        let index = this.uIControl.findIndex(control => control.ControlId == childControl.ControlId);
        if(this.UIFormClientVM.globalControls.length > 0) this.UIFormClientVM.globalControls[index].Values[0] = "";
        if (index > -1) {
            this.smartFb.controls[childControl.DisplayLabel].patchValue("");
            this.uIControl[index].Options = Options;
        }
    }


    setControlReadOnly(control:UIControl) {
        if (control.IsGlobalControl) return true;
        if (!control.IsEditable) return true;
        return false
    }


    close() {
        this.ngbActiveModal.close();
    }


    ngOnDestroy() {
        if (this.observableSubscription)
            this.observableSubscription.unsubscribe();
    }
}

