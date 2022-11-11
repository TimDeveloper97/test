import { Component, OnInit, ViewEncapsulation, Input } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { IPageInfo } from 'ngx-virtual-scroller';
import { MessageService, Constants } from 'src/app/shared';
import { SimilarMaterialConfigService } from '../../services/similar-material-config.service';

@Component({
  selector: 'app-choose-material',
  templateUrl: './choose-material.component.html',
  styleUrls: ['./choose-material.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class ChooseMaterialComponent implements OnInit {

  constructor(
    private messageService: MessageService,
    private activeModal: NgbActiveModal,
    public constants: Constants,
    private service: SimilarMaterialConfigService
  ) { }
    
  checkedTop: boolean = false;
  checkedBot: boolean = false;
  similarMaterialId: string;
  isAction: boolean = false;
  listData: any = [];
  model: any = {
    Page: 1,
    PageSize: 10,
    TotalItem: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    Name: '',
    Code: '',
    SimilarMaterialId: '',
    ManufactureName: '',
    RawMaterialCode: '',
    Quantity: '',
    Pricing: '',
    Note: '',
    Leadtime: '',
    ListSimilarMaterialConfig: [],
    ListBase: [],
    ListIdSelect: [],
    ListIdChecked: [],
    IsRequest: false
  }

  loading: boolean;
  public shouldPrependItems = false;
  listBase = [];
  listSelect: any = [];
  isRequest: boolean;
  listIdSelect: any = [];
  listIdSelectRequest: any = [];

  startIndex = 0;
  ngOnInit() {
    this.model.SimilarMaterialId = this.similarMaterialId;
    this.listIdSelect.forEach(element => {
      this.model.ListIdSelect.push(element);
    });
    this.searchMaterial();
  }

  searchMaterial() {
    this.listSelect.forEach(element => {
      this.model.ListIdSelect.push(element.Id);
    });
    this.listBase.forEach(element => {
      if (element.Checked) {
        this.model.ListIdChecked.push(element.Id);
      }
    });
    this.service.searchMaterial(this.model).subscribe(data => {
      this.listBase = data.ListResult;
      this.listBase.forEach((element, index) => {
        element.Index = index + 1;
      });
      this.model.TotalItem = data.TotalItem;
    }, error => {
      this.messageService.showError(error);
    })
  }

  public fetchMore(event: IPageInfo) {
    if (event.endIndex !== this.listBase.length-1) return;
    this.loading = true;
    this.fetchNextChunk(this.listBase.length, 10).then(chunk => {
        this.listBase = this.listBase.concat(chunk);
        this.loading = false;
    }, () => this.loading = false);
  }

  fetchNextChunk(skip: number, limit: number):Promise<[]>{
    return new Promise((resolve, reject) => {
      this.model.PageSize = limit;
      this.model.PageNumber = skip;
      this.searchMaterial();
  });
  }
  clear() {
    this.model = {
      page: 1,
      PageSize: 10,
      TotalItem: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,

      Id: '',
      Name: '',
      Code: '',
      ListIdSelect: [],
      ListIdChecked: [],
    }
    this.model.IsRequest = this.isRequest;
    if (this.isRequest) {
      this.listIdSelectRequest.forEach(element => {
        this.model.ListIdSelect.push(element);
      });
    } else {
      this.listIdSelect.forEach(element => {
        this.model.ListIdSelect.push(element);
      });
    }
    this.searchMaterial();
  }

  addRow() {
    this.listBase.forEach(element => {
      if (element.Checked) {
        this.listSelect.push(element);
      }
    });
    this.listSelect.forEach(element => {
      var index = this.listBase.indexOf(element);
      if (index > -1) {
        this.listBase.splice(index, 1);
      }
    });
    this.listBase.forEach((element, index) => {
      element.Index = index + 1;
    });
  }

  removeRow() {
    this.listSelect.forEach(element => {
      if (element.Checked) {
        this.listBase.push(element);
      }
    });

    this.listBase.forEach(element => {
      var index = this.listSelect.indexOf(element);
      if (index > -1) {
        this.listSelect.splice(index, 1);
      }
    });
    this.listBase.forEach((element, index) => {
      element.Index = index + 1;
    });
  }

  createSimilarMaterialConfig() {
    if (this.listSelect.length == 0) {
      this.messageService.showMessage('Bạn chưa chọn vật tư');
      return;
    }
    this.model.ListSimilarMaterialConfig = this.listSelect;
    this.service.createSimilarMaterialConfig(this.model).subscribe(
      data => {
        this.messageService.showSuccess('Thêm vật tư thành công!');
        this.closeModal();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  closeModal() {
    this.activeModal.close(true);
  }

  checkAll(isCheck) {
    if (isCheck) {
      this.listBase.forEach(element => {
        if (this.checkedTop) {
          element.Checked = true;
        } else {
          element.Checked = false;
        }
      });
    } else {
      this.listSelect.forEach(element => {
        if (this.checkedBot) {
          element.Checked = true;
        } else {
          element.Checked = false;
        }
      });
    }
  }
}
