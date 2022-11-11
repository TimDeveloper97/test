import { ChangeDetectorRef, Component, Input, OnInit, forwardRef } from '@angular/core';
import { ComboboxService, MessageService, Configuration, FileProcess, AppSetting, Constants, PermissionService, DateUtils } from 'src/app/shared';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from '@kolkov/ngx-gallery';
import { UploadImageVideoComponent } from '../upload-image-video/upload-image-video.component';
import { SaleProductService } from '../sale-product.service';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-image-gallery-manage',
  templateUrl: './image-gallery-manage.component.html',
  styleUrls: ['./image-gallery-manage.component.scss'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => ImageGalleryManageComponent),
    multi: true
  }
  ],
})
export class ImageGalleryManageComponent implements OnInit {
  @Input() Id: string;
  public _listMedia;
  get listMedia(): any {
    return this._listMedia;
  }
  @Input()
  set listMedia(val: any) {
    this._listMedia = val;
  }

  constructor(
    private comboboxService: ComboboxService,
    private messageService: MessageService,
    public config: Configuration,
    public fileProcessImage: FileProcess,
    private uploadService: UploadfileService,
    private appSetting: AppSetting,
    public constants: Constants,
    private router: Router,
    private routeA: ActivatedRoute,
    public fileProcess: FileProcess,
    private modalService: NgbModal,
    public dateUtils: DateUtils,
    public permissionService: PermissionService,
    private _cd: ChangeDetectorRef,
    public saleProductService: SaleProductService,
  ) { }
  @Input()
  get items() { return this._items };
  set items(value: any[]) {
    this.listMedia = value;
  };

  _items = [];
  private _onChange = (_: any) => { };
  private _onTouched = () => { };

  writeValue(value: any | any[]): void {
    if (value != null) {
      this.listMedia = value;
    } else {
      this.listMedia = [];
    }

    this._cd.markForCheck();
  }

  registerOnChange(fn: any): void {
    this._onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this._onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this._cd.markForCheck();
  }

  model: any = {
    PageNumber: 1,
    PageSize: 20
  }
  listImage: any[] = [];
  listVideo: any[] = [];
  ngOnInit() {
    this.getMediaInfo(this.Id)
  }

  getMediaInfo(id) {
    this.saleProductService.getMediaByProductId(id).subscribe((data: any) => {
      if (data) {
        data.forEach(element => {
          if (element.Type == 2) {
            this.listImage.push(element)
          }
          if (element.Type == 3) {
            this.listVideo.push(element)
          }
          if (element.Type != 1) {
            this.listMedia.push(element);
          }
        });
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  showFromUpload() {
    let activeModal = this.modalService.open(UploadImageVideoComponent, { container: 'body', windowClass: 'upload-image-model', backdrop: 'static' })
    activeModal.result.then((result) => {
      if (result) {
        if (result != true) {
          result.forEach(element => {
            this.listMedia.push(element);
            this._onChange(this.listMedia);
            if (element.Type == 2) {
              this.listImage.push(element);
            }
            else {
              this.listVideo.push(element);
            }
          });
        }
      }
    }, (reason) => {
    });
  }

  download(file: any) {
    this.fileProcess.downloadFileBlob(file.Path, file.FileName);
  }
}
