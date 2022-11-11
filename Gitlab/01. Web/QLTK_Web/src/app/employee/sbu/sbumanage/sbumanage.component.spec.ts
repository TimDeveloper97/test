import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SBUManageComponent } from './sbumanage.component';

describe('SBUManageComponent', () => {
  let component: SBUManageComponent;
  let fixture: ComponentFixture<SBUManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SBUManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SBUManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
