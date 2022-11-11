import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClassIficationCreateComponent } from './class-ification-create.component';

describe('ClassIficationCreateComponent', () => {
  let component: ClassIficationCreateComponent;
  let fixture: ComponentFixture<ClassIficationCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClassIficationCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClassIficationCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
