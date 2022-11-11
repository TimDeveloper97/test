import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ProjectRoleService } from '../../service/project-role.service';
import { MessageService } from 'src/app/shared';

@Component({
  selector: 'app-role-create-update',
  templateUrl: './role-create-update.component.html',
  styleUrls: ['./role-create-update.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class RoleCreateUpdateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public roleService : ProjectRoleService,

  ) { }

  ModalInfo = {
    Title: 'Thêm mới vị trí trong dự án',
    SaveText: 'Lưu',
  };

  listIndex: any[] = []

  model: any ={
    Id:'',
    Name:'',
    Index: 0,
    Descipton:'',
    IsDisable: false,
    CreateBy:'',
    CreateDate:null,
    UpdateBy:'',
    UpdateDate:null,
    PlanFunctions: [],
  }
  Id: string;

  ngOnInit(): void {
    this.getCbbRole();
    if(this.model.Id){
      this.getRoleInfor();
      this.ModalInfo.Title="Chỉnh sửa vị trí trong dự án"
    }else{
      this.searchPlanFunction();
    }
  }

  getCbbRole() {
    this.roleService.getCbbRole().subscribe((data: any) => {
      this.listIndex = data;
      if (this.Id == null || this.Id == '') {
        this.model.Index = data[data.length - 1].Index;
      } else {
        this.listIndex.splice(this.listIndex.length - 1, 1);
      }
    });
  }


  searchPlanFunction(){
    this.roleService.SearchPlanFunction().subscribe(data=>{
     this.model.PlanFunctions=data;
      });
    }


  getRoleInfor(){
    this.roleService.searchRoleById(this.model.Id).subscribe(data=>{
      this.model=data;
      this.model.PlanFunctions=data.PlanFunctions;
    });
  }

  closeModal() {
    this.activeModal.close();
  }

  save(){
    if(this.model.Id){
      this.updateRole();
    }
    else{
      this.createRole();
    }
  }

  createRole(){
    this.roleService.createRole(this.model).subscribe(result=>{
      this.messageService.showSuccess('Thêm mới vị trí thành công!');
      this.closeModal();
    },
    error => {
      this.messageService.showError(error);
    }
  );
  }

  updateRole(){
    this.roleService.updateRole(this.model).subscribe(result=>{
      this.messageService.showSuccess('Cập nhật vị trí thành công');
      this.closeModal();
    },
    error => {
      this.messageService.showError(error);
    }
  );
  }
}
