import { Component, OnInit, OnDestroy, Output, EventEmitter, Input, } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AlertModalService } from '../../alert/alert.modal.service';
import { DashboardRow, DashboardSchema } from '../dashboard.builder.model';
import { ValidationMessage } from '../../utilities/validaton.messages';


@Component({
    selector: 'app-addEditRow',
    templateUrl: './add.edit.row.component.html',
    standalone: false
})

export class AddEditRowComponent implements OnInit, OnDestroy {
      observableSubscription: Subscription | null = null;;
    addEditRowForm: FormGroup;
    validationMessage: ValidationMessage;
    @Input() mode: string;
    @Output() sendResponse: EventEmitter<any> = new EventEmitter();
    contentTypes: string[] = [];
    selectDashboardSchema: DashboardSchema;
    IseditRow: boolean = false;
    previousRowPosition: number;


    constructor(public ngbActiveModal: NgbActiveModal, private fb: FormBuilder, private alertModalService: AlertModalService) {
        this.validationMessage = new ValidationMessage();
    }

    ngOnInit() {
        this.createForm();
    }

    createForm() {
        this.addEditRowForm = this.fb.group({
            Position: ['', Validators.required],
            Height: ['', Validators.required],
        });
    }

    editRow(row) {
        this.IseditRow = true;
        this.previousRowPosition = row.Position;
        this.addEditRowForm.controls["Position"].patchValue(row.Position);
        this.addEditRowForm.controls["Height"].patchValue(row.Height);
    }

    deleteRow(selectedRow) {
        let rowIndex = this.selectDashboardSchema.Rows.findIndex(row => row.Position == selectedRow.Position);
        if (rowIndex > -1) {
            this.selectDashboardSchema.Rows.splice(rowIndex, 1);
            this.alertModalService.setSuccessAlertModalTemplate("Row is deleted successfully.");
        }
        this.setRowPosition();

    }

    saveForm() {
        let formValues = this.addEditRowForm.value
        let rowIndex = this.selectDashboardSchema.Rows.findIndex(row => row.Position == this.previousRowPosition);
        if (rowIndex > -1 && this.IseditRow) {
            this.selectDashboardSchema.Rows[rowIndex].Position = formValues.Position;
            this.selectDashboardSchema.Rows[rowIndex].Height = formValues.Height;
            this.setRowPosition();
            this.IseditRow = false;
            this.alertModalService.setSuccessAlertModalTemplate("Row details are updated successfully.");
        }
        else if (!this.IseditRow) {
            this.selectDashboardSchema.Rows.push(new DashboardRow(formValues));
            this.setRowPosition();
        }
        else this.alertModalService.setErrorAlertModalTemplate("Can not add duplicate row.");

    }

    setRowPosition() {
        let rowPosition = 1;
        this.selectDashboardSchema.Rows.forEach(row => {
            row.Position = rowPosition;
            rowPosition++;
        });
        this.resetForm();
        this.sendResponse.emit(this.selectDashboardSchema);
    }

    resetForm() {
        this.IseditRow = false;
        this.addEditRowForm.reset();
        // this.addEditRowForm.controls["Position"].patchValue("");
        // this.addEditRowForm.controls["Height"].patchValue("");

    }

    close() {
        this.sendResponse.emit(this.selectDashboardSchema);
        this.ngbActiveModal.close();
    }

    ngOnDestroy() {

        if (this.observableSubscription)
            this.observableSubscription.unsubscribe();
    }
}

