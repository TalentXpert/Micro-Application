/* Angular Imports */

import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

/* Feature Modules */
import { AppComponent } from "./app.component";
import { ConfigService } from './services/config.service';
import { MicroApplicationModule } from './micro-application/micro-application.module';



  const config = new ConfigService();

@NgModule({
  declarations: [
       
    ],
  imports: [
      BrowserModule,
      FormsModule,
      ReactiveFormsModule,
      HttpClientModule,
      MicroApplicationModule,
      MicroApplicationModule.forRoot({configuration: config.dataServiceBaseUrl()}),
    
     
  ],
  providers: [],
  bootstrap: [AppComponent],
  exports:[]
})
export class AppModule {
  constructor()
  {
  }
 }
