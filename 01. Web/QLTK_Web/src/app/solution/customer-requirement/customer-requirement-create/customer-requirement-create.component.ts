import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { forkJoin } from 'rxjs';
import { AppSetting, ComboboxService, Constants, DateUtils, FileProcess, MessageService } from 'src/app/shared';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { CustomerRequirementService } from '../service/customer-requirement.service';
import { SurveyService } from '../service/Survey.service';
import { ActivatedRoute, Router } from '@angular/router';
import { SurveyCreateComponent } from '../customer-requirement-create/Survey-create/Survey-create.component';
import { SelectMaterialComponent } from 'src/app/education/classroom/select-material/select-material/select-material.component';
import { ModuleGroupChooseProductStandardComponent } from '../../../module/modulegroup/module-group-choose-product-standard/module-group-choose-product-standard.component';
import { SolutionAnalysisProductCreateComponent } from '../solution-analysis-product-create/solution-analysis-product-create.component';
import { SolutionAnalysisEstimateService } from '../service/solution-analysis-estimate.service'
import { SolutionAnalysisSupplierCreateComponent } from '../solution-analysis-supplier-create/solution-analysis-supplier-create.component'; import { SolutionAnalysisProductService } from '../service/solution-analysis-product.service';
import { ProductNeedSolutionCreateModalComponent } from '../product-need-solution-create-modal/product-need-solution-create-modal.component';
import { ChooseSolutionComponent } from 'src/app/project/project/choose-solution/choose-solution.component';
import { ProductNeedPriceCreateModalComponent } from '../product-need-price-create-modal/product-need-price-create-modal.component';
import { ProductCreateComponent } from './product-create/product-create.component';
import { SupplierCreateComponent } from './supplier-create/supplier-create.component';
import { MaterialCreateComponent } from './material-create/material-create.component'
import { CustomerRequirequirmentNeedToHandleComponent } from '../customer-requirequirment-need-to-handle/customer-requirequirment-need-to-handle.component';
import { element } from 'protractor';

@Component({
  selector: 'app-customer-requirement-create',
  templateUrl: './customer-requirement-create.component.html',
  styleUrls: ['./customer-requirement-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CustomerRequirementCreateComponent implements OnInit {
  [x: string]: any;

  constructor(
    //private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private customerRequirementService: CustomerRequirementService,
    private surveyService: SurveyService,
    public constant: Constants,
    private routeA: ActivatedRoute,
    private comboboxService: ComboboxService,
    private dateUtils: DateUtils,
    public fileProcess: FileProcess,
    private uploadService: UploadfileService,
    public router: Router,
    private modalService: NgbModal,
    private solutionAnalysisProductService: SolutionAnalysisProductService,
    private solutionAnalysisEstimateService: SolutionAnalysisEstimateService,
    public appSetting: AppSetting,
  ) { }

  scrollCustomProduct: HTMLElement = null;
  scrollCustomHeader: HTMLElement = null;
  scrollCustomFooter: HTMLElement = null;

  modalInfo = {
    Title: 'Th??m m???i y??u c???u kh??ch h??ng',
    SaveText: 'L??u',
  };

  listSurveyLevel: any[] = [
    { Id: 0, Name: "Ch??a c?? m???c ?????" },
    { Id: 1, Name: "Kh???n c???p" },
    { Id: 2, Name: "Cao" },
    { Id: 3, Name: "Trung b??nh" },
    { Id: 4, Name: "Th???p" },
  ]

  //#region thaint Bug #17937
  contentModel: any = {
    Request: '',
    Solution: '',
    FinishDate: null,
    CreateDate: null,
    Note: '',
    Code: '',
    // Checked: false,
  };
  //#endregion

  validMessage: any = '';
  isAction: boolean = false;
  id: string;
  RequestName: string;
  listSolitionAnalysisSupplier: any[] = [];
  customerRequirementId: any[] = [];
  listUserId: any[] = [];
  SupplierId: any[] = [];
  ListEstimateId: any[] = [];
  listSupplierId: any[] = [];
  listSolutionSupplier: any[] = [];
  listRequest: any[] = [];
  listReportProplem: any[] = [];
  listCustomer: any[] = [];
  listCustomerContact: any[] = [];
  listSurvey: any[] = [];
  listEmployeeDepartment: any[] = [];
  listEmployee: any[] = [];
  listEmployee1: any[] = [];
  listMaterial: any[] = [];
  ListEstimate: any[] = [];
  listProduct: any[] = [];
  listProductId: any[] = [];
  listDepartment: any[] = [];
  listProjectPhase: any[] = [];
  columnName: any[] = [{ Name: 'Code', Title: 'M??' }, { Name: 'Name', Title: 'T??n' },];
  columnNameCustomerContact: any[] = [{ Name: 'Name', Title: 'T??n' }, { Name: 'Address', Title: '??ia ch???' },];
  columnNameEmployee: any[] = [{ Name: 'Code', Title: 'M??' }, { Name: 'Name', Title: 'T??n' },];
  columnNameDepartment: any[] = [{ Name: 'Code', Title: 'M??' }, { Name: 'Name', Title: 'T??n' },];
  treeBoxValue: string;
  selectedRowKeys: any[] = [];
  isDropDownBoxOpened = false;
  startRateRatingIndex = 1;
  surveyIndex = 1;
  DeliveryTime = false;
  SolutionContent: any[] = [];
  j = 1;
  listCustomerRequirement: any[] = [];
  CustomerId: string;
  listCustomerRequirementType: any[] = [
    { Id: 1, Name: "Th??ng tin s???n ph???m s??? s???n su???t" },
    { Id: 2, Name: "y??u c???u t???c ????? ?????u ra" },
    { Id: 3, Name: "Quy tr??nh c??ng ngh??j c???a m??y ????? t???o ra s???n ph???m" },
    { Id: 4, Name: "Ph???m vi cung c???p" },
    { Id: 5, Name: "Timeline tri???n khai" },
    { Id: 6, Name: "Budget " },
    { Id: 7, Name: "Y??u c???u v??? v???t t?? s??? d???ng trong m??y" },
    { Id: 8, Name: "Th???i gian ch???y ????nh gi?? nghi???m thu" },
    { Id: 9, Name: "Ti??u chu???n ????nh gi?? ch???t l?????ng s???n ph???m" },
    { Id: 10, Name: "Y??u c???u v??? an to??n" },
    { Id: 11, Name: "Ti??u chu???n thi???t k??? m??y" },
    { Id: 12, Name: "Y??u c???u v??? thu th???p v?? qu???n l?? d??? li???u, qu???n l?? v???n h??nh " },
    { Id: 13, Name: "Layout l???p ?????t" },
  ]


  listCustomerRequirementRequestType: any[] = [
    { Id: 1, Name: "L??m m??y" },
    { Id: 2, Name: "Y??u c???u kh??c" },
  ]

  listCustomerRequirementRequestSource: any[] = [
    { Id: 1, Name: "Kh??ch h??ng li??n h???" },
    { Id: 2, Name: "TPA li??n h???" },
    { Id: 3, Name: "Online" },
  ]

  listCustomerRequirementRequestStatus: any[] = [
    { Id: 0, Name: "Y??u c???u kh??ch h??ng" },
    { Id: 1, Name: "Ph??n t??ch y??u c???u kh??ch h??ng" },
    { Id: 2, Name: "kh???o s??t" },
    { Id: 3, Name: "Ph??n t??ch gi???i Ph??p" },
    { Id: 4, Name: "L??m gi???i ph??p" },
    { Id: 5, Name: "L???p d??? to??n" },
    { Id: 9, Name: "??ang c???n b??? sung th??m th??ng tin" },
  ]

  state: any[] = [
    { Id: 0, Name: "??ang l??m" },
    { Id: 1, Name: "Ho??n th??nh" },
    { Id: 2, Name: "B??? qua" },
  ]
  //product

  ChangeStepModel = {
    Id: ''
  }


  requirementModel: any = {
    Id: '',
    Name: '',
    Code: '',
    CustomerContactId: '',
    CustomerId: '',
    Request: '',
    Status: '',
    Note: '',
    Budget: '',
    Version: '',
    RequestType: '',
    ListAttach: [],
    Survey: [],
    ListSurvey: [],
    Materials: [],
    ListMaterial: [],
    ListUser: [],
    RateRating: '',
    Petitioner: '',
    DepartmentRequest: '',
    Reciever: '',
    DepartmentReceive: '',
    RequestSource: '',
    ProjectPhaseId: '',
    PriorityLevel: '',
    MaterialId: '',
    ProductId: '',
    UserId: '',
    SurveyId: '',
    Index: 0,
    Step: 0,
    ListContent: [],
    listSolitionAnalysisSupplier: [],
    TradeConditions: "",
    TotalPrice: 0,
    ListRequireEstimateMaterialAttach: [],
    ListRequireEstimateFCMlAttach: [],
    // PlanFinishDate:'',
    // StartDate:'',
    Duration: '',
    CustomerRequirementState: 0,
    SurveyState: 0,
    EstimateState: 0,
    DoSolutionAnalysisState: 0,
  };

  SolitionAnalysisSupplierModel: any = {
    SupplierId: '',
    CustomerRequirementId: ''
  }

  surveyModel: any = {
    Id: '',
    ProjectPhaseId: '',
    SurveyDate: '',
    Content: '',
    Result: '',
    Level: '',
    Times: '',
  }
  MaterialModel: any = {
    Id: '',
    CustomerRequirementId: '',
    MaterialId: '',
    Note: '',
    DeliveryTime: '',
  }

  productModel: any = {
    CustomerRequirementId: '',
    ProductId: '',
  }

  protectSolutionModel: any = {
    Id: '',
    Request: '',
    Solution: '',
    Note: '',
  }

  fileTemplate: any = {
    Id: '',
    Name: "",
    Note: '',
    Type: '',
    FilePath: null,
    FileName: null,
    FileSize: 0,
    UploadDate: new Date(),
    UploadName: ''
  };

  rateRatingModel: any = {
    RateRating: '',
    NameRateRating: '',
  }

  solutionAnalysisRiskModel: any = {
    Id: '',
    Level: '',
    Name: '',
  }

  searchModel: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Code: '',
    Name: '',
  };
  startIndex = 0;

  SolutionModel = {
    CustomerRequirementId: '',
    ProductNeedSolutionId: '',
    SolutionId: '',
    EstimateState: 0,
    ListProductNeedPrice: [],
    SolutionIds: [],
    ListProtectSolution: [],
  };

  StatusSolution = {
    CustomerRequirementId: '',
    SolutionId: '',
    statusSolution: '',
  };

  // SolutionContent = [
  //   {
  //     Id: '',
  //     Request: '',
  //     Solution: '',
  //     FinishDate: '',
  //     Note: '',
  //   }
  // ];
  SolutionByDevice: any = [];
  selectIndex = -1;
  selectIndexSolution = -1;

  NameStatus: any;

  ngOnInit(): void {
    this.appSetting.PageTitle = "Th??m m???i y??u c???u kh??ch h??ng";
    this.user = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    this.requirementModel.Id = this.routeA.snapshot.paramMap.get('Id');
    if (this.requirementModel.Id) {
      this.appSetting.PageTitle = "Ch???nh s???a y??u c???u kh??ch h??ng";
      this.modalInfo.Title = '';
      this.modalInfo.SaveText = 'L??u';
      this.getListCustomer();
      this.getCbbDepartment();
      this.GetProjectPhaseType();
      this.getListDepartmentRequestEmployees();
      this.getListDepartmentReceiveEmployees();
      this.getCustomerContact();
      this.getById('');
      this.getCustomerId(this.requirementModel.Id);
      this.getCustomerContactById(this.routeA.snapshot.paramMap.get('Id'))
    }
    else {
      this.modalInfo.Title = 'Quy tr??nh th???c hi???n y??u c???u kh??ch h??ng';
      this.generateCode();
      this.getListCustomer();
      this.getCbbDepartment();
      this.GetProjectPhaseType();
      this.getListDepartmentRequestEmployees();
      this.getListDepartmentReceiveEmployees();
      this.getCustomerContact();
    }
  }
  getCbbDepartment() {
    this.comboboxService.getCbbDepartment().subscribe(
      data => {
        this.listDepartment = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
  changeDepartmentRequest() {
    this.getListDepartmentRequestEmployees();
    //this.requirementModel.NamePetitioner = this.listEmployee.find(a => a.DepartmentId == this.requirementModel.DepartmentRequest).Name;
  }

  getListDepartmentRequestEmployees() {
    if (this.requirementModel.DepartmentRequest) {
      this.comboboxService.getListDepartmentRequestEmployees(this.requirementModel.DepartmentRequest).subscribe(
        data => {
          this.listEmployee = data;
          //this.requirementModel.NamePetitioner = this.listEmployee.find(a => a.DepartmentId == this.requirementModel.DepartmentRequest).Name;
          var NamePetitioner = this.listEmployee.find(a => a.DepartmentId == this.requirementModel.DepartmentReceive);
          if (NamePetitioner != null) {
            NamePetitioner = NamePetitioner.Name;
          }
        },
        error => {
          this.messageService.showError(error);
        }
      );
    } else {
      this.comboboxService.getListEmployees().subscribe(
        data => {
          this.listEmployee = data;
          //this.requirementModel.NamePetitioner = this.listEmployee.find(a => a.DepartmentId == this.requirementModel.DepartmentRequest).Name;
          var NamePetitioner = this.listEmployee.find(a => a.DepartmentId == this.requirementModel.DepartmentReceive);
          if (NamePetitioner != null) {
            NamePetitioner = NamePetitioner.Name;
          }
        },
        error => {
          this.messageService.showError(error);
        }
      );

    }
  }

  getProjectByCustomerRequirementId() {
    var model = {
      CustomerRequirementId: this.requirementModel.Id,
    }
    this.customerRequirementService.getProjectByCustomerRequirementId(model).subscribe(
      data => {
        this.listProject = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  treeView_itemSelectionChanged(e) {
    this.treeBoxValue = e.selectedRowKeys[0];
    this.requirementModel.ParentId = e.selectedRowKeys[0];
    this.closeDropDownBox();
  }

  closeDropDownBox() {
    this.isDropDownBoxOpened = false;
  }

  syncTreeViewSelection() {
    if (!this.treeBoxValue) {
      this.selectedRowKeys = [];
    } else {
      this.selectedRowKeys = [this.treeBoxValue];
    }
  }

  getListMaterial() {
    this.comboboxService.getListMaterial().subscribe(
      data => {
        this.listMaterial = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getListDepartmentReceiveEmployees() {
    this.comboboxService.getListDepartmentReceiveEmployees(this.requirementModel.DepartmentReceive).subscribe(
      data => {
        this.listEmployee1 = data;
        //this.requirementModel.NameReciever = this.listEmployee1.find(a => a.DepartmentId == this.requirementModel.DepartmentReceive).Name;
        var NameReciever = this.listEmployee1.find(a => a.DepartmentId == this.requirementModel.DepartmentReceive);
        if (NameReciever != null) {
          NameReciever = NameReciever.Name;
        }
        error => {
          this.messageService.showError(error);
        }
      }
    );
  }

  generateCode() {
    this.customerRequirementService.generateCode().subscribe((data: any) => {
      if (data) {
        this.requirementModel.Code = data.Code;
        this.requirementModel.Index = data.Index;
      }
    });
  }

  getListCustomer() {
    this.comboboxService.getListCustomer().subscribe(
      data => {
        this.listCustomer = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  changeDepartmentReceive() {
    this.getListDepartmentReceiveEmployees();
  }

  getCustomerId(Id) {
    this.customerRequirementService.getCustomerId(Id).subscribe(
      data => {
        this.getDomainByCustomerId(data);
        this.CustomerId = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  changeCustomer(CustomerId) {

    if (CustomerId != this.CustomerId) {

      this.requirementModel.CustomerCode = null;
      this.requirementModel.DomainId = null;
      this.listDomain = [];
      this.requirementModel.CustomerContactId = null;
      this.listCustomerContact = []
      this.requirementModel.CustomerContactPhoneNumber = null;
      this.requirementModel.CustomerContactEmail = null;
      this.requirementModel.CustomerCode = this.listCustomer.find(a => a.Id == this.requirementModel.CustomerId).Code;
      this.requirementModel.ListContent = [];
      this.getCustomerContact();
      this.getDomain();
      this.getCustomerContactById(CustomerId)
    }

    if (CustomerId == this.CustomerId) {
     // this.getDomainByCustomerId(this.CustomerId);
     this.requirementModel.CustomerId = this.CustomerId;
      this.getDomain();
      this.customerRequirementService.getById(this.ChangeStepModel.Id).subscribe(
        data => {
          this.requirementModel.ListContent = data.ListContent;
          this.requirementModel.DomainId = data.DomainId;
          this.requirementModel.CustomerContactId = data.CustomerContactId;
          this.getCustomerContact();
          if (CustomerId == data.CustomerId) {
            this.requirementModel.ListContent.forEach(element => {
              if (element.FinishDate) {
                element.FinishDate = this.dateUtils.convertDateToObject(element.FinishDate);
              }
            });

            this.requirementModel.ListContent.forEach(element => {
              if (element.CreateDate) {
                element.CreateDate = this.dateUtils.convertDateToObject(element.CreateDate);
              }
            });
          }

          
        });
    }
  }

  getCustomerContactById(Id) {
    this.customerRequirementService.getCustomerContactById(Id).subscribe(
      data => {
        this.listCustomerRequirement = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  changeCustomerContact() {
    this.requirementModel.CustomerContactPosition = null;
    this.requirementModel.CustomerContactPhoneNumber = null;
    this.requirementModel.CustomerContactEmail = null;
    this.requirementModel.CustomerContactPosition = this.listCustomerContact.find(a => a.Id == this.requirementModel.CustomerContactId).Position;
    this.requirementModel.CustomerContactPhoneNumber = this.listCustomerContact.find(a => a.Id == this.requirementModel.CustomerContactId).PhoneNumber;
    this.requirementModel.CustomerContactEmail = this.listCustomerContact.find(a => a.Id == this.requirementModel.CustomerContactId).Email;
  }

  getCustomerContact() {
    if (this.requirementModel.CustomerId) {
      this.comboboxService.getCustomerContact(this.requirementModel.CustomerId).subscribe(
        data => {
          this.listCustomerContact = data;
          this.listCustomerRequirement = data;
          this.requirementModel.CustomerContactPosition = this.listCustomerContact.find(a => a.Id == this.requirementModel.CustomerContactId).Position;
          this.requirementModel.CustomerContactPhoneNumber = this.listCustomerContact.find(a => a.Id == this.requirementModel.CustomerContactId).PhoneNumber;
          this.requirementModel.CustomerContactEmail = this.listCustomerContact.find(a => a.Id == this.requirementModel.CustomerContactId).Email;
        },
        error => {
          this.messageService.showError(error);
        }
      );
    }
    else {
      this.listCustomerContact = [];
    }

  }

  getDomainByCustomerId(id)
  {
    this.customerRequirementService.getDomain(id).subscribe(
      data => {
        this.listDomain = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
  getDomain() {
    this.listDomain = [];
    if (this.requirementModel.CustomerId) {
      this.customerRequirementService.getDomain(this.requirementModel.CustomerId).subscribe(
        data => {
          this.listDomain = data;

        },
        error => {
          this.messageService.showError(error);
        }
      );
    } else {
      this.listDomain = [];
    }
  }


  getById(check) {
    this.ChangeStepModel.Id = this.requirementModel.Id;
    this.customerRequirementService.getById(this.ChangeStepModel.Id).subscribe(
      data => {
        this.requirementModel = data;

        if (data.CustomerId) {
          this.getCustomerContactById(data.CustomerId);
        }

        this.SolutionModel.EstimateState = data.EstimateState;
        if (!this.requirementModel.Person1) {
          this.requirementModel.Person1 = this.user.employeeId;
        }
        if (!this.requirementModel.Person2) {
          this.requirementModel.Person2 = this.user.employeeId;
        }
        if (!this.requirementModel.Person3) {
          this.requirementModel.Person3 = this.user.employeeId;
        }
        this.getCustomerContact();
        this.requirementModel.CustomerCode = this.listCustomer.find(a => a.Id == this.requirementModel.CustomerId).Code;
        this.getListDepartmentReceiveEmployees();

        //#endregion


        if (this.requirementModel.RealFinishDate) {
          this.requirementModel.RealFinishDate = this.dateUtils.convertDateToObject(this.requirementModel.RealFinishDate);
        }

        if (this.requirementModel.PlanFinishDate) {
          this.requirementModel.PlanFinishDate = this.dateUtils.convertDateToObject(this.requirementModel.PlanFinishDate);
        }

        if (this.requirementModel.StartDate) {
          this.requirementModel.StartDate = this.dateUtils.convertDateToObject(this.requirementModel.StartDate);
        }

        if (this.requirementModel.ProtectVersionUpdateDate) {
          this.requirementModel.ProtectVersionUpdateDate = this.dateUtils.convertDateToObject(this.requirementModel.ProtectVersionUpdateDate);
        }

        this.requirementModel.ListContent.forEach(element => {
          if (element.FinishDate) {
            element.FinishDate = this.dateUtils.convertDateToObject(element.FinishDate);
          }
        });

        this.requirementModel.ListContent.forEach(element => {
          if (element.CreateDate) {
            element.CreateDate = this.dateUtils.convertDateToObject(element.CreateDate);
          }
        });

        this.constant.CustomerRequirementStatus.forEach((item, index) => {
          if (item.Id == this.requirementModel.Status) {
            this.indexNumber = index;
            if (check != '') {
              this.indexNumber = check;
            }
            if (this.requirementModel.Status == 6) {
              this.setScrollCustoms();
            }
          }
        })


      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  indexNumber = 0;
  seclect(item: any, index: number) {
    // if (item.Id <= this.requirementModel.Status) {
    //   this.indexNumber = index;
    // }

    this.indexNumber = index;

    if (item.Id == 6) {
      this.setScrollCustoms();
    }
  }

  setScrollCustoms() {
    setTimeout(() => {
      this.scrollCustomProduct = document.getElementById("scrollCustomProduct");
      this.scrollCustomProduct.removeEventListener('ps-scroll-x', null);

      this.scrollCustomHeader = document.getElementById("scrollCustomHeader");

      this.scrollCustomProduct.addEventListener('ps-scroll-x', (event: any) => {
        this.scrollCustomHeader.scrollLeft = event.target.scrollLeft;
      }, true);

    }, 1000);
  }


  removeSurvey(index) {
    this.listSurvey.splice(index, 1);
  }

  checkDeleteSurveyId(surveyId, index) {
    if (surveyId == '' || surveyId == undefined) {
      this.removeSurvey(index);
    } else {
      this.customerRequirementService.checkDeleteSurvey(surveyId).subscribe((data: any) => {
        this.removeSurvey(index);
      },
        error => {
          this.messageService.showError(error);
        });
    }
  }

  create(isContinue) {
    this.customerRequirementService.create(this.requirementModel).subscribe(
      data => {
        if (isContinue) {
          this.clear();
          this.isAction = true;
          this.requirementModel = {
            TradeConditions: "",
            TotalPrice: 0,
            ListRequireEstimateMaterialAttach: [],
            ListRequireEstimateFCMlAttach: []
          };
          this.requirementModel.Survey = this.listSurvey;
          // this.requirementModel.ListMaterial.forEach(element => {
          //   if (element.DeliveryTime != null && element.DeliveryTime != '') {
          //     element.DeliveryTime = this.dateUtils.convertObjectToDate(element.DeliveryTime);
          //   }
          // });
          this.messageService.showSuccess('Th??m m???i Y??u c???u kh??ch h??ng th??nh c??ng!');
        }
        else {
          this.messageService.showSuccess('Th??m m???i Y??u c???u kh??ch h??ng th??nh c??ng!');
          //this.closeModal(true);
          this.close();
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  update() {
    this.requirementModel.Survey = this.listSurvey;
    this.customerRequirementService.update(this.requirementModel.Id, this.requirementModel).subscribe(
      data => {
        this.messageService.showSuccess('C???p nh???t Y??u c???u kh??ch h??ng th??nh c??ng!');
        this.getById('');
        this.close();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  GetProjectPhaseType() {
    this.comboboxService.getProjectPhaseType().subscribe(data => {
      this.listProjectPhase = data;
    }, error => {
      this.messageService.showError(error);
    });
  }

  save(isContinue: boolean) {
    var listFileUpload = [];
    this.requirementModel.ListAttach.forEach((document, index) => {
      if (document.File) {
        document.File.index = index;
        listFileUpload.push(document.File);
      }
    });

    var materialFiles = [];
    this.requirementModel.ListRequireEstimateMaterialAttach.forEach((document, index) => {
      if (document.File) {
        document.File.index = index;
        materialFiles.push(document.File);
      }
    });

    var fcmFiles = [];
    this.requirementModel.ListRequireEstimateFCMlAttach.forEach((document, index) => {
      if (document.File) {
        document.File.index = index;
        fcmFiles.push(document.File);
      }
    });


    if (this.requirementModel.RealFinishDate) {
      this.requirementModel.RealFinishDate = this.dateUtils.convertObjectToDate(this.requirementModel.RealFinishDate);
    }

    if (this.requirementModel.PlanFinishDate) {
      this.requirementModel.PlanFinishDate = this.dateUtils.convertObjectToDate(this.requirementModel.PlanFinishDate);
    }

    if (this.requirementModel.StartDate) {
      this.requirementModel.StartDate = this.dateUtils.convertObjectToDate(this.requirementModel.StartDate);
    }

    if (this.requirementModel.ProtectVersionUpdateDate) {
      this.requirementModel.ProtectVersionUpdateDate = this.dateUtils.convertObjectToDate(this.requirementModel.ProtectVersionUpdateDate);
    }

    this.requirementModel.ListContent.forEach(element => {
      if (element.FinishDate) {
        element.FinishDate = this.dateUtils.convertObjectToDate(element.FinishDate);
      }
    });

    this.requirementModel.ListContent.forEach(element => {
      if (element.CreateDate) {
        element.CreateDate = this.dateUtils.convertObjectToDate(element.CreateDate);
      }
    });

    if (listFileUpload.length > 0 || materialFiles.length > 0 || fcmFiles.length > 0) {
      let filesUpload = this.uploadService.uploadListFile(listFileUpload, 'CustomerRequirement/');
      let filesMaterial = this.uploadService.uploadListFile(materialFiles, 'CustomerRequireMaterial/');
      // let filesFCM = this.uploadService.uploadListFile(materialFiles, 'CustomerRequireFCM/');
      forkJoin([filesUpload, filesMaterial]).subscribe(results => {
        if (results[0].length > 0) {
          listFileUpload.forEach((item, index) => {
            this.requirementModel.ListAttach[item.index].FilePath = results[0][index].FileUrl;
          });
        }
        if (results[1].length > 0) {
          materialFiles.forEach((item, index) => {
            this.requirementModel.ListRequireEstimateMaterialAttach[item.index].FilePath = results[1][index].FileUrl;
          });
        }

        if (this.requirementModel.Id) {
          this.update()
        }
        else {
          this.create(isContinue);
        }

      })
    } else {
      if (this.requirementModel.Id) {
        this.update()
      }
      else {
        this.create(isContinue);
      }
    }
  }

  nextStep() {
    let valid = this.validNextStep();
    if (valid) {
      this.messageService.showConfirm("B???n c?? ch???c mu???n k???t th??c quy tr??nh hi???n t???i kh??ng?").then(
        data => {
          //this.save(true);
          this.customerRequirementService.nextStep(this.requirementModel).subscribe(
            data => {
              this.getById('');
              this.messageService.showSuccess('K???t th??c quy tr??nh th??nh c??ng!');
            },
            error => {
              this.messageService.showError(error);
            }
          );
        }
      );
    }
    else {
      this.messageService.showMessage(this.validMessage);
    }
  }

  nextThreeStep() {
    let valid = this.validNextthreeStep();
    if (valid) {
      this.messageService.showConfirm("B???n c?? ch???c mu???n h???y gi???i ph??p hi???n t???i kh??ng?").then(
        data => {
          //this.save(true);
          this.customerRequirementService.nextThreeStep(this.requirementModel).subscribe(
            data => {
              this.getById('');
              this.messageService.showSuccess('H???y gi???i ph??p th??nh c??ng!');
            },
            error => {
              this.messageService.showError(error);
            }
          );
        }
      );
    }
    else {
      this.messageService.showMessage(this.validMessage);
    }
  }


  backStep() {
    this.ChangeStepModel.Id = this.requirementModel.Id;
    this.messageService.showConfirm("B???n c?? ch???c mu???n tr??? v??? quy tr??nh tr?????c kh??ng?").then(
      data => {
        this.customerRequirementService.backStep(this.ChangeStepModel).subscribe(
          data => {
            this.getById('');
            this.messageService.showSuccess('Quay l???i quy tr??nh th??nh c??ng!');
          },
          error => {
            this.messageService.showError(error);
          }
        );
      }
    );
  }

  backThreeStep() {
    this.ChangeStepModel.Id = this.requirementModel.Id;
    this.messageService.showConfirm("B???n c?? ch???c mu???n tr??? v??? quy tr??nh tr?????c kh??ng?").then(
      data => {
        this.customerRequirementService.backThreeStep(this.ChangeStepModel).subscribe(
          data => {
            this.getById('');
            this.messageService.showSuccess('Quay l???i quy tr??nh th??nh c??ng!');
          },
          error => {
            this.messageService.showError(error);
          }
        );
      }
    );
  }



  saveAndContinue() {
    this.save(true);
  }

  clear() {
    this.requirementModel = {
      Code: '',
      Type: 1,
      Index: 0,
      ListAttach: []
    };
    this.generateCode();
  }

  uploadFile($event) {
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileDataSheet) {
      isExist = false;
      for (var ite of this.requirementModel.ListAttach) {
        if (ite.Id != null) {
          if (file.name == ite.FileName) {
            isExist = true;
          }
        }
        else {
          if (file.name == ite.name) {
            isExist = true;
          }
        }
      }
    }
    if (isExist) {
      this.messageService.showConfirm("File ???? t???n t???i. B???n c?? mu???n ghi ???? l??n kh??ng").then(
        data => {
          this.updateFileAndReplaceManualDocument(fileDataSheet, true);
        }, error => {
          this.updateFileAndReplaceManualDocument(fileDataSheet, false);
        });
    }
    else {
      this.updateFileManualDocument(fileDataSheet);
    }
  }

  updateFileAndReplaceManualDocument(fileManualDocuments, isReplace) {
    var isExist = false;
    let docuemntTemplate;
    for (var file of fileManualDocuments) {
      for (let index = 0; index < this.requirementModel.ListAttach.length; index++) {

        if (this.requirementModel.ListAttach[index].Id != null) {
          if (file.name == this.requirementModel.ListAttach[index].FileName) {
            isExist = true;
            if (isReplace) {
              this.requirementModel.ListAttach.splice(index, 1);
            }
          }
        }
        else if (file.name == this.requirementModel.ListAttach[index].name) {
          isExist = true;
          if (isReplace) {
            this.requirementModel.ListAttach.splice(index, 1);
          }
        }
      }

      if (!isExist || isReplace) {
        docuemntTemplate = Object.assign({}, this.fileTemplate);
        docuemntTemplate.File = file;
        docuemntTemplate.FileName = file.name;
        docuemntTemplate.FileSize = file.size;
        this.requirementModel.ListAttach.push(docuemntTemplate);
      }
    }
  }

  updateFileManualDocument(files) {
    let docuemntTemplate;
    for (var file of files) {
      docuemntTemplate = Object.assign({}, this.fileTemplate);
      docuemntTemplate.File = file;
      docuemntTemplate.FileName = file.name;
      docuemntTemplate.FileSize = file.size;
      this.requirementModel.ListAttach.push(docuemntTemplate);
    }
  }

  uploadFileMaterial($event) {
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileDataSheet) {
      isExist = false;
      for (var ite of this.requirementModel.ListRequireEstimateMaterialAttach) {
        if (ite.Id != null) {
          if (file.name == ite.FileName) {
            isExist = true;
          }
        }
        else {
          if (file.name == ite.name) {
            isExist = true;
          }
        }
      }
    }
    if (isExist) {
      this.messageService.showConfirm("File ???? t???n t???i. B???n c?? mu???n ghi ???? l??n kh??ng").then(
        data => {
          this.updateReplaceFileMaterial(fileDataSheet, true);
        }, error => {
          this.updateReplaceFileMaterial(fileDataSheet, false);
        });
    }
    else {
      this.updateFileMaterial(fileDataSheet);
    }
  }

  updateReplaceFileMaterial(file, isReplace) {
    var isExist = false;
    let docuemntTemplate;
    for (var file of file) {
      for (let index = 0; index < this.requirementModel.ListRequireEstimateMaterialAttach.length; index++) {

        if (this.requirementModel.ListRequireEstimateMaterialAttach[index].Id != null) {
          if (file.name == this.requirementModel.ListRequireEstimateMaterialAttach[index].FileName) {
            isExist = true;
            if (isReplace) {
              this.requirementModel.ListRequireEstimateMaterialAttach.splice(index, 1);
            }
          }
        }
        else if (file.name == this.requirementModel.ListRequireEstimateMaterialAttach[index].name) {
          isExist = true;
          if (isReplace) {
            this.requirementModel.ListRequireEstimateMaterialAttach.splice(index, 1);
          }
        }
      }

      if (!isExist || isReplace) {
        docuemntTemplate = Object.assign({}, this.fileTemplate);
        docuemntTemplate.File = file;
        docuemntTemplate.FileName = file.name;
        docuemntTemplate.FileSize = file.size;
        docuemntTemplate.Type = 1;
        this.requirementModel.ListRequireEstimateMaterialAttach.push(docuemntTemplate);
      }
    }
  }

  updateFileMaterial(files) {
    let docuemntTemplate;
    for (var file of files) {
      docuemntTemplate = Object.assign({}, this.fileTemplate);
      docuemntTemplate.File = file;
      docuemntTemplate.FileName = file.name;
      docuemntTemplate.FileSize = file.size;
      docuemntTemplate.Type = 1;
      this.requirementModel.ListRequireEstimateMaterialAttach.push(docuemntTemplate);
    }
  }

  downloadAFile(file) {
    this.fileProcess.downloadFileBlob(file.FilePath, file.FileName);
  }

  showConfirmDeleteFile(document, index) {
    this.messageService.showConfirm("B???n c?? ch???c mu???n xo?? t??i li???u n??y kh??ng?").then(
      data => {
        this.deleteFile(document, index);
      },
      error => {

      }
    );
  }

  deleteFile(document, index) {
    if (document.Id) {
      document.IsDelete = true;
    }
    else {
      this.requirementModel.ListAttach.splice(index, 1);
    }
  }

  showConfirmDeleteFileRequireEstimate(document, index) {
    this.messageService.showConfirm("B???n c?? ch???c mu???n xo?? t??i li???u n??y kh??ng?").then(
      data => {
        this.deleteFileRequireEstimate(document, index);
      },
      error => {

      }
    );
  }

  deleteFileRequireEstimate(document, index) {
    if (document.Id) {
      document.IsDelete = true;
    }
    else {
      this.requirementModel.ListRequireEstimateMaterialAttach.splice(index, 1);
    }
  }

  // closeModal(isOK: boolean) {
  //   this.activeModal.close(isOK ? isOK : this.isAction);
  // }


  close() {
    this.router.navigate(['giai-phap/yeu-cau-khach-hang']);
  }

  validNextStep() {
    let dataLength = 0;
    let materialCodes = '';
    // ??ang ph??n t??ch
    if (this.requirementModel.Step == 1) {

      dataLength = this.requirementModel.ListMaterial.forEach(element => {
        if (!element.Price || element.Price == 0 || !element.LeadTimeV) {
          materialCodes += materialCodes != '' ? ', ' + element.Code : element.Code;
        }
      });

      // if (materialCodes) {
      //   this.validMessage = 'C??c m?? v???t t??: ' + materialCodes + '. B???n ch??a nh???p ?????y ????? th??ng tin Gi?? ho???c Th???i gian giao h??ng!';
      //   return false;
      // }

    }
    return true;
  }

  validNextthreeStep() {
    let dataLength = 0;
    let materialCodes = '';
    // ??ang ph??n t??ch
    if (this.requirementModel.Step == 1) {

      dataLength = this.requirementModel.ListMaterial.forEach(element => {
        if (!element.Price || element.Price == 0 || !element.LeadTimeV) {
          materialCodes += materialCodes != '' ? ', ' + element.Code : element.Code;
        }
      });
    }
    return true;
  }


  validateSurvey() {

    let newSurveyModel = Object.assign({}, this.surveyModel);
    this.requirementModel.Survey.push(newSurveyModel);
    this.surveyModel = {
      Id: '',
      ProjectPhaseId: '',
      SurveyDate: '',
      Content: '',
      Result: '',
      Level: '',
      Times: '',
    }

  }



  deleteSolutionProduct(Id: string) {
    this.solutionAnalysisProductService.deleteSolutionProduct(Id).subscribe(
      data => {
        this.messageService.showSuccess('X??a s???n ph???m th??nh c??ng!');
        this.getById(3);
      },
      error => {
        this.messageService.showError(error);
      });
  }


  //#region thaint Bug #17937
  addRow1() {
    if (!this.contentModel.Request) {
      this.messageService.showMessage("B???n kh??ng ???????c ????? tr???ng y??u c???u!");
    }
    else {
      var row = Object.assign({}, this.contentModel);
      row.Request = this.contentModel.Request;
      row.Solution = this.contentModel.Solution;
      row.FinishDate = this.contentModel.FinishDate;
      row.CreateDate = this.contentModel.CreateDate;
      row.Note = this.contentModel.Note;
      row.Code = this.contentModel.Code;
      // row.Checked=this.contentModel.Checked;
      this.requirementModel.ListContent.push(row);
      this.contentModel = {
        Request: '',
        Solution: '',
        FinishDate: null,
        CreateDate: null,
        Note: '',
        Code: '',
        // Checked: false,
      }
    }
  }
  //#endregion

  addProtectSolution() {

    let newProtectSolutionModel = Object.assign({}, this.protectSolutionModel);
    this.SolutionModel.ListProtectSolution.push(newProtectSolutionModel);
    this.protectSolutionModel = {
      Id: '',
      Request: '',
      Solution: '',
      Note: '',
      FinishDate: '',
    }

  }
  //#region  thaint Bug #17937
  deleteContent(Id) {
    this.messageService.showConfirm("B???n c?? ch???c mu???n xo?? n???i dung n??y kh??ng?").then(
      data => {
        this.customerRequirementService.deleteCustomerRequirementContent(Id).subscribe(result => {
          this.getById('');
        });
      },
      error => {

      }
    );
  }
  selectCustomerRequimentNeedToHandle(index) {
    this.selectIndex == index;
  }

  //listBank: any[] = [];
  showCreateCustomerRequirement(Id: string, row: any, index: any, Create: boolean) {
    let activeModal = this.modalService.open(CustomerRequirequirmentNeedToHandleComponent, { container: 'body', windowClass: 'project-payment-create-modal', backdrop: 'static' });
    activeModal.componentInstance.listContentModel = this.requirementModel.ListContent;
    activeModal.componentInstance.Id = Id;
    activeModal.componentInstance.RequirementId = this.requirementModel.Id;
    activeModal.componentInstance.index = index;
    activeModal.componentInstance.Create = Create;
    activeModal.componentInstance.listTemp = this.requirementModel.ListContent;
    activeModal.componentInstance.listCustomerContact = this.listCustomerRequirement;
    if (row) {
      activeModal.componentInstance.model = row;
    }

    activeModal.result.then(result => {
      if (result.modelTemp.length > 0) {
        result.modelTemp.forEach(element => {
          if (result.modelTemp.length > 0)
            this.requirementModel.ListContent.push(element);
        })
      }
      else {
        this.requirementModel.ListContent[index].Request = result.modelTemp.Request;
        this.requirementModel.ListContent[index].Solution = result.modelTemp.Solution;
        this.requirementModel.ListContent[index].Code = result.modelTemp.Code;
        this.requirementModel.ListContent[index].CreateDate = result.modelTemp.CreateDate;
        this.requirementModel.ListContent[index].FinishDate = result.modelTemp.FinishDate;
        this.requirementModel.ListContent[index].Note = result.modelTemp.Note;
        this.requirementModel.ListContent[index].RequestBy = result.modelTemp.RequestBy;
        this.requirementModel.ListContent[index].RequestName = result.modelTemp.RequestName;
      }
    })
  }

  showComfirmDeleteContent(index: any) {
    this.messageService.showConfirm("B???n c?? ch???c mu???n xo?? y??u c???u n??y kh??ng?").then(
      data => {
        this.requirementModel.ListContent.splice(index, 1);
      },
      error => {
      }
    );
  }
  //#endregion
}
