import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { BroadcastEventListener } from 'src/app/signalr/broadcast.event.listener';
import { Subscription } from 'rxjs';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { Constants, MessageService, Configuration, ComponentService } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SignalRService } from 'src/app/signalR/signal-r.service';
import { SolutionService } from '../service/solution.service';
import { SolutionChooseFolderUploadModalComponent } from '../solution-choose-folder-upload-modal/solution-choose-folder-upload-modal.component';
import { SolutionChooseFileUploadModalComponent } from '../solution-choose-file-upload-modal/solution-choose-file-upload-modal.component';

@Component({
  selector: 'app-solution-tab-design-document',
  templateUrl: './solution-tab-design-document.component.html',
  styleUrls: ['./solution-tab-design-document.component.scss']
})
export class SolutionTabDesignDocumentComponent implements OnInit, OnDestroy {

  private onDownload = new BroadcastEventListener<any>('downloadFileSolutionDesignDocument');
  private onDownloadFolder = new BroadcastEventListener<any>('downloadFolderSolutionDesignDocument');
  private downloadSub: Subscription;
  private downloadFolderSub: Subscription;
  @BlockUI() blockUI: NgBlockUI;
  @Input() Id: string;
  @Input() curentVersion: number;

  startIndex = 1;
  solutionModel = {
    SolutionId: '',
    FolderId: '',
    Path: '',
    ApiUrl: '',
    Token: '',
    Name:'',
  };

  folderId: string;
  ListFolder = [];
  ListFile = [];
  ListDesignDocumentId = [];
  folderHeight = 0;
  items = [
    { Id: 1, text: 'Download', icon: 'fa fa-arrow-down text-success' }
  ];

  constructor(
    public constants: Constants,
    private modalService: NgbModal,
    private signalRService: SignalRService,
    private solutionService: SolutionService,
    private messageService: MessageService,
    private config: Configuration,
    private componentService: ComponentService
  ) { }

  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Nhập tên file',
    Items: [
      
    ]
  };

  ngOnInit() {
    this.folderHeight = window.innerHeight - 140;
    this.solutionModel.SolutionId = this.Id;
    this.getListFolder();

    this.signalRService.listen(this.onDownload, true);
    this.downloadSub = this.onDownload.subscribe((data: any) => {
      this.blockUI.stop();
      if (data) {
        if (data.StatusCode == 2) {
          this.messageService.showMessage(data.Message);
        }
        else {

          this.messageService.showMessage('Download thành công!');
        }
      }
    });

    this.signalRService.listen(this.onDownloadFolder, true);
    this.downloadFolderSub = this.onDownloadFolder.subscribe((data: any) => {
      this.blockUI.stop();
      if (data) {
        if (data.StatusCode == 2) {
          this.messageService.showMessage(data.Message);
        }
        else {
          this.messageService.showMessage('Download thành công!');
        }
      }
    });
  }

  getListFolder() {
    this.solutionService.getListFolderSolution(this.Id, this.curentVersion).subscribe((data: any) => {
      if (data) {
        this.ListFolder = data;
        console.log(this.ListFolder);
        for (var item of this.ListFolder) {
          this.ListDesignDocumentId.push(item.Id);
        }
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  showChooseFolderUpload() {
    let activeModal = this.modalService.open(SolutionChooseFolderUploadModalComponent, { container: 'body', windowClass: 'solution-choose-folder-upload-modal', backdrop: 'static' });
    activeModal.componentInstance.solutionId = this.Id;
    activeModal.componentInstance.curentVersion = this.curentVersion;
    activeModal.result.then((result) => {
      this.curentVersion = result;
      this.getListFolder();
    }, (reason) => {
    });
  }

  ngOnDestroy() {
    this.downloadSub.unsubscribe();
    this.signalRService.stopListening(this.onDownload);
    this.downloadFolderSub.unsubscribe();
    this.signalRService.stopListening(this.onDownloadFolder);
  }

  designTypes: number;
  onSelectionChanged(e) {
    this.solutionModel.FolderId = e.selectedRowKeys[0];
    this.searchFile();
    this.folderId = e.selectedRowKeys[0];
    this.designTypes = e.selectedRowsData[0].DesignType;
  }

  searchFile() {
    this.solutionService.getListFileSolution(this.solutionModel).subscribe((data: any) => {
      if (data) {
        this.ListFile = data.ListResult;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  downloadFile(id, name, serverPath, designType: any) {
    this.componentService.showChooseFolder(3, this.designTypes, this.Id).subscribe(result => {
      if (result) {
        var file = {
          Id: id,
          Name: name,
          ServerPath: serverPath,
          DownloadPath: result,
          Token: '',
          ApiUrl: this.config.ServerApi
        };
        let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
        if (currentUser && currentUser.access_token) {
          file.Token = currentUser.access_token;
        }

        this.signalRService.invoke('DownloadFileSolutionDesignDocument', file).subscribe(data => {
          if (data) {
            this.blockUI.start();
          }
          else {
            this.messageService.showMessage("Không kết nối được service");
          }
        });
      }
    });

  }

  itemClick(e) {
    if (!this.folderId) {
      this.messageService.showMessage("Bạn chưa chọn thư mục cần download!")
    } else {
      this.componentService.showChooseFolder(3, this.designTypes, this.Id).subscribe(result => {
        if (result) {
          var file = {
            Id: this.folderId,
            DownloadPath: result,
            ObjectId: this.Id,
            Token: '',
            ApiUrl: this.config.ServerApi
          };
          let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
          if (currentUser && currentUser.access_token) {
            file.Token = currentUser.access_token;
          }

          this.signalRService.invoke('DownloadFolderSolutionDesignDocument', file).subscribe(data => {
            if (data) {
              this.blockUI.start();
            }
            else {
              this.messageService.showMessage("Không kết nối được service");
            }
          });
        }
      });
    }
  }

  uploadFile(id){
    let activeModal = this.modalService.open(SolutionChooseFileUploadModalComponent, { container: 'body', windowClass: 'solution-choose-file-upload-modal', backdrop: 'static' });
    activeModal.componentInstance.solutionId = id;
    activeModal.componentInstance.curentVersion = this.curentVersion;
    activeModal.result.then((result) => {
      this.curentVersion = result;
      this.getListFolder();
    }, (reason) => {
    });
  }
}
