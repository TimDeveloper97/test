import { Component, OnInit, ViewEncapsulation } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { AppSetting, MessageService, Constants } from 'src/app/shared';
import { SimilarMaterialConfigService } from '../../services/similar-material-config.service';

@Component({
  selector: 'app-search-similar-material',
  templateUrl: './search-similar-material.component.html',
  styleUrls: ['./search-similar-material.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SearchSimilarMaterialComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    public appSetting: AppSetting,
    private messageService: MessageService,
    private similarmaterialservice: SimilarMaterialConfigService,
    public constant: Constants
  ) { }

  listSimilarMaterial: any[] = [];

  model: any = {
    Id: '',
    Name: '',
    MaterialName: '',
    MaterialCode: ''
  }

  ngOnInit() {
    this.searchMaterialInSimilarMaterial();
  }

  searchMaterialInSimilarMaterial() {
    this.similarmaterialservice.searchMaterialInSimilarMaterial(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.listSimilarMaterial = data.ListResult;
        this.model.TotalItems = data.TotalItem;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  viewMaterial(Id: string) {
    this.activeModal.close(Id);
  }

  clear() {
    this.model = {
      Id: '',
      Name: '',
      MaterialName: '',
      MaterialCode: ''
    };
    this.searchMaterialInSimilarMaterial();
  }

  closeModal() {
    this.activeModal.close();
  }
}
