import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, MessageService } from 'src/app/shared';
import { CategoryCreateComponent } from '../category-create/category-create.component';
import { CategoryService } from '../service/category.service';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.scss']
})
export class CategoryComponent implements OnInit {

  constructor(
    private modalService: NgbModal,
    private categoryService: CategoryService,
    public appSetting: AppSetting,
    private messageService: MessageService,
  ) { }

  height = 0;
  listCategoryGroup: any[] = [];
  listCategoryGroupId = [];
  categoryGroupId: '';
  items: any;

  ngOnInit(): void {
    this.height = window.innerHeight - 140;
    this.appSetting.PageTitle = "QUẢN LÝ DANH MỤC";
  }

  modelCategoryGroup: any = {
    page: 1,
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    // Id: '',
    // MaterialGroupTPAId: '',
    // MaterialGroupTPAName: '',
    // MaterialGroupTPACode: '',
    // Name: '',
    // Code: '',
    // ParentId: '',
    // Description: '',
  }

  modelCategory: any = {
    page: 1,
    PageSize: 10,
    totalItems: 0,
    TotalItemExten: 0,
    TotalNoFile: 0,
    Date: null,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã/tên danh mục ...',
    Items: [
    ]
  };

   // popup thêm mới và chỉnh sửa danh mục
   showCreateUpdate(Id: string, isUpdate: boolean) {
    let activeModal = this.modalService.open(CategoryCreateComponent, { container: 'body', windowClass: 'category-create-model', backdrop: 'static' })
    if (isUpdate) {
      activeModal.componentInstance.Id = Id;
    } else {
      if (Id) {
        activeModal.componentInstance.parentId = Id;
      } else {
        activeModal.componentInstance.parentId = this.categoryGroupId;
      }

    }
    activeModal.result.then((result) => {
      if (result) {
        this.searchCategoryGroup();
      }
    }, (reason) => {
    });
  }

  searchCategoryGroup() {
    this.categoryService.searchCategoryGroup(this.modelCategoryGroup).subscribe((data: any) => {
      // if (data.ListResult) {
      //   this.listMaterialGroup = data.ListResult;
      //   this.listMaterialGroup.unshift(this.modelAll);
      //   this.modelMaterialGroup.totalItems = data.TotalItem;
      //   if (this.selectedMaterialGroupId == null) {
      //     this.selectedMaterialGroupId = this.listMaterialGroup[0].Id;
      //   }

      //   this.treeView.selectedRowKeys = [this.selectedMaterialGroupId];
      //   for (var item of this.listMaterialGroup) {
      //     this.listMaterialGroupId.push(item.Id);
      //   }
      // }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  itemClick(e) {
    // if (this.materialGroupId == '' || this.materialGroupId == null) {
    //   this.messageService.showMessage("Đây không phải nhóm vật tư!")
    // } else {
    //   if (e.itemData.Id == 1) {
    //     this.showCreateUpdate(this.materialGroupId, false)
    //   }
    //   else if (e.itemData.Id == 2) {
    //     this.showCreateUpdate(this.materialGroupId, true);
    //   }
    //   else if (e.itemData.Id == 3) {
    //     this.showConfirmDeleteMaterialGroup(this.materialGroupId);
    //   }
    // }
  }

  
  onSelectionChanged(e) {
    // if (e.selectedRowKeys[0] != null && e.selectedRowKeys[0] != this.selectedMaterialGroupId) {
    //   this.selectedMaterialGroupId = e.selectedRowKeys[0];
    //   this.searchMaterial(e.selectedRowKeys[0]);
    //   this.materialGroupId = e.selectedRowKeys[0];
    // }

  }

  searchCategory(categoryGroupId: string) {
    // this.modelMaterial.MaterialGroupId = materialGroupId;
    // this.materialService.searchMaterial(this.modelMaterial).subscribe((data: any) => {
    //   if (data.ListResult) {
    //     this.startIndex = ((this.modelMaterial.PageNumber - 1) * this.modelMaterial.PageSize + 1);
    //     this.listMaterial = data.ListResult;
    //     if (this.checkeds) {
    //       this.listMaterial.forEach(element => {
    //         element.Checked = true;
    //       });
    //     }
    //     this.modelMaterial.totalItems = data.TotalItem;
    //     this.modelMaterial.TotalItemExten = data.TotalItemExten;
    //     this.modelMaterial.TotalNoFile = data.TotalNoFile;
    //     this.modelMaterial.Date = data.Date;
    //   }
    // },
    //   error => {
    //     this.messageService.showError(error);
    //   });
  }

  clear(){

  }
}
