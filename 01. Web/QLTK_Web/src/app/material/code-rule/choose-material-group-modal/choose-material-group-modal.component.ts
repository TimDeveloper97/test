import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MaterialGroupService } from '../../services/materialgroup-service';
import { MessageService } from 'src/app/shared';

@Component({
  selector: 'app-choose-material-group-modal',
  templateUrl: './choose-material-group-modal.component.html',
  styleUrls: ['./choose-material-group-modal.component.scss']
})
export class ChooseMaterialGroupModalComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private materialGroupService: MaterialGroupService,
    private messageService: MessageService,
  ) { }

  listMaterialGroup = [];
  listId = [];

  materialGroupModel = {
    MaterialGroupId:'',
    MaterialGroupTPAId: '',
    MaterialGroupName: ''
  }

  ngOnInit() {
    this.getMaterialGroup();
  }

  closeModal() {
    this.activeModal.close();
  }

  onSelectionChanged(e) {
    var materialGroup = e.selectedRowsData[0];
    this.materialGroupModel.MaterialGroupId = materialGroup.Id;
    this.materialGroupModel.MaterialGroupTPAId = materialGroup.MaterialGroupTPAId
    this.materialGroupModel.MaterialGroupName = materialGroup.Name;
  }

  getMaterialGroup() {
    this.materialGroupService.getListMaterialGroup().subscribe((data: any) => {
      if (data) {
        this.listMaterialGroup = data;
        for (var item of this.listMaterialGroup) {
          this.listId.push(item.Id);
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  choose(){
    this.activeModal.close(this.materialGroupModel);
  }

}
