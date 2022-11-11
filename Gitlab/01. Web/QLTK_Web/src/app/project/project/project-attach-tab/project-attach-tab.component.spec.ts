import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ProjectAttachTabComponent } from './project-attach-tab.component';


describe('ProjectAttachTabComponent', () => {
  let component: ProjectAttachTabComponent;
  let fixture: ComponentFixture<ProjectAttachTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectAttachTabComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectAttachTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
