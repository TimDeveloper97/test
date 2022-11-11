import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SubjectsManageComponent } from './subjects-manage.component';

describe('SubjectsManageComponent', () => {
  let component: SubjectsManageComponent;
  let fixture: ComponentFixture<SubjectsManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SubjectsManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubjectsManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
