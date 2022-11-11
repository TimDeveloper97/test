import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, Constants, MessageService } from 'src/app/shared';
import { TechnologySolutionService } from '../../service/technology-solution.service';
import { TechnologySolutionCreateComponent } from '../technology-solution-create/technology-solution-create.component';

@Component({
  selector: 'app-technology-solution-manage',
  templateUrl: './technology-solution-manage.component.html',
  styleUrls: ['./technology-solution-manage.component.scss']
})
export class TechnologySolutionManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private modalService: NgbModal,
    public constant: Constants,
    private messageService: MessageService,
    public techService: TechnologySolutionService,
  ) { }

  logUserId: string;
  listData: any[] = [];

  model: any = {
    OrderBy: 'Index',
    OrderType: true,

    page: 1,
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    Id: '',
    Name: '',
    IsEnable: '',
    Description:'',
    Note: '',
    ManufactureId: '',
    SupplierId: ''
  }

  
  totalRole: 0;
  startIndex: number = 1;

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo tên/ mã... ',
    Items: [
      {
        Name: 'Mã hãng sản xuất',
        FieldName: 'ManufactureId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Manuafacture,
        Columns: [{ Name: 'Code', Title: 'Mã hãng sản xuất' }, { Name: 'Name', Title: 'Tên hãng sản xuất' }],
        DisplayName: 'Code',
        ValueName: 'Id',
        Placeholder: 'Chọn mã hãng sản xuất'
      },
      {
        Name: 'Mã nhà cung cấp',
        FieldName: 'SupplierId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Supplier,
        Columns: [{ Name: 'Code', Title: 'Mã nhà cũng cấp' }, { Name: 'Name', Title: 'Tên nhà cũng cấp' }],
        DisplayName: 'Code',
        ValueName: 'Id',
        Placeholder: 'Chọn mã nhà cũng cấp'
      },
    ]
  };

  ngOnInit(): void {
    this.appSetting.PageTitle = "Công nghệ cho giải pháp";
    this.searchTech();
  }

  searchTech(){
    this.techService.searchTech(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listData = data.ListResult;
        this.model.TotalItems = data.TotalItem;
        
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }


  modelTechModule: any = {
    Id: '',
    ListTech: [],
  }

  onDrop(event: CdkDragDrop<string[]>) {
    moveItemInArray(this.listData, event.previousIndex, event.currentIndex);
   
    this.modelTechModule.ListTech = [];
    this.save();
  }

  
  save(){
    this.listData.forEach(element => {
      if (element) {
        this.modelTechModule.ListTech.push(element);
      }
    });
    
    this.techService.createIndex(this.modelTechModule).subscribe(
      data => {
          this.searchTech();
          this.messageService.showSuccess('Chỉnh sửa vị trí công nghệ thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
  

  clear() {
    this.model = {
      OrderBy: 'Index',
      OrderType: true,
  
      page: 1,
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      Id: '',
      Name: '',
      IsEnable: '',
      Description:'',
      Note: ''
    }
    this.searchTech();
  }

  showCreateUpdate(Id){
    let activeModal = this.modalService.open(TechnologySolutionCreateComponent, { container: 'body', windowClass: 'tech-solution-create-update-modal', backdrop: 'static' });
    activeModal.componentInstance.Id = Id;
    activeModal.result.then(result => {
      if (result) {
      }
      this.searchTech();
    })
  }

  showConfirmDelete(Id: string, IsEnable: boolean, Index: number) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá công nghệ này không?").then(
      data => {
        this.delete(Id, IsEnable, Index);
        
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  delete(Id, IsEnable, Index: number) {
    this.techService.deleteTech({Id: Id, IsEnable: IsEnable, Index: Index, LogUserId: this.logUserId} ).subscribe(result => {
      this.searchTech();
      this.messageService.showSuccess("Xóa công nghệ thành công!")
    },
    error => {
      this.messageService.showError(error);
    });
    
  }

}

