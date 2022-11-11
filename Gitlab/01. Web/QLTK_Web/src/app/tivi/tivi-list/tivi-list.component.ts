import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AppSetting } from 'src/app/shared';

@Component({
  selector: 'app-tivi-list',
  templateUrl: './tivi-list.component.html',
  styleUrls: ['./tivi-list.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TiviListComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,) { }

  ngOnInit(): void {
    this.appSetting.PageTitle = "Danh sách chức năng trên tivi";
  }

}
