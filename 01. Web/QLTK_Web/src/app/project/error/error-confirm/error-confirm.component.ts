import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from '@kolkov/ngx-gallery';

import { MessageService, Configuration, FileProcess, AppSetting, DateUtils, Constants, ComboboxService } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { ErrorService } from '../../service/error.service';

@Component({
  selector: 'app-error-confirm',
  templateUrl: './error-confirm.component.html',
  styleUrls: ['./error-confirm.component.scss']
})
export class ErrorConfirmComponent implements OnInit {

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
    public constants: Constants,
    public fileProcess: FileProcess,
  ) { }

  Id: string;
  planStartDate: any;
  planFinishDate: any;
  actualStartDate: any;
  actualFinishDate: any;
  listErrorGroup: any[] = [];
  listDepartment: any[] = [];
  listDepartmentProcess: any[] = [];
  listEmployee: any[] = [];
  listEmployees: any[] = [];
  listEmployeeFixBy: any[] = [];
  listProject: any[] = [];
  listModule: any[] = [];
  listStage: any[] = [];
  listFile: any[] = [];
  listHistory: any[] = [];
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[] = [];
  listImage = [];
  columnModule: any[] = [{ Name: 'Code', Title: 'Mã Module' }, { Name: 'Name', Title: 'Tên Module' }];
  columnName: any[] = [{ Name: 'Code', Title: 'Mã vấn đề' }, { Name: 'Name', Title: 'Tên vấn đề' }];
  columnProject: any[] = [{ Name: 'Code', Title: 'Mã dự án' }, { Name: 'Name', Title: 'Tên dự án' }];
  columnType: any[] = [{ Name: 'Name', Title: 'Tên loại vấn đề' }];
  columnDeparterment: any[] = [{ Name: 'Code', Title: 'Mã bộ phận' }, { Name: 'Name', Title: 'Tên bộ phận' }];
  columnEmployee: any[] = [{ Name: 'Code', Title: 'Mã nhân viên' }, { Name: 'Name', Title: 'Tên nhân viên' }];
  columnStage: any[] = [{ Name: 'Code', Title: 'Mã công đoạn' }, { Name: 'Name', Title: 'Tên công đoạn' }];
  model: any = {
    Id: '',
    Subject: '',
    Code: '',
    Description: '',
    ErrorGroupId: '',
    ErrorGroupName: '',
    DepartmentId: '',
    DepartmentName: '',
    AuthorId: '',
    AuthorName: '',
    ErrorBy: '',
    PlanStartDate: '',
    PlanFinishDate: '',
    ObjectId: '',
    ModuleErrorVisualId: '',
    DepartmentProcessId: '',
    StageId: '',
    FixBy: '',
    ProjectId: '',
    ProjectName: '',
    Status: '',
    Solution: '',
    Note: '',
    ErrorCost: '',
    ActualStartDate: '',
    ActualFinishDate: '',
    strHistory: '',
    Price: 0,
    ListImage: [],
    ListFile: [],
  }

  moduleModel: any = {
    ProjectId: '',
  }

  fileImage = {
    Id: '',
    MaterialId: '',
    Path: '',
    ThumbnailPath: ''
  }

  fileModel = {
    Id: '',
    ErrorId: '',
    Path: '',
    FileName: '',
    FileSize: '',
    IsDelete: false
  }

  ngOnInit() {
    this.appSetting.PageTitle = "Xác nhận lỗi";
    this.model.Id = this.routeA.snapshot.paramMap.get('Id');
    this.getCbbDepartment();
    this.getCbbDepartmentProcess();
    this.getListErrorGroup();
    this.getListProject();
    this.getCbbStage();
    this.getListEmployee();
    this.galleryOptions = [
      {
        height: '400px',
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
      this.getCbbEmployee(this.model.DepartmentId);
      this.getCbbEmployeeFixBy(this.model.DepartmentProcessId);
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
      if (data.ActualStartDate != null) {
        let dateArray2 = data.ActualStartDate.split('T')[0];
        let dateValue2 = dateArray2.split('-');
        let tempDateFromV2 = {
          'day': parseInt(dateValue2[2]),
          'month': parseInt(dateValue2[1]),
          'year': parseInt(dateValue2[0])
        };
        this.actualStartDate = tempDateFromV2;
      }
      if (data.ActualFinishDate != null) {
        let dateArray3 = data.ActualFinishDate.split('T')[0];
        let dateValue3 = dateArray3.split('-');
        let tempDateFromV3 = {
          'day': parseInt(dateValue3[2]),
          'month': parseInt(dateValue3[1]),
          'year': parseInt(dateValue3[0])
        };
        this.actualFinishDate = tempDateFromV3;
      }
      if (data.PlanFinishDate != null) {
        let dateArray4 = data.PlanFinishDate.split('T')[0];
        let dateValue4 = dateArray4.split('-');
        let tempDateFromV4 = {
          'day': parseInt(dateValue4[2]),
          'month': parseInt(dateValue4[1]),
          'year': parseInt(dateValue4[0])
        };
        this.planFinishDate = tempDateFromV4;
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

  getCbbDepartmentProcess() {
    this.combobox.getCbbDepartment().subscribe(
      data => {
        this.listDepartmentProcess = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getListEmployee() {
    this.combobox.getCbbEmployee().subscribe(
      data => {
        this.listEmployees = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getCbbEmployee(DepartmentId: string) {
    this.combobox.getEmployeeByDepartment(DepartmentId).subscribe(
      data => {
        this.listEmployee = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getCbbEmployeeFixBy(DepartmentProcessId: string) {
    this.combobox.getEmployeeByDepartment(DepartmentProcessId).subscribe(
      data => {
        this.listEmployeeFixBy = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

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
    this.moduleModel.ProjectId = ProjectId;
    this.serviceError.searchModule(this.moduleModel).subscribe((data: any) => {
      if (data) {
        this.listModule = data;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  getCbbStage() {
    this.combobox.getCbbStage().subscribe(
      data => {
        this.listStage = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  startConfirm() {
    this.model.Status = 3;
    this.model.strHistory = "Đang xử lý";
    this.updateError();
  }

  save() {
    this.updateError();
  }

  warning() {
    if (this.model.ErrorBy == "" || this.model.ErrorBy == null) {
      this.messageService.showMessage("Bạn không được để trống người gây lỗi");
    } else if (this.model.StageId == "" || this.model.StageId == null) {
      this.messageService.showMessage("Bạn không được để trống công đoạn");
    } else if (this.model.DepartmentProcessId == "" || this.model.DepartmentProcessId == null) {
      this.messageService.showMessage("Bạn không được để trống phòng ban khắc phục");
    } else if (this.model.FixBy == "" || this.model.FixBy == null) {
      this.messageService.showMessage("Bạn không được để trống người khắc phục");
    } else if (this.model.Note == "" || this.model.Note == null) {
      this.messageService.showMessage("Bạn không được để trống nguyên nhân");
    } else if (this.model.Solution == "" || this.model.Solution == null) {
      this.messageService.showMessage("Bạn không được để trống giải pháp");
    } else {
      this.model.Status = 4;
      this.model.strHistory = "Hoàn thành sửa lỗi"
      this.updateError();
    }
  }

  success() {
    if (this.model.ActualFinishDate == "" || this.model.ActualFinishDate == null) {
      this.messageService.showMessage("Bạn không được để trống ngày QC");
    }
    else {
      this.model.Status = 5;
      this.model.strHistory = "Xác nhận hoàn thành lỗi"
      this.updateError();
    }
  }

  qc() {
    if (this.actualFinishDate) {
      this.messageService.showMessage("Bạn không được để trống ngày QC");
    } else {
      this.model.Status = 3;
      this.model.strHistory = "Vẫn còn lỗi"
      this.updateError();
    }
  }

  updateError() {
    this.uploadService.uploadListFile(this.listFile, 'Error/').subscribe((event: any) => {
      if (event.length > 0) {
        var count = 0;
        this.model.ListFile.forEach(item => {
          if (item.Path == '') {
            item.Path = event[count].FileUrl;
            count++;
          }
        });
      }
      if (this.planStartDate) {
        this.model.PlanStartDate = this.dateUtils.convertObjectToDate(this.planStartDate);
      }
      if (this.planFinishDate) {
        this.model.planFinishDate = this.dateUtils.convertObjectToDate(this.planFinishDate);
      }
      if (this.actualStartDate) {
        this.model.ActualStartDate = this.dateUtils.convertObjectToDate(this.actualStartDate);
      }
      if (this.actualFinishDate) {
        this.model.ActualFinishDate = this.dateUtils.convertObjectToDate(this.actualFinishDate);
      }

      this.serviceError.updateError(this.model).subscribe(
        () => {
          this.messageService.showSuccess('Cập nhật lỗi thành công!');
        },
        error => {
          this.messageService.showError(error);
        });
    }, error => {
      this.messageService.showError(error);
    });
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

  uploadFileClick($event) {
    this.fileProcess.onFileChange($event);
    for (var element of this.fileProcess.FilesDataBase) {
      var a = 0;
      for (var item of this.model.ListFile) {
        if (item.FileName == element.name) {
          var b = a;
          this.messageService.showConfirm("File đã tồn tại. Bạn có muốn ghi đè lên không").then(
            data => {
              this.model.ListFile.splice(b, 1);
            },
            error => {
              
            });
        }
        a++;
      }
    }
    if (this.fileProcess.totalbyte > 5000000) {
      this.messageService.showMessage("Dung lượng tệp tin tải lên quá 5MB, hãy kiểm tra lại");
      this.fileProcess.FilesDataBase = [];
      this.fileProcess.totalbyte = 0;
    }
    else {
      var file = Object.assign({}, this.fileModel);
      file.FileName = this.fileProcess.FilesDataBase[this.fileProcess.FilesDataBase.length - 1].name;
      file.FileSize = this.fileProcess.FilesDataBase[this.fileProcess.FilesDataBase.length - 1].size;
      this.model.ListFile.push(file);
    }
    this.listFile = this.fileProcess.FilesDataBase;
    this.fileProcess.FilesDataBase = [];
  }

  showConfirmDeleteFile(index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa bằng chứng khắc phục này không?").then(
      data => {
        this.model.ListFile.splice(index, 1);
        this.messageService.showSuccess("Xóa file thành công!");
      },
      error => {
        
      }
    );
  }

  downloadAFile(path: string) {
    if (!path) {
      this.messageService.showError("Không có file để tải");
      return;
    }
    var link = document.createElement('a');
    link.setAttribute("type", "hidden");
    link.href = this.config.ServerFileApi + path;
    link.download = "aaaaa";
    document.body.appendChild(link);
    // link.focus();
    link.click();
    document.body.removeChild(link);
  }

}
