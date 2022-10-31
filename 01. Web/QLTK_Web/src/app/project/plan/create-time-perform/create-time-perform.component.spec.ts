import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateTimePerformComponent } from './create-time-perform.component';

describe('CreateTimePerformComponent', () => {
  let component: CreateTimePerformComponent;
  let fixture: ComponentFixture<CreateTimePerformComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateTimePerformComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateTimePerformComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
