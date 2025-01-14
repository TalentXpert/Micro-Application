import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ApplicationPageContainerComponent } from './application-page-container/application.page.container.component';
import { FilterComponent } from './filters/filter-form/filter.form.component';
import { MicroApplicationRoutingModule } from './micro-application-routing-module';
import { SmartFormComponent } from './smart-form/smart.form.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { GridColumnSettingComponent } from './columnsetting/gridcolumnsetting.component';
import { MicroApplicationEvent } from './micro-application.event';
import { FormBuilderContainerComponent } from './form-builder/form.builder.container.component';
import { ApplicationPageService } from './services/application.page.service';
import { UtilityService } from './library/utility.service';
import { PageBuilderService } from './services/page.builder.service';
import { AddEditFormComponent } from './form-builder/add-form/add.edit.form.component';
import { ViewPageContentComponent } from './view-page-content/view.page.content.component';
import { ImportExcelComponent } from './import-excel/import.excel.component';
import { FormConfigurationContainerComponent } from './form-configuration/form.configuration.container.component';
import { ModuleWithProviders } from '@angular/core';
import { AlertModalService } from './alert/alert.modal.service';
import { AlertModalComponent } from './alert/alert.modal.component';
import { DashboardContainerComponent } from './dashboard/dashboard.container/dashboard.container.component';
import { DashboardService } from './services/dashboard.service';
import { DrawChartComponent } from './dashboard/draw-chart/draw.chart.component';
import { DashboardPanelComponent } from './dashboard/dashboard-panel/dashboard.panel.component';
import { GoogleChartsModule } from 'angular-google-charts';
import { ChartBuilderContainerComponent } from './chart-builder/chart.builder.container.component';
import { AppFormControlService } from './services/app.form.control.service';
import { DashboardBuilderService } from './services/dashboard.builder.service';
import { DashboardBuilderContainerComponent } from './dashboard-builder/dashboard.builder.container.component';
import { AddEditDashboardComponent } from './dashboard-builder/add-dashboard/add.edit.dashboard.component';
import { AddEditRowComponent } from './dashboard-builder/add-row/add.edit.row.component';
import { AddEditPanelComponent } from './dashboard-builder/add.edit.panel/add.edit.panel.component';
import { ChartBuilderService } from './services/chart.builder.service';
import { UIFormComponent } from './UI-form/UI.form.component';
import { SelectFromListFormComponent } from './select-from-list-form/select.from.list.form.component';
import { FormDataCollectionService } from './services/form.data.collection.service';
import { SpecialFormComponent } from './special-form/special.form.component';


@NgModule({
    imports:
        [
            CommonModule,
            FormsModule,
            ReactiveFormsModule,
            NgSelectModule,
            DragDropModule,
            MicroApplicationRoutingModule,
            GoogleChartsModule
        ],
    declarations: [

        ApplicationPageContainerComponent,
        FilterComponent,
        SmartFormComponent,
        GridColumnSettingComponent,
        FormConfigurationContainerComponent,
        FormBuilderContainerComponent,
        AddEditFormComponent,
        ViewPageContentComponent,
        ImportExcelComponent,
        AlertModalComponent,
        DashboardContainerComponent,
        DrawChartComponent,
        DashboardPanelComponent,
        ChartBuilderContainerComponent,
        DashboardBuilderContainerComponent,
        AddEditDashboardComponent,
        AddEditRowComponent,
        AddEditPanelComponent,
        UIFormComponent,
        SelectFromListFormComponent,
        SpecialFormComponent
    ],
    providers: [MicroApplicationEvent, UtilityService, AlertModalService],
    exports:
        [
            CommonModule,
            FormsModule,
            ReactiveFormsModule,
            ApplicationPageContainerComponent,
            UIFormComponent
        ]
})

export class MicroApplicationModule {
   static forRoot(configuration: any): ModuleWithProviders<MicroApplicationModule>{
        console.log(configuration);
        return {
          ngModule: MicroApplicationModule,
          providers: [ApplicationPageService,
                      AppFormControlService,
                      PageBuilderService,
                      DashboardService,
                      DashboardBuilderService,
                      ChartBuilderService,
                      FormDataCollectionService,
                      {provide: 'config', useValue: configuration}
                ]
        };
      }
}
