import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FunctionManageComponent } from './function-manage.component';

describe('FunctionManageComponent', () => {
  let component: FunctionManageComponent;
  let fixture: ComponentFixture<FunctionManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FunctionManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FunctionManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
