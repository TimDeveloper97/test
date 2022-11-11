import { Component, OnInit } from '@angular/core';

import { Constants, ComboboxService, MessageService, Configuration, DateUtils } from 'src/app/shared';
import { GeneralTemplateService } from '../services/general-template.service';

@Component({
  selector: 'app-general-design',
  templateUrl: './general-design.component.html',
  styleUrls: ['./general-design.component.scss']
})
export class GeneralDesignComponent implements OnInit {

  constructor(
    public constant: Constants,
    public dateUtils: DateUtils,
    private config: Configuration,
    private combobox: ComboboxService,
    private messageService: MessageService,
    private service: GeneralTemplateService
  ) { }
  projectProductName: '';
  projectProductCode: '';
  columnName: any[] = [{ Name: 'Code', Title: 'Mã sản phẩm' }, { Name: 'Name', Title: 'Tên sản phẩm' }];
  listProject: any[] = [];
  listProjectproduct: any[] = [];
  listDepartment: any[] = [];
  listEmployee: any[] = [];
  listModule: any[] = [];
  listMaterial: any[] = [];
  requestDate:any;
  model: any = {
    Id: '',
    ProjectId: '',
    ProjectProductId: '',
    DepartmentRequestId: '',
    DepartmentPerformId: '',
    Name: '',
    Code: '',
    EmployeeId: '',
    RequestDate: null,
    CustomerName: '',
    Categories: 0,
  };

  exportModel: any = {
    ListModule: [],
    ListMaterial: []
  }

  modelTest: any = {
    PathFileMaterial: '',
    List3D: [],
    ListModuleDesignDocument: [],
    ListRawMaterial: [],
    ListMaterialDB: [],
    ListModuleError: [],
    ListManufacture: [],
    ListConvertUnit: [],
    ListDesignStructure: [],
    ListDesignStructureFile: [],
    Module: '',
    ListModule: [],
    ListUnit: [],
    SelectedPath: '',
    ApiUrl: '',
    PathDownload: '',
    ModuleCode: '',
    FileName: ''
  }

  ngOnInit() {
    this.getListProject();
    //this.getCbbDepartment();
    //this.getCbbEmployee();
  }

  getListProject() {
    this.combobox.getListProject().subscribe(
      data => {
        this.listProject = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getCbbEmployee() {
    this.combobox.getCbbEmployee().subscribe(
      data => {
        this.listEmployee = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getData() {
    this.getListProjectProductByProjectId();
    this.getCustomerByProjectId();
  }

  getDataModule() {
    this.generalDesign();
    this.getModuleByProjectproductId();
  }

  getListProjectProductByProjectId() {
    this.combobox.getListProjectProductByProjectId(this.model.ProjectId).subscribe(
      data => {
        this.listProjectproduct = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getCustomerByProjectId() {
    this.service.GetCustomerByProjectId(this.model.ProjectId).subscribe(
      data => {
        this.model.CustomerName = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getModuleByProjectproductId() {
    this.service.GetModuleByProjectproductId(this.model.Id).subscribe(
      data => {
        this.projectProductName = data.Name;
        this.projectProductCode = data.Code;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getCbbDepartment() {
    this.combobox.getCbbDepartment().subscribe(
      data => {
        this.listDepartment = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  generalDesign() {
    this.service.GeneralDesign(this.model).subscribe((data: any) => {
      if (data) {
        this.projectProductName = data.ListModule[0].ModuleName;
        this.projectProductCode = data.ListModule[0].ModuleCode;
        this.listModule = data.ListModule;
        this.listMaterial = data.ListMaterial;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  exportGeneralDesign() {
    if (this.requestDate) {
      this.model.RequestDate = this.dateUtils.convertObjectToDate(this.requestDate);
    }
    this.exportModel = this.model;
    this.exportModel.ListModule = this.listModule;
    this.exportModel.ListMaterial = this.listMaterial;
    this.service.exportExcel(this.exportModel).subscribe(d => {
      var link = document.createElement('a');
      link.setAttribute("type", "hidden");
      link.href = this.config.ServerApi + d;
      link.download = 'Download.docx';
      document.body.appendChild(link);
      // link.focus();
      link.click();
      document.body.removeChild(link);
      // if (d) {
      //   this.modelTest.PathFileMaterial = d;
      //   this.modelTest.ApiUrl = this.config.ServerApi;
      //   this.modelTest.ModuleCode = this.modelMechanical.ProductCode;
      //   this.modelTest.Type = 1;
      //   this.modelTest.FileName = "DTSB.";
      //   this.DowloadTemplateToFolder();
      // }
    }, e => {
      this.messageService.showError(e);
    });
  }
}
