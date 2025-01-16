import { Component, OnInit, OnDestroy, Output, EventEmitter, Input, } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AlertModalService } from '../../alert/alert.modal.service';
import { DashboardBuilderService } from '../../services/dashboard.builder.service';
import { Content, DashboardPanel, DashboardSchema } from '../dashboard.builder.model';
import { ValidationMessage } from '../../utilities/validaton.messages';



@Component({
    selector: 'app-addEditPanel',
    templateUrl: './add.edit.panel.component.html',
    standalone:false
})

export class AddEditPanelComponent implements OnInit, OnDestroy {
      observableSubscription: Subscription | null = null;;
    addEditPanelForm: FormGroup = new FormGroup({});
    validationMessage: ValidationMessage;
    @Input() rowList: number[] = [];
    @Input() mode: string="";
    @Output() sendResponse: EventEmitter<any> = new EventEmitter();
    contentTypes: string[] = [];
    content: Content[] = [];
    selectDashboardSchema: DashboardSchema = new DashboardSchema();
    IseditPanel: boolean = false;
    panelList: DashboardPanel[] = [];
    rowIndex: number=0;
    previousPanelPosition: number=0;


    constructor(public ngbActiveModal: NgbActiveModal, private fb: FormBuilder, private alertModalService: AlertModalService,
        private dashboardBuilderService: DashboardBuilderService) {
        this.validationMessage = new ValidationMessage();
    }

    ngOnInit() {
        this.createForm();
        this.selectDashboardSchema.Rows.forEach(row => this.rowList.push(row.Position))
        this.getContentTypes();
    }

    createForm() {
        this.addEditPanelForm = this.fb.group({
            Row: ['', Validators.required],
            Title: ['', Validators.required],
            Width: ['', Validators.required],
            ContentType: ['', Validators.required],
            Position: ['', Validators.required],
            ContentId: [''],

        });
    }

    getContentTypes() {
        this.observableSubscription = this.dashboardBuilderService.getContentTypes().subscribe((data) => {
            this.contentTypes = data;
        },
            (error) => {
                throw error;
            })
    }

    getContents(contentType: string) {
        if (contentType) {
            this.observableSubscription = this.dashboardBuilderService.getContents(contentType).subscribe((data) => {
                this.content = data;
            },
                (error) => {
                    throw error;
                })
        }

    }

    getContent(panel) {
        let contentList =this.dashboardBuilderService.content.get(panel.ContentType)
        if (contentList) {
            let index = contentList.findIndex(content => content.Id == panel.ContentId);
            if (index > -1) return contentList[index].Name;
            else return null;
        }
        else return null;

    }

    onSelectRow(rowId) {
        this.panelList.length = 0;
        this.rowIndex = this.selectDashboardSchema.Rows.findIndex(row => row.Position == rowId);
        if (this.rowIndex > -1) {
            this.panelList =[... this.selectDashboardSchema.Rows[this.rowIndex].Panels];
            this.panelList.forEach(element =>
                this.getContents(element.ContentType)
                )
        }
    }

    editPanel(panel) {
        this.IseditPanel = true;
        this.previousPanelPosition= panel.Position;
        this.addEditPanelForm.controls["Title"].patchValue(panel.Title);
        this.addEditPanelForm.controls["Width"].patchValue(panel.Width);
        this.addEditPanelForm.controls["ContentType"].patchValue(panel.ContentType);
        this.addEditPanelForm.controls["Position"].patchValue(panel.Position);
        this.addEditPanelForm.controls["ContentId"].patchValue(panel.ContentId);
    }

    deletePanel(panel) {
        let panelIndex = this.selectDashboardSchema.Rows[this.rowIndex].Panels.findIndex(pnl => pnl.Position == panel.Position);
        if (panelIndex > -1) {
            this.selectDashboardSchema.Rows[this.rowIndex].Panels.splice(panelIndex, 1);
            this.alertModalService.setSuccessAlertModalTemplate("Panel is deleted successfully.");
        }
       this.setPanelPosition();
    }

    saveForm() {
        let formValues = this.addEditPanelForm.value
        if (this.rowIndex > -1) {
            let panelIndex = this.selectDashboardSchema.Rows[this.rowIndex].Panels.findIndex(panel => panel.Position ==  this.previousPanelPosition);
            if (this.IseditPanel) {
                this.selectDashboardSchema.Rows[this.rowIndex].Panels[panelIndex].Position = formValues.Position;
                this.selectDashboardSchema.Rows[this.rowIndex].Panels[panelIndex].ContentId = formValues.ContentId;
                this.selectDashboardSchema.Rows[this.rowIndex].Panels[panelIndex].Title = formValues.Title;
                this.selectDashboardSchema.Rows[this.rowIndex].Panels[panelIndex].ContentType = formValues.ContentType;
                this.selectDashboardSchema.Rows[this.rowIndex].Panels[panelIndex].Width = formValues.Width;
                this.setPanelPosition();
                this.alertModalService.setSuccessAlertModalTemplate("Panel details are updated successfully.");
            }
            else if (!this.IseditPanel) {
                this.selectDashboardSchema.Rows[this.rowIndex].Panels.push(new DashboardPanel(formValues));
                this.setPanelPosition();
                this.alertModalService.setSuccessAlertModalTemplate("Panel details are added successfully.");
            }
            else this.alertModalService.setErrorAlertModalTemplate("Can not add duplicate Panel.");
        }
    }

    setPanelPosition() {
        let panelPosition = 1;
        this.selectDashboardSchema.Rows[this.rowIndex].Panels.forEach(panel => {
            panel.Position = panelPosition;
            panelPosition++;
        })
        this.resetForm();
        this.panelList =[... this.selectDashboardSchema.Rows[this.rowIndex].Panels];
        this.sendResponse.emit(this.selectDashboardSchema);
    }

    resetForm() {
        this.IseditPanel = false;
        this.addEditPanelForm.controls["Title"].patchValue("");
        this.addEditPanelForm.controls["Width"].patchValue("");
        this.addEditPanelForm.controls["ContentType"].patchValue("");
        this.addEditPanelForm.controls["Position"].patchValue("");
        this.addEditPanelForm.controls["ContentId"].patchValue("");
      }

    close() {
        this.ngbActiveModal.close();
    }

    ngOnDestroy() {

        if (this.observableSubscription)
            this.observableSubscription.unsubscribe();
    }
}

