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
import { EditorModule } from '@tinymce/tinymce-angular';

import { SharedModule } from '../shared/shared.module';

import { PracticeRoutingModule } from './practice-routing.module';

import { DegreeManageComponent } from './degree/degree-manage/degree-manage.component';
import { DegreeCreateComponent } from './degree/degree-create/degree-create.component';
import { SpecializeManageComponent } from './specialize/specialize-manage/specialize-manage.component';
import { SpecializeCreateComponent } from './specialize/specialize-create/specialize-create.component';
import { SkillGroupManageComponent } from './skillgroup/skill-group-manage/skill-group-manage.component';
import { SkillGroupCreateComponent } from './skillgroup/skill-group-create/skill-group-create.component';
import { WorkPlaceManageComponent } from './workplace/work-place-manage/work-place-manage.component';
import { WorkPlaceCreateComponent } from './workplace/work-place-create/work-place-create.component';
import { ExpertCreateComponent } from './Expert/expert-create/expert-create.component';
import { ExpertManageComponent } from './Expert/expert-manage/expert-manage.component';
import { SelectWorkPlaceComponent } from './Expert/select-work-place/select-work-place/select-work-place.component';
import { SelectSpecializeComponent } from './Expert/select-specialize/select-specialize/select-specialize.component';
import { SkillManageComponent } from './skills/skill-manage/skill-manage.component';
import { SkillCreateComponent } from './skills/skill-create/skill-create.component';
import { SkillUpdateComponent } from './skills/skill-update/skill-update.component';
import { ChoosePraciteComponent } from './skills/choose-pracite/choose-pracite.component';
import { PracticeManagaComponent } from './practice/practice-managa/practice-managa.component';
import { PracticeCreateComponent } from './practice/practice-create/practice-create.component';
import { PracticeUpdateComponent } from './practice/practice-update/practice-update.component';
import { PracticeMaterialComponent } from './practice/practice-material/practice-material.component';
import { PracticeMaterialChooseComponent } from './practice/practice-material-choose/practice-material-choose.component';
import { PracticeSupMaterialComponent } from './practice/practice-sup-material/practice-sup-material.component';
import { PracticeSupMaterialChooseComponent } from './practice/practice-sup-material-choose/practice-sup-material-choose.component';
import { PracticeExpertsComponent } from './practice/practice-experts/practice-experts.component';
import { PracticeExpertsChooseComponent } from './practice/practice-experts-choose/practice-experts-choose.component';
import { PracticeFileComponent } from './practice/practice-file/practice-file.component';
import { PracticeContentComponent } from './practice/practice-content/practice-content.component';
import { PracticeMaterialConsumableComponent } from './practice/practice-material-consumable/practice-material-consumable.component';
import { SelectPracticeMaterialComsumableComponent } from './practice/select-practice-material-comsumable/select-practice-material-comsumable.component';
import { PacticeProductComponent } from './practice/pactice-product/pactice-product.component';
import { PracticeVersionTabComponent } from './practice/practice-version-tab/practice-version-tab.component';
import { PracticeGroupCreateComponent } from './practice/practice-group-create/practice-group-create.component';
import { BankCreateComponent } from './Expert/bank-create/bank-create.component';
import { PracticeSkillComponent } from './practice/practice-skill/practice-skill.component';
import { PracticeSkillChooseComponent } from './practice/practice-skill-choose/practice-skill-choose.component';
import { PracticeModuleComponent } from './practice/practice-module/practice-module.component';
import { PracticeSupModuleChooseComponent } from './practice/practice-sup-module-choose/practice-sup-module-choose.component';
import { PracticeUpdateContentComponent } from './practice/practice-update-content/practice-update-content.component';

@NgModule({
  declarations: [
    DegreeManageComponent,
    SpecializeManageComponent,
    SkillGroupManageComponent,
    WorkPlaceManageComponent,
    ExpertManageComponent,
    SkillManageComponent,
    PracticeManagaComponent,
    PracticeUpdateComponent,
    PracticeMaterialComponent,
    PracticeSupMaterialComponent,
    PracticeExpertsComponent,
    PracticeFileComponent,
    PracticeContentComponent,
    PracticeMaterialConsumableComponent,
    PacticeProductComponent,
    PracticeVersionTabComponent,
    PracticeSkillComponent,
    PracticeModuleComponent,
    DegreeCreateComponent,
    SpecializeCreateComponent,
    SkillGroupCreateComponent,
    ExpertCreateComponent,
    WorkPlaceCreateComponent,
    SelectSpecializeComponent,
    SelectWorkPlaceComponent,
    SkillCreateComponent,
    SkillUpdateComponent,
    ChoosePraciteComponent,
    PracticeCreateComponent,
    PracticeMaterialChooseComponent,
    PracticeSupMaterialChooseComponent,
    PracticeExpertsChooseComponent,
    SelectPracticeMaterialComsumableComponent,
    PracticeGroupCreateComponent,
    BankCreateComponent,
    PracticeSkillChooseComponent,
    PracticeSupModuleChooseComponent,
    PracticeUpdateContentComponent,
  ],
  imports: [
    CommonModule,
    PracticeRoutingModule,
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
    EditorModule
  ],
  entryComponents:
    [
      DegreeCreateComponent,
      SpecializeCreateComponent,
      SkillGroupCreateComponent,
      ExpertCreateComponent,
      WorkPlaceCreateComponent,
      SelectSpecializeComponent,
      SelectWorkPlaceComponent,
      SkillCreateComponent,
      SkillUpdateComponent,
      ChoosePraciteComponent,
      PracticeCreateComponent,
      PracticeMaterialChooseComponent,
      PracticeSupMaterialChooseComponent,
      PracticeExpertsChooseComponent,
      SelectPracticeMaterialComsumableComponent,
      PracticeGroupCreateComponent,
      BankCreateComponent,
      PracticeSkillChooseComponent,
      PracticeSupModuleChooseComponent,
      PracticeUpdateContentComponent,
    ],
    providers:[
      NgbActiveModal 
    ]
})
export class PracticeModule { }
