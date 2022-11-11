import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ErrorManageComponent } from './error-manage.component';

describe('ErrorManageComponent', () => {
  let component: ErrorManageComponent;
  let fixture: ComponentFixture<ErrorManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ErrorManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ErrorManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
