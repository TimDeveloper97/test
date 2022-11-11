import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ShowSelectSkillEmployeeComponent } from './show-select-skill-employee.component';

describe('ShowSelectSkillEmployeeComponent', () => {
  let component: ShowSelectSkillEmployeeComponent;
  let fixture: ComponentFixture<ShowSelectSkillEmployeeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShowSelectSkillEmployeeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowSelectSkillEmployeeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
