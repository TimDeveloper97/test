import { Component, OnInit, ViewChild, ElementRef, ViewEncapsulation } from '@angular/core';
import { MessageService, FileProcess, Configuration } from 'src/app/shared';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { MaterialService } from '../../services/material-service';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmOverwriteModalComponent } from '../confirm-overwrite-modal/confirm-overwrite-modal.component';

@Component({
  selector: 'app-import-material-buy-history-modal',
  templateUrl: './import-material-buy-history-modal.component.html',
  styleUrls: ['./import-material-buy-history-modal.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ImportMaterialBuyHistoryModalComponent implements OnInit {

  ModalInfo = {
    Title: 'Import lịch sử giá vật tư'
  }

  importModel: any = {
    Path: ''
  }
  nameFile = "";
  fileToUpload: File = null;
  fileTemplate = this.config.ServerApi + 'Template/Lịch sử giá mua_Template.xlsx';

  @ViewChild('fileInput',{static:false})
  myInputVariable: ElementRef;

  constructor(
    private messageService: MessageService,
    private uploadService: UploadfileService,
    private materialService: MaterialService,
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
    this.uploadService.uploadFile(this.fileToUpload, 'MaterialBuyHistory/').subscribe(
      data => {
        if (data != null) {
          this.importModel.Link = data.FullFileUrl;
          if (this.importModel.Link != '') {
            this.materialService.importFile(this.importModel).subscribe((event: any) => {
              if (event.Message == null) {
                this.messageService.showSuccess("Import lịch sử giá vật tư thành công!");
                this.activeModal.close(true);
              }
              else {
                this.messageService.showMessage(event.Message);
                this.nameFile = '';
              }

            }, error => {
              this.messageService.showMessage(error.error);
              this.nameFile = '';
            });
          }
        } else {
          this.messageService.showMessage(data.mess);
          this.nameFile = '';
        }
      },
      error => {
        this.messageService.showMessage(error);
        this.nameFile = '';
      }
    );
  }

  save() {
    if (this.myInputVariable.nativeElement.value == "") {
      this.messageService.showMessage("Bạn chưa chọn file import!");
    }
    else {
      this.uploadFile();
      this.myInputVariable.nativeElement.value = "";
    }

  }

  closeModal() {
    this.activeModal.close(true);
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
