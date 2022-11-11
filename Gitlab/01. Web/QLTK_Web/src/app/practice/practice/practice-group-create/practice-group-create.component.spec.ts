import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PracticeGroupCreateComponent } from './practice-group-create.component';

describe('PracticeGroupCreateComponent', () => {
  let component: PracticeGroupCreateComponent;
  let fixture: ComponentFixture<PracticeGroupCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PracticeGroupCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PracticeGroupCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
