import { Component, OnInit, Input, ViewChild } from '@angular/core';

import { DxTreeListComponent } from 'devextreme-angular';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { MessageService, Constants, ComboboxService } from 'src/app/shared';
import { ModuleServiceService } from 'src/app/module/services/module-service.service';
import { MaterialService } from '../../services/material-service';

@Component({
  selector: 'app-similar-material-config',
  templateUrl: './similar-material-config.component.html',
  styleUrls: ['./similar-material-config.component.scss']
})
export class SimilarMaterialConfigComponent implements OnInit {

  @Input() Id: string;
  @ViewChild(DxTreeListComponent) treeView;
  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private service: ModuleServiceService,
    public constants: Constants,
    private materialService: MaterialService,
    private comboboxService: ComboboxService,

  ) { }

  listData: any[] = [];
  listSimilarMaterial: any[] = [];
  listSimilarMaterialId = [];
  selectedSimilarMaterialId = '';
  similarMaterialId: '';

  modelMaterial: any = {
    Id: '',
    Name: '',
    Code: '',
    ManufactureId:'', 
    ManufactureName:'',
  }

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
    this.getCBBManufacture();
    this.getById();
    this.searchSimilarMaterial();
    this.searchSimilarMaterialConfig('');
    this.selectedSimilarMaterialId = localStorage.getItem("selectedSimilarMaterialId");
    localStorage.removeItem("selectedSimilarMaterialId");
  }

  onSelectionChanged(e) {
    this.selectedSimilarMaterialId = e.selectedRowKeys[0];
    this.searchSimilarMaterialConfig(e.selectedRowKeys[0]);
    this.similarMaterialId = e.selectedRowKeys[0];
  }

  searchSimilarMaterial() {
    this.service.searchSimilarMaterial(this.similarMaterialModel).subscribe((data: any) => {
      if (data.ListResult) {
        this.listSimilarMaterial = data.ListResult;
        if (this.selectedSimilarMaterialId == null && this.listSimilarMaterial.length > 0) {
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
  ListManufacture: any[];
  getCBBManufacture() {
    this.comboboxService.getCbbManufacture().subscribe((data: any) => {
      if (data) {
        this.ListManufacture = data;
      }
      this.ListManufacture.forEach(element => {
        if (this.modelMaterial.ManufactureId == element.Id) {
          this.modelMaterial.ManufactureName = element.Name;
        }
      });
    },
      error => {
        this.messageService.showError(error);
      });
  }

  getById() {
    this.modelMaterial.Id = this.Id;
    this.materialService.getMaterialInfo(this.modelMaterial).subscribe(data => {
      this.modelMaterial = data;

      this.ListManufacture.forEach(element => {
        if (this.modelMaterial.ManufactureId == element.Id) {
          this.modelMaterial.ManufactureName = element.Name;
        }
      });
    });
  }
}
