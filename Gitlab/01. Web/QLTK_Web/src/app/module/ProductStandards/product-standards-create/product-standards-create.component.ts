import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { MessageService, FileProcess, Constants, AppSetting, Configuration, ComboboxService } from 'src/app/shared';
import { ProductStandardsService } from '../../services/product-standards.service';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from '@kolkov/ngx-gallery';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-product-standards-create',
  templateUrl: './product-standards-create.component.html',
  styleUrls: ['./product-standards-create.component.scss'],
})
export class ProductStandardsCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private config: Configuration,
    private messageService: MessageService,
    private productstandar: ProductStandardsService,
    public fileProcess: FileProcess,
    private uploadService: UploadfileService,
    public constants: Constants,
    public appset: AppSetting,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private combobox: ComboboxService,
    public fileProcessImage: FileProcess,
    private routeA: ActivatedRoute,
    private router: Router,
  ) { }

  ModalInfo = {
    Title: 'Thêm mới tiêu chuẩn',
    SaveText: 'Lưu',
  };

  StartIndex = 1;
  isAction: boolean = false;
  Id: string;
  check: boolean = false;
  productStandardGroupId: string;
  listProductStandard: any[] = []
  listProductStandardGroup = [];
  listDepartment = [];
  listSBU = [];
  ListFile = [];
  DateNow = new Date();
  UserName = JSON.parse(localStorage.getItem('qltkcurrentUser')).userfullname;
  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }];

  model: any = {
    Id: '',
    ProductStandardGroupId: '',
    DepartmentId: '',
    SBUId: '',
    Code: '',
    Name: '',
    Content: '',
    Target: '',
    Version: '0',
    EditContent: '',
    DataType: 1,
    ListFile: [],
    ListImage: [],
    ListImageV: [],
  }



  fileModel = {
    Id: '',
    ProductStandardId: '',
    Path: '',
    FileName: '',
    FileSize: '',
    IsDelete: false
  }

  fileImage = {
    Id: '',
    ModuleId: '',
    FileName: '',
    FilePath: '',
    ThumbnailPath: '',
    Note: ''
  }
  ListImage = [];
  ListImageV = [];
  
  galleryImages: NgxGalleryImage[] = [];
  galleryOptions: NgxGalleryOptions[];

  galleryImagesV: NgxGalleryImage[] = [];
  galleryOptionsV: NgxGalleryOptions[];

  ngOnInit() {
    this.appset.PageTitle = "Thêm mới tiêu chuẩn sản phẩm";
    this.fileProcess.FilesDataBase = [];
    this.model.ProductStandardGroupId = this.productStandardGroupId;
    this.model.DepartmentId = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
    this.getSBUIdandDeepartmentId();
    this.getCbProductStandardGroup();

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

    this.galleryOptionsV = [
      {
        height: '350px',
        width: '100%',
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        thumbnailActions: [{ icon: 'fa fa-times-circle', onClick: this.deleteImageV.bind(this), titleText: 'Xoá ảnh' }]
      },
      // max-width 800
      {
        breakpoint: 800,
        width: '100%',
        imagePercent: 80,
        thumbnailsPercent: 20,
        thumbnailsMargin: 20,
        thumbnailMargin: 20,
        thumbnailActions: [{ icon: 'fa fa-times-circle', onClick: this.deleteImageV.bind(this), titleText: 'Xoá ảnh' }]
      },
      // max-width 400
      {
        breakpoint: 400,
        preview: false
      }
    ];
    this.getCbSBU();
    if (this.Id) {
      this.ModalInfo.SaveText = 'Lưu';
      this.getProductStandardInfo();
    }
    else {
      this.ModalInfo.Title = "Thêm mới tiêu chuẩn sản phẩm";
    }
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

  deleteImageV(event, index): void {
    let imageDelete = this.galleryImagesV[index].medium;
    let indexDelete;
    this.model.ListImageV.forEach(element => {
      let url = this.config.ServerFileApi + element.ThumbnailPath;
      if (url == imageDelete) {
        indexDelete = this.model.ListImageV.indexOf(element);
      }
    });
    this.galleryImagesV.splice(index, 1);
    this.model.ListImageV.splice(indexDelete, 1);
  }


  getProductStandardInfo() {
    this.productstandar.getProductStandard({ Id: this.Id }).subscribe(data => {
      this.model = data;
      this.appset.PageTitle = "Chỉnh sửa tiêu chuẩn sản phẩm - " + this.model.Code + " - " + this.model.Name;
      for (var item of data.ListImage) {
        this.galleryImages.push({
          small: this.config.ServerFileApi + item.ThumbnailPath,
          medium: this.config.ServerFileApi + item.FilePath,
          big: this.config.ServerFileApi + item.FilePath
        });
      }
      for (var item of data.ListImageV) {
        this.galleryImagesV.push({
          small: this.config.ServerFileApi + item.ThumbnailPath,
          medium: this.config.ServerFileApi + item.FilePath,
          big: this.config.ServerFileApi + item.FilePath
        });
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  getSBUIdandDeepartmentId() {
    this.productstandar.getSBUIdandDeepartmentId().subscribe(data => {
      this.model.SBUId = data.SBUId;
      this.model.DepartmentId = data.Id;
      this.getCbDepartment()
    }, error => {
      this.messageService.showError(error);
    });
  }

  getCbProductStandardGroup() {
    this.combobox.getCbbProductStandardGroup().subscribe(
      data => {
        this.listProductStandardGroup = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  clickSBU() {
    this.check = true;
    this.getCbDepartment();
  }

  getCbSBU() {
    this.combobox.getCbbSBU().subscribe(
      data => {
        this.listSBU = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getCbDepartment() {
    this.combobox.getCbbDepartmentBySBU(this.model.SBUId).subscribe(
      data => {
        this.listDepartment = data;
        if (this.check) {
          this.model.DepartmentId = this.listDepartment[0].Id;
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  createProductStandard(isContinue) {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    this.uploadService.uploadListFile(this.model.ListFile, 'ProductStandard/').subscribe((event: any) => {
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
        this.productstandar.createProductStandard(this.model).subscribe(
          data => {
            if (isContinue) {
              this.isAction = true;
              this.model = {};
              this.messageService.showSuccess('Thêm tiêu chuẩn sản phẩm thành công!');
              this.router.navigate(['module/quan-ly-tieu-chuan-san-pham']);
            }
            else {
              this.messageService.showSuccess('Thêm tiêu chuẩn sản phẩm thành công!');
              this.router.navigate(['module/quan-ly-tieu-chuan-san-pham']);
            }
          },
          error => {
            this.messageService.showError(error);
          }
        );
      } else {
        this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
          data => {
            this.productstandar.createProductStandard(this.model).subscribe(
              data => {
                if (isContinue) {
                  this.isAction = true;
                  this.model = {};
                  this.messageService.showSuccess('Thêm tiêu chuẩn sản phẩm thành công!');
                  this.router.navigate(['module/quan-ly-tieu-chuan-san-pham']);
                }
                else {
                  this.messageService.showSuccess('Thêm tiêu chuẩn sản phẩm thành công!');
                  this.router.navigate(['module/quan-ly-tieu-chuan-san-pham']);
                }
              },
              error => {
                this.messageService.showError(error);
              }
            );
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
      this.messageService.showMessage("Có lỗi trong quá trình xử lý");
    }
    else {
      this.createProductStandard(isContinue);
    }
  }

  saveAndContinue() {
    this.save(true);
  }
  listFiles = [];
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
  DownloadAFile(path: string) {
    if (!path) {
      this.messageService.showError("Không có file để tải");
      return;
    }
    this.productstandar.DownloadAFile({ Path: path }).subscribe(() => {
      var link = document.createElement('a');
      link.setAttribute("type", "hidden");
      link.href = this.config.ServerApi + path;
      link.download = 'Download.zip';
      document.body.appendChild(link);
      // link.focus();
      link.click();
      document.body.removeChild(link);
    }, error => {
      this.messageService.showError(error);
    });
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

  closeModal(isOK: boolean) {
    this.router.navigate(['module/quan-ly-tieu-chuan-san-pham']);
  }

  uploadFileClickImage($event) {
    this.ListImage = [];
    var fileImage = this.fileProcessImage.getFileOnFileChange($event);
    for (var item of fileImage) {
      this.ListImage.push(item);
    }

    this.uploadService.uploadListFile(this.ListImage, 'ImageModule/').subscribe((event: any) => {
      if (event.length > 0) {
        event.forEach(item => {
          var file = Object.assign({}, this.fileImage);
          file.FilePath = item.FileUrl;
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

  uploadFileClickImageV($event) {
    this.ListImageV = [];
    var fileImageV = this.fileProcessImage.getFileOnFileChange($event);
    for (var item of fileImageV) {
      this.ListImageV.push(item);
    }

    this.uploadService.uploadListFile(this.ListImageV, 'ImageModule/').subscribe((event: any) => {
      if (event.length > 0) {
        event.forEach(item => {
          var file = Object.assign({}, this.fileImage);
          file.FilePath = item.FileUrl;
          file.ThumbnailPath = item.FileUrlThum;
          this.model.ListImageV.push(file);
          this.galleryImagesV.push({
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
