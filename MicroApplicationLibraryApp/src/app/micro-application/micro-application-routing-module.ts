import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ApplicationPageContainerComponent } from './application-page-container/application.page.container.component';
import { FormBuilderContainerComponent } from './form-builder/form.builder.container.component';
import { FormConfigurationContainerComponent } from './form-configuration/form.configuration.container.component';
import { DashboardContainerComponent } from './dashboard/dashboard.container/dashboard.container.component';
import { ChartBuilderContainerComponent } from './chart-builder/chart.builder.container.component';
import { DashboardBuilderContainerComponent } from './dashboard-builder/dashboard.builder.container.component';


const microApplicationRoutes: Routes = [
    
    { path: 'fb', component: FormBuilderContainerComponent },
    { path: 'fc', component: FormConfigurationContainerComponent },
    { path: 'ap', component: ApplicationPageContainerComponent },
    { path: 'assetdashboard', component: DashboardContainerComponent },
    { path: 'cb', component: ChartBuilderContainerComponent },
    { path: 'db', component: DashboardBuilderContainerComponent },
    

];

@NgModule({
    imports: [
        RouterModule.forChild(microApplicationRoutes)
    ],
    exports: [
        RouterModule
    ]
})
export class MicroApplicationRoutingModule { }
