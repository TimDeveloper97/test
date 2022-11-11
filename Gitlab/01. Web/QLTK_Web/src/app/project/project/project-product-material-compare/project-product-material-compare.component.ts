import { Component, OnInit, ViewChild, ElementRef, ViewEncapsulation } from '@angular/core';
import { MessageService, Configuration, FileProcess, ComboboxService, Constants } from 'src/app/shared';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { ProjectProductBomService } from '../../service/project-product-bom.service';


@Component({
  selector: 'app-project-product-material-compare',
  templateUrl: './project-product-material-compare.component.html',
  styleUrls: ['./project-product-material-compare.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ProjectProductMaterialCompareComponent implements OnInit {

  modalInfo = {
    Title: 'Những thay đổi về dữ liệu vật tư',
    SaveText: 'Lưu',
  };

  totalAmount : number =0;
  listData : any[];
  listData1 : any[];
  height =0;
  isAction: boolean = false;
  constructor(
    public constant: Constants,
    private messageService: MessageService,
    private activeModal: NgbActiveModal,
    private config: Configuration,
    private uploadService: UploadfileService,
    public fileProcess: FileProcess,
    private service: ProjectProductBomService,
    private combobox: ComboboxService,
    private modalService: NgbModal
  ) { }

  @ViewChild('scrollPlanDesignProject',{static:false}) scrollPlanDesignProject: ElementRef;
  @ViewChild('scrollPlanDesignProject1',{static:false}) scrollPlanDesignProject1: ElementRef;

  @ViewChild('scrollPlanDesignProjectHeader',{static:false}) scrollPlanDesignProjectHeader: ElementRef;
  @ViewChild('scrollPlanDesignProjectHeader1',{static:false}) scrollPlanDesignProjectHeader1: ElementRef;

  ngOnInit(): void {
    this.listData;
    this.listData1;
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }
  ngAfterViewInit() {    

    if (this.scrollPlanDesignProject && this.scrollPlanDesignProject.nativeElement) {
      this.scrollPlanDesignProject.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
        this.scrollPlanDesignProjectHeader.nativeElement.scrollLeft = event.target.scrollLeft;
      }, true);
    }
    if (this.scrollPlanDesignProject1 && this.scrollPlanDesignProject1.nativeElement) {
      this.scrollPlanDesignProject1.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
        this.scrollPlanDesignProjectHeader1.nativeElement.scrollLeft = event.target.scrollLeft;
      }, true);
    }
  }

  ngOnDestroy() {
    if (this.scrollPlanDesignProject && this.scrollPlanDesignProject.nativeElement) {
      this.scrollPlanDesignProject.nativeElement.removeEventListener('ps-scroll-x', null);
    }
    if (this.scrollPlanDesignProject1 && this.scrollPlanDesignProject1.nativeElement) {
      this.scrollPlanDesignProject1.nativeElement.removeEventListener('ps-scroll-x', null);
    }
  }

}
