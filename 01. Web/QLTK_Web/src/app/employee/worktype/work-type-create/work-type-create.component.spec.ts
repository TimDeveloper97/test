import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { WorkTypeCreateComponent } from './work-type-create.component';


describe('WorkTypeCreateComponent', () => {
  let component: WorkTypeCreateComponent;
  let fixture: ComponentFixture<WorkTypeCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkTypeCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkTypeCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
