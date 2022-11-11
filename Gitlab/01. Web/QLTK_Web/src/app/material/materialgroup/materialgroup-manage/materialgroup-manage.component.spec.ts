import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MaterialgroupManageComponent } from './materialgroup-manage.component';

describe('MaterialgroupManageComponent', () => {
  let component: MaterialgroupManageComponent;
  let fixture: ComponentFixture<MaterialgroupManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MaterialgroupManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MaterialgroupManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
