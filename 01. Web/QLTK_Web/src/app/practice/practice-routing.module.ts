import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard } from '../auth/guards/auth.guard';
import { DegreeManageComponent } from './degree/degree-manage/degree-manage.component';
import { ExpertManageComponent } from './Expert/expert-manage/expert-manage.component';
import { PracticeCreateComponent } from './practice/practice-create/practice-create.component';
import { PracticeManagaComponent } from './practice/practice-managa/practice-managa.component';
import { PracticeUpdateComponent } from './practice/practice-update/practice-update.component';
import { SkillGroupManageComponent } from './skillgroup/skill-group-manage/skill-group-manage.component';
import { SkillManageComponent } from './skills/skill-manage/skill-manage.component';
import { SpecializeManageComponent } from './specialize/specialize-manage/specialize-manage.component';
import { WorkPlaceManageComponent } from './workplace/work-place-manage/work-place-manage.component';


const routes: Routes = [
  { path: 'quan-ly-trinh-do', component: DegreeManageComponent, canActivate: [AuthGuard] },
  { path: 'quan-ly-chuyen-mon', component: SpecializeManageComponent, canActivate: [AuthGuard] },
  { path: 'quan-ly-nhom-ky-nang', component: SkillGroupManageComponent, canActivate: [AuthGuard] },
  { path: 'quan-ly-don-vi-cong-tac', component: WorkPlaceManageComponent, canActivate: [AuthGuard] },
  { path: 'quan-ly-chuyen-gia', component: ExpertManageComponent, canActivate: [AuthGuard] },
  { path: 'quan-ly-ky-nang', component: SkillManageComponent, canActivate: [AuthGuard] },
  {
    path: 'quan-ly-bai-thuc-hanh', 
    children: [
      { path: '', component: PracticeManagaComponent, canActivate: [AuthGuard] },
      { path: 'them-moi-bai-thuc-hanh', component: PracticeCreateComponent, canActivate: [AuthGuard] },
      { path: 'chinh-sua-bai-thuc-hanh/:Id', component: PracticeUpdateComponent, canActivate: [AuthGuard] },
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PracticeRoutingModule { }
