import { Component, ViewChild, OnInit } from '@angular/core';
import { Router } from '@angular/router';

declare var $: any;

import { PerfectScrollbarComponent, PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { AppSetting } from '../../shared';

@Component({
  selector: 'app-leftbar',
  templateUrl: './leftbar.component.html',
  styleUrls: ['./leftbar.component.scss']
})
export class LeftbarComponent implements OnInit {

  private scrollConfig: PerfectScrollbarConfigInterface;

  @ViewChild(PerfectScrollbarComponent) componentScroll: PerfectScrollbarComponent;

  constructor(private router: Router, private appSetting: AppSetting) {


  }

  ngOnInit() {
    this.scrollConfig = {
      suppressScrollX: true,
      suppressScrollY: false,
      minScrollbarLength: 20,
      wheelPropagation: true
    };

    this.router.events.subscribe((path) => {
      this.changeLink();
    });
  }

  private  changeLink(){
    var navHeight = 0;
    if (!this.appSetting.MenuFolded) {
      navHeight = $("#main-content section.wrapper .content-wrapper").innerHeight() + 90;

      if( navHeight < $(window).innerHeight() - 60){
        navHeight = $(window).innerHeight() - 60;
      }
    } else {
      navHeight = $(window).innerHeight() - 60;
    }
    //console.log(navHeight);
    $("#main-menu-wrapper").height(navHeight);

    this.componentScroll.directiveRef.update();
  }

}
