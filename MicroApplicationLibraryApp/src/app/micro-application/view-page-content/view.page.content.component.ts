import { Component, OnInit, OnDestroy, EventEmitter, Output } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';
import { SmartAction } from '../application-page-container/application.page.model';
import { ApplicationPageService } from '../services/application.page.service';
import { PageContentView } from './view.page.content.model';



@Component({
    selector: 'app-viewPageContent',
    templateUrl: './view.page.content.component.html',
    standalone:false
})

export class ViewPageContentComponent implements OnInit, OnDestroy {
    observableSubscription = new Subscription();
    SmartAction = new  SmartAction();
    DataKey: string ="";
    pageContentView = new  PageContentView();

    constructor(public ngbActiveModal: NgbActiveModal, private applicationPageService: ApplicationPageService,
    ) {

    }

    ngOnInit() {
        if (this.SmartAction && this.DataKey) this.getViewPageContents();
    }


    getViewPageContents() {
        this.observableSubscription = this.applicationPageService.getViewPageContents(this.SmartAction.FormId, this.DataKey).subscribe(response => {
               this.pageContentView = response;
        });
    }

    close() {
        this.ngbActiveModal.close();
    }

    // innerHtmlContent(data){
    //     if (data) return data.replace(/\n/g, "<br />");
    //     return data;
    // }

    ngOnDestroy() {
        if (this.observableSubscription) {
            this.observableSubscription.unsubscribe();
        }
    }
}
