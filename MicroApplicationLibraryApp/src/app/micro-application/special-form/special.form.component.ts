import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { InputSelectFromListForm } from '../select-from-list-form/select.from.list.form.model';

@Component({
    selector: 'app-special-form',
    templateUrl: './special.form.component.html',
    standalone:false
})

export class SpecialFormComponent  {
    inputSelectFromListForm: InputSelectFromListForm;
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

