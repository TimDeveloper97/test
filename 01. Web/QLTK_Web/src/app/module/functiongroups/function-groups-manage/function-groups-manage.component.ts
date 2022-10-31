import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { AppSetting, MessageService, Constants } from 'src/app/shared';
import { FunctionGroupsService } from '../../services/function-groups.service';
import { FunctionGroupsCreateComponent } from '../function-groups-create/function-groups-create.component';

@Component({
  selector: 'app-function-groups-manage',
  templateUrl: './function-groups-manage.component.html',
  styleUrls: ['./function-groups-manage.component.scss']
})
export class FunctionGroupsManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private titleservice: Title,
    private serviceFunctionGroup:FunctionGroupsService,
    public constant: Constants
  ) { }

  startIndex = 0;
  listData: any[] = [];

  modelFunctionGroups: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    Id: '',
    Name: '',
    Code: '',
  }

  ngOnInit() {
    this.appSetting.PageTitle = "Nhóm tính năng";
    this.searchFunctionGroup();
  }

  searchFunctionGroup() {
    this.serviceFunctionGroup.searchFunctionGroup(this.modelFunctionGroups).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.modelFunctionGroups.PageNumber - 1) * this.modelFunctionGroups.PageSize + 1);
        this.listData = data.ListResult;
        this.modelFunctionGroups.TotalItems = data.TotalItem; 
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  clear() {
    this.modelFunctionGroups = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,
      Id: '',
      Name: '',
      Code: '',
    }
    this.searchFunctionGroup();
  }

  showConfirmDeleteFunctionGroups(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nhóm tính năng này không?").then(
      data => {
        this.deleteFunctionGroup(Id);
      },
      error => {
        
      }
    );
  }

  deleteFunctionGroup(Id: string) {
    this.serviceFunctionGroup.deleteFunctionGroup({ Id: Id }).subscribe(
      data => {
        this.searchFunctionGroup();
        this.messageService.showSuccess('Xóa nhóm tính năng thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  ShowCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(FunctionGroupsCreateComponent, { container: 'body', windowClass: 'functiongroups-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchFunctionGroup();
      }
    }, (reason) => {
    });
  }

}
