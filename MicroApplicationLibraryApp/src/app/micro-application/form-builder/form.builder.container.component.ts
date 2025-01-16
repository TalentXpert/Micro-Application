import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { Subscription } from 'rxjs';
import { FormBuilderViewModel } from './form.builder.viewModel';
import { MicroApplicationEvent, MicroApplicationEventData, MicroApplicationOperation } from '../micro-application.event';
import { AppFormControlListForDataTable, AppFormFixedControlAddUpdateVM, AppFormList } from './form.builder.model';
import { AlertModalService } from '../alert/alert.modal.service';
import { ApplicationPageService } from '../services/application.page.service';
import { UtilityService } from '../library/utility.service';
import { PageBuilderService } from '../services/page.builder.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AddEditFormComponent } from './add-form/add.edit.form.component';
import { AppFormControlService } from '../services/app.form.control.service';


@Component({
    selector: 'app-formBuilder',
    templateUrl: './form.builder.container.component.html',
    styleUrls: ['./form.builder.container.component.css'],
    standalone:false
})

export class FormBuilderContainerComponent implements OnInit {
    observableSubscription: Subscription;
    applicationFormBuilderForm: FormGroup;
    formBuilderViewModel: FormBuilderViewModel;
    selectedFormId: string = "";
    selectedFormControl: AppFormControlListForDataTable;
    IsClickedOnCard: boolean;
    //smartControls: UIControl[] = [];
    //layoutControlValue: string;
    get addOptionsList() { return this.applicationFormBuilderForm.controls; }
    get OptionsFormArray() { return this.addOptionsList['Options'] as FormArray; }
    get OptionsFormControlsArray() {
        return (this.applicationFormBuilderForm.controls["Options"] as FormArray).controls as FormGroup[];
    }
    

    constructor(private formBuilder: FormBuilder, private applicationPageEvent: MicroApplicationEvent,
        private appFormControlService: AppFormControlService, private alertModalService: AlertModalService, private applicationPageService:
            ApplicationPageService, private utilityService: UtilityService,
        private pageBuilderService: PageBuilderService, private modalService: NgbModal,) {
        this.formBuilderViewModel = new FormBuilderViewModel(applicationPageEvent, utilityService);

    }

    ngOnInit() {
        this.createForm();
        this.getPageInfo();
    }

    createForm() {
        this.applicationFormBuilderForm = this.formBuilder.group({
            form: ['', Validators.required],
            Options: this.formBuilder.array([]),
        });
    }


    getPageInfo() {
        this.observableSubscription = this.pageBuilderService.getPageInfo().subscribe((data) => {
            this.applicationPageEvent.publish(new MicroApplicationEventData(MicroApplicationOperation.LoadPageBuilderInfoVM, data));

        },
            (error) => {
                throw error;
            })
    }
    // getForms() {
    //     this.observableSubscription = this.appFormControlService.getAllForms().subscribe((data) => {
    //         this.appFormList = [];
    //         this.appFormList = data;

    //     },
    //         (error) => {
    //             throw error;
    //         })
    // }


    // getApplicationControls() {
    //     this.observableSubscription = this.appFormControlService.getAppControls().subscribe((data) => {
    //         this.applicationPageEvent.publish(new MicroApplicationEventData(MicroApplicationOperation.LoadApplicationControls, data));

    //     },
    //         (error) => {
    //             throw error;
    //         })
    // }

    // onSelectForm() {
    //     this.clearArray();
    //     if (this.selectedFormId) {
    //         this.smartControls = [];
    //         this.observableSubscription = this.appFormControlService.getGlobalControls(this.selectedFormId).subscribe((response) => {
    //             if (this.checkHasIsFormLayoutOwnerProperty(response)) {
    //                 for (let uIControl of this.smartControls) {
    //                     this.applicationFormBuilderForm.addControl(uIControl.DisplayLabel, new FormControl(uIControl.Value ? uIControl.Value : "", uIControl.Validators))
    //                     if (uIControl.IsFormLayoutOwner) this.getFormControls(uIControl.Value);
    //                 }
    //             }
    //             else this.getFormControls(null);

    //         },
    //             (error) => {
    //                 throw error;
    //             })
    //     }
    // }

    onSelectForm() {
        this.clearArray();
        if (this.selectedFormId) {
            this.observableSubscription = this.pageBuilderService.getFormFixedControls(this.selectedFormId).subscribe((data) => {
                this.applicationPageEvent.publish(new MicroApplicationEventData(MicroApplicationOperation.LoadFormFixedControls, data));
            },
                (error) => {
                    throw error;
                })
        }
    }

    // checkHasIsFormLayoutOwnerProperty(smartControls: UIControl[]): boolean {
    //     let listIsFormLayoutOwnerProperty = smartControls.filter(sm => sm.IsFormLayoutOwner == true);
    //     if (listIsFormLayoutOwnerProperty.length > 0) {
    //         this.smartControls = smartControls;
    //         return true;
    //     }
    //     return false
    // }

    // loadChildOptions(options: UIControl) {
    //     this.clearArray();
    //     let value = this.applicationFormBuilderForm.controls[options.DisplayLabel].value;
    //     if (options.IsFormLayoutOwner && value) this.getFormControls(value);
    //  else if (!options.IsFormLayoutOwner) this.loadChildrenValue(options);
    // }

    // getFormControls(globalFormValue: string) {

    //     let appFormControlRequestVM = new AppFormControlRequestVM();
    //     appFormControlRequestVM.FormId = this.selectedFormId;
    //     appFormControlRequestVM.GlobalFormValue = globalFormValue;
    //     this.observableSubscription = this.appFormControlService.getFormControls(appFormControlRequestVM).subscribe((data) => {
    //         this.layoutControlValue = globalFormValue;
    //         this.applicationPageEvent.publish(new MicroApplicationEventData(MicroApplicationOperation.LoadAppFormControlListVM, data));
    //     },
    //         (error) => {
    //             throw error;
    //         })
    // }

    // loadChildrenValue(control: UIControl) {
    //     if (control) {
    //         let childControl = this.smartControls.find(c => c.ParentControlIdentifier == control.ControlIdentifier);
    //         if (childControl) {
    //             let formValues = this.applicationFormBuilderForm.value;
    //             let parentValue = formValues[control.DisplayLabel];
    //             this.observableSubscription = this.applicationPageService.getChildren(childControl.ControlId, parentValue).subscribe(options => {
    //                 this.addSmartSelectionOption(options, childControl);

    //             },
    //                 error => {
    //                     throw error;
    //                 });
    //         }

    //     }
    // }

    // addSmartSelectionOption(Options, childControl) {
    //     let index = this.smartControls.findIndex(control => control.ControlId == childControl.ControlId);
    //     this.smartControls[index].Value = "";
    //     if (index > -1) {
    //         this.applicationFormBuilderForm.controls[childControl.DisplayLabel].patchValue("");
    //         this.smartControls[index].Options = Options;
    //     }
    // }

    drop(event: CdkDragDrop<any[]>, indexRow: number, indexColumn: number) {
        let field = event.previousContainer.data[event.previousIndex];
        if (field.OrganizationId == null && field.ColumnId == '1') {
            if (event.previousContainer.id != event.container.id) {
                //if (this.checkAppControlExistsInFormControls(field)) {
                let field = event.previousContainer.data[event.previousIndex];
                this.formBuilderViewModel.ChangeColumnId(field);
                this.selectedFormControl = field;
                this.setSelectedFormControlProperties();
                this.dropCard(event.previousContainer.data, event.container.data, event.previousIndex, event.currentIndex);
                //}
            }
        }
        else {
            if (field.ColumnId == 2) {
                moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
            }
        }
    }

    // checkCardPosition(field, currentIndex): boolean {
    //     var fixedFormControl = this.formBuilderViewModel.appFormControlListForDataTable.filter(c => c.ColumnId == 2);
    //     if (currentIndex > fixedFormControl.length - 1) {
    //         var index = this.formBuilderViewModel.appFormControlListForDataTable.findIndex(c => c.Id == field.Id)
    //         if (index > -1) this.formBuilderViewModel.appFormControlListForDataTable[index].Position = currentIndex;
    //         return true;
    //     }
    //     return false;
    // }

    dropCard(previousIndexData, currentIndexData, previousIndex, currentIndex) {
        transferArrayItem(previousIndexData, currentIndexData, previousIndex, currentIndex);
    }

    // checkAppControlExistsInFormControls(field: AppFormControlListForDataTable): boolean {
    //     var index = this.formBuilderViewModel.appFormControlListVM.findIndex(c => c.AppControlId == field.AppControlId);
    //     if (index > -1) return false;
    //     return true;
    // }

    nodeClick(event: AppFormControlListForDataTable) {
        this.IsClickedOnCard = true;
        this.OptionsFormArray.clear();
        if(event.Options)  this.getDropdownList(event.Options);
        this.selectedFormControl = event;
        this.setSelectedFormControlProperties();
    }

    setSelectedFormControlProperties() {
        if (this.selectedFormControl) {
            var index = this.formBuilderViewModel.appFormControlListForDataTable.findIndex(c => c.Id == this.selectedFormControl.Id);
            if (index > -1 && this.formBuilderViewModel.appFormControlListForDataTable[index].ColumnId == 2) {
                if((this.selectedFormControl.Maximum && this.selectedFormControl.Minimum) && this.selectedFormControl.Maximum < this.selectedFormControl.Minimum){
                    this.selectedFormControl.Maximum ="";
                    this.selectedFormControl.Minimum ="";
                    this.alertModalService.setInformaitonModalTemplate("Minimum value must be less than maximum value.");
                }
                else {
                    this.formBuilderViewModel.appFormControlListForDataTable[index].AppControlId = this.selectedFormControl.AppControlId ? this.selectedFormControl.AppControlId : this.selectedFormControl.Id;
                    this.formBuilderViewModel.appFormControlListForDataTable[index].IsUnique = this.selectedFormControl.IsUnique;
                    this.formBuilderViewModel.appFormControlListForDataTable[index].Position = this.selectedFormControl.Position;
                    this.formBuilderViewModel.appFormControlListForDataTable[index].IsEditable = this.selectedFormControl.IsEditable;
                    this.formBuilderViewModel.appFormControlListForDataTable[index].IsMandatory = this.selectedFormControl.IsMandatory;
                    
                    this.formBuilderViewModel.appFormControlListForDataTable[index].Minimum = this.selectedFormControl.Minimum;
                    this.formBuilderViewModel.appFormControlListForDataTable[index].Maximum = this.selectedFormControl.Maximum;
                    this.formBuilderViewModel.appFormControlListForDataTable[index].DisplayLabel = this.selectedFormControl.DisplayLabel;
                    this.formBuilderViewModel.appFormControlListForDataTable[index].IsSingleLine = this.selectedFormControl.IsSingleLine;
                    let formValues = this.applicationFormBuilderForm.value;
                    if (formValues.Options.length > 0) {
                         let options = formValues.Options.map(x => x.Option).join(", ");
                         this.formBuilderViewModel.appFormControlListForDataTable[index].Options = options;
                    }
                    this.formBuilderViewModel.appFormControlListForDataTable[index].IsGlobalControl = this.selectedFormControl.IsGlobalControl == true ? true : false;
                }
                
                
            }
        }

    }

    save() {
        let formControlsToAdd = this.formBuilderViewModel.appFormControlListForDataTable.filter(fc => fc.ColumnId == 2)
        let appFormControlAddUpdateVM = new AppFormFixedControlAddUpdateVM(formControlsToAdd);
        appFormControlAddUpdateVM.FormId = this.selectedFormId;
        this.observableSubscription = this.pageBuilderService.save(appFormControlAddUpdateVM).subscribe((data) => {
            if (data) this.alertModalService.setSuccessAlertModalTemplate("Controls to the form added successfully.");
            this.selectedFormControl = new AppFormControlListForDataTable(null, 0) ;
            this.IsClickedOnCard = false;
            this.selectedFormId = "";
            let formControlsRemove = this.formBuilderViewModel.appFormControlListForDataTable.filter(fc => fc.ColumnId == 2);
            formControlsRemove.forEach(element => {
                this.remove(element);
            })

        },
            (error) => {
                throw error;
            })
    }


    remove(field: AppFormControlListForDataTable) {
        this.formBuilderViewModel.ChangeColumnId(field);
    }

    clearArray() {
        this.formBuilderViewModel.removeItemFromList();
        this.selectedFormControl =  new AppFormControlListForDataTable(null, 0) ;;
        this.IsClickedOnCard = false;
    }


    addEditForm(mode) {
        const dialogRef = this.modalService.open(AddEditFormComponent, { backdrop: 'static' });
        dialogRef.componentInstance.topMenus = this.formBuilderViewModel.pageBuilderInfoVM.TopMenus;
        dialogRef.componentInstance.mode = mode;
        if (this.selectedFormId && mode == 'Edit') {
            let index = this.formBuilderViewModel.appFormList.findIndex(form => form.Id == this.selectedFormId);
            if (index > -1) dialogRef.componentInstance.appFormList = this.formBuilderViewModel.appFormList[index];
        }
        dialogRef.componentInstance.sendResponse.subscribe((appFormList) => {
            if (appFormList) this.formBuilderViewModel.appFormList.push(appFormList);
        });
    }

    getDropdownList(optionList){
        let options = optionList.split(',');
        let listOfOption:string[]=[];
        for (var i = 0; i < options.length; i++) {
          //this.addOption();
          listOfOption.push(options[i]);
        }

        this.OptionsFormArray.clear();
        for (var i = 0; i < listOfOption.length; i++) {
            this.OptionsFormArray.push(this.formBuilder.group({
                Option: [listOfOption[i], Validators.required],
            }))
        }
        // let OptionsFormArray = this.setOptions(listOfOption)
        // this.applicationFormBuilderForm.setControl('Options', OptionsFormArray);

    }

    // setOptions(data: any[]): any {
    //     let fa = new FormArray([]);
    //     data.forEach(d => {
    //         fa.push(this.formBuilder.group({
    //             Option: new FormControl(d)
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

    deleteOptions(index) {
        if (this.OptionsFormArray.length == 1) {
            this.alertModalService.setErrorAlertModalTemplate("Minimum 1 option required.");
            return;
        }
        else this.OptionsFormArray.removeAt(index);
    }

    setControlReadOnly(selectedFormControl: AppFormControlListForDataTable){
        if(selectedFormControl && selectedFormControl.CanBeGlobalControl)  return true;
        return false;

    }

    getClassForInterviewCard(field) {
        if (this.selectedFormControl && this.selectedFormControl.Id == field.Id) return 'tasktype tasktype-mb-2 scheduleinterviewyellow';
        return 'tasktype tasktype-mb-2';
    }

    ngOnDestroy() {
        if (this.observableSubscription) {
            this.observableSubscription.unsubscribe();
        }
    }
}