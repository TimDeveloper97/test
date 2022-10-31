import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FunctionGroupsManageComponent } from './function-groups-manage.component';

describe('FunctionGroupsManageComponent', () => {
  let component: FunctionGroupsManageComponent;
  let fixture: ComponentFixture<FunctionGroupsManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FunctionGroupsManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FunctionGroupsManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
