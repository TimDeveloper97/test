import { Component, OnInit, ViewEncapsulation, ElementRef, ViewChild } from '@angular/core';
import { MessageService, Configuration, FileProcess, Constants } from 'src/app/shared';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { MaterialService } from 'src/app/material/services/material-service';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ModuleSketchesService } from '../../services/module-sketches-service';
import { HistoryVersionComponent } from 'src/app/shared/component/history-version/history-version.component';
import { HistoryVersionService } from 'src/app/shared/services/history-version.service';

@Component({
  selector: 'app-sketches-import-material',
  templateUrl: './sketches-import-material.component.html',
  styleUrls: ['./sketches-import-material.component.scss'],
  encapsulation: ViewEncapsulation.None
})

export class SketchesImportMaterialComponent implements OnInit {

  ModalInfo = {
    Title: 'Import vật tư',
  }

  ModuleId: string;

  importModel: any = {
    Path: ''
  }

  listSketchMaterialElectronic: any[];
  listSketchMaterialMechanical: any[];
  nameFile = "";
  fileToUpload: File = null;
  fileTemplate = this.config.ServerApi + 'Template/importvattu_phacthaothietke_template.xlsm';

  @ViewChild('fileInput',{static:false})
  myInputVariable: ElementRef;

  constructor(
    private messageService: MessageService,
    private uploadService: UploadfileService,
    private sketchesService: ModuleSketchesService,
    private modalService: NgbModal,
    private activeModal: NgbActiveModal,
    private config: Configuration,
    public fileProcess: FileProcess,
    public constant: Constants,
    private serviceHistory: HistoryVersionService
  ) { }

  modelSketchMaterialElectronic: any = {
    Id: '',
    ModuleId: '',
    Note: '',
    Quantity: '',
    MaterialName: '',
    MaterialId: '',
    Leadtime: '',
  }

  ngOnInit() {
    this.importModel.ModuleId = this.ModuleId;
    // this.searchSketchesMaterialElectronic();
  }

  handleFileInput(files: FileList) {
    this.fileToUpload = files.item(0);
    this.nameFile = files.item(0).name;
  }

  uploadFile() {
    this.uploadService.uploadFile(this.fileToUpload, 'SketchMaterial/').subscribe(
      data => {
        if (data != null) {
          this.importModel.Link = data.FullFileUrl;
          if (this.importModel.Link != '') {
            this.sketchesService.importFile(this.importModel).subscribe((event: any) => {
              if (!event.Message && (!event.RowMaterialEmpty || event.RowMaterialEmpty.length == 0)) {
                this.messageService.showSuccess("Import vật tư thành công!");
                this.listSketchMaterialElectronic = data.listElectronic;
                this.listSketchMaterialMechanical = data.listMechanical;
                this.activeModal.close(true);
              }
              else {
                if (event.Messag) {
                  this.messageService.showMessage(event.Message);
                }
                else {
                  this.messageService.showMessage('Dòng: ' + event.RowMaterialEmpty.join(',') + ' vật tư không tồn tại');
                }
              }
            }, error => {
              this.messageService.showMessage(error.error);
            });
          }
        }
        else {
          this.messageService.showMessage(data.mess);
        }
      },
      error => {
        this.messageService.showMessage(error);
      }
    );
  }

  // searchSketchesMaterialElectronic() {
  //   this.sketchesService.searchSketchesMaterialElectronic(this.sketchesService).subscribe((data: any) => {
  //     if (data.ListResult) {
  //       //this.StartIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
  //       if(data != null){
  //         this.listSketchMaterialElectronic = data.ListResult;
  //         this.modelSketchMaterialElectronic.TotalItems = data.TotalItem;
  //       }

  //     }
  //   },
  //     error => {
  //       this.messageService.showError(error);
  //     });
  // }

  save() {
    this.uploadFile();
    this.myInputVariable.nativeElement.value = "";
  }

  closeModal() {
    this.activeModal.close(false);
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

  showConfirmUploadVersion() {
    if (this.myInputVariable.nativeElement.value == "") {
      this.messageService.showMessage("Bạn chưa chọn file import!");
      return;
    }
    this.messageService.showConfirmFile("Bạn có muốn thay đổi version không?").then(
      async data => {
        if (data) {
          await this.showEditContent();
        } else {
          this.save();
        }
      }
    );
  }

  async showEditContent() {
    let activeModal = this.modalService.open(HistoryVersionComponent, { container: 'body', windowClass: 'show-history-version-modal', backdrop: 'static' });
    activeModal.componentInstance.id = this.ModuleId;
    activeModal.componentInstance.type = this.constant.HistoryVersion_Version_Module;
    activeModal.result.then(async (result) => {
      if (result) {
        await this.save();
        await this.updateVersion(result);
      }
    }, (reason) => {
    });
  }

  updateVersion(model: any) {
    this.serviceHistory.updateVersion(model).subscribe(
      () => {
        this.messageService.showSuccess('Cập nhật version thành công!');
      }, error => {
        this.messageService.showError(error);
      }
    );
  }
}
