import { Component, OnInit, OnDestroy, Output, EventEmitter, Input, } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AlertModalService } from '../../alert/alert.modal.service';
import { DashboardBuilderService } from '../../services/dashboard.builder.service';
import { MenuModel } from '../../menu/menu.model';
import { DashboardSchema } from '../dashboard.builder.model';
import { ValidationMessage } from '../../utilities/validaton.messages';


@Component({
    selector: 'app-addEditDashboard',
    templateUrl: './add.edit.dashboard.component.html',
    standalone:false
})

export class AddEditDashboardComponent implements OnInit, OnDestroy {
    observableSubscription: Subscription;
    addEditDashboardBuilderForm: FormGroup;
    validationMessage: ValidationMessage;
    topMenus: MenuModel[]=[];
    @Input() mode: string;
    @Input() dashboardSchema: DashboardSchema;
    @Output() sendResponse: EventEmitter<any> = new EventEmitter();


    constructor(public ngbActiveModal: NgbActiveModal, private fb: FormBuilder, private alertModalService: AlertModalService,
        private dashboardBuilderService: DashboardBuilderService) {
        this.validationMessage = new ValidationMessage();
    }

    ngOnInit() {
        this.createForm();
        this.getTopMenu();
        if (this.mode=='Edit' && this.dashboardSchema) this.patchFormValues();
    }

    createForm() {
        this.addEditDashboardBuilderForm = this.fb.group({
            Name: ["", Validators.compose([Validators.required, Validators.minLength(2), Validators.maxLength(64)])],
            Description: ["", Validators.compose([Validators.minLength(2), Validators.maxLength(64)])],
            MinimumRowHeight: [""],
            MenuId: ["", Validators.required],
            Position: [""],
        });
    }

    getTopMenu() {
        this.observableSubscription = this.dashboardBuilderService.getTopMenus().subscribe((data) => {
            this.topMenus = data;
        },
            (error) => {
                throw error;
            })
    }


    patchFormValues() {
        this.addEditDashboardBuilderForm.controls['Name'].patchValue(this.dashboardSchema.Name);
        this.addEditDashboardBuilderForm.controls['Description'].patchValue(this.dashboardSchema.Description);
        this.addEditDashboardBuilderForm.controls['MinimumRowHeight'].patchValue(this.dashboardSchema.MinimumRowHeight);
        this.addEditDashboardBuilderForm.controls['MenuId'].patchValue(this.dashboardSchema.MenuId);
        this.addEditDashboardBuilderForm.controls['Position'].patchValue(this.dashboardSchema.Position);
      }

    saveForm() {
        this.sendResponse.emit(this.addEditDashboardBuilderForm.value);
        this.close();
    }

    close() {
        this.ngbActiveModal.close();
    }

    ngOnDestroy() {

        if (this.observableSubscription)
            this.observableSubscription.unsubscribe();
    }
}

