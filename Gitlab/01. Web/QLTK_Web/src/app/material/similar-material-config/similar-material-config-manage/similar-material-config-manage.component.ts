import { Component, OnInit, ViewChild } from '@angular/core';

import { DxTreeListComponent } from 'devextreme-angular';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { AppSetting, MessageService, Constants, Configuration } from 'src/app/shared';
import { SimilarMaterialConfigService } from '../../services/similar-material-config.service';
import { SimilarMaterialCreateComponent } from '../../similar-material/similar-material-create/similar-material-create.component';
import { ChooseMaterialComponent } from '../choose-material/choose-material.component';
import { ShowMaterialComponent } from '../show-material/show-material.component';
import { SearchSimilarMaterialComponent } from '../search-similar-material/search-similar-material.component';

@Component({
  selector: 'app-similar-material-config-manage',
  templateUrl: './similar-material-config-manage.component.html',
  styleUrls: ['./similar-material-config-manage.component.scss']
})
export class SimilarMaterialConfigManageComponent implements OnInit {

  @ViewChild(DxTreeListComponent) treeView;
  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private similarmaterialservice: SimilarMaterialConfigService,
    public constant: Constants,
    private config: Configuration,
  ) {
    this.items = [
      { Id: 1, text: 'Sửa', icon: 'fa fa-edit' },
      { Id: 2, text: 'Xóa', icon: 'fas fa-times' }
    ];
  }

  startIndex = 0;
  height = 0;
  items: any;
  listData: any[] = [];
  listSimilarMaterial: any[] = [];
  listSimilarMaterialId = [];
  selectedSimilarMaterialId = '';
  similarMaterialId: '';

  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    
    Id: '',
    Name: '',
    Code: ''
  }

  similarMaterialModel: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Name',
    OrderType: true,
    Id: '',
    Name: ''
  }

  allModel: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Name',
    OrderType: true,
    Id: '',
    Name: 'Tất cả',
    Code: '',
  }
  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Mã vật tư',
    Items: [
      {
        Name: 'Tên vật tư',
        FieldName: 'Name',
        Placeholder: 'Nhập tên vật tư',
        Type: 'text'
      },
    ]
  };
  ngOnInit() {
    this.height = window.innerHeight - 150;
    this.appSetting.PageTitle = "Quản lý vật tư tương tự";
    this.searchSimilarMaterial();
    this.searchSimilarMaterialConfig("");
    this.selectedSimilarMaterialId = localStorage.getItem("selectedSimilarMaterialId");
    localStorage.removeItem("selectedSimilarMaterialId");
  }

  itemClick(e) {
    if (this.similarMaterialId == '' || this.similarMaterialId == null) {
      this.messageService.showMessage("Đây không phải nhóm vật tư tương tự!")
    } else {
      if (e.itemData.Id == 1) {
        this.showCreateUpdateSimilarMaterial(this.similarMaterialId);
      }
      else if (e.itemData.Id == 2) {
        this.showConfirmDeleteSimilarMaterial(this.similarMaterialId);
      }
    }
  }

  searchMaterial() {
    this.similarmaterialservice.searchSimilarMaterialConfig(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listData = data.ListResult;
        this.model.TotalItems = data.TotalItem;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  clear() {
    this.model = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,
      Id: '',
      Name: '',
      Code: ''
    }
    this.model.SimilarMaterialId = this.similarMaterialId;
    this.searchMaterial();
  }

  onSelectionChanged(e) {
    if (e.selectedRowKeys[0] != null) {
      this.selectedSimilarMaterialId = e.selectedRowKeys[0];
      this.searchSimilarMaterialConfig(e.selectedRowKeys[0]);
      this.similarMaterialId = e.selectedRowKeys[0];
      this.similarMaterialModel.Name = e.selectedRowsData[0].Name;
      this.model.SimilarMaterialId = e.selectedRowKeys[0];
    }
  }

  searchSimilarMaterial() {
    this.similarmaterialservice.searchSimilarMaterial(this.similarMaterialModel).subscribe((data: any) => {
      if (data.ListResult) {
        this.listSimilarMaterial = data.ListResult;
        //this.listSimilarMaterial.unshift(this.allModel);
        this.similarMaterialModel.TotalItems = data.TotalItem;

        if (this.selectedSimilarMaterialId == null) {
          this.selectedSimilarMaterialId = this.listSimilarMaterial[0].Id;
        }
        this.treeView.selectedRowKeys = [this.selectedSimilarMaterialId];
        for (var item of this.listSimilarMaterial) {
          this.listSimilarMaterialId.push(item.Id);
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  searchSimilarMaterialConfig(SimilarMaterialId: string) {
    this.model.SimilarMaterialId = SimilarMaterialId;
    this.similarmaterialservice.searchSimilarMaterialConfig(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listData = data.ListResult;
        this.model.TotalItems = data.TotalItem;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  save() {
    this.model.ListSimilarMaterialConfig = this.listData;
    this.similarmaterialservice.updateSimilarMaterialConfig(this.model).subscribe(
      data => {
        this.messageService.showSuccess('Cập nhật thông tin vật tư thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  showConfirmDeleteSimilarMaterial(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá vật tư tương tự này không?").then(
      data => {
        this.deleteSimilarMaterial(Id);
      },
      error => {
        
      }
    );
  }

  deleteSimilarMaterial(Id: string) {
    this.similarmaterialservice.deleteSimilarMaterial({ Id: Id }).subscribe(
      data => {
        this.searchSimilarMaterial();
        this.searchSimilarMaterialConfig("");
        this.messageService.showSuccess('Xóa vật tư tương tự thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirmDeleteSimilarMaterialConfig(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá vật tư này không?").then(
      data => {
        this.deleteSimilarMaterialConfig(Id);
      },
      error => {
        
      }
    );
  }

  deleteSimilarMaterialConfig(Id: string) {
    this.similarmaterialservice.deleteSimilarMaterialConfig({ Id: Id }).subscribe(
      data => {
        this.searchSimilarMaterialConfig(this.similarMaterialId);
        this.messageService.showSuccess('Xóa vật tư thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  exportExcel() {
    this.similarmaterialservice.exportExcel(this.model).subscribe(d => {
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

  showCreateUpdate(similarMaterialId: string) {
    if (similarMaterialId == "" || similarMaterialId == null) {
      this.messageService.showMessage('Bạn phải chọn vật tư tương tự');
      return;
    }
    let activeModal = this.modalService.open(ChooseMaterialComponent, { container: 'body', windowClass: 'choose-material-model', backdrop: 'static' })
    activeModal.componentInstance.similarMaterialId = similarMaterialId;
    var ListIdSelect = [];
    this.listData.forEach(element => {
      ListIdSelect.push(element.MaterialId);
    });

    activeModal.componentInstance.listIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result) {
        this.searchSimilarMaterialConfig(similarMaterialId);
      }
    }, (reason) => {
    });
  }

  showMaterial(Id: string) {
    let activeModal = this.modalService.open(ShowMaterialComponent, { container: 'body', windowClass: 'show-material-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchSimilarMaterial();
      }
    }, (reason) => {
    });
  }

  showCreateUpdateSimilarMaterial(Id: string) {
    let activeModal = this.modalService.open(SimilarMaterialCreateComponent, { container: 'body', windowClass: 'similar-material-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchSimilarMaterial();
      }
    }, (reason) => {
    });
  }

  showSearchSimilarMaterial() {
    let activeModal = this.modalService.open(SearchSimilarMaterialComponent, { container: 'body', windowClass: 'search-similar-material', backdrop: 'static' })
    activeModal.result.then((result) => {
      if (result) {
        this.searchSimilarMaterial();
        this.selectedSimilarMaterialId = result;
      }
    }, (reason) => {
    });
  }

}
