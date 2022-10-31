import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { MessageService, Configuration, FileProcess } from 'src/app/shared';
import { SupplierService } from '../../services/supplier-service';
import { UploadfileService } from 'src/app/upload/uploadfile.service';

@Component({
  selector: 'app-import-supplier',
  templateUrl: './import-supplier.component.html',
  styleUrls: ['./import-supplier.component.scss']
})
export class ImportSupplierComponent implements OnInit {

  modalInfo = {
    Title: 'Import nhà cung cấp'
  }

  importModel: any = {
    Path: ''
  }
  nameFile = "";
  fileToUpload: File = null;

  @ViewChild('fileInput',{static:false})
  myInputVariable: ElementRef;

  constructor(
    private messageService: MessageService,
    private uploadService: UploadfileService,
    private service: SupplierService,
    private activeModal: NgbActiveModal,
    private config: Configuration,
    public fileProcess: FileProcess,
  ) { }

  ngOnInit() {
  }

  handleFileInput(files: FileList) {
    this.fileToUpload = files.item(0);
    this.nameFile = files.item(0).name;
  }

  uploadFile() {
    this.uploadService.uploadFile(this.fileToUpload, 'ManufacturerFileImport/').subscribe(
      data => {
        if (data != null) {
          this.importModel.Link = data.FullFileUrl;
          if (this.importModel.Link != '') {
            this.service.importFile(this.importModel).subscribe((event: any) => {
              if (event.Message == null) {
                this.messageService.showSuccess("Import nhà cung cấp thành công!");
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

  closeModal() {
    this.activeModal.close(true);
  }

  downloadTemplate() {
    this.service.getGroupInTemplate().subscribe(d => {
      var link = document.createElement('a');
      link.setAttribute("type", "hidden");
      link.href = this.config.ServerApi + d;
      link.download = 'Download.docx';
      document.body.appendChild(link);
      // link.focus();
      link.click();
      document.body.removeChild(link);
    }, e => {
      this.messageService.showError(e);
    });
  }
}
