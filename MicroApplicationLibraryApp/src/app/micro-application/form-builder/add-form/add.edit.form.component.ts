import { Component, OnInit, OnDestroy, Output, EventEmitter, Input, } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AppFormList, AppPageSaveUpdateVM } from '../form.builder.model';
import { PageBuilderService } from '../../services/page.builder.service';
import { AlertModalService } from '../../alert/alert.modal.service';
import { MenuModel } from '../../menu/menu.model';
import { PermissionService, PermissionVM } from '../../services/permission.sevice';
import { ValidationMessage } from '../../utilities/validaton.messages';



@Component({
    selector: 'app-addEditForm',
    templateUrl: './add.edit.form.component.html',
    standalone:false
})

export class AddEditFormComponent implements OnInit, OnDestroy {
      observableSubscription: Subscription | null = null;;
    addEditForm: FormGroup;
    validationMessage: ValidationMessage;
    @Input() topMenus: MenuModel[] = [];
    @Input() mode: string;
    @Input() appFormList: AppFormList;
    @Output() sendResponse: EventEmitter<any> = new EventEmitter();
    permissionVM: PermissionVM[]=[];


    constructor(public ngbActiveModal: NgbActiveModal, private fb: FormBuilder, private pageBuilderService: PageBuilderService,
        private alertModalService: AlertModalService, private permissionService: PermissionService) {
        this.validationMessage = new ValidationMessage();
    }

    ngOnInit() {
        this.createForm();
        this.loadPermissions();
        if (this.mode == 'Edit' && this.appFormList) this.patchFormValues();
    }

    createForm() {
        this.addEditForm = this.fb.group({
            Name: ["", Validators.compose([Validators.required, Validators.minLength(2), Validators.maxLength(64)])],
            Position: [""],
            MenuId: ["", Validators.required],
            EditPermission: ["", Validators.required],
            DeletePermission: ["", Validators.required],
            ViewPermission: ["", Validators.required]
        });
    }

    loadPermissions() {
        //this.onclickedSelectAllCheckbox();
          this.observableSubscription = this.permissionService.getPermissions().subscribe((data) => {
            this.permissionVM= data;
          },
            error => {
              throw error;
            });
      }

    patchFormValues() {
        this.addEditForm.controls['Name'].patchValue(this.appFormList.Name);
        this.addEditForm.controls['Position'].patchValue(this.appFormList.Position);
        this.addEditForm.controls['MenuId'].patchValue(this.appFormList.MenuId);
        this.addEditForm.controls['EditPermission'].patchValue(this.appFormList.EditPermission? this.appFormList.EditPermission: "");
        this.addEditForm.controls['DeletePermission'].patchValue(this.appFormList.DeletePermission ? this.appFormList.DeletePermission: "");
        this.addEditForm.controls['ViewPermission'].patchValue(this.appFormList.ViewPermission ? this.appFormList.ViewPermission: "");
    }

    saveForm() {
        let newForm = new AppPageSaveUpdateVM(this.addEditForm.value);
        if (this.mode =='Edit') newForm.Id = this.appFormList.Id;
        this.observableSubscription = this.pageBuilderService.addForm(newForm).subscribe((appFormList) => {
            this.sendResponse.emit(appFormList);
            this.alertModalService.setSuccessAlertModalTemplate("Form added successfully.");
            this.close();

        },
            (error) => {
                throw error;
            })
    }

    close() {
        this.ngbActiveModal.close();
    }

    ngOnDestroy() {
        
        if (this.observableSubscription)
            this.observableSubscription.unsubscribe();
    }
}

