import { Component, OnInit, OnDestroy } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DesignStructureManagerComponent } from '../design-structure-manager/design-structure-manager.component';
import { Configuration, MessageService, AppSetting, PermissionService, Constants } from 'src/app/shared';
import { DesignStructureService } from '../services/designstructure-service';
import { SignalRService } from 'src/app/signalR/signal-r.service';
import { BroadcastEventListener } from 'src/app/signalr/broadcast.event.listener';
import { Subscription } from 'rxjs';
import { NgBlockUI, BlockUI } from 'ng-block-ui';

@Component({
  selector: 'app-design-structure',
  templateUrl: './design-structure.component.html',
  styleUrls: ['./design-structure.component.scss'],
})
export class DesignStructureComponent implements OnInit, OnDestroy {

  constructor(
    private modalService: NgbModal,
    private config: Configuration,
    private designStructureService: DesignStructureService,
    private messageService: MessageService,
    private signalRService: SignalRService,
    public appSetting: AppSetting,
    public permissionService: PermissionService,
    public constant: Constants
  ) { }
  model: any = {
    Type: 1,
    ObjectType: 1,
    ObjectCode: '',
    ApiUrl: '',
    ModuleName: '',
    CreateBy: '',
    ModuleGroup: ''
  }

  @BlockUI() blockUI: NgBlockUI;
  private onCreateFolder = new BroadcastEventListener<any>('sendCreateFolder');
  private notiSub: Subscription;
  ngOnInit() {
    this.model.ApiUrl = this.config.ServerFileApi;
    this.appSetting.PageTitle = "Tạo cấu trúc thư mục";
    this.signalRService.listen(this.onCreateFolder, true);
    this.notiSub = this.onCreateFolder.subscribe((data: any) => {
      if (data) {
        if (data.StatusCode == 1) {
          this.messageService.showSuccess('Tạo thư mục thành công!');
        } else {
          this.messageService.showMessage('Tạo thư mục không thành công!');
        }
      }
      this.blockUI.stop();
    });
  }

  ngOnDestroy() {
    this.notiSub.unsubscribe();
    this.signalRService.stopListening(this.onCreateFolder);
  }

  createDesignStructure() {
    this.designStructureService.getInfoDesignStructureCreate(this.model).subscribe((data: any) => {
      if (data) {
        data.ApiUrl = this.config.ServerFileApi;
        this.createFolder(data);
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  createFolder(data) {
    this.signalRService.invoke('CreateFolder', data).subscribe(data => {
      if (data) {
        this.blockUI.start();
      }
      else {
        this.messageService.showMessage("Không kết nối được service");
      }
    });
  }

  showCreateDesignStructure() {
    let activeModal = this.modalService.open(DesignStructureManagerComponent, { container: 'body', windowClass: 'designstructuremanager-modal', backdrop: 'static' })
  }

  changeObjectType(ObjectType)
  {
    this.model.ObjectType = ObjectType;
  }
}
