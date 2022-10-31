import { Component, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';

import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Router, ActivatedRoute } from '@angular/router';
import { Configuration, MessageService, FileProcess, Constants, AppSetting, DateUtils, ComboboxService, PermissionService } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { SolutionService } from '../service/solution.service';
import { forkJoin } from 'rxjs';
import { ChooseProjectSolutionComponent } from '../choose-project-solution/choose-project-solution.component';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from '@kolkov/ngx-gallery';
import { NgSelectComponent } from '@ng-select/ng-select';

@Component({
  selector: 'app-solution-create',
  templateUrl: './solution-create.component.html',
  styleUrls: ['./solution-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SolutionCreateComponent implements OnInit {

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
    public dateUtils: DateUtils,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private combobox: ComboboxService,
    public permissionService: PermissionService,
    private modalService: NgbModal,
    private router: Router,
    public fileProcessImage: FileProcess,

  ) { }
  @ViewChild(NgSelectComponent) ngSelectComponent: NgSelectComponent;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[] = [];
  
  isAction: boolean = false;
  Id: string;
  finishDate: any;
  startDate: any;
  listEmployeeBusiness: any[] = [];
  listEmployeesMaker: any[] = [];
  listSolutionGroup = [];
  listSBU: any[] = [];
  dropdownList: any[] = [];
  listDepartmentBusiness = [];
  listCustomerRequirements = [];
  listDepartmentMaker = [];
  listCustomer = [];
  listProject = [];
  listFile = [];
  listJob = [];
  listApplication = [];
  CustomerRequirements =[];
  columnCustomer: any[] = [{ Name: 'Code', Title: 'Mã khách hàng' }, { Name: 'Name', Title: 'Tên khách hàng' }];
  columnStatus: any[] = [{ Name: 'Name', Title: 'Trạng thái' }];
  columnProject: any[] = [{ Name: 'Code', Title: 'Mã dự án' }, { Name: 'Name', Title: 'Tên dự án' }];
  columnSBU: any[] = [{ Name: 'Code', Title: 'Mã SBU' }, { Name: 'Name', Title: 'Tên SBU' }];
  columnDepartment: any[] = [{ Name: 'ExCodeten', Title: 'Mã phòng ban' }, { Name: 'Name', Title: 'Tên phòng ban' }];
  columnEmployee: any[] = [{ Name: 'Name', Title: 'Tên nhân viên' }];
  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm giải pháp' }, { Name: 'Name', Title: 'Tên nhóm giải pháp' }];
  columnCustomerRequirements: any[] = [{ Name: 'Code', Title: 'Mã nhóm yêu cầu' }, { Name: 'Name', Title: 'Tên nhóm yêu cầu' }];
  columnJob: any[] = [{ Name: 'Code', Title: 'Mã lĩnh vực' }, { Name: 'Name', Title: 'Tên lĩnh vực' }];

  listType: any[] = [
    { Id: 1, Name: '3D giải pháp' },
    { Id: 2, Name: 'Bản vẽ tổng 2D' },
    { Id: 3, Name: 'Bản giải trình' },
    { Id: 4, Name: 'DMVT' },
    { Id: 5, Name: 'FCM' },
    { Id: 6, Name: 'Thông số kỹ thuật' }
  ]

  listStatus = [
    { Id: 1, Name: "Đang triển khai" },
    { Id: 2, Name: "Tạm dừng" },
    { Id: 3, Name: "Hủy" },
    { Id: 4, Name: "Đã hoàn thành" },
  ]


  model: any = {
    Id: '',
    Name: '',
    Code: '',
    Description: '',
    CustomerId: '',
    EndCustomerId: '',
    Price: 0,
    FinishDate: null,
    SolutionGroupId: '',
    SBUBusinessId: '',
    DepartmentBusinessId: '',
    BusinessUserId: '',
    SBUSolutionMakerId: '',
    DepartmentSolutionMakerId: '',
    SolutionMaker: '',
    ProjectId: '',
    ListFile: [],
    Status: 1,
    SaleNoVat: '',
    StartDate: null,
    Index: 0,
    MechanicalMaker: '',
    ElectricMaker: '',
    ElectronicMaker: '',
    CurentVersion: 1,
    EditContent: '',
    BusinessDomain:'FAS',
    //Ảnh
    ListImage: [],
    SolutionTechnologies:[]
  }

  fileModel = {
    Id: '',
    SolutionId: '',
    Type: 0,
    Path: '',
    FileName: '',
    FileSize: '',
  }
  ngOnInit() {
    this.appSetting.PageTitle = "Thêm mới giải pháp";
    this.model.SBUBusinessId = JSON.parse(localStorage.getItem('qltkcurrentUser')).sbuId;
    this.model.DepartmentBusinessId = JSON.parse(localStorage.getItem('qltkcurrentUser')).departmentId;
    this.model.SBUSolutionMakerId = JSON.parse(localStorage.getItem('qltkcurrentUser')).sbuId;
    this.model.DepartmentSolutionMakerId = JSON.parse(localStorage.getItem('qltkcurrentUser')).departmentId;
    JSON.parse(localStorage.getItem('qltkcurrentUser'))
    this.fileProcess.FilesDataBase = [];
    this.getListSolutionGroup();
    this.getCbbSBU();
    this.getListCustomer();
    this.getListProject();
    this.getListDepartmentBusiness();
    this.getListDepartmentMaker();
    this.getListEmployeeBusiness();
    this.getSolutionTechnologies();
    this.getListJob();
    this.getApplication();

    this.getListEmployeeMaker();
    if (this.Id) {
      this.getSolutionInfo();
    }
    else {
      this.getSolutionCode();
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
  thumbnailActions: NgxGalleryOptions[];

  getSolutionCode() {
    this.service.getSolutionCode(this.model).subscribe(data => {
      this.model.Code = data.Code;
      this.model.Index = data.Index;
    }, error => {
      this.messageService.showError(error);
    });
  }

  getSolutionInfo() {
    this.service.getSolutionInfo({ Id: this.Id }).subscribe(data => {
      this.model = data;
    }, error => {
      this.messageService.showError(error);
    });
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

  getListDepartmentBusiness() {
    this.combobox.getCbbDepartmentBySBU(this.model.SBUBusinessId).subscribe(
      data => {
        this.listDepartmentBusiness = data;
        if (this.listDepartmentBusiness.length > 0) {
          this.model.DepartmentBusinessId = this.listDepartmentBusiness[0].Id;
          this.getListEmployeeBusiness();
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  getListJob(){
    this.combobox.getListJob().subscribe(
      data => {
        this.listJob = data;
       
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  getApplication(){
    this.combobox.getApplication().subscribe(
      data => {
        this.listApplication = data;
       
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  getListCustomerRequirementAndJob() {
    this.combobox.getCustomerRequirement(this.model.CustomerId).subscribe(
      data => {
        this.listCustomerRequirements = data;
        if (this.listCustomerRequirements.length > 0) {
          this.model.CustomerRequirementId = this.listCustomerRequirements[0].Id;
          this.model.CustomerRequirementCode = this.listCustomerRequirements[0].Code;
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  getCustomerRequirementCode(){
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

  getDomainById(){
    this.service.getDomainById(this.model.CustomerRequirementDomains).subscribe(
      data => {
        this.model.CustomerRequirementJob = data.Name;
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  getListDepartmentMaker() {
    this.combobox.getCbbDepartmentBySBU(this.model.SBUSolutionMakerId).subscribe(
      data => {
        this.listDepartmentMaker = data;
        if (this.listDepartmentMaker.length > 0) {
          this.model.DepartmentSolutionMakerId = this.listDepartmentMaker[0].Id;
          this.getListEmployeeMaker();
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  getListEmployeeBusiness() {
    this.combobox.getListUserWithDepartment(this.model.DepartmentBusinessId).subscribe(
      data => {
        this.listEmployeeBusiness = data;
        if (this.listEmployeeBusiness.length > 0) {
          this.model.BusinessUserId = this.listEmployeeBusiness[0].Id;
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
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

  getListEmployeeMaker() {
    this.combobox.getListUserWithDepartment(this.model.DepartmentSolutionMakerId).subscribe(
      data => {
        this.listEmployeesMaker = data;
        if (this.listEmployeesMaker.length > 0) {
          this.model.SolutionMaker = this.listEmployeesMaker[0].Id;
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
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


  supCreate(isContinue) {
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
    this.service.createSolution(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {
            Id: '',
            Name: '',
            Code: '',
            Description: '',
            CustomerId: '',
            EndCustomerId: '',
            Price: 0,
            FinishDate: null,
            SolutionGroupId: '',
            SBUBusinessId: null,
            DepartmentBusinessId: null,
            BusinessUserId: null,
            SBUSolutionMakerId: null,
            DepartmentSolutionMakerId: null,
            SolutionMaker: null,
            ProjectId: '',
            ListFile: [],
            Status: 1,
            SaleNoVat: '',
            StartDate: '',
            MechanicalMaker: '',
            ElectricMaker: '',
            ElectronicMaker: ''
          };
          this.listFile = [];
          this.listSBU = [];
          this.listDepartmentBusiness = [];
          this.listDepartmentMaker = [];
          this.listEmployeeBusiness = [];
          this.listEmployeesMaker = [];
          this.ListImage = [];
          this.galleryImages = [];

          forkJoin([
            this.combobox.getCbbSBU(),
            this.service.getSolutionCode(this.model)]
          ).subscribe(([res1, res2]) => {
            this.listSBU = res1;
            this.model.Index = res2.Index;
            this.model.Code = res2.Code;
          });
          this.finishDate = null;
          this.startDate = null;
          this.messageService.showSuccess('Thêm giải pháp thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm giải pháp thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  createSolution(isContinue) {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    var check = this.model.ListFile.filter(o => o.Type == 0 || o.Type == '');
    if (check.length > 0) {
      this.messageService.showMessage("Bạn chưa chọn loại tài liệu!");
      return;
    }
    this.model.ListFile = [];
    this.uploadService.uploadListFile(this.listFile, 'Solution/').subscribe((event: any) => {
      if (event.length > 0) {
        event.forEach(item => {
          var file = Object.assign({}, this.fileModel);
          file.FileName = item.FileName;
          file.FileSize = item.FileSize;
          file.Path = item.FileUrl;
          this.model.ListFile.push(file);
        });
      }
      if (validCode) {
        this.supCreate(isContinue);
      } else {
        this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
          data => {
            this.supCreate(isContinue);
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
    this.createSolution(isContinue);
  }

  saveAndContinue() {
    this.save(true);
  }

  uploadFileClick($event) {
    // this.fileProcess.onFileChange($event);
    // if (this.fileProcess.totalbyte > 5000000) {
    //   this.messageService.showMessage("Dung lượng tệp tin tải lên quá 5MB, hãy kiểm tra lại");
    //   this.fileProcess.FilesDataBase = [];
    //   this.fileProcess.totalbyte = 0;
    // }
    // else {
    //   this.listFile = this.fileProcess.FilesDataBase;
    // }
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    for (var file of fileDataSheet) {
      isExist = false;
      for (var ite of this.listFile) {
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

  updateFileAndReplaceManualDocument(fileManualDocuments, isReplace) {
    var isExist = false;

    for (var file of fileManualDocuments) {
      for (let index = 0; index < this.listFile.length; index++) {

        if (this.listFile[index].Id != null) {
          if (file.name == this.listFile[index].FileName) {
            isExist = true;
            if (isReplace) {
              this.listFile.splice(index, 1);
            }
          }
        }
        else if (file.name == this.listFile[index].name) {
          isExist = true;
          if (isReplace) {
            this.listFile.splice(index, 1);
          }
        }
      }

      if (!isExist || isReplace) {
        file.IsFileUpload = true;
        this.listFile.push(file);
      }
    }
  }

  updateFileManualDocument(fileManualDocuments) {
    for (var file of fileManualDocuments) {
      file.IsFileUpload = true;
      this.listFile.push(file);
    }
  }

  showConfirmDeleteFile(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa file đính kèm này không?").then(
      data => {
        this.listFile.splice(index, 1);
        this.messageService.showSuccess("Xóa file thành công!");
      },
      error => {
        
      }
    );
  }

  closeModal(result: boolean): void {
    this.router.navigate(['giai-phap/quan-ly-giai-phap/']);
  }

  ListImage = [];
  fileImage = {
    Id: '',
    ModuleId: '',
    FileName: '',
    FilePath: '',
    ThumbnailPath: '',
    Note: ''
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

  addTagFn(name) {
    return { Name: name, tag: true, IsNew: true };
  }
}
