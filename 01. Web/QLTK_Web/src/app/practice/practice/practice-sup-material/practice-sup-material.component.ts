import { Component, OnInit, Input, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';

import { MessageService, Configuration, Constants, AppSetting, PermissionService, ComponentService } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PracticeService } from '../../service/practice.service';
import { PracticeSupMaterialChooseComponent } from '../practice-sup-material-choose/practice-sup-material-choose.component';
import { PracticeSupModuleChooseComponent } from '../practice-sup-module-choose/practice-sup-module-choose.component';
import { HistoryVersionComponent } from 'src/app/shared/component/history-version/history-version.component';
import { HistoryVersionService } from 'src/app/shared/services/history-version.service';

@Component({
  selector: 'app-practice-sup-material',
  templateUrl: './practice-sup-material.component.html',
  styleUrls: ['./practice-sup-material.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class PracticeSupMaterialComponent implements OnInit {
  @Input() Id: string;
  constructor(
    private router: Router,
    private messageService: MessageService,
    private config: Configuration,
    private modalService: NgbModal,
    private practiceService: PracticeService,
    public constants: Constants,
    public appSetting: AppSetting,
    public permissionService: PermissionService,
    private serviceHistory: HistoryVersionService,
    private componentService: ComponentService
  ) { }

  listData: any = [];
  model: any = {
    page: 1,
    PageSize: 10,
    TotalItem: 0,
    PageNumber: 1,
    OrderBy: 'Name',
    OrderType: true,
    Id: '',
    Name: '',
    Quantity: '',
    TotalPrice: '',
    MaterialId: '',
    PracticeId: '',
    listSelect: []
  };

  maxLeadTime = 0;
  totalAmount = 0;

  ngOnInit() {
    this.appSetting.PageTitle = "Chỉnh sửa bài thực hành, giáo trình";
    this.model.PracticeId = this.Id;
    this.searchPrachSupMaterial();
  }

  showClick() {
    let activeModal = this.modalService.open(PracticeSupMaterialChooseComponent, { container: 'body', windowClass: 'practice-sup-material-model', backdrop: 'static' });
    activeModal.componentInstance.PracticeId = this.model.PracticeId;
    var ListIdSelect = [];
    this.listData.forEach(element => {
      if (element.Type == 1) {
        ListIdSelect.push(element.Id);
      }
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.listData.push(element);
          this.calculateTotal();
        });
      }
    }, (reason) => {
    });
  }

  showClickChooseModule() {
    let activeModal = this.modalService.open(PracticeSupModuleChooseComponent, { container: 'body', windowClass: 'practice-sup-module-model', backdrop: 'static' });
    activeModal.componentInstance.PracticeId = this.model.PracticeId;
    var ListIdSelect = [];
    this.listData.forEach(element => {
      if (element.Type == 2) {
        ListIdSelect.push(element.Id);
      }
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.listData.push(element);
          this.calculateTotal();
        });
      }
    }, (reason) => {
    });
  }

  calculateTotal() {
    this.totalAmount = this.listData.reduce((a, b) => a + (b.Quantity * b.Pricing), 0);
    if (this.listData && this.listData.length > 0){
      this.maxLeadTime = Math.max.apply(Math, this.listData.map(function (o) { return o.Leadtime; }));
    }
  }

  searchPrachSupMaterial() {
    if (!this.permissionService.checkPermission(['F040711'])) {
      this.practiceService.searchPracticeSupMaterial(this.model).subscribe(data => {
        this.listData = data.ListResult;
        this.maxLeadTime = data.MaxDeliveryDay;
        this.calculateTotal();
      }, error => {
        this.messageService.showError(error);
      }
      );
    }
  }

  save() {
    this.practiceService.createPracticeSupMaterial({ PracticeId: this.model.PracticeId, Materials: this.listData }).subscribe(
      data => {
        this.messageService.showSuccess('Lưu thông tin thiết bị phụ trợ thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  showConfirmDelete(model:any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá thiết bị phụ trợ này không?").then(
      data => {
        this.delete(model);
      },
      error => {
        
      }
    );
  }

  delete(model:any) {
    var index = this.listData.indexOf(model);
    if (index > -1) {
      this.listData.splice(index, 1);
    }
  }

  closeModal() {
    this.router.navigate(['thuc-hanh/quan-ly-bai-thuc-hanh']);
  }

  exportExcel() {
    this.practiceService.exportExcelPracticeSupMaterial(this.model).subscribe(d => {
      var link = document.createElement('a');
      link.setAttribute("type", "hidden");
      link.href = this.config.ServerApi + d;
      link.download = 'Download.docx';
      document.body.appendChild(link);
      // link.focus();
      link.click();
      document.body.removeChild(link);
    }, e => {
      this.messageService.showError(e);
    });
  }

  showConfirmUploadVersion() {
    this.messageService.showConfirmFile("Bạn có muốn thay đổi version không?").then(
      async data => {
        if (data) {
          await this.showEditContent();
        } else {
          this.save();
        }
      }
    );
  }

  async showEditContent() {
    let activeModal = this.modalService.open(HistoryVersionComponent, { container: 'body', windowClass: 'show-history-version-modal', backdrop: 'static' });
    activeModal.componentInstance.id = this.Id;
    activeModal.componentInstance.type = this.constants.HistoryVersion_Version_Practice;
    activeModal.result.then(async (result) => {
      if (result) {
        await this.save();
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

  showImportMaterial() {
    let templatePath = this.config.ServerApi + 'Template/Template_BTH_ThietBiPhuTro.xlsx';
    this.componentService.showImportExcel(templatePath, false).subscribe(file => {
      if (file) {
        this.practiceService.importSubMaterial(file).subscribe(data => {
          if (data) {
            var isExist = false;
            data.forEach(material => {
              isExist = false;
              this.listData.forEach(materialSub => {
                if (material.Type == materialSub.Type && material.MaterialId == materialSub.MaterialId) {
                  isExist = true;
                  materialSub.Quantity += material.Quantity;
                }
              });

              if (!isExist) {
                this.listData.push(material);
              }
            });
            this.calculateTotal();
            this.messageService.showSuccess('Import vật tư thành công!');
          }
        }, error => {
          this.messageService.showError(error);
        });
      }
    });
  }
}
