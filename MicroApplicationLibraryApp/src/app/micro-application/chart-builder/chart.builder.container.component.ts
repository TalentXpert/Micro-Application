import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';

import { MicroApplicationEvent, MicroApplicationEventData, MicroApplicationOperation } from '../micro-application.event';
import { ChartBuilderService } from '../services/chart.builder.service';
import { ChartBuilderViewModel } from './chart.builder.viewmodel';
import { ChartColumnSchema, ChartSchema, ChartType } from './chart.builder.model';
import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { AlertModalService } from '../alert/alert.modal.service';
import { ValidationMessage } from '../utilities/validaton.messages';


@Component({
    selector: 'app-chartBuilder',
    templateUrl: './chart.builder.container.component.html',
    styleUrls:  ['./chart.builder.container.component.css'],
    standalone: false,
})

export class ChartBuilderContainerComponent implements OnInit {
    observableSubscription: Subscription;
    chartBuilderForm: FormGroup;
    validationMessage: ValidationMessage;
    chartBuilderViewModel: ChartBuilderViewModel;
    chartTypes: ChartType[] = [];
    selectedChartColumn: ChartColumnSchema;
    IsClickedOnCard: boolean = false;
    dataSourceId: string = "";
    selectedChartId: string = "";


    constructor(private formBuilder: FormBuilder, private chartBuilderService: ChartBuilderService,
        private applicationPageEvent: MicroApplicationEvent, private alertModalService: AlertModalService) {
        this.validationMessage = new ValidationMessage();
        this.chartBuilderViewModel = new ChartBuilderViewModel(applicationPageEvent);

    }

    ngOnInit() {
        this.createForm();
        this.getDashboardSchema();
        this.getChartTypes();
        this.getCharts();
    }

    createForm() {
        this.chartBuilderForm = this.formBuilder.group({
            Name: ['', Validators.required],
            DataSource: ['', Validators.required],
            ChartType: ['', Validators.required],
            Description: ["", Validators.compose([Validators.minLength(2), Validators.maxLength(64)])],
            MinWidth:[""],
            MinHeight:[""],
        });
    }

    getDashboardSchema() {
        this.observableSubscription = this.chartBuilderService.getDashboardSchema().subscribe((data) => {
            this.applicationPageEvent.publish(new MicroApplicationEventData(MicroApplicationOperation.LoadDataSourecesForChartBuilder, data));
        },
            (error) => {
                throw error;
            })
    }

    getCharts() {
        this.observableSubscription = this.chartBuilderService.getCharts().subscribe((data) => {
            this.applicationPageEvent.publish(new MicroApplicationEventData(MicroApplicationOperation.LoadAllCharts, data));
        },
            (error) => {
                throw error;
            })
    }

    onSelectDataSource(dataSrcId) {
        if (dataSrcId) {
            this.observableSubscription = this.chartBuilderService.getDataSourceColumns(dataSrcId).subscribe((data) => {
                this.applicationPageEvent.publish(new MicroApplicationEventData(MicroApplicationOperation.LoadDataSoureceColumns, data));
                this.dataSourceId = dataSrcId;
                if (this.selectedChartId) {
                    this.applicationPageEvent.publish(new MicroApplicationEventData(MicroApplicationOperation.LoadColumnsOfSelectedChart, this.selectedChartId));
                }
            },
                (error) => {
                    throw error;
                })
        }
    }

    getChartTypes() {
        this.observableSubscription = this.chartBuilderService.getChartTypes().subscribe((data) => {
            this.chartTypes = data;
        },
            (error) => {
                throw error;
            })
    }

    onSelectChart(chart: ChartSchema) {
        this.selectedChartId = chart.Id;
        this.onSelectDataSource(chart.DataSourceId);
        this.patchForm(chart);

    }

    patchForm(chart: ChartSchema) {
        this.dataSourceId = chart.DataSourceId;
        this.chartBuilderForm.controls["ChartType"].patchValue(chart.ChartType);
        this.chartBuilderForm.controls["DataSource"].patchValue(chart.DataSourceId);
        this.chartBuilderForm.controls["Name"].patchValue(chart.Name);
        this.chartBuilderForm.controls["Description"].patchValue(chart.Description);
        this.chartBuilderForm.controls["MinWidth"].patchValue(chart.MinWidth);
        this.chartBuilderForm.controls["MinHeight"].patchValue(chart.MinHeight);
    }

    deleteChart(chart) {

    }

    drop(event: CdkDragDrop<any[]>, indexRow: number, indexColumn: number) {
        let field = event.previousContainer.data[event.previousIndex];
        if (field.ColumnId == '1') {
            if (event.previousContainer.id != event.container.id) {

                let field = event.previousContainer.data[event.previousIndex];
                this.chartBuilderViewModel.ChangeColumnId(field);
                this.selectedChartColumn = field;
                this.setSelectedFormControlProperties();
                this.dropCard(event.previousContainer.data, event.container.data, event.previousIndex, event.currentIndex);

            }
        }
        else {
            if (field.ColumnId == 2) {
                moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
            }
        }
    }

    dropCard(previousIndexData, currentIndexData, previousIndex, currentIndex) {
        transferArrayItem(previousIndexData, currentIndexData, previousIndex, currentIndex);
    }

    nodeClick(event: ChartColumnSchema) {
        this.IsClickedOnCard = true;
        this.selectedChartColumn = event;
        this.setSelectedFormControlProperties();
    }

    setSelectedFormControlProperties() {
        if (this.selectedChartColumn) {
            var index = this.chartBuilderViewModel.chartColumnSchema.findIndex(c => c.DatabaseColumnName == this.selectedChartColumn.DatabaseColumnName);
            if (index > -1 && this.chartBuilderViewModel.chartColumnSchema[index].ColumnId == 2) {
                this.chartBuilderViewModel.chartColumnSchema[index].Color = this.selectedChartColumn.Color;
                this.chartBuilderViewModel.chartColumnSchema[index].DataType = this.selectedChartColumn.DataType
                this.chartBuilderViewModel.chartColumnSchema[index].DatabaseColumnName = this.selectedChartColumn.DatabaseColumnName;
                this.chartBuilderViewModel.chartColumnSchema[index].Title = this.selectedChartColumn.Title;
                this.chartBuilderViewModel.chartColumnSchema[index].IsMandatory = this.selectedChartColumn.IsMandatory;
            }
        }

    }

    resetForm() {
        this.applicationPageEvent.publish(new MicroApplicationEventData(MicroApplicationOperation.LoadDataSoureceColumns, this.chartBuilderViewModel.selectedDataSourceColumns));
        this.clearFields();
    }

    clearFields(){
        this.selectedChartId = "";
        this.IsClickedOnCard = false;
        this.chartBuilderForm.controls["ChartType"].patchValue("");
        this.chartBuilderForm.controls["Name"].patchValue("");
        this.chartBuilderForm.controls["Description"].patchValue("");
        this.chartBuilderForm.controls["MinWidth"].patchValue("");
        this.chartBuilderForm.controls["MinHeight"].patchValue("");
    }

    getClassForInterviewCard(field) {
        if (this.selectedChartColumn && this.selectedChartColumn.Title == field.Title) return 'tasktype tasktype-mb-2 scheduleinterviewyellow';
        return 'tasktype tasktype-mb-2';
    }


    save() {
        let columns = this.chartBuilderViewModel.chartColumnSchema.filter(fc => fc.ColumnId == 2)
        let chartSchema = new ChartSchema();
        chartSchema.Columns = columns;
        chartSchema.DataSourceId = this.dataSourceId;
        chartSchema.ChartType = this.chartBuilderForm.controls["ChartType"].value;
        chartSchema.Name = this.chartBuilderForm.controls["Name"].value;
        chartSchema.Description = this.chartBuilderForm.controls["Description"].value;
        chartSchema.MinHeight = this.chartBuilderForm.controls["MinHeight"].value;
        chartSchema.MinWidth = this.chartBuilderForm.controls["MinHeight"].value;
        if (this.selectedChartId) chartSchema.Id = this.selectedChartId;
        this.observableSubscription = this.chartBuilderService.save(chartSchema).subscribe((data) => {
            if (data) this.alertModalService.setSuccessAlertModalTemplate("Colums saved successfully.");
            this.applicationPageEvent.publish(new MicroApplicationEventData(MicroApplicationOperation.AddUpdateChartSchema,chartSchema));
            this.dataSourceId = "";
            this.chartBuilderViewModel.chartColumnSchema.length = 0;
            this.clearFields();
            
        },
            (error) => {
                throw error;
            })
    }


    remove(field: ChartColumnSchema) {
        this.chartBuilderViewModel.ChangeColumnId(field);
    }

    ngOnDestroy() {
        if (this.observableSubscription) {
            this.observableSubscription.unsubscribe();
        }
    }
}