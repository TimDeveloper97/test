import { Component, OnInit, ViewEncapsulation, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { ProjectGeneralDesignService } from '../../service/project-general-design.service';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, ComboboxService, Constants, DateUtils, Configuration } from 'src/app/shared';
import { ProjectProductService } from '../../service/project-product.service';
import { GeneralTemplateService } from 'src/app/tool/services/general-template.service';
import { forkJoin } from 'rxjs';
import { ChooseMaterialGeneralDesignComponent } from '../choose-material-general-design/choose-material-general-design.component';
import { ShowChoosePlanDesignComponent } from '../show-choose-plan-design/show-choose-plan-design.component';
import { PracticeSupModuleChooseComponent } from 'src/app/practice/practice/practice-sup-module-choose/practice-sup-module-choose.component';
import { ShowChooseModuleComponent } from '../show-choose-module/show-choose-module.component';
import { ShowChooseModuleSaleComponent } from '../show-choose-module-sale/show-choose-module-sale.component';
import { ProjectMaterialThreeTableCompareComponent } from '../project-material-three-table-compare/project-material-three-table-compare.component';

@Component({
  selector: 'app-project-general-design-create',
  templateUrl: './project-general-design-create.component.html',
  styleUrls: ['./project-general-design-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ProjectGeneralDesignCreateComponent implements OnInit, AfterViewInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private service: ProjectGeneralDesignService,
    private combobox: ComboboxService,
    private modalService: NgbModal,
    public constant: Constants,
    public dateUtils: DateUtils,
    private config: Configuration,
    private serviceProjectProduct: ProjectProductService,
    private serviceGeneralTemplate: GeneralTemplateService
  ) { }

  modalInfo = {
    Title: 'Th??m m???i t???ng h???p thi???t k???',
    SaveText: 'L??u',
  };

  @ViewChild('scrollModuleHeader', { static: false }) scrollModuleHeader: ElementRef;
  @ViewChild('scrollModule', { static: false }) scrollModule: ElementRef;
  @ViewChild('scrollMaterialHeader', { static: false }) scrollMaterialHeader: ElementRef;
  @ViewChild('scrollMaterial', { static: false }) scrollMaterial: ElementRef;
  @ViewChild('scrollModuleHeaderFalse') scrollModuleHeaderFalse: ElementRef;
  @ViewChild('scrollModuleFalse', { static: false }) scrollModuleFalse: ElementRef;
  isAction: boolean = false;
  Id: string;
  checked: boolean;
  checkedFalse: boolean;
  requestDate: any;
  projectId: string;
  sbuRequestId: string;
  sbuCreateId: string;
  projectProductId: string;
  projectProductName: string;
  projectProductCode: string;
  listDepartment: any[] = [];
  listDepartmentRequest: any[] = [];
  listDepartmentCreate: any[] = [];
  listManager = [];
  listSBU = [];
  listModule: any[] = [];
  listModuleFalse: any[] = [];
  listMaterial: any[] = [];
  listProject: any[] = [];
  listProjectproduct: any[] = [];
  listEmployee: any[] = [];
  dateNow = new Date();
  columnName: any[] = [{ Name: 'Code', Title: 'M?? s???n ph???m' }, { Name: 'Name', Title: 'T??n s???n ph???m' }];
  columnSBU: any[] = [{ Name: 'Code', Title: 'M?? SBU' }, { Name: 'Name', Title: 'T??n SBU' }];
  columnDepartment: any[] = [{ Name: 'Code', Title: 'M?? ph??ng ban' }, { Name: 'Name', Title: 'T??n ph??ng ban' }];
  columnEmployee: any[] = [{ Name: 'Code', Title: 'M?? nh??n vi??n' }, { Name: 'Name', Title: 'T??n nh??n vi??n' }];
  listStatus: any[] = [
    { Id: 1, Name: 'Ch??a xu???t' },
    { Id: 2, Name: '???? xu???t' }
  ]

  totalAmounts: 0;
  totalPriceContract: 0;

  model: any = {
    Id: '',
    Index: '',
    DepartmentRequestId: '',
    DepartmentCreateId: '',
    RequestDate: null,
    ProjectProductId: '',
    ProjectId: '',
    CreateIndex: 0,
    DesignBy: '',
    DesignStatus: null,
    CustomerName: '',
    CreateDate: this.dateNow,
    IsUpdate: false,
    ListModule: [],
    ListMaterial: []
  }

  modelModule: any = {
    Id: '',
    ModuleName: '',
    ModuleCode: '',
    Quantity: '',
    RealQuantity: '',
  }

  materialModel: any = {
    Id: '',
    Name: '',
    Code: '',
    Manafacture: '',
    Price: 0,
    Quantity: 0,
    ContractPrice: 0,
    CreateIndex: 0
  }

  ngOnDestroy() {
    this.scrollModule.nativeElement.removeEventListener('ps-scroll-x', null);
    this.scrollModuleFalse.nativeElement.removeEventListener('ps-scroll-x', null);
    this.scrollMaterial.nativeElement.removeEventListener('ps-scroll-x', null);
  }

  ngOnInit() {

    this.model.Id = this.Id;
    this.model.ProjectId = this.projectId;
    this.model.ProjectProductId = this.projectProductId;

    forkJoin([
      this.combobox.getListProject(),
      //this.combobox.getCbbEmployee(),
      this.combobox.getListProjectProductByProjectId(this.model.ProjectId),
      this.serviceGeneralTemplate.GetCustomerByProjectId(this.model.ProjectId),
      this.combobox.getCbbSBU()]
    ).subscribe(([res1, res2, res3, res4]) => {
      this.listProject = res1;
      this.listProjectproduct = res2;
      this.model.CustomerName = res3;
      this.listSBU = res4;
    });
    //this.getListProject();
    //this.getListEmployee();
    if (this.Id) {
      this.modalInfo.Title = 'Ch???nh s???a t???ng h???p thi???t k???';
      this.modalInfo.SaveText = 'L??u';
      this.getProjectGeneralDesignInfo();
    }
    else {
      //this.generalDesign();
      this.modalInfo.Title = "Th??m m???i t???ng h???p thi???t k???";
    }
  }

  ngAfterViewInit() {
    this.scrollModule.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollModuleHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
    this.scrollModuleFalse.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollModuleHeaderFalse.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
    this.scrollMaterial.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollMaterialHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }

  getData() {
    this.getListProjectProductByProjectId();
    this.getCustomerByProjectId();
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
    this.serviceGeneralTemplate.GetCustomerByProjectId(this.model.ProjectId).subscribe(
      data => {
        this.model.CustomerName = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getProjectGeneralDesignInfo() {
    this.service.getProjectGeneralDesignInfo({ Id: this.Id }).subscribe(data => {
      this.model = data;
      this.listModule = data.ListModule;
      this.listMaterial = data.ListMaterial;
      if (data.RequestDate != null) {
        let dateArray1 = data.RequestDate.split('T')[0];
        let dateValue1 = dateArray1.split('-');
        let tempDateFromV1 = {
          'day': parseInt(dateValue1[2]),
          'month': parseInt(dateValue1[1]),
          'year': parseInt(dateValue1[0])
        };
        this.requestDate = tempDateFromV1;
      }
      forkJoin([
        this.service.getListDepartment(''),
        this.combobox.getCbbSBU(),
        this.serviceGeneralTemplate.GetCustomerByProjectId(this.model.ProjectId)]
      ).subscribe(([res1, res2, res3]) => {
        this.listDepartment = res1;
        this.listDepartment.forEach(element => {
          if (element.Id == data.DepartmentRequestId) {
            this.sbuRequestId = element.SBUId;
          }
          if (data.DepartmentCreateId) {
            if (element.Id == data.DepartmentCreateId) {
              this.sbuCreateId = element.SBUId;
            }
          }
        });
        this.listSBU = res2;
        this.model.CustomerName = res3;
        forkJoin([
          this.service.getListDepartment(this.sbuRequestId),
          this.service.getListDepartment(this.sbuCreateId)]
        ).subscribe(([res3, res4]) => {
          this.listDepartmentRequest = res3;
          this.listDepartmentCreate = res4;
        });
      });
    }, error => {
      this.messageService.showError(error);
    });
  }

  getListProject() {
    this.combobox.getListProject().subscribe(data => {
      this.listProject = data;
    }, error => {
      this.messageService.showError(error);
    });
  }

  getListEmployee() {
    this.combobox.getEmployeeByDepartment(this.model.DepartmentCreateId).subscribe(data => {
      this.listEmployee = data;
    }, error => {
      this.messageService.showError(error);
    });
  }

  getCbbSBU() {
    this.combobox.getCbbSBU().subscribe(data => {
      this.listSBU = data;
    }, error => {
      this.messageService.showError(error);
    });
  }

  getListDepartmentiRequest() {
    this.service.getListDepartment(this.sbuRequestId).subscribe(data => {
      this.listDepartmentRequest = data;
    }, error => {
      this.messageService.showError(error);
    });
  }

  getListDepartmentiCreate() {
    this.service.getListDepartment(this.sbuCreateId).subscribe(data => {
      this.listDepartmentCreate = data;
    }, error => {
      this.messageService.showError(error);
    });
  }

  getProjectProductInfo() {
    this.serviceProjectProduct.getProjectProductInfo({ Id: this.projectProductId }).subscribe(data => {
      this.model.ProjectId = data.ProjectId;
      this.model.Index = data.ContractIndex;
    }, error => {
      this.messageService.showError(error);
    });
  }

  createProjectGeneralDesign(isExport) {
    this.supCreate(isExport);
  }

  updateProjectGeneralDesign(isExcel: any) {
    this.supUpdate(isExcel);
  }

  save() {
    for (const item of this.listModule) {
      if (item.Checked == true || item.ModuleStatus == 2) {

        if (item.TotalNoDone > 0) {
          this.messageService.showMessage("Module c?? l???i QC ?????t nh??ng ch??a t??ch Kh???c ph???c tri???t ?????!");
          return;
        }
      }
    }

    if (this.Id) {
      this.updateProjectGeneralDesign(false);
    }
    else {
      this.createProjectGeneralDesign(false);
    }
  }

  supCreate(isExport) {
    var checkCreate = false;
    if (this.requestDate) {
      this.model.RequestDate = this.dateUtils.convertObjectToDate(this.requestDate);
    }

    this.listModule.forEach(element => {
      if (element.Checked == true || element.ModuleStatus == 2) {
        this.model.ListModule.push(element);

        if (element.TotalNoDone > 0) {
          this.messageService.showMessage("Module c?? l???i QC ?????t nh??ng ch??a t??ch Kh???c ph???c tri???t ?????!");
          return;
        }
      }
      if (element.Checked == true && element.ModuleStatus == 1) {
        checkCreate = true;
      }
    });

    this.listModuleFalse.forEach(element => {
      if (element.Checked == true || element.ModuleStatus == 2) {
        this.model.ListModule.push(element);
      }
      if (element.Checked == true && element.ModuleStatus == 1) {
        checkCreate = true;
      }
    });

    this.model.ListMaterial = [];
    for (var item of this.listMaterial) {
      if (item.IsDelete == true && item.Quantity < item.OldQuantity) {
        this.messageService.showMessage("S??? l?????ng c???a v???t t?? " + item.Name + " ??ang nh??? h??n l???n l???p tr?????c!");
        return;
      } else {
        this.model.ListMaterial.push(item);
      }
      if (item.CreateIndex == this.model.CreateIndex) {
        checkCreate = true;
      }
    }

    if (!checkCreate) {
      this.messageService.showMessage("B???n ch??a ch???n s???n ph???m ho???c th??m v???t t??!");
      return;
    }

    this.service.addProjectGeneralDesign(this.model).subscribe(
      data => {
        this.messageService.showSuccess('Th??m m???i t???ng h???p thi???t k??? th??nh c??ng!');
        this.service.getListMaterialOfModule(this.model).subscribe(
            data1 => {
              if (data1.IsExit) {
                let activeModal = this.modalService.open(ProjectMaterialThreeTableCompareComponent, { container: 'body', windowClass: 'project-material-three-table-compare', backdrop: 'static' })
                activeModal.componentInstance.listData = data1.oldMaterialChangeModels;
                activeModal.componentInstance.listData1 = data1.newMaterialChangeModels;
                activeModal.componentInstance.listData2 = data1.allMaterialOfModules;
                activeModal.componentInstance.ModuleProjectProducts = data1.ModuleIdProjectProducts;
                activeModal.componentInstance.ProjectProductId = data1.ProjectProductId;
                activeModal.componentInstance.ModuleId = data1.ModuleId;
                
                activeModal.result.then((result) => {
                  if (result) {
                    if (isExport) {
                      this.exportExcel(data);
                    }
                  }
                }, (reason) => {
                });
              }
            },
            error => {
              this.messageService.showError(error);
            }
          );

        this.closeModal(true);
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  supUpdate(isExcel) {
    var checkUpdate = false;

    if (this.requestDate) {
      this.model.RequestDate = this.dateUtils.convertObjectToDate(this.requestDate);
    }

    for (var item of this.listModule) {
      if (item.CheckQuantity > item.Quantity) {
        this.messageService.showMessage("S??? l?????ng c???a S???n ph???m: " + item.ContractName + " kh??ng ???????c nh??? h??n " + item.CheckQuantity);
        return;
      }
    }
    for (var item of this.listModuleFalse) {
      if (item.CheckQuantity > item.Quantity) {
        this.messageService.showMessage("S??? l?????ng Module kh??ng l???p t???ng h???p: " + item.ContractName + " kh??ng ???????c nh??? h??n " + item.CheckQuantity);
        return;
      }
    }
    this.model.ListModule = [];
    this.listModule.forEach(element => {
      if (element.Checked == true && element.CreateIndex == this.model.CreateIndex || element.Checked == true && element.CreateIndex == 0) {
        this.model.ListModule.push(element);
        checkUpdate = true;
      }

    });
    this.listModuleFalse.forEach(element => {
      if (element.Checked == true && element.CreateIndex == this.model.CreateIndex || element.Checked == true && element.CreateIndex == 0) {
        this.model.ListModule.push(element);
        checkUpdate = true;
      }
    });
    this.model.ListMaterial = [];
    for (var item of this.listMaterial) {
      if (item.IsDelete == true && item.Quantity < item.OldQuantity) {
        this.messageService.showMessage("S??? l?????ng c???a v???t t?? " + item.Name + " ??ang nh??? h??n l???n l???p tr?????c!");
        return;
      } else {
        this.model.ListMaterial.push(item);
      }
      if (item.CreateIndex == this.model.CreateIndex) {
        checkUpdate = true;
      }
    }

    if (!checkUpdate) {
      this.messageService.showMessage("B???n ch??a ch???n s???n ph???m ho???c th??m v???t t??!");
      return;
    }
    this.service.updateProjectGeneralDesign(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('C???p nh???t t???ng h???p thi???t k??? th??nh c??ng!');
        this.service.getListMaterialOfModule(this.model).subscribe(
          data => {
            if (data.IsExit) {
              let activeModal = this.modalService.open(ProjectMaterialThreeTableCompareComponent, { container: 'body', windowClass: 'project-material-three-table-compare', backdrop: 'static' })
              activeModal.componentInstance.listData = data.oldMaterialChangeModels;
              activeModal.componentInstance.listData1 = data.newMaterialChangeModels;
              activeModal.componentInstance.listData2 = data.allMaterialOfModules;
              activeModal.componentInstance.ModuleProjectProducts = data.ModuleIdProjectProducts;
              activeModal.componentInstance.projectProductId = data.ProjectProductId;
              activeModal.componentInstance.ModuleId = data.ModuleId;

              activeModal.result.then((result) => {
                if (result) {
                  if (isExcel) {
                    this.exportExcel(this.Id);
                  }
                }
              }, (reason) => {
              });
            }
          },
          error => {
            this.messageService.showError(error);
          }
        );
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  showConfirmDeleteMaterial(index, IsDelete: boolean) {
    if (this.model.IsUpdate == true) {
      this.messageService.showMessage("B???n ch??? c?? th??? c???p nh???t v???t t?? cho phi??n b???n m???i nh???t!");
    } else if (IsDelete == true) {
      this.messageService.showMessage("B???n kh??ng ???????c x??a v???t t?? c???a l???n t???ng h???p tr?????c!");
    } else {
      this.messageService.showConfirm("B???n c?? ch???c mu???n xo?? v???t t?? ph??? n??y kh??ng?").then(
        data => {
          this.listMaterial.splice(index, 1);
        },
        error => {

        }
      );
    }
  }

  generalDesign() {
    if (this.model.ProjectProductId) {
      this.service.generalDesign(this.model).subscribe((data: any) => {
        if (data) {
          this.totalAmounts = data.TotalAmount;
          this.totalPriceContract = data.TotalPriceContract;
          this.projectProductName = data.Data.ModuleName;
          this.projectProductCode = data.Data.ModuleCode;
          this.model.DesignStatus = data.Data.DesignStatus;
          if (data.Data.DataType == 2) {
            this.listModule = data.ListModuleProduct;
            this.listModuleFalse = data.ListModuleProductFalse;
          } else if (data.Data.DataType == 4 && this.model.DesignStatus != this.constant.DesignStatus.Sale || data.Data.DataType == 3 && this.model.DesignStatus != this.constant.DesignStatus.Sale) {
            if (data.Data.IsGeneralDesign) {
              this.listModule = data.ListModule;
            } else {
              this.listModuleFalse = data.ListModule;
            }
          }
          if (!this.Id) {
            this.model.Index = data.Data.Index;
            this.model.CreateIndex = data.CreateIndex;

            this.listMaterial = data.ListMaterial;
          }
          if (data.CheckVersion) {
            this.model.RequestDate = data.Models.RequestDate;
            if (data.Models.RequestDate != null) {
              let dateArray1 = data.Models.RequestDate.split('T')[0];
              let dateValue1 = dateArray1.split('-');
              let tempDateFromV1 = {
                'day': parseInt(dateValue1[2]),
                'month': parseInt(dateValue1[1]),
                'year': parseInt(dateValue1[0])
              };
              this.requestDate = tempDateFromV1;
            }
            this.model.Index = data.Models.Index;
            this.sbuCreateId = data.Models.SBUCreateId;
            this.sbuRequestId = data.Models.SBURequestId;
            this.model.DepartmentCreateId = data.Models.DepartmentCreateId;
            this.model.DepartmentRequestId = data.Models.DepartmentRequestId;
            this.model.DesignBy = data.Models.DesignBy;
            forkJoin([
              this.service.getListDepartment(this.sbuCreateId),
              this.service.getListDepartment(this.sbuRequestId),
              this.combobox.getEmployeeByDepartment(this.model.DepartmentCreateId)]
            ).subscribe(([data1, data2, data3]) => {
              this.listDepartmentCreate = data1;
              this.listDepartmentRequest = data2;
              this.listEmployee = data3;
            });
          } else {
            forkJoin([
              this.service.getData()]
            ).subscribe(([res5]) => {
              this.sbuCreateId = res5.SBUID;
              this.model.DepartmentCreateId = res5.DepartmantId;
              this.model.DesignBy = res5.Id
              forkJoin([
                this.service.getListDepartment(this.sbuCreateId),
                this.combobox.getEmployeeByDepartment(this.model.DepartmentCreateId)]
              ).subscribe(([data1, data2]) => {
                this.listDepartmentCreate = data1;
                this.listEmployee = data2;
              });
            });
          }
        }
      }, error => {
        this.messageService.showError(error);
      });
    }
  }

  saveAndExportGeneralDesign() {
    for (const item of this.listModule) {
      if (item.Checked == true || item.ModuleStatus == 2) {

        if (item.TotalNoDone > 0) {
          this.messageService.showMessage("Module c?? l???i QC ?????t nh??ng ch??a t??ch Kh???c ph???c tri???t ?????!");
          return;
        }
      }
    }

    if (!this.Id) {
      var list = [];
      var group1 = this.listModule.filter(t => t.Checked == true && t.CreateIndex > 0);
      if (group1.length > 0) {
        for (var item of group1) {
          list.push(item);
        }
      }
      var group2 = this.listModule.filter(t => t.Checked == true && t.CreateIndex == 0);
      if (group2.length > 0) {
        for (var item of group2) {
          list.push(item);
        }
      }
      var group3 = this.listModule.filter(t => t.Checked == false && t.CreateIndex == 0);
      if (group3.length > 0) {
        for (var item of group3) {
          list.push(item);
        }
      }
      this.listModule = list;
      this.createProjectGeneralDesign(true);
    }
    else {
      this.updateProjectGeneralDesign(true);
    }
    // this.service.getListMaterialOfModule(this.model).subscribe(
    //   data => {
    //     if (data.IsExit) {
    //       let activeModal = this.modalService.open(ProjectMaterialThreeTableCompareComponent, { container: 'body', windowClass: 'project-material-three-table-compare', backdrop: 'static' })
    //       activeModal.componentInstance.listData = data.oldMaterialChangeModels;
    //       activeModal.componentInstance.listData1 = data.newMaterialChangeModels;
    //       activeModal.componentInstance.listData2 = data.allMaterialOfModules;
    //       activeModal.componentInstance.ModuleProjectProducts = data.ModuleIdProjectProducts;
    //       activeModal.componentInstance.ProjectProductId = data.ProjectProductId;
    //       activeModal.componentInstance.ModuleId = data.ModuleId;
          
    //       activeModal.result.then((result) => {
    //         if (result) {
    //         }
    //       }, (reason) => {
    //       });
    //     }
    //   },
    //   error => {
    //     this.messageService.showError(error);
    //   }
    // );
  }

  exportGeneralDesign() {
    var count = 0;
    this.listModule.forEach(element => {
      if (!element.Checked) {
        count++
      }
      if (element.Checked && element.CreateIndex == 0) {
        element.CreateIndex = this.model.CreateIndex;
      }
    });

    if (count < this.listModule.length) {
      var check = true;
      for (var i = 0; i < this.listModule.length; i++) {
        if (this.listModule[i].Checked && this.listModule[i].TotalError > 0) {
          check = false;
        }
        break;
      }

      if (check) {
        if (this.requestDate) {
          this.model.RequestDate = this.dateUtils.convertObjectToDate(this.requestDate);
        }
        this.listModule.forEach(element => {
          if (element.CheckQuantity > element.Quantity) {
            this.messageService.showMessage("S??? l?????ng c???a S???n ph???m: " + element.ContractName + " kh??ng ???????c nh??? h??n " + element.Quantity);
            return;
          }
        });

        this.model.ListModule = this.listModule;

        this.model.ListMaterial = [];
        for (var item of this.listMaterial) {
          if (item.IsDelete == true && item.Quantity < item.OldQuantity) {
            this.messageService.showMessage("S??? l?????ng c???a v???t t?? " + item.Name + " ??ang nh??? h??n l???n l???p tr?????c!");
            return;
          } else {
            this.model.ListMaterial.push(item);
          }
        }
        this.service.exportGeneralDesign(this.model).subscribe(d => {
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
      else {
        this.messageService.showMessage('S???n ph???m v???n c??n l???i. Kh??ng th??? xu???t bi???u m???u!');
      }
    }
    else {
      this.messageService.showMessage('Ch??a ch???n s???n ph???m. Kh??ng th??? xu???t bi???u m???u!');
    }
  }

  exportBOM() {
    this.service.exportExcelBOM({ Id: this.Id, ProjectProductId: this.model.ProjectProductId, ProjectId: this.model.ProjectId }).subscribe(d => {
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

  showClickChooseModule() {
    if (this.model.IsUpdate == true) {
      this.messageService.showMessage("B???n ch??? c?? th??? c???p nh???t v???t t?? cho phi??n b???n m???i nh???t!");
      return;
    }
    let activeModal = this.modalService.open(ShowChooseModuleComponent, { container: 'body', windowClass: 'show-choose-module-model', backdrop: 'static' });
    activeModal.componentInstance.PracticeId = '';
    var ListIdSelect = [];
    this.listMaterial.forEach(element => {
      if (element.Type == 2) {
        ListIdSelect.push(element.Id);
      }
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          var value = Object.assign({}, this.materialModel);
          value.Id = element.Id;
          value.Code = element.Code;
          value.Name = element.Name;
          value.Manafacture = element.ManufactureName;
          value.Price = element.Pricing;
          value.Quantity = 1;
          value.ContractPrice = 0;
          value.Type = element.Type;
          value.CreateIndex = this.model.CreateIndex;
          this.listMaterial.push(value);
        });
      }
    }, (reason) => {
    });
  }

  showClickChooseModuleSale() {
    if (this.model.IsUpdate == true) {
      this.messageService.showMessage("B???n ch??? c?? th??? c???p nh???t v???t t?? cho phi??n b???n m???i nh???t!");
      return;
    }
    let activeModal = this.modalService.open(ShowChooseModuleSaleComponent, { container: 'body', windowClass: 'show-choose-module-sale-model', backdrop: 'static' });
    activeModal.componentInstance.projectId = this.projectId;
    activeModal.componentInstance.projectProductId = this.projectProductId;
    var ListIdSelect = [];
    this.listMaterial.forEach(element => {
      if (element.Type == 3) {
        ListIdSelect.push(element.Id);
      }
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          var value = Object.assign({}, this.materialModel);
          value.Id = element.Id;
          value.Code = element.Code;
          value.Name = element.Name;
          value.Manafacture = element.ManufactureName;
          value.Price = element.Pricing;
          value.Quantity = 1;
          value.ContractPrice = 0;
          value.Type = element.Type;
          value.CreateIndex = this.model.CreateIndex;
          this.listMaterial.push(value);
        });
      }
    }, (reason) => {
    });
  }

  showMaterial() {
    if (this.model.IsUpdate == true) {
      this.messageService.showMessage("B???n ch??? c?? th??? c???p nh???t v???t t?? cho phi??n b???n m???i nh???t!");
      return;
    }
    let activeModal = this.modalService.open(ChooseMaterialGeneralDesignComponent, { container: 'body', windowClass: 'choose-material-general-design', backdrop: 'static' });
    var listIdSelect = [];
    this.listMaterial.forEach(element => {
      if (element.Type == 1) {
        listIdSelect.push(element.Id);
      }
    });

    activeModal.componentInstance.listIdSelect = listIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          var value = Object.assign({}, this.materialModel);
          value.Id = element.Id;
          value.Code = element.Code;
          value.Name = element.Name;
          value.Manafacture = element.ManufactureName;
          value.Price = element.Price;
          value.Quantity = 1;
          value.ContractPrice = 0;
          value.Type = element.Type;
          value.CreateIndex = this.model.CreateIndex;
          this.listMaterial.push(value);
        });
      }
    }, (reason) => {

    });
  }

  fullcheck(Checked: any) {
    if (Checked == true) {
      for (var item of this.listModule) {
        item.Checked = true;
        if (item.CreateIndex == 0 || item.CreateIndex == this.model.CreateIndex) {
        }
      }
    }
    else {
      for (var item of this.listModule) {
        if (item.CreateIndex == 0 || item.CreateIndex == this.model.CreateIndex) {
          item.Checked = false;
        }
      }
    }
  }

  fullcheckFlase(Checked: any) {
    if (Checked == true) {
      for (var item of this.listModuleFalse) {
        item.Checked = true;
        if (item.CreateIndex == 0 || item.CreateIndex == this.model.CreateIndex) {
          //this.checkModule(item.Checked, item.Id);
        }
      }
    }
    else {
      for (var item of this.listModuleFalse) {
        if (item.CreateIndex == 0 || item.CreateIndex == this.model.CreateIndex) {
          item.Checked = false;
          //this.checkModule(item.Checked, item.Id);
        }
      }
    }
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(true);
  }

  changeQuantity(index) {
    this.listModule[index].Amount = this.listModule[index].Quantity * this.listModule[index].Pricing;
  }

  selectIndex = -1;
  loadValue(param, index) {
    this.selectIndex = index;
  }

  selectIndexFalse = -1;
  loadValueFalse(param, index) {
    this.selectIndexFalse = index;
  }

  exportExcel(Id: string) {
    this.model.Id = Id;
    this.service.exportExcelManage(this.model).subscribe(d => {
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

  showConfirmCancel() {
    var list = this.listModule.filter(i => i.CreateIndex > this.model.CreateIndex);
    if (list.length > 0) {
      this.messageService.showMessage("B???n ch??? c?? th??? h???y ph?? duy???t cho l???n t???ng h???p g???n nh???t");
      return;
    }
    this.messageService.showConfirm("B???n c?? ch???c mu???n h???y ph?? duy???t kh??ng?").then(
      data => {
        this.cancelApprovedProductGaneralDesign();
      },
      error => {

      }
    );
  }

  cancelApprovedProductGaneralDesign() {
    var planModel = {
      Id: this.model.Id,
      ApproveStatus: 0
    }
    this.service.updateApproveStatus(planModel).subscribe(data => {
      this.messageService.showSuccess('H???y ph?? duy???t t???ng h???p th??nh c??ng!');
      this.model.ApproveStatus = 0;
      //this.save();
    }, error => {
      this.messageService.showError(error);
    });
  }

  approvedProductGaneralDesign(ApproveStatus: number) {
    var listModulePlan = [];
    this.listModule.forEach(element => {
      if (element.Checked == true && element.CreateIndex == this.model.CreateIndex || element.Checked == true && element.CreateIndex == 0) {
        listModulePlan.push(element);
      }
    });
    let activeModal = this.modalService.open(ShowChoosePlanDesignComponent, { container: 'body', windowClass: 'show-choose-plan-design', backdrop: 'static' });
    activeModal.componentInstance.id = this.model.Id;
    activeModal.componentInstance.projectProductId = this.projectProductId;
    activeModal.componentInstance.approveStatus = ApproveStatus;
    activeModal.componentInstance.listModule = listModulePlan;
    activeModal.result.then((result) => {
      this.model.ApproveStatus = result;
      //this.save();
    }, (reason) => {
    });
  }
}
