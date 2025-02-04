import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { Subscription } from 'rxjs';
import { FormConfiguartionViewModel } from './form.configuration.viewModel';
import { MicroApplicationEvent, MicroApplicationEventData, MicroApplicationOperation } from '../micro-application.event';
import { AppFormControlService } from '../services/app.form.control.service';
import { AlertModalService } from '../alert/alert.modal.service';
import { ApplicationPageService } from '../services/application.page.service';
import { UtilityService } from '../library/utility.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppFormControlAddUpdateVM, AppFormControlListForDataTable, AppFormControlRequestVM, AppFormResetRequestVM } from './form.configuration.model';
import { AppFormList } from '../form-builder/form.builder.model';
import { SmartControlOption, UIControl } from '../application-page-container/application.page.model';



@Component({
    selector: 'app-formConfiguration',
    templateUrl: './form.configuration.container.component.html',
    styleUrls: ['./form.configuration.container.component.css'],
    standalone:false
})

export class FormConfigurationContainerComponent implements OnInit {
      observableSubscription: Subscription | null = null;;
    appFormConfigForm: FormGroup= new FormGroup({});
    fCViewModel: FormConfiguartionViewModel;
    selectedFormId: string = "";
    selectedFormControl: AppFormControlListForDataTable= new AppFormControlListForDataTable(null, 0);
    IsClickedOnCard: boolean= false;
    smartControls: UIControl[] = [];
    layoutControlValue?: string;
    appFormList: AppFormList[] = [];
    get addOptionsList() { return this.appFormConfigForm.controls; }
    get OptionsFormArray() { return this.addOptionsList['Options'] as FormArray; }
     get OptionsFormControlsArray() {
        return (this.appFormConfigForm.controls["Options"] as FormArray).controls as FormGroup[];
    }
    

    constructor(private formBuilder: FormBuilder, private applicationPageEvent: MicroApplicationEvent,
        private appFormControlService: AppFormControlService, private alertModalService: AlertModalService, private applicationPageService:
            ApplicationPageService, private utilityService: UtilityService, private modalService: NgbModal,) {
        this.fCViewModel = new FormConfiguartionViewModel(applicationPageEvent, utilityService);

    }

    ngOnInit() {
        this.createForm();
        this.getForms();
        this.getApplicationControls();
    }

    createForm() {
        this.appFormConfigForm = this.formBuilder.group({
            form: ['', Validators.required],
            Options: this.formBuilder.array([]),
        });
    }


    getForms() {
        this.observableSubscription = this.appFormControlService.getAllCustomizableForms().subscribe((data) => {
            this.appFormList = [];
            this.appFormList = data;

        },
            (error) => {
                throw error;
            })
    }


    getApplicationControls() {
        this.observableSubscription = this.appFormControlService.getAppControls().subscribe((data) => {
            this.applicationPageEvent.publish(new MicroApplicationEventData(MicroApplicationOperation.LoadApplicationControlsOnFC, data));

        },
            (error) => {
                throw error;
            })
    }

    onSelectForm() {
        this.clearArray();
        if (this.selectedFormId) {
            this.smartControls = [];
            this.observableSubscription = this.appFormControlService.getGlobalControls(this.selectedFormId).subscribe((response) => {
                if (this.checkHasIsFormLayoutOwnerProperty(response)) {
                    for (let uIControl of this.smartControls) {
                        this.appFormConfigForm.addControl(uIControl.DisplayLabel, new FormControl(uIControl.Value ? uIControl.Value : "", uIControl.Validators))
                        if (uIControl && uIControl.IsFormLayoutOwner) this.getFormControls(uIControl.Value);
                    }
                }
                else this.getFormControls();

            },
                (error) => {
                    throw error;
                })
        }
    }


    checkHasIsFormLayoutOwnerProperty(smartControls: UIControl[]): boolean {
        let listIsFormLayoutOwnerProperty = smartControls.filter(sm => sm.IsFormLayoutOwner == true);
        if (listIsFormLayoutOwnerProperty.length > 0) {
            this.smartControls = smartControls;
            return true;
        }
        return false
    }

    loadChildOptions(options: UIControl) {
        this.clearArray();
        let value = this.appFormConfigForm.controls[options.DisplayLabel].value;
        if (options.IsFormLayoutOwner && value) this.getFormControls(value);
        else if (!options.IsFormLayoutOwner) this.loadChildrenValue(options);
    }

    getFormControls(globalFormValue?: string) {

        let appFormControlRequestVM = new AppFormControlRequestVM();
        appFormControlRequestVM.FormId = this.selectedFormId;
        appFormControlRequestVM.GlobalFormValue = globalFormValue;
        this.observableSubscription = this.appFormControlService.getFormControls(appFormControlRequestVM).subscribe((data) => {
            this.layoutControlValue = globalFormValue;
            this.applicationPageEvent.publish(new MicroApplicationEventData(MicroApplicationOperation.LoadAppFormControlListVMOnFC, data));
        },
            (error) => {
                throw error;
            })
    }

    loadChildrenValue(control: UIControl) {
        if (control) {
            let childControl = this.smartControls.find(c => c.ParentControlIdentifier == control.ControlIdentifier);
            if (childControl) {
                let formValues = this.appFormConfigForm.value;
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

    addSmartSelectionOption(Options:SmartControlOption[], childControl:UIControl) {
        let index = this.smartControls.findIndex(control => control.ControlId == childControl.ControlId);
        this.smartControls[index].Value = "";
        if (index > -1) {
            this.appFormConfigForm.controls[childControl.DisplayLabel].patchValue("");
            this.smartControls[index].Options = Options;
        }
    }

    drop(event: CdkDragDrop<any[]>, indexRow: number, indexColumn: number) {
        let field = event.previousContainer.data[event.previousIndex];
        if (field.OrganizationId == null && field.ColumnId == '1') {
            if (event.previousContainer.id != event.container.id) {
                let field = event.previousContainer.data[event.previousIndex];
                this.fCViewModel.ChangeColumnId(field);
                this.selectedFormControl = field;
                this.dropCard(event.previousContainer.data, event.container.data, event.previousIndex, event.currentIndex);

            }
        }
        else {
            if (field.ColumnId == 2 && this.checkCardPosition(field, event.currentIndex)) {
                moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
            }
        }
    }

    checkCardPosition(field:any, currentIndex:number): boolean {
        var fixedFormControl = this.fCViewModel.appFormControlListForDataTable.filter(c => c.ColumnId == 2 && c.IsFixed == true);
        if (currentIndex > fixedFormControl.length - 1) {
            var index = this.fCViewModel.appFormControlListForDataTable.findIndex(c => c.Id == field.Id)
            if (index > -1) this.fCViewModel.appFormControlListForDataTable[index].Position = currentIndex;
            return true;
        }
        return false;
    }

    dropCard(previousIndexData:any[], currentIndexData:any[], previousIndex:number, currentIndex:number) {
        transferArrayItem(previousIndexData, currentIndexData, previousIndex, currentIndex);
    }


    nodeClick(event: AppFormControlListForDataTable) {
        this.IsClickedOnCard = true;
        this.OptionsFormArray.clear();
        if (event.Options) this.getDropdownList(event.Options);
        this.selectedFormControl = event;
        this.setSelectedFormControlProperties();
    }

    setSelectedFormControlProperties() {
        if (this.selectedFormControl) {
            var index = this.fCViewModel.appFormControlListForDataTable.findIndex(c => c.Id == this.selectedFormControl.Id);
            if (index > -1 && this.fCViewModel.appFormControlListForDataTable[index].ColumnId == 2) {
                if ((this.selectedFormControl.Maximum && this.selectedFormControl.Minimum) && parseInt(this.selectedFormControl.Maximum) < parseInt(this.selectedFormControl.Minimum)) {
                    this.selectedFormControl.Maximum = "";
                    this.selectedFormControl.Minimum = "";
                    this.alertModalService.setInformaitonModalTemplate("Minimum value must be less than maximum value.");
                }
                else {
                    this.fCViewModel.appFormControlListForDataTable[index].AppControlId = this.selectedFormControl.AppControlId ? this.selectedFormControl.AppControlId : this.selectedFormControl.Id;
                    this.fCViewModel.appFormControlListForDataTable[index].IsUnique = this.selectedFormControl.IsUnique
                    this.fCViewModel.appFormControlListForDataTable[index].Position = this.selectedFormControl.Position;
                    this.fCViewModel.appFormControlListForDataTable[index].IsEditable = this.selectedFormControl.IsEditable;
                    this.fCViewModel.appFormControlListForDataTable[index].IsMandatory = this.selectedFormControl.IsMandatory;

                    this.fCViewModel.appFormControlListForDataTable[index].Minimum = this.selectedFormControl.Minimum;
                    this.fCViewModel.appFormControlListForDataTable[index].Maximum = this.selectedFormControl.Maximum;
                    this.fCViewModel.appFormControlListForDataTable[index].DisplayLabel = this.selectedFormControl.DisplayLabel;
                    this.fCViewModel.appFormControlListForDataTable[index].IsSingleLine = this.selectedFormControl.IsSingleLine;
                    let formValues = this.appFormConfigForm.value;
                    if (formValues.Options.length > 0) {
                        let options = formValues.Options.map((x:any) => x.Option).join(", ");
                        this.fCViewModel.appFormControlListForDataTable[index].Options = options;
                    }
                    this.fCViewModel.appFormControlListForDataTable[index].IsGlobalControl = this.selectedFormControl.IsGlobalControl;
                }
            }
        }

    }

    save() {

        let formControlsToAdd = this.fCViewModel.appFormControlListForDataTable.filter(fc => fc.ColumnId == 2 && fc.IsFixed == false)
        let appFormControlAddUpdateVM = new AppFormControlAddUpdateVM(formControlsToAdd);
        appFormControlAddUpdateVM.FormId = this.selectedFormId;
        appFormControlAddUpdateVM.GlobalControlValue = this.layoutControlValue;

        this.observableSubscription = this.appFormControlService.saveAppFormControls(appFormControlAddUpdateVM).subscribe((data) => {
            if (data) this.alertModalService.setSuccessAlertModalTemplate("Controls to the form added successfully.")
            this.selectedFormControl = new AppFormControlListForDataTable(null, 0);
            this.IsClickedOnCard = false;
            this.selectedFormId = "";
            this.layoutControlValue = "";
            let formControlsRemove = this.fCViewModel.appFormControlListForDataTable.filter(fc => fc.ColumnId == 2);
            formControlsRemove.forEach(element => {
                this.remove(element);
            })
        },
            (error) => {
                throw error;
            })
    }

    resetForm() {
        let appFormControlAddUpdateVM = new AppFormResetRequestVM();
        appFormControlAddUpdateVM.FormId = this.selectedFormId;
        appFormControlAddUpdateVM.LayoutControlValue = this.layoutControlValue;
        this.observableSubscription = this.appFormControlService.restForm(appFormControlAddUpdateVM).subscribe((response) => {
            if (response) {
                this.alertModalService.setSuccessAlertModalTemplate("Application controls are  successfully removed from selected Form.")
                let formControlsRemove = this.fCViewModel.appFormControlListForDataTable.filter(fc => fc.ColumnId == 2 && fc.IsFixed == false);
                formControlsRemove.forEach(element => {
                    this.remove(element);
                })
            }
        },
            (error) => {
                throw error;
            })
    }

    remove(field: AppFormControlListForDataTable) {
        this.fCViewModel.ChangeColumnId(field);
    }

    clearArray() {
        this.fCViewModel.removeItemFromList();
        this.selectedFormControl = new AppFormControlListForDataTable(null, 0);
        this.IsClickedOnCard = false;
    }

    getDropdownList(optionList: string) {
        let options = optionList.split(',');
        let listOfOption : string[]=[];
        for (var i = 0; i < options.length; i++) {
            listOfOption.push(options[i]);
        }
        this.OptionsFormArray.clear();
        for (var i = 0; i < listOfOption.length; i++) {
            this.OptionsFormArray.push(this.formBuilder.group({
                Option: [listOfOption[i], Validators.required],
            }))
        }
       
    }

    // setOptions(data: any[]): FormArray {
    //     let fa;
    //     data.forEach(d => {
    //         fa.push(this.formBuilder.group({
    //             Option: [d],
    //         }));
    //     });
    //     return fa;
    // }


    addOptions() {
        this.OptionsFormArray.clear();
        for (var i = 1; i < 2; i++) {
            this.addOption();
        }
    }

    addOption() {
        this.OptionsFormArray.push(this.formBuilder.group({
            Option: ['', Validators.required],
        }))
    }

    deleteOptions(index:number) {
        if (this.OptionsFormArray.length == 1) {
            this.alertModalService.setErrorAlertModalTemplate("Minimum 1 option required.");
            return;
        }
        else this.OptionsFormArray.removeAt(index);
    }

    getClassForInterviewCard(field:AppFormControlListForDataTable) {
        if (this.selectedFormControl && this.selectedFormControl.Id == field.Id) return 'tasktype tasktype-mb-2 scheduleinterviewyellow';
        return 'tasktype tasktype-mb-2';
    }

    ngOnDestroy() {
        if (this.observableSubscription) {
            this.observableSubscription.unsubscribe();
        }
    }
}