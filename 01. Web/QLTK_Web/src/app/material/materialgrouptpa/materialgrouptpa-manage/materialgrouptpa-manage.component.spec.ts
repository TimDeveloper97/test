import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MaterialgrouptpaManageComponent } from './materialgrouptpa-manage.component';

describe('MaterialgrouptpaManageComponent', () => {
  let component: MaterialgrouptpaManageComponent;
  let fixture: ComponentFixture<MaterialgrouptpaManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MaterialgrouptpaManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MaterialgrouptpaManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
