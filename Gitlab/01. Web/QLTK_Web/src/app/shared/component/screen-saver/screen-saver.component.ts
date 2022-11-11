import { Component, OnInit } from '@angular/core';
import { AppSetting } from '../..';

@Component({
  selector: 'app-screen-saver',
  templateUrl: './screen-saver.component.html',
  styleUrls: ['./screen-saver.component.scss']
})
export class ScreenSaverComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
  ) { }

  ngOnInit() {
    this.appSetting.PageTitle = "   ";
  }

}
