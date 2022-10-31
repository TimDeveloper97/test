import { Component, OnInit, Input } from '@angular/core';
import { MessageService, Configuration, Constants, AppSetting, PermissionService, ComponentService } from 'src/app/shared';
import { Title } from '@angular/platform-browser';
import { PracticeService } from '../../service/practice.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ProductChooseModuleComponent } from 'src/app/device/product-choose-module/product-choose-module.component';
import { ProductService } from 'src/app/device/services/product.service';
import { ifStmt } from '@angular/compiler/src/output/output_ast';
import { HistoryVersionComponent } from 'src/app/shared/component/history-version/history-version.component';
import { HistoryVersionService } from 'src/app/shared/services/history-version.service';

@Component({
  selector: 'app-practice-module',
  templateUrl: './practice-module.component.html',
  styleUrls: ['./practice-module.component.scss']
})
export class PracticeModuleComponent implements OnInit {

  constructor(
    private config: Configuration,
    private messageService: MessageService,
    private titleservice: Title,
    public appSetting: AppSetting,
    public constants: Constants,
    private service: PracticeService,
    public permissionService: PermissionService,
    private modalService: NgbModal,
    private productService: ProductService,
    private componentService: ComponentService,
    private serviceHistory: HistoryVersionService
  ) { }

  fileTemplate = this.config.ServerApi + 'Template/Import_Baithuchanh_Module_Template.xls';
  ListModule: any[] = [];
  TotalAmount = 0;
  Module: any = {
    ModuleId: '',
    ModuleName: '',
    ModuleCode: '',
    ModuleGroupCode: '',
    Qty: '',
    LeadTime: '',
    Pricing: ''
  };
  @Input() Id: string;
  MaxLeadTimeModule = 0;
  MaxPriceModule = 0;
  ngOnInit() {
    this.searchPracticeModule();
  }
  searchPracticeModule() {
    if (!this.permissionService.checkPermission(['F040706'])) {

      this.service.searchPracticeModule({ PracticeId: this.Id }).subscribe((data: any) => {
        if (data) {
          this.ListModule = data.Modules;
          this.TotalAmount = this.ListModule.reduce((a, b) => a + (b.Qty * b.Pricing), 0);
          if (this.ListModule.length > 0) {
            var listModuleId = [];
            this.ListModule.forEach(element => {
              listModuleId.push(element.ModuleId)
            });
            this.getModulePrice(listModuleId);
          }
          this.changeMaxLeadTimeModule();
        }
      },
        error => {
          this.messageService.showError(error);
        });
    }
  }


  showImportModule() {
    this.componentService.showImportExcel(this.fileTemplate, false).subscribe(data => {
      if (data) {
        this.service.importProductModule(data, this.Id).subscribe(
          data => {
            var listModuleNewId = [];
            data.forEach(element => {
              var group = this.ListModule.filter(t => t.ModuleId == element.Id);
              if (group.length == 0) {
                var newModule = Object.assign({}, this.Module);
                newModule.ModuleId = element.Id;
                newModule.ModuleCode = element.Code;
                newModule.ModuleName = element.Name;
                newModule.ModuleGroupCode = element.ModuleGroupCode;
                newModule.LeadTime = element.LeadTime;
                newModule.Pricing = element.Pricing;
                newModule.Qty = element.Quantity;
                this.ListModule.push(newModule);

                this.changeMaxLeadTimeModule();
                listModuleNewId.push(newModule.ModuleId);
              }
            });
            this.getModulePrice(listModuleNewId);
            // this.changeMaxLeadTimeModule();
            this.calculateTotalAmount();
            this.messageService.showSuccess('Import danh sách module thành công!');
          },
          error => {
            this.messageService.showError(error);
          });
      }
    });
  }
  calculateTotalAmount() {
    this.TotalAmount = 0;
    this.ListModule.forEach(mod => {
      this.TotalAmount += mod.Pricing * mod.Qty;
    });
  }
  //Hiển thì popup chọn Module
  showSelectModule() {
    let activeModal = this.modalService.open(ProductChooseModuleComponent, { container: 'body', windowClass: 'productchoosemodulecomponent-model', backdrop: 'static' });
    var ListIdSelect = [];
    this.ListModule.forEach(element => {
      ListIdSelect.push(element.ModuleId);
    });

    activeModal.componentInstance.listIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        var listModuleNewId = [];
        result.forEach(element => {
          var newModule = Object.assign({}, this.Module);
          newModule.ModuleId = element.Id;
          newModule.ModuleName = element.Name;
          newModule.Qty = element.Qty;
          newModule.ModuleCode = element.Code;
          newModule.ModuleGroupCode = element.ModuleGroupCode;
          newModule.LeadTime = element.LeadTime;
          newModule.Pricing = element.Pricing;
          this.ListModule.push(newModule);
          listModuleNewId.push(element.Id);
        });
        this.getModulePrice(listModuleNewId);
        this.changeMaxLeadTimeModule();
      }
    }, (reason) => {

    });
  }

  //Lấy giá module
  getModulePrice(listModuleNewId: any[]) {
    this.productService.getModulePrice(listModuleNewId).subscribe(
      data => {
        if (data && data.length > 0) {
          this.ListModule.forEach(mod => {
            data.forEach(element => {
              if (element.ModuleId == mod.ModuleId) {
                mod.Price = element.Price;
                mod.IsNoPrice = element.IsNoPrice;
              }
            });
          });
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  changeMaxLeadTimeModule() {
    if (this.ListModule.length > 0) {
      this.MaxLeadTimeModule = Math.max.apply(Math, this.ListModule.map(function (o) { return o.LeadTime; }));
      this.MaxPriceModule = Math.max.apply(Math, this.ListModule.map(function (o) { return o.Pricing; }));
    } else {
      this.MaxLeadTimeModule = 0;
      this.MaxPriceModule = 0;
    }

  }

  showConfirmDeleteModule(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file đính kèm này không?").then(
      data => {
        this.ListModule.splice(index, 1);
        this.messageService.showSuccess("Xóa Module thành công!");
      },
      error => {
        
      }
    );
  }

  save() {
    this.service.addModuleInPractice({ Id: this.Id, ListModuleInPractice: this.ListModule }).subscribe(
      data => {
        this.messageService.showSuccess('Lưu module thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
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
}
