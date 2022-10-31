import { Component, OnInit, ViewEncapsulation } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { Configuration, MessageService, AppSetting, FileProcess, Constants, ComboboxService } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { UploadfileService } from 'src/app/upload/uploadfile.service';

import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from '@kolkov/ngx-gallery';
import { ActivatedRoute } from '@angular/router';
import { ProductStandardsService } from 'src/app/module/services/product-standards.service';

@Component({
  selector: 'app-show-qc-check-list',
  templateUrl: './show-qc-check-list.component.html',
  styleUrls: ['./show-qc-check-list.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ShowQcCheckListComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private config: Configuration,
    private messageService: MessageService,
    private productstandar: ProductStandardsService,
    public fileProcess: FileProcess,
    private uploadService: UploadfileService,
    public constants: Constants,
    public appset: AppSetting,
    public fileProcessImage: FileProcess,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private routeA: ActivatedRoute,
    private combobox: ComboboxService
  ) { }

  ModalInfo = {
    Title: 'Xem chi tiết tiêu chuẩn',
    SaveText: 'Lưu',
  };
  StartIndex = 1;
  isAction: boolean = false;
  Id: string;
  listProductStandard: any[] = []
  listProductStandardGroup = [];
  listDepartment = [];
  listSBU = [];
  ListFile: any[] = [];
  ListHistory: any[] = [];
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
    Version: '',
    EditContent: '',
    DataType: 1,
    ListFile: [],
    ListHistory: []
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
    this.fileProcess.FilesDataBase = [];
    this.getCbProductStandardGroup();
    this.getCbSBU();
    if (this.Id) {
      this.appset.PageTitle = 'Xem tiêu chuẩn sản phẩm';
      this.getProductStandardInfo();
    }
    else {
      this.appset.PageTitle = "Thêm mới tiêu chuẩn sản phẩm";
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
    this.productstandar.getShowQCProductStandard({ Id: this.Id }).subscribe(data => {
      this.model = data;
      
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
      this.getCbDepartment();
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
      },
      error => {
        this.messageService.showError(error);
      }
    );
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

  DownloadAFile(path: string, fileName: string) {
    if (!path) {
      this.messageService.showError("Không có file để tải");
      return;
    }
    this.fileProcess.downloadFileBlob(path, fileName)
    // var link = document.createElement('a');
    // link.setAttribute("type", "hidden");
    // link.href = this.config.ServerFileApi + path;
    // link.download = "aaaaa";
    // document.body.appendChild(link);
    // link.focus();
    // link.click();
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
    this.activeModal.close(isOK ? isOK : this.isAction);
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

  downloadAFile(file: { FilePath: any; FileName: any; }) {
    this.fileProcess.downloadFileBlob(file.FilePath, file.FileName);
  }

}
