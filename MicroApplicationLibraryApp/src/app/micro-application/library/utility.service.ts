import { Injectable } from "@angular/core";

import { BehaviorSubject } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class UtilityService {

  public setLoaderVisibilityChange = new BehaviorSubject<any>({});
  getLoaderVisibilityChange = this.setLoaderVisibilityChange.asObservable();
  datePlaceholder: string = 'Select Date';
  calenderCultureFormat: string = "";
  
  // datePickerConfig: IMyDpOptions = {
  //   disableUntil: { year: 0, month: 0, day: 0 },
  //   dateFormat: 'dd/mm/yyyy',
  //   editableDateField: false
  // };
 
  sortDataForOptions(array: Array<any>, key: any): Array<any> {
    if (array) {
        if (array.length > 0) {
            if (key)
                return array.sort((a, b) => a[key] < b[key] ? -1 : 1);
            else
                return array.sort((a, b) => a < b ? -1 : 1);
        }
    }
    return [];
}
setLoaderVisibility(setLoader: boolean) {
  this.setLoaderVisibilityChange.next(setLoader)
}

 //convert object to formdata
 createFormData(object: Object, formdata?: FormData, childValue?: string): any {

  for (let property in object) {
    const formKey = childValue ? `${childValue}[${property}]` : property;
    if (formKey == "CV") continue;
    if (object[property] instanceof Date) {
      if(formdata) formdata.append(formKey, object[property].toISOString());
    } else if (typeof object[property] === 'object' && !(object[property] instanceof File)) {
      this.createFormData(object[property], formdata, formKey);
    }

    else {
      if(formdata) formdata.delete(formKey);
      if(formdata) formdata.append(formKey, object[property]);
    }
  }
  return formdata;
}
convertDateVMToReadableFormat(date: any) {
  var month_names = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
  return "" + date.Day + "-" + month_names[date.Month - 1] + "-" + date.Year;
}

convertDateToReadableFormat(date: any) {
  var month_names = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
  return "" + date.day + " " + month_names[date.month - 1] + " " + date.year;
}

}