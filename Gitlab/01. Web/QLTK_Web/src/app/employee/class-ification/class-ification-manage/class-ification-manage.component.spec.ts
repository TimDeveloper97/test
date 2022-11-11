import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClassIficationManageComponent } from './class-ification-manage.component';

describe('ClassIficationManageComponent', () => {
  let component: ClassIficationManageComponent;
  let fixture: ComponentFixture<ClassIficationManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClassIficationManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClassIficationManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
