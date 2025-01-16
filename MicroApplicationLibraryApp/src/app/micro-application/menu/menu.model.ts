import { Subscription } from 'rxjs';
import { MicroApplicationEvent, MicroApplicationOperation } from '../micro-application.event';

export class MenuModel {
  public Id: string="";
  public Name: string="";
  public Link: string="";
  public IsFixed: boolean=false;
  public IsLanding: boolean=false;
  public Children: MenuModel[] = [];

}

export class MenuViewModel {
  subscription: Subscription = new Subscription();
  PermissionList: string[] = [];
  IsUserLoggedIn: boolean = false;
  menuList: MenuModel[] = [];


  constructor(private applicationPageEvent: MicroApplicationEvent) {
    this.menuList =[];
    var list = this.getMenusList();
    if(list) this.menuList = list;
    if(this.menuList && this.menuList.length > 0) this.IsUserLoggedIn =true;
    this.subscribe();
  }

  subscribe() {
    this.subscription = this.applicationPageEvent.event.subscribe((event) => {
      if (event.operation == MicroApplicationOperation.LoadMenusForMicroApplication)
        this.loadMenuList(event.data);

    });
  }

  loadMenuList(menuModel: MenuModel[]) {
    this.IsUserLoggedIn = true;
    this.menuList = [];
    this.menuList = menuModel;
    this.saveMenusList(menuModel);
  }

  clearlocalStorage() {
    this.IsUserLoggedIn = false;
    this.menuList.length= 0;
    localStorage.removeItem('menuList');
    localStorage.removeItem('user');
  }

  saveMenusList(menuList: MenuModel[]): boolean {
    if (menuList != null) {
      localStorage.setItem('menuList', JSON.stringify(menuList));
    }
    else {
      localStorage.removeItem('menuList');
    }
    return true;
  }

  getMenusList(): any {
    var menuList = localStorage.getItem('menuList');
    if (menuList) {
      return JSON.parse(menuList);
    }
    return menuList;
  }

  unsubscribe() {
    this.subscription.unsubscribe();
  }
}
