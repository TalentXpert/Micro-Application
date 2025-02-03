import { Component, OnInit, Output, EventEmitter, OnChanges, SimpleChanges, } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { UIFormClientVM } from '../UI-form/UI.form.model';

@Component({
    selector: 'app-smart-form',
    templateUrl: './smart.form.component.html',
    standalone:false
})

export class SmartFormComponent  {
    uIFormClientVM: UIFormClientVM = new UIFormClientVM();
    @Output() sendResponse: EventEmitter<boolean> = new EventEmitter();

    constructor(public ngbActiveModal: NgbActiveModal) {

    }

    formResponseSaved(){
        this.sendResponse.emit(true); 
        this.close();
    }


    close() {
        this.ngbActiveModal.close();
    }


   
}

