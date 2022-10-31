import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';

import { AppSetting, Constants, MessageService } from 'src/app/shared';
import { ModuleCreateService } from '../../services/module-create-service';

@Component({
  selector: 'app-module-version-tab',
  templateUrl: './module-version-tab.component.html',
  styleUrls: ['./module-version-tab.component.scss']
})

export class ModuleVersionTabComponent implements OnInit {
  @Input() Id: string;
  constructor(
    private router: Router,
    public appSetting: AppSetting,
    private moduleServiceService: ModuleCreateService,
    public constants: Constants,
    public messageService: MessageService
  ) { }

  ngOnInit() {
    this.moduleModel.Id = this.Id;
    this.getHistoryInfo();
  }

  listHistory: any[] = [];
  moduleModel: any = {
    OrderBy: 'Version',
    ListHistory: []
  }

  getHistoryInfo() {
    this.moduleServiceService.getById(this.moduleModel).subscribe(data => {
      this.listHistory = data.ListHistory;
    },
    error => {
      this.messageService.showError(error);
    });
  }

  closeModal() {
    this.router.navigate(['module/quan-ly-module']);
  }
}
