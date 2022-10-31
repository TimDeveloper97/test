import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AppSetting } from 'src/app/shared';

@Component({
  selector: 'app-tivi-layout',
  templateUrl: './tivi-layout.component.html',
  styleUrls: ['./tivi-layout.component.scss'],
  encapsulation:ViewEncapsulation.None
})
export class TiviLayoutComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,) { }

  ngOnInit(): void {
  }

}
