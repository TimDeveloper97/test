import { Component, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { DxTreeListComponent } from 'devextreme-angular';

import { Constants, MessageService } from 'src/app/shared';
import { ModuleServiceService } from '../../services/module-service.service';

@Component({
  selector: 'app-module-show-similar-material',
  templateUrl: './module-show-similar-material.component.html',
  styleUrls: ['./module-show-similar-material.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ModuleShowSimilarMaterialComponent implements OnInit {

  @ViewChild(DxTreeListComponent) treeView;
  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private service: ModuleServiceService,
    public constants: Constants,
  ) { }

  Id: string;
  listData: any[] = [];
  listSimilarMaterial: any[] = [];
  listSimilarMaterialId = [];
  selectedSimilarMaterialId = '';
  similarMaterialId: '';

  model: any = {
    Id: '',
    Name: '',
    Code: '',
  }

  similarMaterialModel: any = {
    Id: '',
    MaterialId: '',
  }

  ngOnInit() {
    this.similarMaterialModel.MaterialId = this.Id;
    this.searchSimilarMaterial();
    this.searchSimilarMaterialConfig('');
    this.selectedSimilarMaterialId = localStorage.getItem("selectedSimilarMaterialId");
    localStorage.removeItem("selectedSimilarMaterialId");
  }

  onSelectionChanged(e) {
    this.selectedSimilarMaterialId = e.selectedRowKeys[0];
    this.searchSimilarMaterialConfig(e.selectedRowKeys[0]);
    this.similarMaterialId = e.selectedRowKeys[0];
    this.similarMaterialModel.Name = e.selectedRowsData[0].Name;
  }

  searchSimilarMaterial() {
    this.service.searchSimilarMaterial(this.similarMaterialModel).subscribe((data: any) => {
      if (data.ListResult) {
        this.listSimilarMaterial = data.ListResult;
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

  searchSimilarMaterialConfig(similarMaterialId: string) {
    this.model.similarMaterialId = similarMaterialId;
    this.service.searchSimilarMaterialConfig(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.listData = data.ListResult;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  clear() {
    this.model = {
      Id: '',
      Name: '',
      Code: '',
    }
    this.listSimilarMaterial = [];
    this.listSimilarMaterialId = [];
    this.selectedSimilarMaterialId = '';
    this.similarMaterialId = '';
    this.searchSimilarMaterial();
    this.searchSimilarMaterialConfig('');
  }

  closeModal() {
    this.activeModal.close();
  }

}
