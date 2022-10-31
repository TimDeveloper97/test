import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FunctionGroupsCreateComponent } from './function-groups-create.component';

describe('FunctionGroupsCreateComponent', () => {
  let component: FunctionGroupsCreateComponent;
  let fixture: ComponentFixture<FunctionGroupsCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FunctionGroupsCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FunctionGroupsCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
