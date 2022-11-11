import { Component, OnInit } from '@angular/core';
import { ConfigService } from '../../services/config.service';
import { AppSetting, MessageService, Constants } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Title } from '@angular/platform-browser';
import { ConfigCreateComponent } from '../config-create/config-create.component';

@Component({
  selector: 'app-config-manage',
  templateUrl: './config-manage.component.html',
  styleUrls: ['./config-manage.component.scss']
})
export class ConfigManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private titleservice: Title,
    private service: ConfigService,
    public constant: Constants
  ) { }

  StartIndex = 0;
  totalItems: number = 0;
  listData: any[] = [];

  ngOnInit() {
    this.appSetting.PageTitle = "Cấu hình";
    this.searchConfig();
  }

  searchConfig() {
    this.service.searchConfig().subscribe((data: any) => {
      if (data.ListResult) {
        this.listData = data.ListResult;
        this.totalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  ShowCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(ConfigCreateComponent, { container: 'body', windowClass: 'config-create--model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchConfig();
      }
    }, (reason) => {
    });
  }

}
