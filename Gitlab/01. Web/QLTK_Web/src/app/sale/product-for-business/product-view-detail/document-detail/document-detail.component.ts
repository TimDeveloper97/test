import { ChangeDetectorRef, Component, Input, OnInit, forwardRef } from '@angular/core';
import { Constants, MessageService, FileProcess } from 'src/app/shared';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { SaleProductService } from '../../sale-product.service';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-document-detail',
  templateUrl: './document-detail.component.html',
  styleUrls: ['./document-detail.component.scss']
})
export class DocumentDetailComponent implements OnInit {
  @Input() Id: string;
  constructor(
    public constants: Constants,
    private messageService: MessageService,
    public fileProcess: FileProcess,
    private uploadService: UploadfileService,
    private _cd: ChangeDetectorRef,
    public saleProductService: SaleProductService,
  ) { }

  ngOnInit() {
    this.getDocumentInfo(this.Id);
  }
  listFileSolution:any[]=[];
  listCatalog:any[]=[];
  listTechnicalTraning:any[]=[];
  listSaleTraining:any[]=[];
  listUserManual:any[]=[];
  listFixError:any[]=[];
  listOtherDocument:any[]=[];
  getDocumentInfo(id) {
    this.saleProductService.getDocumentByProductId(id).subscribe((data: any) => {
      if (data) {
        data.forEach(element => {
          if (element.Type == 1) {
            this.listFileSolution.push(element)
          }
          if (element.Type == 2) {
            this.listCatalog.push(element)
          }
          if (element.Type == 3) {
            this.listTechnicalTraning.push(element)
          }
          if (element.Type == 4) {
            this.listSaleTraining.push(element)
          }
          if (element.Type == 5) {
            this.listUserManual.push(element)
          }
          if (element.Type == 6) {
            this.listFixError.push(element)
          }
          if (element.Type == 7) {
            this.listOtherDocument.push(element)
          }
        });
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }
}
