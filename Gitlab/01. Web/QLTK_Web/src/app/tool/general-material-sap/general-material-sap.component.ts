import { Component, OnInit } from '@angular/core';
import { AppSetting } from 'src/app/shared/config/appsetting';
import { MessageService, Configuration, Constants, ComboboxService, FileProcess, ComponentService } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DownloadDmvtSapService } from '../services/download-dmvt-sap.service';
import { BroadcastEventListener } from 'src/app/signalr/broadcast.event.listener';
import { Subscription } from 'rxjs';
import { BlockUI, NgBlockUI } from 'ng-block-ui';


@Component({
  selector: 'app-general-material-sap',
  templateUrl: './general-material-sap.component.html',
  styleUrls: ['./general-material-sap.component.scss']
})
export class GeneralMaterialSapComponent implements OnInit {
  private onDownload = new BroadcastEventListener<any>('download');
  private downloadSub: Subscription;
  //@ViewChild(DxTreeListComponent) treeView;
  @BlockUI() blockUI: NgBlockUI;
  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private config: Configuration,
    public constant: Constants,
    public comboboxService: ComboboxService,
    private serviceDMVTSap: DownloadDmvtSapService,
    private fileProcess: FileProcess,
    private componentService: ComponentService

  ) { }

  columnName: any[] = [{ Name: 'Code', Title: 'Mã dự án' }, { Name: 'Name', Title: 'Tên dự án' }];
  productColumnName: any[] = [{ Name: 'Code', Title: 'Mã sản phẩm' }, { Name: 'Name', Title: 'Tên sản phẩm' }];

  StartIndex = 1;
  listData: any[] = [];
  modelModule: any = {
    Id: '',
    Quantity: '',
    ProjectId: '',
    ListIdSelect: [],
    ListIdChecked: [],
    Modules: []
  }

  model: any = {
    ProjectId: '',
    ListSelect: [],
  }
  // get comboboxx
  listProject: [];
  listProjectproduct: [];
  moduleInProjectProduct: any = {
    ModuleId: ''
  }

  ngOnInit() {
    this.appSetting.PageTitle = "Xuất DMVT SAP";
    this.getListProject();
  }

  getListProject() {
    this.comboboxService.getListProject().subscribe(
      data => {
        this.listProject = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getListProjectProject() {
    this.comboboxService.getListProjectProduct(this.model.ProjectId).subscribe(
      data => {
        this.listProjectproduct = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  changeProject() {
    if (this.modelModule.ProjectId != this.model.ProjectId) {
      this.listSelect = [];
      this.listProjectproduct = [];
    }

    this.getListProjectProject();
    this.getModuleInProjectProduct();
  }

  changeProjectProduct() {
    if (this.modelModule.ProjectProductId != this.model.ProjectProductId) {
      this.listSelect = [];
    }
    this.getModuleInProjectProduct();
  }

  listModule: any[] = [];
  getModuleInProjectProduct() {

    this.modelModule.ProjectProductId = this.model.ProjectProductId;
    this.modelModule.ProjectId = this.model.ProjectId;
    this.listSelect.forEach(element => {
      this.modelModule.ListIdSelect.push(element.ProjectProductId);
    });
    this.listModule.forEach(element => {
      if (element.Checked) {
        this.modelModule.ListIdChecked.push(element.Id);
      }
    });

    this.serviceDMVTSap.GetModuleInProjectProductByProjectId(this.modelModule).subscribe((data: any) => {
      if (data.ListResult) {
        this.listModule = data.ListResult;
        this.modelModule.totalItems = data.TotalItem;
        this.modelModule.State = data.State;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  listSelect: any[] = [];
  ListIdSelect: any = [];
  addRow() {
    this.listModule.forEach(element => {
      if (element.Checked) {
        this.listSelect.push(element);
      }
    });
    this.listSelect.forEach(element => {
      var index = this.listModule.indexOf(element);
      if (index > -1) {
        this.listModule.splice(index, 1);
      }
    });
    this.getModuleInProjectProduct();
  }

  removeRow() {
    this.listSelect.forEach(element => {
      if (element.Checked) {
        this.listModule.push(element);
      }
    });
    this.listModule.forEach(element => {
      var index = this.listSelect.indexOf(element);
      if (index > -1) {
        this.listSelect.splice(index, 1);
      }
    });
  }

  generalDMVTSAP() {
    this.modelModule.Modules = this.listSelect;
    this.serviceDMVTSap.GenerateMaterialSAP(this.modelModule).subscribe(data => {
      var link = document.createElement('a');
      link.setAttribute("type", "hidden");
      link.href = this.config.ServerApi + data;
      link.download = 'Download.zip';
      document.body.appendChild(link);
      // link.focus();
      link.click();
      document.body.removeChild(link);
      this.messageService.showMessage("Xuất BOM thành công!");
    }, e => {
      this.messageService.showError(e);
    });
  }
  checked = false;
  checkeds = false;
  selectAllFunction() {
    if (this.checked) {
      this.listModule.forEach(element => {
        element.Checked = true;
      });
    } else {
      this.listModule.forEach(element => {
        element.Checked = false;
      });
    }
  }

  selectAllFunctions() {
    if (this.checked) {
      this.listSelect.forEach(element => {
        element.Checked = true;
      });
    } else {
      this.listSelect.forEach(element => {
        element.Checked = false;
      });
    }
  }

  importModule() {
    var path = 'Template/ModuleImportSAP.xlsx';
    let fileTemplateMaterial = this.config.ServerApi + path;
    this.componentService.showImportExcel(fileTemplateMaterial, false).subscribe(file => {
      if (file) {
        this.serviceDMVTSap.importModule(file).subscribe(
          data => {
            var link = document.createElement('a');
            link.setAttribute("type", "hidden");
            link.href = this.config.ServerApi + data;
            link.download = 'Download.zip';
            document.body.appendChild(link);
            // link.focus();
            link.click();
            document.body.removeChild(link);
            this.messageService.showMessage("Xuất BOM thành công!");
          }, error => {
            this.messageService.showError(error);
          }
        );
      }
    });
  }
}