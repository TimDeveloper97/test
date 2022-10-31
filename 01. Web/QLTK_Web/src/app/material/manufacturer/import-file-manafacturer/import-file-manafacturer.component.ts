import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';

import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { MessageService, Configuration, FileProcess } from 'src/app/shared';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { ManufactureService } from '../../services/manufacture-service';

@Component({
  selector: 'app-import-file-manafacturer',
  templateUrl: './import-file-manafacturer.component.html',
  styleUrls: ['./import-file-manafacturer.component.scss']
})
export class ImportFileManafacturerComponent implements OnInit {

  modalInfo = {
    Title: 'Import hãng sản xuất'
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
    private manufacturerService: ManufactureService,
    private modalService: NgbModal,
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
            this.manufacturerService.importFile(this.importModel).subscribe((event: any) => {
              if (event.Message == null) {
                this.messageService.showSuccess("Import hãng sản xuất thành công!");
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
    this.manufacturerService.getGroupInTemplate().subscribe(d => {
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
