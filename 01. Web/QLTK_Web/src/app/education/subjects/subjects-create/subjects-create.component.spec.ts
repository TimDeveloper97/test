import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SubjectsCreateComponent } from './subjects-create.component';

describe('SubjectsCreateComponent', () => {
  let component: SubjectsCreateComponent;
  let fixture: ComponentFixture<SubjectsCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SubjectsCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubjectsCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
