import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { MenuModel, MenuViewModel } from './menu.model';
import { MicroApplicationGlobalService } from '../services/micro.application.global.service';


@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css'],
  standalone:false,
  styles: ['.disableLink {pointer-events: none;cursor: not-allowed;}']
})
export class MenuComponent implements OnInit, OnDestroy {
  menuViewModel: MenuViewModel;
    observableSubscription: Subscription | null = null;;

  constructor(private microApplicationGlobalService: MicroApplicationGlobalService, private router: Router) {
    this.menuViewModel = this.microApplicationGlobalService.menuViewModel;
  }

  ngOnInit() {
    
  }

  logout() {
    this.menuViewModel.clearlocalStorage();
    this.router.navigate(['']);
  }


  getChildren(index:number){
    let childList:MenuModel[]=[];
      this.menuViewModel.menuList[index].Children.forEach(child=>{
        childList.push(child); 
      });
    return childList;
  }
 
  handleEvent(menu:MenuModel){
    this.router.navigate([menu.Link],{ queryParams: { pageId: menu.Id }, queryParamsHandling: 'merge' });
  }

  ngOnDestroy() {
    if (this.observableSubscription) {
      this.observableSubscription.unsubscribe();
    }
  }
}
