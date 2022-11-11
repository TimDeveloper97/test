import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { NgbActiveModal, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { VirtualScrollerModule } from 'ngx-virtual-scroller'; import {
  DevExtremeModule, DxTreeListModule, DxTreeViewModule, DxDropDownBoxModule, DxContextMenuModule,
  DxDateBoxModule,
  DxDataGridModule
} from 'devextreme-angular';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';

import { SharedModule } from '../shared/shared.module';

import { EducationRoutingModule } from './education-routing.module';

import { RoomTypeManageComponent } from './roomtype/room-type-manage/room-type-manage.component';
import { RoomtypeCreateComponent } from './roomtype/room-type-create/roomtype-create.component';
import { SelectMaterialComponent } from './classroom/select-material/select-material/select-material.component';
import { ClassRoomManageComponent } from './classroom/class-room-manage/class-room-manage.component';
import { ClassRoomCreateComponent } from './classroom/class-room-create/class-room-create.component';
import { SubjectsManageComponent } from './subjects/subjects-manage/subjects-manage.component';
import { SubjectsCreateComponent } from './subjects/subjects-create/subjects-create.component';
import { SelectClassRoomComponent } from './subjects/select-class-room/select-class-room/select-class-room.component';
import { SelectJobComponent } from './education-program/select-job/select-job/select-job.component';
import { EducationProgramCreateComponent } from './education-program/education-program-create/education-program-create.component';
import { EducationProgramManageComponent } from './education-program/education-program-manage/education-program-manage.component';
import { JobgroupMangeComponent } from './jobgroup/jobgroup-mange/jobgroup-mange.component';
import { JobgroupCreateComponent } from './jobgroup/jobgroup-create/jobgroup-create.component';
import { JobCreateComponent } from './job/job-create/job-create.component';
import { JobManageComponent } from './job/job-manage/job-manage.component';
import { JobUpdateComponent } from './job/job-update/job-update.component';
import { ListSubjectsJobChooseComponent } from './job/list-subjects-job-choose/list-subjects-job-choose.component';
import { SelectModuleComponent } from './classroom/select-module/select-module.component';
import { SelectProductComponent } from './classroom/select-product/select-product.component';
import { ShowSelectProductComponent } from './classroom/show-select-product/show-select-product.component';
import { ShowSelectModuleComponent } from './classroom/show-select-module/show-select-module.component';
import { ClassRoomDesignDocumentComponent } from './classroom/class-room-design-document/class-room-design-document.component';
import { ClassRoomChooseFolderUploadModalComponent } from './classroom/class-room-choose-folder-upload-modal/class-room-choose-folder-upload-modal.component';
import { SubjectSelectSkillComponent } from './subjects/subject-select-skill/subject-select-skill.component';
import { JobProjectAttachComponent } from './job/job-project-attach/job-project-attach.component';

@NgModule({
  declarations: [
    RoomTypeManageComponent,
    RoomtypeCreateComponent,
    ClassRoomManageComponent,
    ClassRoomCreateComponent,
    SubjectsManageComponent,
    EducationProgramManageComponent,
    JobgroupMangeComponent,
    JobManageComponent,
    JobUpdateComponent,
    ClassRoomDesignDocumentComponent,
    SelectMaterialComponent,
    JobCreateComponent,
    JobgroupCreateComponent,
    ListSubjectsJobChooseComponent,
    SubjectsCreateComponent,
    SelectClassRoomComponent,
    ListSubjectsJobChooseComponent,
    SelectJobComponent,
    EducationProgramCreateComponent,
    SelectModuleComponent,
    SelectProductComponent,
    ShowSelectProductComponent,
    ShowSelectModuleComponent,
    ClassRoomChooseFolderUploadModalComponent,
    SubjectSelectSkillComponent,
    RoomtypeCreateComponent,
    JobProjectAttachComponent
  ],
  imports: [
    CommonModule,
    EducationRoutingModule,
    FormsModule,
    SharedModule,
    PerfectScrollbarModule,
    NgbModule,
    VirtualScrollerModule,
    DevExtremeModule,
    DxTreeListModule,
    DxTreeViewModule,
    DxDropDownBoxModule,
    DxContextMenuModule,
    DxDateBoxModule,
    DxDataGridModule,
    CurrencyMaskModule,
    NgxGalleryModule,
  ],
  entryComponents: [
    SelectMaterialComponent,
    JobCreateComponent,
    JobgroupCreateComponent,
    ListSubjectsJobChooseComponent,
    SubjectsCreateComponent,
    SelectClassRoomComponent,
    ListSubjectsJobChooseComponent,
    SelectJobComponent,
    EducationProgramCreateComponent,
    SelectModuleComponent,
    SelectProductComponent,
    ShowSelectProductComponent,
    ShowSelectModuleComponent,
    ClassRoomChooseFolderUploadModalComponent,
    SubjectSelectSkillComponent,
    RoomtypeCreateComponent
  ],
  providers:[
    NgbActiveModal 
  ]
})
export class EducationModule { }
