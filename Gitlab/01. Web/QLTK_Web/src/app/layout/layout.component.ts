import { Component, OnInit } from '@angular/core';

import {  AppSetting } from '../shared';
import { NtsNavigationService } from './navigation/navigation.service';
import { navigation } from './navigation/navigation';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit {

  constructor(
    public appSetting:AppSetting,    
    private ntsNavigationService: NtsNavigationService,) {

  }

  ngOnInit() {
  }

}
