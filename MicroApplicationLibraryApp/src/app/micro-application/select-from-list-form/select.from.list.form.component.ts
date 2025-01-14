import { Component, OnInit, OnDestroy, Input, Output, EventEmitter } from '@angular/core';
import { Subscription, elementAt } from 'rxjs';
import { InputSelectFromListForm, SelectFromListFormData, SelectFromListFormInput, SelectFromListFormModel } from './select.from.list.form.model';
import { FormDataCollectionService } from '../services/form.data.collection.service';
import { AlertModalService } from '../alert/alert.modal.service';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MicroApplicationEvent, MicroApplicationEventData, MicroApplicationOperation } from '../micro-application.event';

@Component({
    selector: 'app-SelectFromListForm',
    templateUrl: './select.from.list.form.component.html',
    standalone:false
})

export class SelectFromListFormComponent implements OnInit, OnDestroy {
    observableSubscription: Subscription;
    selectFromListFormInputForm: FormGroup;
    @Input() InputSelectFromListForm: InputSelectFromListForm;
    @Output() IsFormResponseSaved: EventEmitter<boolean> = new EventEmitter();
    selectFromListFormInput: SelectFromListFormInput;
    selectFromListFormModel: SelectFromListFormModel;

    constructor(private formDataCollectionService: FormDataCollectionService,
        private alertModalService: AlertModalService, private formBuilder: FormBuilder,
        private applicationPageEvent: MicroApplicationEvent,) {
        this.selectFromListFormModel = new SelectFromListFormModel(applicationPageEvent);

    }


    ngOnInit() {
        if (this.InputSelectFromListForm) {
            this.observableSubscription = this.formDataCollectionService.getSelectFromListFormInput(this.InputSelectFromListForm.FormId, this.InputSelectFromListForm.EntityId).subscribe((data) => {
                this.selectFromListFormInput = data;
                this.applicationPageEvent.publish(new MicroApplicationEventData(MicroApplicationOperation.LoadSelectFromListFormInputItem, this.selectFromListFormInput.Items));
                this.createForm();

            },
                (error) => {
                    throw error;
                })
        }
    }

    createForm(): void {
        const group: any = {};
        group[this.selectFromListFormInput.FormDataLabel] = new FormControl(this.selectFromListFormInput.FormDataValue);
        this.selectFromListFormInputForm = new FormGroup(group);
    }


    onclickedSelectAllCheckbox() {
        this.selectFromListFormModel.selectedAll = !this.selectFromListFormModel.selectedAll;
        if (this.selectFromListFormModel.selectFromListFormInputItemVM.length > 0) {
            this.selectFromListFormModel.selectFromListFormInputItemVM.forEach(e => e.IsSelected = this.selectFromListFormModel.selectedAll);
        }
    }

    onclickedCheckbox(permissions) {
        permissions.IsSelected = !permissions.IsSelected;
        this.selectFromListFormModel.onclickedCheckbox();
    }

    save() {
        let selectedRole = this.selectFromListFormModel.selectFromListFormInputItemVM.filter(e => e.IsSelected == true);
        let selectFromListFormData = new SelectFromListFormData();
        selectFromListFormData.FormId = this.InputSelectFromListForm.FormId;
        selectFromListFormData.EntityId = this.InputSelectFromListForm.EntityId;
        selectedRole.forEach(element => {
            selectFromListFormData.SelectedItems.push(element.ItemId);
        })
        if (selectedRole.length > 0) {
            this.observableSubscription = this.formDataCollectionService.saveSelectFromListForm(selectFromListFormData).subscribe((dataSaved) => {
                if (dataSaved) {
                    this.IsFormResponseSaved.emit(dataSaved);
                    this.alertModalService.setSuccessAlertModalTemplate("Data saved successfully.");
                }
            },
                (error) => {
                    throw error;
                })
        }
        else {
            this.alertModalService.setInformaitonModalTemplate("Please select atleast one role.");
        }
    }


    ngOnDestroy() {
        if (this.observableSubscription)
            this.observableSubscription.unsubscribe();
    }
}

