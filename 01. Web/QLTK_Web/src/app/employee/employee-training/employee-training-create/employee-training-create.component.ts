import { Component, OnInit, ViewEncapsulation, ElementRef, ViewChild, OnDestroy, AfterViewInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, Constants, DateUtils, FileProcess } from 'src/app/shared';
import { EmployeeTrainingService } from '../../service/employee-training.service';
import { ChooseCourseComponent } from '../choose-course/choose-course.component';
import { ChooseEmployeeComponent } from '../choose-employee/choose-employee.component';
import { forkJoin } from 'rxjs';
import { UploadfileService } from 'src/app/upload/uploadfile.service';

@Component({
  selector: 'app-employee-training-create',
  templateUrl: './employee-training-create.component.html',
  styleUrls: ['./employee-training-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class EmployeeTrainingCreateComponent implements OnInit, OnDestroy, AfterViewInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private service: EmployeeTrainingService,
    private modalService: NgbModal,
    public constant: Constants,
    public dateUntil: DateUtils,
    public fileProcess: FileProcess,
    private uploadService: UploadfileService,
  ) { }

  modalInfo = {
    Title: 'Thêm mới chương trình đào tạo',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  Id: string;
  courseId: string;
  employeeId: string;
  listWorkSkill: any[] = [];
  listEmployeeByCouserId = [];
  selectIndexSkill = -1;

  model: any = {
    Id: null,
    Name: '',
    Description: '',
    ListCourse: [],
    ListAttachs :[]
  }

  modelUpdatePoint: any = {
    EmployeeId: '',
    CourseId: '',
    CourseTrainingId: '',
    ListWorkSkill: [],
    CheckSave: false,
  }

  modelCourse: any = {
    Id: '',
    EmployeeId: '',
    ListEmployees: [],
    IsDelete: false
  }

  employeeModel: any = {
    Id: '',
    EmployeeId: '',
    Code: '',
    Name: '',
    IsDelete: false,
    DepartmentName: '',
    SBUName: ''
  }
  totalEmployee = 0;
  workSkills = [];
  Attachs: any[]=[];

  fileTemplate: any = {
    Id: '',
    Note: null,
    FilePath: null,
    FileName: null,
    FileSize: 0,
    UploadDate: new Date(),
    UploadName: ''
  };


  @ViewChild('scrollHeaderEmployee', { static: false }) scrollHeaderEmployee: ElementRef;
  @ViewChild('scrollEmployee', { static: false }) scrollEmployee: ElementRef;
  @ViewChild('scrollHeaderCourse', { static: false }) scrollHeaderCourse: ElementRef;
  @ViewChild('scrollCourse', { static: false }) scrollCourse: ElementRef;
  @ViewChild('scrollHeaderSkill', { static: false }) scrollHeaderSkill: ElementRef;
  @ViewChild('scrollSkill', { static: false }) scrollSkill: ElementRef;

  ngOnInit() {
    this.model.Id = this.Id;
    if (this.Id) {
      this.modalInfo.Title = 'Chỉnh sửa chương trình đào tạo';
      this.modalInfo.SaveText = 'Lưu';
      this.getEmployeeTrainingInfo();
    }
    else {
      this.modalInfo.Title = "Thêm mới chương trình đào tạo";
    }

  }

  ngAfterViewInit() {
    this.scrollEmployee.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollHeaderEmployee.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);

    this.scrollCourse.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollHeaderCourse.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
    this.scrollSkill.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollHeaderSkill.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }

  ngOnDestroy() {
    this.scrollCourse.nativeElement.removeEventListener('ps-scroll-x', null);
    this.scrollCourse.nativeElement.removeEventListener('ps-scroll-x', null);
  }

  getEmployeeTrainingInfo() {
    this.service.getEmployeeTrainingInfo({ Id: this.Id }).subscribe(data => {
      this.model = data;
      this.Attachs = data.ListAttachs;
      this.model.ListCourse.forEach(element => {
        element.StartDateV = this.dateUntil.convertDateToObject(element.StartDate);
        element.EndDateV = this.dateUntil.convertDateToObject(element.EndDate);
      });
    }, error => {
      this.messageService.showError(error);
    });
  }

  save(isContinue: boolean) {
      var listFileUpload = [];
      this.Attachs.forEach((document, index) => {
        if (document.File) {
          document.File.index = index;
          listFileUpload.push(document.File);
        }
      });

      if (listFileUpload.length > 0 ) {
        let fileAttachs = this.uploadService.uploadListFile(listFileUpload, 'EmployeeTraining/');
        forkJoin([fileAttachs]).subscribe(results => {
          var count =0;
          let data1 = results[0];
          if (data1 && data1.length > 0) {
            count++;
            listFileUpload.forEach((item, index) => {
              this.Attachs[item.index].FilePath = data1[index].FileUrl;
            });
          }
          if (this.Id) {
            this.updateEmployeeTraining();
            this.updatePointEmployee(true);
          }
          else {
            this.createEmployeeTraining(isContinue);
          }
        });
      }else{
        if (this.Id) {
          this.updateEmployeeTraining();
          this.updatePointEmployee(true);
        }
        else {
          this.createEmployeeTraining(isContinue);
        }
      }
  }

  saveAndContinue() {
    this.save(true);
  }

  createEmployeeTraining(isContinue) {
    this.model.ListAttachs = this.Attachs;
    this.model.ListCourse.forEach(element => {
      if (element.StartDateV != null && element.StartDateV != "") {
        element.StartDate = this.dateUntil.convertObjectToDate(element.StartDateV);
      }
      if (element.EndDateV != null && element.EndDateV != "") {
        element.EndDate = this.dateUntil.convertObjectToDate(element.EndDateV);
      }
    });
    this.service.addEmployeeTraining(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {};
          this.messageService.showSuccess('Thêm mới chương trình đào tạo thành công!');
          this.selectCouserChange(null, -1);
          this.selectEmployeeChange(null, -1);
          this.totalEmployee = 0;
        }
        else {
          this.messageService.showSuccess('Thêm mới chương trình đào tạo thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  updateEmployeeTraining() {
    this.model.ListAttachs = this.Attachs;
    this.model.ListCourse.forEach(element => {
      if (element.StartDateV != null && element.StartDateV != "") {
        element.StartDate = this.dateUntil.convertObjectToDate(element.StartDateV);
      }
      if (element.EndDateV != null && element.EndDateV != "") {
        element.EndDate = this.dateUntil.convertObjectToDate(element.EndDateV);
      }
    });
    this.service.updateEmployeeTraining(this.model).subscribe(
      data => {
        this.messageService.showSuccess('Cập nhật chương trình đào tạo thành công!');
        this.getEmployeeTrainingInfo();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  updatePointEmployee(check) {
    this.modelUpdatePoint.CourseId = this.courseId;
    this.modelUpdatePoint.CourseTrainingId = this.model.ListCourse[this.selectIndex].Id;
    this.modelUpdatePoint.ListWorkSkill = this.workSkills;
    if(check == true){
      this.modelUpdatePoint.CheckSave =true;
    }else{
      this.modelUpdatePoint.CheckSave =false;
    }
    this.modelUpdatePoint.EmployeeId = this.employeeId;
    this.modelUpdatePoint.EmployeeCourseTrainingId = this.model.ListCourse[this.selectIndex].ListEmployees[this.selectIndexSkill].Id;
    this.service.updatePointEmployee(this.modelUpdatePoint).subscribe(
      data => {
        if(check ==false){
          this.messageService.showSuccess('Cập nhật điểm cho nhân viên thành công!');
        }
        this.model.ListCourse[this.selectIndex].ListEmployees[this.selectIndexSkill].Status = 1;
        this.model.ListCourse[this.selectIndex].Status = data.CourseStatus;
        this.getWorkKillEndEmployee(this.employeeId);
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  selectIndex = -1;
  courseEmployeeTraningId: string;
  selectCouserChange(param, index) {
    if (param == null && index == -1) {
      this.workSkills = [];
      this.totalEmployee = 0;
      this.selectIndex = index;
    } else {
      if (this.selectIndex != index) {
        this.selectIndex = index;
        this.courseId = param.CourseId;
        this.selectIndexSkill = -1;
        this.totalEmployee = 0;
        this.workSkills = [];

        if (!param.ListEmployees || param.ListEmployees.length == 0) {
          param.ListEmployees = [];
          this.service.getEmployeeByCourseId(this.courseId).subscribe(data => {
            param.ListEmployees = data;
            this.totalEmployee = param.ListEmployees.length;
          }, error => {
            this.messageService.showError(error);
          });
        }
        else {
          this.totalEmployee = param.ListEmployees.length;
        }
      }
    }
  }

  selectEmployeeChange(param, index) {
    if (param == null && index == -1) {
      this.employeeId == null;
      this.selectIndexSkill = index;
      this.workSkills = [];
    } else {
      if (this.selectIndexSkill != index) {
        this.selectIndexSkill = index;
        this.employeeId = param.EmployeeId;

        this.getWorkKillEndEmployee(param.EmployeeId);
      }
    }

  }

  getWorkKillEndEmployee(employeeId: string) {
    this.service.getWorkKillEndEmployee({ EmployeeId: employeeId, CourseId: this.courseId }).subscribe(
      data => {
        this.workSkills = data;
      },
      error => {
        this.messageService.showError(error);
        this.workSkills = [];
      }
    );
  }

  showConfirmDeleteCourse(index) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá khóa học này không?").then(
      data => {
        if (!this.model.ListCourse[index].Id) {
          this.model.ListCourse.splice(index, 1);
        } else {
          this.model.ListCourse[index].IsDelete = true;
        }
        this.selectIndex--;
        this.totalEmployee = 0;
        if (this.selectIndex >= 0) {
          this.courseId = this.model.ListCourse[this.selectIndex].CourseId;
          if (!this.model.ListCourse[this.selectIndex].ListEmployees || this.model.ListCourse[this.selectIndex].ListEmployees.length == 0) {
            this.model.ListCourse[this.selectIndex].ListEmployees = [];
            this.service.getEmployeeByCourseId(this.courseId).subscribe(data => {
              this.model.ListCourse[this.selectIndex].ListEmployees = data;
              this.totalEmployee = this.model.ListCourse[this.selectIndex].length;
            }, error => {
              this.messageService.showError(error);
            });
          }
          else {
            this.totalEmployee = this.model.ListCourse[this.selectIndex].ListEmployees.length;
          }
        }
        this.selectIndexSkill = -1;
        this.employeeId = null;
      },
      error => {

      }
    );
  }

  showConfirmDeleteEmployee(index) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nhân viên này không?").then(
      data => {
        if (!this.model.ListCourse[this.selectIndex].ListEmployees[index].Id) {
          this.model.ListCourse[this.selectIndex].ListEmployees.splice(index, 1);
        } else {
          this.model.ListCourse[this.selectIndex].ListEmployees[index].IsDelete = true;
        }
        this.selectIndexSkill = -1;
        this.employeeId = null;
        var total =0;
        this.model.ListCourse[this.selectIndex].ListEmployees.forEach( element => {
          if(element.IsDelete == false){
            total++;
          }
        });
        this.totalEmployee =total;
      },
      error => {

      }
    );
  }

  showChooseCourse() {
    let activeModal = this.modalService.open(ChooseCourseComponent, { container: 'body', windowClass: 'choose-course', backdrop: 'static' });
    var ListIdSelect = [];
    this.model.ListCourse.forEach(element => {
      ListIdSelect.push(element.CourseId);
    });
    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        ListIdSelect = [];
        result.forEach(element => {
          element.StartDateV = this.dateUntil.getDateToObject(element.StartDate);
          element.EndDateV = this.dateUntil.getDateToObject(element.EndDate);
          element.Status = 0;
          element.IsDelete = false;
          this.model.ListCourse.push(element);
          ListIdSelect.push(element.CourseId);
        });

        this.getEmployeeByCourse(ListIdSelect);
      }
    }, (reason) => {

    });
  }

  getEmployeeByCourse(courseIds) {
    this.service.getEmployeeByCourse(courseIds).subscribe(
      data => {
        this.model.ListCourse.forEach(course => {
          if (data[course.CourseId]) {
            course.ListEmployees = data[course.CourseId];
          }
        });

      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  showEmployeeSkill() {
    if (this.selectIndex < 0) {
      this.messageService.showMessage("Bạn chưa chọn khóa học!");
      return;
    }
    let activeModal = this.modalService.open(ChooseEmployeeComponent, { container: 'body', windowClass: 'choose-employee', backdrop: 'static' });
    var listIdSelect = [];
    this.model.ListCourse[this.selectIndex].ListEmployees.forEach(element => {
      listIdSelect.push(element.EmployeeId);
    });

    activeModal.componentInstance.ListIdSelect = listIdSelect;
    activeModal.componentInstance.courseId = this.courseId;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          var value = Object.assign({}, this.employeeModel);
          value.Id = element.Id;
          value.EmployeeId = element.EmployeeId;
          value.Code = element.Code;
          value.Name = element.Name;
          value.SBUName = element.SBUName;
          value.DepartmentName = element.DepartmentName;
          value.Status = 0;
          this.model.ListCourse[this.selectIndex].ListEmployees.push(value);
        });

        var total =0;
        this.model.ListCourse[this.selectIndex].ListEmployees.forEach( element => {
          if(element.IsDelete == false){
            total++;
          }
        });
        this.totalEmployee =total;      }
    }, (reason) => {

    });
  }
  uploadFile($event: any) {
    var fileDataSheet = this.fileProcess.getFileOnFileChange($event);
    var isExist = false;
    var list =this.Attachs;
    for (var file of fileDataSheet) {
      isExist = false;
      if (list != null) {
        for (var ite of list) {
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
    let docuemntTemplate;
      for (var file of fileManualDocuments) {
        for (let index = 0; index < this.Attachs.length; index++) {

          if (this.Attachs[index].Id != null) {
            if (file.name == this.Attachs[index].FileName) {
              isExist = true;
              if (isReplace) {
                this.Attachs.splice(index, 1);
              }
            }
          }
          else if (file.name == this.Attachs[index].name) {
            isExist = true;
            if (isReplace) {
              this.Attachs.splice(index, 1);
            }
          }
        }

        if (!isExist || isReplace) {
          docuemntTemplate = Object.assign({}, this.fileTemplate);
          docuemntTemplate.File = file;
          docuemntTemplate.FileName = file.name;
          docuemntTemplate.FileSize = file.size;
          this.model.ListAttach.push(docuemntTemplate);
        }
    }

  }

  updateFileManualDocument(files) {
    let documentTemplate;
    for (var file of files) {
      documentTemplate = Object.assign({}, this.fileTemplate);
      documentTemplate.File = file;
      documentTemplate.FileName = file.name;
      documentTemplate.FileSize = file.size;
      documentTemplate.CreateDate = new Date();
      this.Attachs.push(documentTemplate);
  }


  }

  downloadAFile(file) {
    this.fileProcess.downloadFileBlob(file.FilePath, file.FileName);
  }

  showConfirmDeleteFile(document, index) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá tài liệu này không?").then(
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
      this.Attachs.splice(index, 1);
    }
  }
}
