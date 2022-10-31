import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DashbroadProjectComponent } from './dashbroad-project.component';

describe('DashbroadProjectComponent', () => {
  let component: DashbroadProjectComponent;
  let fixture: ComponentFixture<DashbroadProjectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DashbroadProjectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DashbroadProjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
