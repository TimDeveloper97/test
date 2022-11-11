import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SolutionManageComponent } from './solution-manage.component';

describe('SolutionManageComponent', () => {
  let component: SolutionManageComponent;
  let fixture: ComponentFixture<SolutionManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SolutionManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SolutionManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
