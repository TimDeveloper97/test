import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DegreeManageComponent } from './degree-manage.component';

describe('DegreeManageComponent', () => {
  let component: DegreeManageComponent;
  let fixture: ComponentFixture<DegreeManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DegreeManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DegreeManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
