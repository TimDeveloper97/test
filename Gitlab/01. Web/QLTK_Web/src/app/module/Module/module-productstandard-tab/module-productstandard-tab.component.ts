import { Component, OnInit, Input, ViewEncapsulation } from '@angular/core';

import { Constants, AppSetting, MessageService } from 'src/app/shared';
import { ModuleServiceService } from '../../services/module-service.service';
import { ModuleGroupService } from '../../services/module-group-service';
import { HistoryVersionComponent } from 'src/app/shared/component/history-version/history-version.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { HistoryVersionService } from 'src/app/shared/services/history-version.service';

@Component({
  selector: 'app-module-productstandard-tab',
  templateUrl: './module-productstandard-tab.component.html',
  styleUrls: ['./module-productstandard-tab.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ModuleProductstandardTabComponent implements OnInit {
  @Input() ModuleGroupId: string;
  @Input() Id: string;
  
  constructor(
    private messageService: MessageService,
    public appSetting: AppSetting,
    private service: ModuleServiceService,
    private serviceGroup: ModuleGroupService,
    public constants: Constants,
    private modalService: NgbModal,
    private serviceHistory: HistoryVersionService
  ) { }
  height = 200;
  listDA: any[] = [];
  listData: any[] = [];
  listProductStandard: any[] = [];
  listSelect: any = [];
  listModuleModuleProductsand: any = [];

  model: any = {
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    Id: '',
    ModuleId: '',
    ProductStandardId: '',
    ModuleGroupId: '',
    List: []
  }

  ngOnInit() {
    this.height = window.innerHeight - 290;
    this.model.ModuleGroupId = this.ModuleGroupId;
    this.model.ModuleId = this.Id;
    this.getModuleGroupInfo();
  }

  getModuleGroupInfo() {
    this.serviceGroup.getModuleGroupInfo({ Id: this.ModuleGroupId }).subscribe(data => {
      this.listData = data.ListProductStandard;
      this.getModuleProductStandardInfo();
    },
      error => {
        this.messageService.showError(error);
      });
  }

  getModuleProductStandardInfo() {
    this.service.getModuleProductStandardInfo({ ModuleId: this.Id }).subscribe(data => {
      this.listDA = data.ListProductStandard;
      for (var item of this.listData) {
        for (var element of this.listDA) {
          if (element.ProductStandardId == item.Id) {
            item.Checked = true;
          }
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  addModuleProductsand() {
    this.listData.forEach(element => {
      if (element.Checked) {
        this.listModuleModuleProductsand.push(element.Id);
      }
    });
    this.model.List = this.listModuleModuleProductsand;
    this.service.AddModuleGroupProductStandard(this.model).subscribe(
      data => {
        this.messageService.showSuccess('Cập nhật tiêu chuẩn thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
    this.listModuleModuleProductsand = [];
  }

  showConfirmUploadVersion() {
    this.messageService.showConfirmFile("Bạn có muốn thay đổi version không?").then(
      async data => {
        if (data) {
          await this.showEditContent();
        } else {
          this.addModuleProductsand();
        }
      }
    );
  }

  async showEditContent() {
    let activeModal = this.modalService.open(HistoryVersionComponent, { container: 'body', windowClass: 'show-history-version-modal', backdrop: 'static' });
    activeModal.componentInstance.id = this.Id;
    activeModal.componentInstance.type = this.constants.HistoryVersion_Version_Module;
    activeModal.result.then(async (result) => {
      if (result) {
        await this.addModuleProductsand();
        await this.updateVersion(result);
      }
    }, (reason) => {
    });
  }

  updateVersion(model: any) {
    this.serviceHistory.updateVersion(model).subscribe(
      () => {
        this.messageService.showSuccess('Cập nhật version thành công!');
      }, error => {
        this.messageService.showError(error);
      }
    );
  }
}
