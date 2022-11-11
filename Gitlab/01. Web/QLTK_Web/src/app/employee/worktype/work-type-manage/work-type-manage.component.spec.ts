import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { WorkTypeManageComponent } from './work-type-manage.component';


describe('WorkTypeManageComponent', () => {
  let component: WorkTypeManageComponent;
  let fixture: ComponentFixture<WorkTypeManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkTypeManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkTypeManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
