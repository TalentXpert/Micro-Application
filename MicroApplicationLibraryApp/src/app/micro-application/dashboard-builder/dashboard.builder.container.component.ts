import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { DashboardBuilderViewModel } from './dashboard.builder.viewModel';
import { MicroApplicationEvent, MicroApplicationEventData, MicroApplicationOperation } from '../micro-application.event';
import { DashboardBuilderService } from '../services/dashboard.builder.service';
import { DashboardPanel, DashboardRow, DashboardSchema } from './dashboard.builder.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AddEditDashboardComponent } from './add-dashboard/add.edit.dashboard.component';
import { AlertModalService } from '../alert/alert.modal.service';
import { AddEditRowComponent } from './add-row/add.edit.row.component';
import { AddEditPanelComponent } from './add.edit.panel/add.edit.panel.component';


@Component({
    selector: 'app-dashboardBuilder',
    templateUrl: './dashboard.builder.container.component.html',
    styleUrls: ['./dashboard.builder.container.component.css'],
    standalone:false
})

export class DashboardBuilderContainerComponent implements OnInit {
      observableSubscription: Subscription | null = null;;
    dashboardBuilderForm: FormGroup = new FormGroup({});
    dashboardBuilderViewModel: DashboardBuilderViewModel;
    newDashboardSchema: DashboardSchema;
    rows: DashboardRow[] = [];
    rowPositionList: number[] = [];

    constructor(private formBuilder: FormBuilder, private dashboardBuilderService: DashboardBuilderService,
        private applicationPageEvent: MicroApplicationEvent, private modalService: NgbModal, private alertModalService: AlertModalService,) {
        this.dashboardBuilderViewModel = new DashboardBuilderViewModel(this.applicationPageEvent);
        this.newDashboardSchema = new DashboardSchema();

    }

    ngOnInit() {
        this.createForm();
        this.getDashboardSchema();
    }

    getDashboardSchema() {
        this.observableSubscription = this.dashboardBuilderService.getDashboardSchema().subscribe((data) => {
            this.applicationPageEvent.publish(new MicroApplicationEventData(MicroApplicationOperation.LoadAllDashboardSchema, data));

        },
            (error) => {
                throw error;
            })
    }

    onSelectDashboard() {
        let index = this.dashboardBuilderViewModel.dashboardSchemaList.findIndex(schema => schema.Id == this.dashboardBuilderViewModel.selectDashboardSchema);
        if (index > -1) {
            this.newDashboardSchema = new DashboardSchema();
            this.newDashboardSchema.Id = this.dashboardBuilderViewModel.dashboardSchemaList[index].Id;
            this.newDashboardSchema.Description = this.dashboardBuilderViewModel.dashboardSchemaList[index].Description;
            this.newDashboardSchema.MenuId = this.dashboardBuilderViewModel.dashboardSchemaList[index].MenuId;
            this.newDashboardSchema.Name = this.dashboardBuilderViewModel.dashboardSchemaList[index].Name;
            this.newDashboardSchema.MinimumRowHeight = this.dashboardBuilderViewModel.dashboardSchemaList[index].MinimumRowHeight;
            this.newDashboardSchema.Position = this.dashboardBuilderViewModel.dashboardSchemaList[index].Position;
            this.newDashboardSchema.Rows = this.dashboardBuilderViewModel.dashboardSchemaList[index].Rows;


        }
    }

    addEditDashboard(mode:string) {
        const dialogRef = this.modalService.open(AddEditDashboardComponent, { backdrop: 'static' });
        dialogRef.componentInstance.mode = mode;
        if (this.dashboardBuilderViewModel.selectDashboardSchema) {
            let index = this.dashboardBuilderViewModel.dashboardSchemaList.findIndex(schema => schema.Id == this.dashboardBuilderViewModel.selectDashboardSchema);
            if (index > -1) {
                dialogRef.componentInstance.dashboardSchema = this.dashboardBuilderViewModel.dashboardSchemaList[index];

            }
        }
        dialogRef.componentInstance.sendResponse.subscribe((response:DashboardSchema) => {
            this.newDashboardSchema.Id = this.dashboardBuilderViewModel.selectDashboardSchema ? this.dashboardBuilderViewModel.selectDashboardSchema : undefined;
            this.newDashboardSchema.Description = response.Description;
            this.newDashboardSchema.MenuId = response.MenuId;
            this.newDashboardSchema.Name = response.Name;
            this.newDashboardSchema.MinimumRowHeight = response.MinimumRowHeight;
            this.newDashboardSchema.Position = response.Position;

            this.save();
        });

    }

    createForm() {
        this.dashboardBuilderForm = this.formBuilder.group({
            dashboard: ['', Validators.required],
        });
    }

    addRow() {
        this.rows = [];
        let index = this.dashboardBuilderViewModel.dashboardSchemaList.findIndex(schema => schema.Id == this.dashboardBuilderViewModel.selectDashboardSchema);
        const dialogRef = this.modalService.open(AddEditRowComponent, { backdrop: 'static' });
        dialogRef.componentInstance.mode = "Add";
        dialogRef.componentInstance.selectDashboardSchema = this.dashboardBuilderViewModel.dashboardSchemaList[index];
        dialogRef.componentInstance.sendResponse.subscribe((response:DashboardSchema) => {
            if (response) {
                this.dashboardBuilderViewModel.dashboardSchemaList[index].Rows = response.Rows;
                this.newDashboardSchema.Rows = response.Rows;
                this.save();
            }
        });
    }

    addPanel() {
        this.rowPositionList.length = 0;
        let index = this.dashboardBuilderViewModel.dashboardSchemaList.findIndex(schema => schema.Id == this.dashboardBuilderViewModel.selectDashboardSchema);
        if (index > -1) {
            const dialogRef = this.modalService.open(AddEditPanelComponent, { backdrop: 'static', windowClass:' JobDetailsModal' });
            dialogRef.componentInstance.mode = "Add";
            dialogRef.componentInstance.selectDashboardSchema = this.dashboardBuilderViewModel.dashboardSchemaList[index];
            dialogRef.componentInstance.sendResponse.subscribe((response:DashboardSchema) => {
                if (response) {
                    this.newDashboardSchema = response;
                    this.save();
                }
            });
        }

    }


    save() {

        this.observableSubscription = this.dashboardBuilderService.saveDashboardSchema(this.newDashboardSchema).subscribe((data) => {
            let index = this.dashboardBuilderViewModel.dashboardSchemaList.findIndex(Schema => Schema.Id == data.Id);
            if (index > -1) {
                this.dashboardBuilderViewModel.dashboardSchemaList.splice(index, 1); 
               
            }
            this.dashboardBuilderViewModel.dashboardSchemaList.push(data);
            this.newDashboardSchema.Id = data.Id;
            this.dashboardBuilderViewModel.selectDashboardSchema = data.Id? data.Id:"";
        },
            (error) => {
                throw error;
            })
    }

    ngOnDestroy() {
        if (this.observableSubscription) {
            this.observableSubscription.unsubscribe();
        }
    }
}