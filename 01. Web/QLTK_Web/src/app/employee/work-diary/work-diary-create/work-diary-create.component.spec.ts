import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkDiaryCreateComponent } from './work-diary-create.component';

describe('WorkDiaryCreateComponent', () => {
  let component: WorkDiaryCreateComponent;
  let fixture: ComponentFixture<WorkDiaryCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkDiaryCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkDiaryCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
