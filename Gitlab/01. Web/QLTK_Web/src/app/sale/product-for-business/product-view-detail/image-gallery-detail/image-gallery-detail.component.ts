import { ChangeDetectorRef, Component, Input, OnInit, forwardRef } from '@angular/core';
import { ComboboxService, MessageService, Configuration, FileProcess, AppSetting, Constants, PermissionService, DateUtils } from 'src/app/shared';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from '@kolkov/ngx-gallery';
import { SaleProductService } from '../../sale-product.service';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-image-gallery-detail',
  templateUrl: './image-gallery-detail.component.html',
  styleUrls: ['./image-gallery-detail.component.scss']
})
export class ImageGalleryDetailComponent implements OnInit {
  @Input() Id: string;
  constructor(
    public config: Configuration,
    public fileProcessImage: FileProcess,
    public constants: Constants,
    public fileProcess: FileProcess,
    private messageService: MessageService,
    public dateUtils: DateUtils,
    public permissionService: PermissionService,
    public saleProductService: SaleProductService,
  ) { }
listImage:any[]=[];
listVideo:any[]=[];
listMedia:any[]=[];
model: any = {
  PageNumber: 1,
  PageSize: 20
}
  ngOnInit() {
    this.getMediaInfo(this.Id)
      }
      getMediaInfo(id){
        this.saleProductService.getMediaByProductId(id).subscribe((data: any) => {
          if (data) {
            data.forEach(element => {
              if(element.Type==2)
              {
                this.listImage.push(element)
              }
              if(element.Type==3)
              {
                this.listVideo.push(element)
              }
              if(element.Type!=1)
              {
                this.listMedia.push(element);
              }
            });
          }
        },
          error => {
            this.messageService.showError(error);
          });
      }

}
