import { Routes } from '@angular/router';

export const routes: Routes = [
    { path: 'appPage', loadChildren: () => import('./micro-application/micro-application.module').then(m => m.MicroApplicationModule)},
];
