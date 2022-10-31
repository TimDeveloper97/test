import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StageManageComponent } from './stage-manage.component';

describe('StageManageComponent', () => {
  let component: StageManageComponent;
  let fixture: ComponentFixture<StageManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StageManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StageManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
