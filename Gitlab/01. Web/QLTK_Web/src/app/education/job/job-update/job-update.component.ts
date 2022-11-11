import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Title } from '@angular/platform-browser';

import { AppSetting, Configuration, FileProcess, MessageService, Constants, ComboboxService } from 'src/app/shared';

import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { JobServiceService } from '../../service/job-service.service';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { ListSubjectsJobChooseComponent } from '../list-subjects-job-choose/list-subjects-job-choose.component';
import { ProductService } from 'src/app/device/services/product.service';

@Component({
  selector: 'app-job-update',
  templateUrl: './job-update.component.html',
  styleUrls: ['./job-update.component.scss']
})
export class JobUpdateComponent implements OnInit {

  @ViewChild('fileInput', { static: false }) fileInput: ElementRef;
  @ViewChild('scrollPracticeHeader', { static: false }) scrollPracticeHeader: ElementRef;
  @ViewChild('scrollPracticeMaterialHeader', { static: false }) scrollPracticeMaterialHeader: ElementRef;

  constructor(
    public appSetting: AppSetting,
    private router: Router,
    private config: Configuration,
    public fileProcess: FileProcess,
    private productService: ProductService,
    private messageService: MessageService,
    private modalService: NgbModal,
    private routeA: ActivatedRoute,
    private uploadService: UploadfileService,
    private jobService: JobServiceService,
    private comboboxService: ComboboxService,
    public constant: Constants,
    public activeModal: NgbActiveModal,
  ) { }

  startIndex = 1;
  pagination;
  lstpageSize = [5, 10, 15, 20, 25, 30];
  listJob: any[] = [];
  listGroupJob = [];
  listDegree = [];
  FilesDataBase: any[] = [];
  listFile: any[] = [];
  SubjectName: string;
  logUserId: string;
  Id: string;

  scrollConfig: PerfectScrollbarConfigInterface = {
    suppressScrollX: false,
    suppressScrollY: true,
    minScrollbarLength: 20,
    wheelPropagation: true
  };
  StartIndex = 1;
  selectIndex = -1;
  productIndex = -1;
  model: any = {
    page: 1,
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    Name: '',
    Code: '',
    Description: '',
    Documents: '',
    DegreeId: '',
    JobGroupId: '',
    SubjectId: '',
    JobId: '',
    SubjectName: '',
    ListJobSubject: [],
    ListJobAttach: [],
    ListSubject: [],
    ListFile: [],
    ListClassRoom: [],
    ListClassRoomProductModel :[],
    ListModuleProduct :[]
  }
  ListFile = [];
  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }];
  columnNameDegree: any[] = [{ Name: 'Code', Title: 'Mã trình độ' }, { Name: 'Name', Title: 'Tên trình độ' }];
  fileModel = {
    Id: '',
    Path: '',
    FileName: '',
    FileSize: '',
    IsDelete: false
  }
  totalAmount = 0;
  MaxLeadTimeModule = 0;
  TotalModuleNoPrice = 0;
  TotalModuleNoLeadTime = 0;

  listModule: any[];
  ListProduct: any[];
  listBase: any = [];
  listSelect: any = [];
  IsRequest: boolean;
  JobModel: string;
  listSubjectJob: any[] = [];
  listSubject: any[] = [];
  selectSubjectIndex = -1;
  ListClassRoom : any[] =[];
  ListClassRoomProduct : any[] =[];
  ListModuleProductModel : any[] =[];
  indexSubject =-1;
  indexClassRoom =-1;
  indexProduct =-1;


  @ViewChild('scrollModuleHeader', { static: false }) scrollModuleHeader: ElementRef;
  @ViewChild('scrollModule', { static: false }) scrollModule: ElementRef;
  @ViewChild('scrollModule1', { static: false }) scrollModule1: ElementRef;
  @ViewChild('scrollModuleHeader1', { static: false }) scrollModuleHeader1: ElementRef;
  @ViewChild('scrollModule2', { static: false }) scrollModule2: ElementRef;
  @ViewChild('scrollModuleHeader2', { static: false }) scrollModuleHeader2: ElementRef;




  // ngOnDestroy() {
  //   this.scrollModule.nativeElement.removeEventListener('ps-scroll-x', null);
  // }
  ngOnInit() {
    this.appSetting.PageTitle = "Chỉnh sửa nghề";
    this.getListDegree();
    this.getListGroupJob();
    this.model.JobId = this.routeA.snapshot.paramMap.get('Id');
    if (this.model.JobId) {
      this.getJobInfo();
    }

  }
  ngAfterContentInit() {

  }

  ngAfterViewInit() {
    this.scrollModule.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollModuleHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
    this.scrollModule1.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollModuleHeader1.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
    this.scrollModule2.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollModuleHeader2.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }


  getListGroupJob() {
    this.comboboxService.getCBBListGroupJob().subscribe(
      data => {
        this.listGroupJob = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getListDegree() {
    this.comboboxService.getListDegree().subscribe(
      data => {
        this.listDegree = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
  // danh mục môn học công nghệ
  //Hiển thì popup chọn 
  showSelectTestCeiteria(IsRequest) {
    let activeModal = this.modalService.open(ListSubjectsJobChooseComponent, { container: 'body', windowClass: 'listsubjectsjobchoose-model', backdrop: 'static' });
    var ListIdSelect = [];
    this.listSubject.forEach(element => {
      ListIdSelect.push(element.Id);
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.listSubject.push(element);
        });
      }
    }, (reason) => {

    });
  }

  showConfirmDeleteSubject(model: any) {
    this.messageService.showConfirm("Bạn có chắc muốn xóa môn học này không?").then(
      data => {
        this.deleteSubject(model);
      },
      error => {

      }
    );
  }

  deleteSubject(model) {
    var index = this.listSubject.indexOf(model);
    if (index > -1) {
      this.listSubject.splice(index, 1);
    }
  }

  getJobInfo() {
    this.jobService.getJobInfor({ Id: this.model.JobId }).subscribe(data => {
      this.model = data;
      this.listSubject = this.model.ListSubject;
      this.model.ListFile = this.model.ListJobAttach;
      this.ListClassRoom = this.model.ListClassRoom;
      this.ListClassRoomProduct = this.model.ListClassRoomProductModel;
      this.ListModuleProductModel = this.model.ListModuleProduct;
      this.totalAmount = 0;
      this.ListModuleProductModel.forEach(mod => {
      this.totalAmount += mod.Price * mod.Qty;
    });
    });
  }
  getListSubjectInfo() {
    this.jobService.GetJobInfo({ Id: this.model.JobId }).subscribe(
      data => {
        this.JobModel = data.Id;
        this.model.Id = data.Id;
        this.searchSubject();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  searchSubject() {
    this.jobService.SearchSubject({ Id: this.model.JobId }).subscribe(data => {
      this.listSubject = data.ListResult;
    }, error => {
      this.messageService.showError(error);
    })
  }
  // danh mục tài liệu

  uploadFileClick($event) {
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


  save() {
    this.model.CreateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
    var listFileUpload = [];
    this.model.ListFile.forEach((document, index) => {
      if (document.IsFileUpload) {
        listFileUpload.push(document);
      }
    });
    this.uploadService.uploadListFile(this.model.ListFile, 'JobAttach/').subscribe((event: any) => {
      if (event.length > 0) {
        event.forEach((item, index) => {
          var file = Object.assign({}, this.fileModel);
          file.FileName = item.FileName;
          file.FileSize = item.FileSize;
          file.Path = item.FileUrl;
          this.model.ListFile.push(file);
        });
      }
      for (var item of this.model.ListFile) {
        if (item.Path == null || item.Path == "") {
          this.model.ListFile.splice(this.model.ListFile.indexOf(item), 1);
        }
      }
      this.model.ListJobSubject = this.listSubject;
      this.jobService.update(this.model).subscribe((event: any) => {
        this.messageService.showSuccess("Cập nhật nghề thành công!");
        this.close(true);
      }, error => {
        this.messageService.showError(error);
      });
    }, error => {
      this.messageService.showError(error);
    });

  }

  DownloadAFile(file) {
    this.fileProcess.downloadFileBlob(file.Path, file.FileName);
  }
  close(isOK: boolean) {
    this.router.navigate(['phong-hoc/quan-ly-nghe']);
  }
  selectError(index) {
    this.indexClassRoom = index;
    if (this.selectIndex != index) {
      this.selectIndex = index;
      this.ListProduct = [];
      var classRoom = this.ListClassRoom;
      var Id = classRoom[index].Id;
      var model = {
        Id: Id,
      }
      this.jobService.getProductClassInfo(model).subscribe(
        data => {
        this.ListClassRoomProduct = data.ListClassRoomProductModel;
        this.ListModuleProductModel = data.ListModuleProduct;
        this.totalAmount = 0;
        data.ListModuleProduct.forEach(mod => {
          this.totalAmount += mod.Price * mod.Qty;
        });
        },
        error => {
          this.messageService.showError(error);
        }
      );
    }
    else {
      this.selectIndex = -1;
      this.indexClassRoom =-1;
      this.ListClassRoomProduct = this.model.ListClassRoomProductModel;
      this.ListModuleProductModel = this.model.ListModuleProduct;
      this.totalAmount = 0;
      this.model.ListModuleProduct.forEach(mod => {
          this.totalAmount += mod.Price * mod.Qty;
        });
      if(this.indexSubject != -1){
        this.selectSubject(this.indexSubject);
      }
    }
  }
  selectProduct(index) {
    this.indexProduct = index;
    if (this.productIndex != index) {
      this.productIndex = index;
      this.listModule = [];
      var product = this.ListClassRoomProduct;
      var Id = product[index].Id;
      var model = {
        Id: Id,
      }
      this.productService.getModuleByProductId(model).subscribe(data => {
        this.ListModuleProductModel = data;
        this.listModule = this.ListModuleProductModel;
        // if (this.ListModuleProductModel.length > 0) {
        //   this.ListModuleProductModel.forEach(element => {
        //     listModuleId.push(element.ModuleId);
        //   });
        //   this.getModulePrice(listModuleId);
        // }
        // this.getMaxQuantityModuleByPractice();
        this.changeMaxLeadTimeModule();
        // this.calculateTotalAmount();
        this.totalAmount = 0;
        this.ListModuleProductModel.forEach(mod => {
            this.totalAmount += mod.Price * mod.Qty;
          });
      },
        error => {
          this.messageService.showError(error);
        }
      );
    }
    else {
      this.productIndex = -1;
      this.ListModuleProductModel = this.model.ListModuleProduct;
      this.totalAmount = 0;
      this.model.ListModuleProduct.forEach(mod => {
          this.totalAmount += mod.Price * mod.Qty;
        });
      if(this.indexClassRoom != -1){
        this.selectError(this.indexClassRoom);
      }
    }
  }
  getModulePrice(listModuleNewId: any[]) {
    this.productService.getModulePrice(listModuleNewId).subscribe(
      data => {
        if (data && data.length > 0) {
          this.listModule.forEach(mod => {
            data.forEach(element => {
              if (element.ModuleId == mod.ModuleId) {
                mod.Price = element.Price;
                mod.IsNoPrice = element.IsNoPrice;
              }
            });
          });

          this.calculateTotalAmount();
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
  calculateTotalAmount() {
    this.totalAmount = 0;
    this.listModule.forEach(mod => {
      this.totalAmount += mod.Price * mod.Qty;
    });
  }
  changeMaxLeadTimeModule() {
    if (this.listModule.length > 0) {
      this.MaxLeadTimeModule = Math.max.apply(Math, this.listModule.map(function (o) { return o.LeadTime; }));
    } else {
      this.MaxLeadTimeModule = 0;
    }

    this.TotalModuleNoPrice = this.listModule.filter(d => d.IsNoPrice).length;
    this.TotalModuleNoLeadTime = this.listModule.filter(d => d.LeadTime == 0 || !d.LeadTime).length;
  }

  selectSubject(index) {
    this.indexSubject = index;
    if (this.selectSubjectIndex != index) {
      this.selectSubjectIndex = index;
      this.ListClassRoom = [];
      var subjects = this.model.ListSubject;
      var Id = subjects[index].Id;
      this.jobService.getClassBySubjectId(Id).subscribe(data => {
        this.ListClassRoom = data.ListClassRoom;
        this.ListClassRoomProduct = data.ListClassRoomProductModel;
        this.ListModuleProductModel = data.ListModuleProduct;
        this.totalAmount = 0;
        data.ListModuleProduct.forEach(mod => {
          this.totalAmount += mod.Price * mod.Qty;
        });
      },
        error => {
          this.messageService.showError(error);
        }
      );
    }
    else {
      this.selectSubjectIndex = -1;
      this.indexSubject =-1;
      this.ListClassRoom = this.model.ListClassRoom;
      this.ListClassRoomProduct = this.model.ListClassRoomProductModel;
      this.ListModuleProductModel = this.model.ListModuleProduct;
      this.totalAmount = 0;
      this.model.ListModuleProduct.forEach(mod => {
          this.totalAmount += mod.Price * mod.Qty;
        });
    }
  }
}
