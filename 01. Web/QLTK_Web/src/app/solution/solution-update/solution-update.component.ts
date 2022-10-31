import { Component, OnInit, ViewEncapsulation } from '@angular/core';

import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { Configuration, MessageService, FileProcess, Constants, AppSetting, DateUtils, ComboboxService, PermissionService } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { SolutionService } from '../service/solution.service';
import { forkJoin } from 'rxjs';
import { ChooseProjectSolutionComponent } from '../choose-project-solution/choose-project-solution.component';
import { ActivatedRoute } from '@angular/router';
import { Router, } from '@angular/router';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from '@kolkov/ngx-gallery';
import { SurveyCreateComponent } from '../customer-requirement/customer-requirement-create/Survey-create/Survey-create.component';
import { SurveyContentCreateComponent } from '../customer-requirement/survey-content/survey-content-create/survey-content-create.component';
import { SurveyMaterialCreateComponent } from '../customer-requirement/survey-material-create/survey-material-create.component';
import { SelectMaterialComponent } from 'src/app/education/classroom/select-material/select-material/select-material.component';


@Component({
  selector: 'app-solution-update',
  templateUrl: './solution-update.component.html',
  styleUrls: ['./solution-update.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SolutionUpdateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    public appSetting: AppSetting,
    private config: Configuration,
    private messageService: MessageService,
    private service: SolutionService,
    public fileProcess: FileProcess,
    private uploadService: UploadfileService,
    public constants: Constants,
    public appset: AppSetting,
    private modalService: NgbModal,
    public dateUtils: DateUtils,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private combobox: ComboboxService,
    public permissionService: PermissionService,
    private routeA: ActivatedRoute,
    private router: Router,

  ) { }

  isAction: boolean = false;
  Id: string;
  startDate: any;
  finishDate: any;
  listEmployeeBusiness: any[] = [];
  content: any[] = [];
  listCustomerRequirements = [];
  listEmployeesMaker: any[] = [];
  listSolutionGroup = [];
  listSBU: any[] = [];
  listDepartmentBusiness = [];
  listDepartmentMaker = [];
  listCustomer = [];
  listProject = [];
  ListFile = [];
  listJob = [];
  listApplication = [];
  listSurvey: any[] = [];
  listProjectPhase: any[] = [];
  CustomerRequirements = [];
  dropdownList: any[] = [];

  listMaterialId: any[] = [];
  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }];
  columnCustomer: any[] = [{ Name: 'Code', Title: 'Mã khách hàng' }, { Name: 'Name', Title: 'Tên khách hàng' }];
  listType: any[] = [
    { Id: 1, Name: '3D giải pháp' },
    { Id: 2, Name: 'Bản vẽ tổng 2D' },
    { Id: 3, Name: 'Bản giải trình' },
    { Id: 4, Name: 'DMVT' },
    { Id: 5, Name: 'FCM' },
    { Id: 6, Name: 'Thông số kỹ thuật' }
  ]

  listSurveyLevel: any[] = [
    { Id: 1, Name: "Khẩn cấp" },
    { Id: 2, Name: "Cao" },
    { Id: 3, Name: "Trung bình" },
    { Id: 4, Name: "Thấp" },
  ]

  state: any[] = [
    { Id: 0, Name: "Đang làm" },
    { Id: 1, Name: "Hoàn thành" },
    { Id: 2, Name: "Bỏ qua" },
  ]

  listStatus = [
    { Id: 1, Name: "Đang triển khai" },
    { Id: 2, Name: "Tạm dừng" },
    { Id: 3, Name: "Hủy" },
    { Id: 4, Name: "Đã hoàn thành" },
  ]

  ListMaterial: any[] = [];

  model: any = {
    Id: '',
    Name: '',
    Code: '',
    Description: '',
    CustomerId: '',
    EndCustomerId: '',
    Price: null,
    FinishDate: null,
    SolutionGroupId: '',
    SBUBusinessId: '',
    DepartmentBusinessId: '',
    BusinessUserId: '',
    SBUSolutionMakerId: '',
    DepartmentSolutionMakerId: '',
    SolutionMaker: '',
    Has3DSolution: false,
    Has2D: false,
    HasExplan: false,
    HasDMVT: false,
    HasFCM: false,
    HasTSKT: false,
    ListFile: [],
    Status: 1,
    SaleNoVat: '',
    StartDate: null,
    ListProjectSolution: [],
    MechanicalMaker: '',
    ElectricMaker: '',
    ElectronicMaker: '',
    CurentVersion: 1,
    EditContent: '',
    ListImage: [],
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
  thumbnailActions: NgxGalleryOptions[];

  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[] = [];

  columnStatus: any[] = [{ Name: 'Name', Title: 'Trạng thái' }];
  columnProject: any[] = [{ Name: 'Code', Title: 'Mã dự án' }, { Name: 'Name', Title: 'Tên dự án' }];
  columnSBU: any[] = [{ Name: 'Code', Title: 'Mã SBU' }, { Name: 'Name', Title: 'Tên SBU' }];
  columnDepartment: any[] = [{ Name: 'Code', Title: 'Mã phòng ban' }, { Name: 'Name', Title: 'Tên phòng ban' }];
  columnEmployee: any[] = [{ Name: 'Name', Title: 'Tên nhân viên' }];
  columnCustomerRequirements: any[] = [{ Name: 'Code', Title: 'Mã nhóm yêu cầu' }, { Name: 'Name', Title: 'Tên nhóm yêu cầu' }];

  fileModel = {
    Id: '',
    SolutionId: '',
    Path: '',
    FileName: '',
    FileSize: '',
    Type: 0,
  }
  selectIndex = -1;

  ngOnInit() {
    this.appSetting.PageTitle = "Chỉnh sửa giải pháp";
    this.Id = this.routeA.snapshot.paramMap.get('Id');
    this.fileProcess.FilesDataBase = [];
    this.getListCustomerRequirement();
    this.getSolutionTechnologies();
    this.GetProjectPhaseType();
    this.getListJob();
    this.getApplication();
    // this.getListSolutionGroup();
    // this.getCbbSBU();
    // this.getListCustomer();
    // this.getListProject();
    this.getCbbData();
    if (this.Id) {
      this.getSolutionInfo();
    }

    this.galleryOptions = [
      {
        height: '350px',
        width: '100%',
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        thumbnailActions: [{ icon: 'fa fa-times-circle', onClick: this.deleteImage.bind(this), titleText: 'Xoá ảnh' }]
      },
      // max-width 800
      {
        breakpoint: 800,
        width: '100%',
        imagePercent: 80,
        thumbnailsPercent: 20,
        thumbnailsMargin: 20,
        thumbnailMargin: 20,
        thumbnailActions: [{ icon: 'fa fa-times-circle', onClick: this.deleteImage.bind(this), titleText: 'Xoá ảnh' }]
      },
      // max-width 400
      {
        breakpoint: 400,
        preview: false
      }
    ];
  }

  getCbbData() {
    let cbbListSolutionGroup = this.combobox.getListSolutionGroup();
    let cbbCustomer = this.combobox.getListCustomer();
    let cbbListProject = this.combobox.getListProject();
    let cbbSBU = this.combobox.getCbbSBU();
    forkJoin([cbbListSolutionGroup, cbbSBU, cbbCustomer, cbbListProject]).subscribe(results => {
      this.listSolutionGroup = results[0];
      this.listSBU = results[1];
      this.listCustomer = results[2];
      this.listProject = results[3];
    });
  }

  showConfirmDeleteSurvey(Id, index) {

    this.service.checkDeleteSurvey(Id).subscribe((data: any) => {
      this.removeSurvey(index);
    },
      error => {
        this.messageService.showError(error);
      });

  }

  removeSurvey(index) {
    this.listSurvey.splice(index, 1);
  }

  getSolutionInfo() {
    this.service.getSolutionInfo({ Id: this.Id }).subscribe(data => {
      this.model = data;
      this.listSurvey = data.ListSurvey;
      for(var item of this.listSurvey){
        if (item.Time)
        {
          item.Time = item.Time.hour + ":" + (item.Time.minute < 10 ? '0' : '') + item.Time.minute;
        }
      }
      this.appSetting.PageTitle = 'Chỉnh sửa giải pháp - ' + this.model.Code + " - " + this.model.Name;
      this.ListResult = data.ListProjectSolution;
      this.getCustomerRequirementCode();
      if (data.FinishDate) {
        let dateArray = data.FinishDate.split('T')[0];
        let dateValue = dateArray.split('-');
        let tempDateFrom = {
          'day': parseInt(dateValue[2]),
          'month': parseInt(dateValue[1]),
          'year': parseInt(dateValue[0])
        };
        this.finishDate = tempDateFrom;
      }

      if (data.StartDate) {
        let dateArray = data.StartDate.split('T')[0];
        let dateValue = dateArray.split('-');
        let tempDateFrom = {
          'day': parseInt(dateValue[2]),
          'month': parseInt(dateValue[1]),
          'year': parseInt(dateValue[0])
        };
        this.startDate = tempDateFrom;
      }
      for (var item of data.ListImage) {
        this.galleryImages.push({
          small: this.config.ServerFileApi + item.ThumbnailPath,
          medium: this.config.ServerFileApi + item.FilePath,
          big: this.config.ServerFileApi + item.FilePath
        });
      }
      this.getListCustomerRequirement();

      forkJoin([
        this.combobox.getCbbDepartmentBySBU(this.model.SBUBusinessId),
        this.combobox.getCbbDepartmentBySBU(this.model.SBUSolutionMakerId),
        this.combobox.getListUserWithDepartment(this.model.DepartmentBusinessId),
        this.combobox.getListUserWithDepartment(this.model.DepartmentSolutionMakerId)]
      ).subscribe(([res1, res2, res3, res4]) => {
        this.listDepartmentBusiness = res1;
        this.listDepartmentMaker = res2;
        this.listEmployeeBusiness = res3;
        this.listEmployeesMaker = res4;
      });
    });
  }

  getListJob() {
    this.combobox.getListJob().subscribe(
      data => {
        this.listJob = data;

      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  getApplication() {
    this.combobox.getApplication().subscribe(
      data => {
        this.listApplication = data;

      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  uploadFileMaterial($event) {
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileDataSheet) {
      isExist = false;
      for (var ite of this.model.ListRequireEstimateMaterialAttach) {
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
      this.messageService.showConfirm("File đã tồn tại. Bạn có muốn ghi đè lên không").then(
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

  updateFileMaterial(files) {
    let docuemntTemplate;
    for (var file of files) {
      docuemntTemplate = Object.assign({}, this.fileTemplate);
      docuemntTemplate.File = file;
      docuemntTemplate.FileName = file.name;
      docuemntTemplate.FileSize = file.size;
      docuemntTemplate.Type = 1;
      this.model.ListRequireEstimateMaterialAttach.push(docuemntTemplate);
    }
  }

  updateReplaceFileMaterial(file, isReplace) {
    var isExist = false;
    let docuemntTemplate;
    for (var file of file) {
      for (let index = 0; index < this.model.ListRequireEstimateMaterialAttach.length; index++) {

        if (this.model.ListRequireEstimateMaterialAttach[index].Id != null) {
          if (file.name == this.model.ListRequireEstimateMaterialAttach[index].FileName) {
            isExist = true;
            if (isReplace) {
              this.model.ListRequireEstimateMaterialAttach.splice(index, 1);
            }
          }
        }
        else if (file.name == this.model.ListRequireEstimateMaterialAttach[index].name) {
          isExist = true;
          if (isReplace) {
            this.model.ListRequireEstimateMaterialAttach.splice(index, 1);
          }
        }
      }

      if (!isExist || isReplace) {
        docuemntTemplate = Object.assign({}, this.fileTemplate);
        docuemntTemplate.File = file;
        docuemntTemplate.FileName = file.name;
        docuemntTemplate.FileSize = file.size;
        docuemntTemplate.Type = 1;
        this.model.ListRequireEstimateMaterialAttach.push(docuemntTemplate);
      }
    }
  }

  getListSolutionGroup() {
    this.combobox.getListSolutionGroup().subscribe(
      data => {
        this.listSolutionGroup = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getListCustomer() {
    this.combobox.getListCustomer().subscribe(
      data => {
        this.listCustomer = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
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

  getCbbSBU() {
    this.combobox.getCbbSBU().subscribe(
      data => {
        this.listSBU = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getListDepartmentBusiness() {
    this.combobox.getCbbDepartmentBySBU(this.model.SBUBusinessId).subscribe(
      data => {
        this.listDepartmentBusiness = data;
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  getListDepartmentMaker() {
    this.combobox.getCbbDepartmentBySBU(this.model.SBUSolutionMakerId).subscribe(
      data => {
        this.listDepartmentMaker = data;
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  getListEmployeeBusiness() {
    this.model.BusinessUserId = null;
    this.combobox.getListUserWithDepartment(this.model.DepartmentBusinessId).subscribe(
      data => {
        this.listEmployeeBusiness = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getListEmployeeMaker() {
    this.model.SolutionMaker = null;
    this.combobox.getListUserWithDepartment(this.model.DepartmentSolutionMakerId).subscribe(
      data => {
        this.listEmployeesMaker = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getSolutionTechnologies() {
    this.combobox.getSolutionTechnologies().subscribe(
      data => {
        this.dropdownList = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getListCustomerRequirement() {
    this.combobox.getCustomerRequirement(this.model.CustomerId).subscribe(
      data => {
        this.listCustomerRequirements = data;
        if (this.listCustomerRequirements.length > 0) {
          this.model.CustomerRequirementId = this.listCustomerRequirements[0].Id;
          this.model.CustomerRequirementCode = this.listCustomerRequirements[0].Code;
          this.model.CustomerRequirementJob = this.listCustomerRequirements[0].Job;
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  getCustomerRequirementCode() {
    this.model.CustomerRequirementJob = "";
    this.combobox.getCustomerRequirementById(this.model.CustomerRequirementId).subscribe(
      data => {
        this.CustomerRequirements = data;
        if (this.CustomerRequirements.length > 0) {
          this.model.CustomerRequirementCode = this.CustomerRequirements[0].Code;
          this.model.CustomerRequirementDomains = this.CustomerRequirements[0].Exten;
          this.getDomainById();
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  getDomainById() {
    this.service.getDomainById(this.model.CustomerRequirementDomains).subscribe(
      data => {
        if (data) {
          this.model.CustomerRequirementJob = data.Name;
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  supUpdate() {
    if (this.finishDate) {
      this.model.FinishDate = this.dateUtils.convertObjectToDate(this.finishDate);
    } else {
      this.model.finishDate = this.finishDate
    }

    if (this.startDate) {
      this.model.StartDate = this.dateUtils.convertObjectToDate(this.startDate);
    } else {
      this.model.StartDate = this.startDate;
    }
    this.model.ListProjectSolution = this.ListResult;
    this.service.updateSolution(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật giải pháp thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  updateSolution() {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    var check = this.model.ListFile.filter(o => o.Type == 0 || o.Type == '');
    if (check.length > 0) {
      this.messageService.showMessage("Bạn chưa chọn loại tài liệu!");
      return;
    }
    this.uploadService.uploadListFile(this.model.ListFile, 'Solution/').subscribe((event: any) => {
      if (event.length > 0) {
        event.forEach((item, index) => {
          var file = Object.assign({}, this.fileModel);
          file.FileName = item.FileName;
          file.FileSize = item.FileSize;
          file.Path = item.FileUrl;
          file.Type = item.Type;
          this.model.ListFile.push(file);
        });
      }
      for (var item of this.model.ListFile) {
        if (item.Path == null || item.Path == "") {
          this.ListFile.splice(this.ListFile.indexOf(item), 1);
        }
      }
      if (validCode) {
        this.supUpdate();
      } else {
        this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
          data => {
            this.supUpdate();
          },
          error => {

          }
        );
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  save(isContinue: boolean) {
    if (this.Id) {
      this.updateSolution();
    }
    else {
      this.messageService.showMessage("Có lỗi trong quá trình xử lý");
    }
  }


  uploadFileClick($event) {
    //   var dataProcess = this.fileProcess.getFileOnFileChange($event);
    //   for (var item of dataProcess) {
    //     var a = 0;
    //     for (var ite of this.model.ListFile) {
    //       if (item.name == ite.FileName) {
    //         var b = a;
    //         var check = this.model.ListFile.length;
    //         this.messageService.showConfirmFile("File đã tồn tại. Bạn có muốn ghi đè lên không").then(
    //           data => {
    //             if (data) {
    //               this.model.ListFile.splice(b, 1);
    //             }
    //             else {
    //               //var index = this.model.ListFile.indexOf(check);
    //               this.model.ListFile.splice(check, 1);
    //             }
    //           });
    //       }
    //       a++;
    //     }
    //     if (this.fileProcess.totalbyte > 5000000) {
    //       this.messageService.showMessage("Dung lượng tệp tin tải lên quá 5MB, hãy kiểm tra lại");
    //       this.fileProcess.FilesDataBase = [];
    //       this.fileProcess.totalbyte = 0;
    //     }
    //     else {
    //       this.ListFile.push(item);
    //       var file = Object.assign({}, this.fileModel);
    //       file.FileName = item.name;
    //       file.FileSize = item.size;
    //       this.model.ListFile.push(file);
    //     }
    //   }
    // }
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileDataSheet) {
      isExist = false;
      for (var ite of this.model.ListFile) {
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
      this.messageService.showConfirm("File đã tồn tại. Bạn có muốn ghi đè lên không").then(
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

  GetProjectPhaseType() {
    this.combobox.getProjectPhaseType().subscribe(data => {
      this.listProjectPhase = data;
    }, error => {
      this.messageService.showError(error);
    });
  }

  updateSurvey(Id: String) {
    let activeModal = this.modalService.open(SurveyCreateComponent, { container: 'body', windowClass: 'survey-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.componentInstance.customerId = this.model.CustomerId;
    activeModal.result.then(result => {
      if (result) {
      }
    })
  }
  updateSurveyMaterial(Id: string) {
    let activeModal = this.modalService.open(SurveyContentCreateComponent, { container: 'body', windowClass: 'survey-content-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then(result => {
      if (result) {
        this.select(this.selectIndex);
      }
    })
  }

  showCreateUpdate(row, index, isAdd: boolean) {
    let activeModal = this.modalService.open(SurveyCreateComponent, { container: 'body', windowClass: 'survey-create-model', backdrop: 'static' })
    activeModal.componentInstance.row = Object.assign({}, row);
    activeModal.componentInstance.isAdd = isAdd;
    activeModal.componentInstance.CustomerRequirementId = this.model.CustomerRequirementId;
    activeModal.componentInstance.customerId = this.model.CustomerId;
    activeModal.result.then((result) => {
        this.getSolutionInfo();
      //this.close();

    }, (reason) => {
    });
    // this.getById('');
  }

  updateFileAndReplaceManualDocument(fileManualDocuments, isReplace) {
    var isExist = false;

    for (var file of fileManualDocuments) {
      for (let index = 0; index < this.model.ListFile.length; index++) {

        if (this.model.ListFile[index].Id != null) {
          if (file.name == this.model.ListFile[index].FileName) {
            isExist = true;
            if (isReplace) {
              this.model.ListFile.splice(index, 1);
            }
          }
        }
        else if (file.name == this.model.ListFile[index].name) {
          isExist = true;
          if (isReplace) {
            this.model.ListFile.splice(index, 1);
          }
        }
      }

      if (!isExist || isReplace) {
        file.IsFileUpload = true;
        this.model.ListFile.push(file);
      }
    }
  }

  updateFileManualDocument(fileManualDocuments) {
    for (var file of fileManualDocuments) {
      file.IsFileUpload = true;
      this.model.ListFile.push(file);
    }
  }


  downloadAFile(file: any) {
    this.fileProcess.downloadFileBlob(file.Path, file.FileName);
  }

  showConfirmDeleteFile(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file đính kèm này không?").then(
      data => {
        this.model.ListFile.splice(index, 1);
        this.messageService.showSuccess("Xóa file thành công!");
      },
      error => {

      }
    );
  }

  select(index) {
    if (this.selectIndex != index) {
      this.selectIndex = index;
      this.content = [];
      this.searchContent(this.listSurvey[index].Id);
      this.searchMaterial(this.listSurvey[index].Id);
    }
    else {
      this.selectIndex = -1;
      this.content = [];
    }
  }

  searchContent(id: any) {
    this.service.getSurveyContentId(id).subscribe((data: any) => {
      if (data.ListResult) {
        this.content = data.ListResult;
      }
    },
      error => {
        this.messageService.showError(error);
      });

  }

  searchMaterial(id: any) {
    this.service.getSurveyMaterialId(id).subscribe((data: any) => {
      if (data.ListResult) {
        this.ListMaterial = data.ListResult;
      }
    },
      error => {
        this.messageService.showError(error);
      });

  }

  closeModal(isOK: boolean) {
    this.router.navigate(['giai-phap/quan-ly-giai-phap/']);
  }
  ListResult = [];
  showClick() {
    let activeModal = this.modalService.open(ChooseProjectSolutionComponent, { container: 'body', windowClass: 'project-solution', backdrop: 'static' });
    var listIdSelect = [];
    this.ListResult.forEach(element => {
      listIdSelect.push(element.ProjectId);
    });
    activeModal.componentInstance.listIdSelect = listIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.ListResult.push(element);
        });
      }
    }, (reason) => {

    });
  }

  showConfirmDelete(row) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa dự án này không?").then(
      data => {
        this.removeRow(row);
      },
      error => {

      }
    );
  }
  removeRow(row) {
    var index = this.ListResult.indexOf(row);
    if (index > -1) {
      this.ListResult.splice(index, 1);
    }
  }

  ListImage = [];
  fileImage = {
    Id: '',
    ModuleId: '',
    FileName: '',
    FilePath: '',
    ThumbnailPath: '',
    Note: '',
    Type: 0
  }


  deleteImage(event, index): void {
    let imageDelete = this.galleryImages[index].medium;
    let indexDelete;
    this.model.ListImage.forEach(element => {
      let url = this.config.ServerFileApi + element.ThumbnailPath;
      if (url == imageDelete) {
        indexDelete = this.model.ListImage.indexOf(element);
      }
    });
    this.galleryImages.splice(index, 1);
    this.model.ListImage.splice(indexDelete, 1);
  }


  uploadFileClickImage($event) {
    this.ListImage = [];
    var fileImage = this.fileProcess.getFileOnFileChange($event);
    for (var item of fileImage) {
      this.ListImage.push(item);
    }

    this.uploadService.uploadListFile(this.ListImage, 'ImageModule/').subscribe((event: any) => {
      if (event.length > 0) {
        event.forEach(item => {
          var file = Object.assign({}, this.fileImage);
          file.FilePath = item.FileUrl;
          file.ThumbnailPath = item.FileUrlThum;
          file.FileName = item.FileName;
          file.Type = 1;
          this.model.ListImage.push(file);
          this.galleryImages.push({
            small: this.config.ServerFileApi + item.FileUrlThum,
            medium: this.config.ServerFileApi + item.FileUrlThum,
            big: this.config.ServerFileApi + item.FileUrl
          });
        });
      }
    }, error => {
      this.messageService.showError(error);
    });

  }


  showCreateMaterial() {
    let activeModal = this.modalService.open(SurveyMaterialCreateComponent, { container: 'body', windowClass: 'survey-material-create-model', backdrop: 'static' })

    activeModal.componentInstance.SurveyId = this.model.CustomerRequirementId;
    activeModal.result.then((results) => {
      if (results) {
        //this.model.ListMaterial.push(results);
        results.forEach(element => {
          this.model.ListMaterial.push({
            IsNew: true,
            Name: element.Name,
            ManufactureCode: element.ManufactureCode,
            Code: element.Code,
            Note: element.Note,
            Quantity: element.Quantity,
          });
        });
      }
    }, (reason) => {
    });
  }

  showComfirmDeleteContent(Id: string, index) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nội dung khảo sát này không?").then(
      data => {
        this.deleteContent(Id);
        this.removeSurveyConten(index);
      },
      error => {

      }
    );
  }

  deleteContent(Id: string) {
    this.service.deleteConten(Id).subscribe(
      data => {

        this.messageService.showSuccess('Xóa nhóm vật tư thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  removeSurveyConten(index) {
    this.content.splice(index, 1);
  }

  showConfirmDeleteMaterial(Id: string, index) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nhóm vật tư này không?").then(
      data => {
        this.deleteMaterial(Id);
        this.removeSurvey(index);
      },
      error => {

      }
    );
  }

  deleteMaterial(Id: string) {
    this.service.deleteMaterial(Id).subscribe(
      data => {
        this.messageService.showSuccess('Xóa nhóm vật tư thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  removeSurveyMaterial(index) {
    this.ListMaterial.splice(index, 1);
  }

}
