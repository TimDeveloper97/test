import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectTransferAttachComponent } from './project-transfer-attach.component';

describe('ProjectTransferAttachComponent', () => {
  let component: ProjectTransferAttachComponent;
  let fixture: ComponentFixture<ProjectTransferAttachComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectTransferAttachComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectTransferAttachComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
