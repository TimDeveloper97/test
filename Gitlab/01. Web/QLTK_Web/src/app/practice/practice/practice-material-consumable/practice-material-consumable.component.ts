import { Component, OnInit, Input, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';

import { MessageService, Configuration, Constants, AppSetting, PermissionService, ComponentService } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PracticeService } from '../../service/practice.service';
import { PracticeMaterialChooseComponent } from '../practice-material-choose/practice-material-choose.component';
import { SelectPracticeMaterialComsumableComponent } from '../select-practice-material-comsumable/select-practice-material-comsumable.component';
import { HistoryVersionComponent } from 'src/app/shared/component/history-version/history-version.component';
import { HistoryVersionService } from 'src/app/shared/services/history-version.service';

@Component({
  selector: 'app-practice-material-consumable',
  templateUrl: './practice-material-consumable.component.html',
  styleUrls: ['./practice-material-consumable.component.scss'],
  encapsulation: ViewEncapsulation.None

})
export class PracticeMaterialConsumableComponent implements OnInit {

  @Input() Id: string;
  constructor(
    private router: Router,
    public appSetting: AppSetting,
    private messageService: MessageService,
    private config: Configuration,
    private modalService: NgbModal,
    private practiceService: PracticeService,
    public constants: Constants,
    public permissionService: PermissionService,
    private serviceHistory: HistoryVersionService,
    private componentService: ComponentService
  ) { }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm mã hoặc tên vật tư ...',
    Items: [

    ]
  };

  listData: any = [];
  model: any = {
    page: 1,
    PageSize: 10,
    TotalItem: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    Id: '',
    Name: '',
    Code: '',
    Quantity: '',
    TotalPrice: '',
    MaterialId: '',
    PracticeId: '',
    listSelect: []
  };
  totalAmount = 0;
  maxLeadTime = 0;

  ngOnInit() {
    this.appSetting.PageTitle = "Chỉnh sửa bài thực hành/công đoạn";
    this.model.PracticeId = this.Id;
    this.searchPracticeMaterialConsumable();
  }

  showClick() {
    let activeModal = this.modalService.open(SelectPracticeMaterialComsumableComponent, { container: 'body', windowClass: 'practice-material-consumable-select', backdrop: 'static' });
    activeModal.componentInstance.PracticeId = this.model.PracticeId;
    var ListIdSelect = [];
    this.listData.forEach(element => {
      ListIdSelect.push(element.MaterialId);
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
    if (this.listData && this.listData.length > 0) {
      this.maxLeadTime = Math.max.apply(Math, this.listData.map(function (o) { return o.Leadtime; }));
    }
  }

  searchPracticeMaterialConsumable() {
    if (!this.permissionService.checkPermission(['F040715'])) {
      this.practiceService.searchPracticeMaterialConsumable(this.model).subscribe(data => {
        this.listData = data.ListResult;
        this.calculateTotal();
      }, error => {
        this.messageService.showError(error);
      }
      );
    }

  }

  save() {
    this.model.listSelect = this.listData;
    this.practiceService.createPracticeMaterialConsumable(this.model).subscribe(
      data => {
        this.messageService.showSuccess('Lưu thông tin vật tư tiêu hao thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  showConfirmDelete(model:any) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá vật tư tiêu hao này không?").then(
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
    this.practiceService.exportExcelPracticeConsumable(this.model).subscribe(d => {
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
    let templatePath = this.config.ServerApi + 'Template/Template_BTH_VatTuTieuHao.xlsx';
    this.componentService.showImportExcel(templatePath, false).subscribe(file => {
      if (file) {
        this.practiceService.importMaterialConsumable(file).subscribe(data => {
          if (data) {
            var isExist = false;
            data.forEach(material => {
              isExist = false;
              this.listData.forEach(materialSub => {
                if (material.MaterialId == materialSub.MaterialId) {
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
