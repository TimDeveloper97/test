import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { MessageService, Configuration, FileProcess } from 'src/app/shared';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { CustomerService } from '../../service/customer.service';


@Component({
  selector: 'app-import-file',
  templateUrl: './import-file.component.html',
  styleUrls: ['./import-file.component.scss']
})
export class ImportFileComponent implements OnInit {

  ModalInfo = {
    Title: 'Import khách hàng'
  }

  importModel: any = {
    Path: ''
  }
  nameFile = "";
  fileToUpload: File = null;
  fileTemplate = this.config.ServerApi + 'Template/Khách hàng_Template.xls';

  @ViewChild('fileInput',{static:false})
  myInputVariable: ElementRef;

  constructor(
    private messageService: MessageService,
    private uploadService: UploadfileService,
    private customerService: CustomerService,
    private modalService: NgbModal,
    private activeModal: NgbActiveModal,
    private config: Configuration,
    public fileProcess: FileProcess,
  ) { }
  isAction: boolean = false;
  ngOnInit() {
  }

  handleFileInput(files: FileList) {
    this.fileToUpload = files.item(0);
    this.nameFile = files.item(0).name;
  }

  uploadFile() {
    this.uploadService.uploadFile(this.fileToUpload, 'Customer/').subscribe(
      data => {
        if (data != null) {
          this.importModel.Link = data.FullFileUrl;
          if (this.importModel.Link != '') {
            this.customerService.importFile(this.importModel).subscribe((event: any) => {
              if (event.Message == null) {
                this.messageService.showSuccess("Import khách hàng thành công!");
                this.closeModal(true);
              }
              else {
                this.messageService.showMessage(event.Message);
              }

            }, error => {
              this.messageService.showMessage(error.error);
            });
          }
        } else {
          this.messageService.showMessage(data.mess);
        }
      },
      error => {
        this.messageService.showMessage(error);
      }
    );
  }

  save() {
    if (this.myInputVariable.nativeElement.value == "") {
      this.messageService.showMessage("Bạn chưa chọn file import!");
    }
    var check = this.myInputVariable.nativeElement.value;
    var testcheck1 = ".xls";
    var testcheck2 = ".xlsx";
    if (check.lastIndexOf(testcheck1) == -1 && check.lastIndexOf(testcheck2) == -1) {
      this.messageService.showMessage("Bạn phải chọn file có định dạng .xls hoặc .xlsx");
    }
    else {
      this.uploadFile();
      this.myInputVariable.nativeElement.value = "";
    }

  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  dowloadFileTemplate() {
    var link = document.createElement('a');
    link.setAttribute("type", "hidden");
    link.href = this.fileTemplate;
    link.download = 'Download.zip';
    document.body.appendChild(link);
    // link.focus();
    link.click();
    document.body.removeChild(link);
  }

}
