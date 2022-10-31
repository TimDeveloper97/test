import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProblemExistManageComponent } from './problem-exist-manage.component';

describe('ProblemExistManageComponent', () => {
  let component: ProblemExistManageComponent;
  let fixture: ComponentFixture<ProblemExistManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProblemExistManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProblemExistManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
