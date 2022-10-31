import { Component, OnInit } from '@angular/core';
import { ErrorService } from 'src/app/project/service/error.service';
import { MessageService, Configuration } from 'src/app/shared';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from '@kolkov/ngx-gallery';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-show-image-error',
  templateUrl: './show-image-error.component.html',
  styleUrls: ['./show-image-error.component.scss']
})
export class ShowImageErrorComponent implements OnInit {

  constructor(
    private config: Configuration,
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private serviceError: ErrorService
  ) { }

  Id: string;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[] = [];

  model: any = {
    Id: ''
  }

  ngOnInit() {
    this.model.Id = this.Id;
    this.galleryOptions = [
      {
        width: '100%',
        thumbnailsColumns: 4,
        previewCloseOnClick: true,
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

  closeModal() {
    this.activeModal.close(true);
  }
}
