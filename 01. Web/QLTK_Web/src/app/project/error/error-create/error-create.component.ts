import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from '@kolkov/ngx-gallery';

import { MessageService, Configuration, FileProcess, AppSetting, DateUtils, ComboboxService } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { ErrorService } from '../../service/error.service';

@Component({
  selector: 'app-error-create',
  templateUrl: './error-create.component.html',
  styleUrls: ['./error-create.component.scss']
})
export class ErrorCreateComponent implements OnInit {

  constructor(
    private combobox: ComboboxService,
    private messageService: MessageService,
    private config: Configuration,
    public fileProcessImage: FileProcess,
    private uploadService: UploadfileService,
    private appSetting: AppSetting,
    public dateUtils: DateUtils,
    private serviceError: ErrorService,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private router: Router,
    private routeA: ActivatedRoute,
  ) { }

  Id: string;
  planStartDate: any;
  planFinishDate: any;
  listErrorGroup: any[] = [];
  listDepartment: any[] = [];
  listEmployee: any[] = [];
  listProject: any[] = [];
  listModule: any[] = [];
  listStage: any[] = [];
  columnModule: any[] = [{ Name: 'Code', Title: 'Mã Module' }, { Name: 'Name', Title: 'Tên Module' }];
  columnName: any[] = [{ Name: 'Code', Title: 'Mã vấn đề' }, { Name: 'Name', Title: 'Tên vấn đề' }];
  columnProject: any[] = [{ Name: 'Code', Title: 'Mã dự án' }, { Name: 'Name', Title: 'Tên dự án' }];
  columnType: any[] = [{Name: 'Name', Title: 'Tên loại vấn đề' }];
  columnDeparterment: any[] = [{ Name: 'Code', Title: 'Mã bộ phận' }, { Name: 'Name', Title: 'Tên bộ phận' }];
  columnEmployee: any[] = [{ Name: 'Code', Title: 'Mã nhân viên' }, { Name: 'Name', Title: 'Tên nhân viên' }];
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[] = [];
  listImage = [];
  model: any = {
    Id: '',
    Subject: '',
    Code: '',
    Description: '',
    ErrorGroupId: '',
    DepartmentId: '',
    AuthorId: '',
    ErrorBy: '',
    PlanStartDate: '',
    PlanFinishDate: '',
    ObjectId: '',
    ModuleErrorVisualId: '',
    DepartmentProcessId: '',
    StageId: '',
    FixBy: '',
    ProjectId: '',
    Status: '',
    strHistory: '',
    Type: 1,
    Price: 0,
    ListImage: [],
  }

  fileImage = {
    Id: '',
    MaterialId: '',
    Path: '',
    ThumbnailPath: ''
  }

  ngOnInit() {
    this.model.Id = this.routeA.snapshot.paramMap.get('Id');
    this.getCbbDepartment();
    this.getListErrorGroup();
    this.getListProject();
    this.getCbbEmployee();
    this.galleryOptions = [
      {
        width: '100%',
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide
      },
      // max-width 800
      {
        breakpoint: 800,
        width: '100%',
        imagePercent: 80,
        thumbnailsPercent: 20,
        thumbnailsMargin: 20,
        thumbnailMargin: 20
      },
      // max-width 400
      {
        breakpoint: 400,
        preview: false
      }
    ];
    if (this.model.Id) {
      this.getErrorInfo();
    }
  }

  getErrorInfo() {
    this.serviceError.getErrorInfo(this.model).subscribe(data => {
      this.model = data;
      //this.getCbbEmployee(this.model.DepartmentId);
      this.getCbbModule(this.model.ProjectId);
      if (data.PlanStartDate != null) {
        let dateArray1 = data.PlanStartDate.split('T')[0];
        let dateValue1 = dateArray1.split('-');
        let tempDateFromV1 = {
          'day': parseInt(dateValue1[2]),
          'month': parseInt(dateValue1[1]),
          'year': parseInt(dateValue1[0])
        };
        this.planStartDate = tempDateFromV1;
      }
      if (data.PlanFinishDate != null) {
        let dateArray2 = data.PlanFinishDate.split('T')[0];
        let dateValue2 = dateArray2.split('-');
        let tempDateFromV2 = {
          'day': parseInt(dateValue2[2]),
          'month': parseInt(dateValue2[1]),
          'year': parseInt(dateValue2[0])
        };
        this.planFinishDate = tempDateFromV2;
      }
      for (var item of data.ListImage) {
        this.galleryImages.push({
          small: this.config.ServerFileApi + item.ThumbnailPath,
          medium: this.config.ServerFileApi + item.Path,
          big: this.config.ServerFileApi + item.Path
        });
      }
    }, error => {
      this.messageService.showError(error);
    });
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

  // getCbbEmployee(DepartmentId: string) {
  //   this.combobox.getEmployeeByDepartment(DepartmentId).subscribe(
  //     data => {
  //       this.listEmployee = data;
  //     },
  //     error => {
  //       this.messageService.showError(error);
  //     }
  //   );
  // }

  getListErrorGroup() {
    this.combobox.getListErrorGroup().subscribe(
      data => {
        this.listErrorGroup = data;
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

  getCbbModule(ProjectId: string) {
    this.model.ProjectId = ProjectId;
    this.serviceError.searchModule(this.model).subscribe((data: any) => {
      if (data) {
        this.listModule = data;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  // getListModule() {
  //   this.combobox.getListModule().subscribe(
  //     data => {
  //       this.listModule = data;
  //     },
  //     error => {
  //       this.messageService.showError(error);
  //     }
  //   );
  // }

  save(isContinue: boolean) {
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      if (this.model.Id == '') {
        this.createError(isContinue);
      } else {
        this.model.strHistory = "Cập nhật lỗi";
        this.updateError();
      }
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          if (this.model.Id == null) {
            this.createError(isContinue);
          } else {
            this.model.strHistory = "Cập nhật lỗi";
            this.updateError();
          }
        },
        error => {
          
        });
    }
  }

  request() {
    if (this.model.ProjectId == "") {
      this.messageService.showMessage("Bạn không được để trống dự án");
    }
    else if (this.model.ObjectId == "") {
      this.messageService.showMessage("Bạn không được để trống module");
    }
    else if (this.model.DepartmentId == "") {
      this.messageService.showMessage("Bạn không được để trống phòng ban gây lỗi");
    }
    else if (this.model.AuthorId == "") {
      this.messageService.showMessage("Bạn không được để trống người gây lỗi");
    }
    else {
      this.model.Status = 2;
      this.model.strHistory = "Yêu cầu xác nhận"
      this.updateError();
    }
  }

  showConfirm(Id: string) {
    this.router.navigate(['du-an/quan-ly-loi/xac-nhan-loi/', Id]);
  }

  createError(isContinue) {
    if (this.planStartDate) {
      this.model.PlanStartDate = this.dateUtils.convertObjectToDate(this.planStartDate);
    }
    if (this.planFinishDate) {
      this.model.PlanFinishDate = this.dateUtils.convertObjectToDate(this.planFinishDate);
    }
    this.model.strHistory = "Tạo lỗi"
    this.serviceError.createError(this.model).subscribe(
      data => {
        if (isContinue) {
          this.model = {
            ListImage: [],
          };
          this.planStartDate = '';
          this.planFinishDate = '';
          this.listImage = [];
          this.galleryImages = [];
          this.messageService.showSuccess('Thêm mới lỗi thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới lỗi thành công!');
          this.closeModal();
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  updateError() {
    if (this.planStartDate) {
      this.model.PlanStartDate = this.dateUtils.convertObjectToDate(this.planStartDate);
    }
    if (this.planFinishDate) {
      this.model.PlanFinishDate = this.dateUtils.convertObjectToDate(this.planFinishDate);
    } else { this.model.PlanFinishDate = null; }

    this.serviceError.updateError(this.model).subscribe(
      () => {
        this.closeModal();
        this.messageService.showSuccess('Cập nhật lỗi thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  closeModal() {
    this.router.navigate(['du-an/quan-ly-loi']);
  }

  uploadFileClickImage($event) {
    this.listImage = [];
    var fileImage = this.fileProcessImage.getFileOnFileChange($event);
    for (var item of fileImage) {
      this.listImage.push(item);
    }

    this.uploadService.uploadListFile(this.listImage, 'Error/').subscribe((event: any) => {
      if (event.length > 0) {
        event.forEach(item => {
          var file = Object.assign({}, this.fileImage);
          file.Path = item.FileUrl;
          file.ThumbnailPath = item.FileUrlThum;
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
}
