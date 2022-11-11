import { Component, OnInit, Input } from '@angular/core';

import { MessageService, Constants, ComboboxService } from 'src/app/shared';
import { Router } from '@angular/router';
import { ConverUnitService } from '../../services/conver-unit.service';
import { MaterialService } from '../../services/material-service';

@Component({
  selector: 'app-conver-unit',
  templateUrl: './conver-unit.component.html',
  styleUrls: ['./conver-unit.component.scss']
})
export class ConverUnitComponent implements OnInit {

  @Input() Id: string;
  constructor(
    private messageService: MessageService,
    public constants: Constants,
    private combobox: ComboboxService,
    private router: Router,
    private service: ConverUnitService,
    private materialService: MaterialService,
    private comboboxService: ComboboxService,

  ) { }
  value = '';
  unitId = '';
  unitName = '';
  materialId = '';
  quantity = '';
  convertQuantity = '';
  LossRate: '';
  listUnit: any[] = [];
  listConverUnit: any[] = [];

  model: any = {
    Id: '',
    UnitId: '',
    UnitName: '',
    MaterialId: '',
    Quantity: '',
    ConvertQuantity: '',
    ListConverUnit: [],
    LossRate: ''
  }

  modelMaterial: any = {
    Id: '',
    Name: '',
    Code: '',
    ManufactureId: '',
    ManufactureName: '',
  }

  ngOnInit() {
    this.model.MaterialId = this.Id;
    this.getCBBManufacture();
    this.getListConverUnit();
    this.getCBBUnit();
    this.getUnitName();
    this.getById();
  }

  getListConverUnit() {
    this.service.getListConverUnit(this.model).subscribe((data: any) => {
      if (data) {
        this.listConverUnit = data.ListConverUnit;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  getCBBUnit() {
    this.combobox.getCbbUnit().subscribe((data: any) => {
      if (data) {
        this.listUnit = data;
        for (var i of this.listUnit) {
          if (i.Id == "" || i.Id == null) {
            this.listUnit.splice(this.listUnit.indexOf(i), 1);
          }
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  getUnitName() {
    this.service.getUnitName(this.model).subscribe((data: any) => {
      if (data) {
        this.unitName = data.UnitName;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  save() {
    this.model.ListConverUnit = this.listConverUnit;
    this.service.addConverUnit(this.model).subscribe(
      data => {
        this.messageService.showSuccess('Cập nhật chuyển đổi đơn vị thành công!');
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  selectIndex = 1;
  loadValue(param, index) {
    this.selectIndex = index;
    this.value = '';
  }

  addRowParameter() {
    if (this.unitId == '' || this.unitId == null) {
      this.messageService.showMessage("Đơn vị chuyển đổi không được để trống!");
    } else if (this.quantity == '' || this.quantity == null) {
      this.messageService.showMessage("Số lượng không được để trống!");
    } else if (this.convertQuantity == '' || this.convertQuantity == null) {
      this.messageService.showMessage("Số lượng chuyển đổi không được để trống!");
    }
    else {
      var addConverUnit = Object.assign({}, this.model);
      addConverUnit.UnitId = this.unitId;
      addConverUnit.UnitName = this.unitName;
      addConverUnit.MaterialId = this.materialId;
      addConverUnit.Quantity = this.quantity;
      addConverUnit.ConvertQuantity = this.convertQuantity;
      addConverUnit.LossRate = this.LossRate;
      this.listConverUnit.push(addConverUnit);

      //refresh dòng trống
      this.unitId = '';
      this.materialId = '';
      this.quantity = '';
      this.convertQuantity = '';
    }
  }

  deleteRow(id, index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xoá chuyển đổi này không?").then(
      data => {
        this.listConverUnit.splice(index, 1);
      },
      error => {
        
      }
    );
  }

  closeModal() {
    this.router.navigate(['vat-tu/quan-ly-vat-tu']);
  }

  ListManufacture: any[];
  getCBBManufacture() {
    this.comboboxService.getCbbManufacture().subscribe((data: any) => {
      if (data) {
        this.ListManufacture = data;
      }
      // this.ListManufacture.forEach(element => {
      //   if (this.modelMaterial.ManufactureId == element.Id) {
      //     this.modelMaterial.ManufactureName = element.Name;
      //   }
      // });
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
