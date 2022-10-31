import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NsMaterialGroupManageComponent } from './ns-material-group-manage.component';

describe('NsMaterialGroupManageComponent', () => {
  let component: NsMaterialGroupManageComponent;
  let fixture: ComponentFixture<NsMaterialGroupManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NsMaterialGroupManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NsMaterialGroupManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
