import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MessageService, Configuration, FileProcess } from 'src/app/shared';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MaterialService } from 'src/app/material/services/material-service';
import { EmployeeServiceService } from '../../service/employee-service.service';

@Component({
  selector: 'app-import-employee',
  templateUrl: './import-employee.component.html',
  styleUrls: ['./import-employee.component.scss']
})
export class ImportEmployeeComponent implements OnInit {

  modalInfo = {
    Title: 'Import nhân viên'
  }

  importModel: any = {
    Path: ''
  }
  nameFile = "";
  fileToUpload: File = null;
  fileTemplate = this.config.ServerApi + 'Template/Import_Nhân viên_Template.xls';

  @ViewChild('fileInput')
  myInputVariable: ElementRef;

  constructor(
    private messageService: MessageService,
    private uploadService: UploadfileService,
    private service: EmployeeServiceService,
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
    this.uploadService.uploadFile(this.fileToUpload, 'Employee/').subscribe(
      data => {
        if (data != null) {
          this.importModel.Link = data.FullFileUrl;
          if (this.importModel.Link != '') {
            this.service.importFileEmployee(this.importModel).subscribe((event: any) => {
              if (event.Message == null) {
                this.messageService.showSuccess("Import nhân viên thành công!");
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
