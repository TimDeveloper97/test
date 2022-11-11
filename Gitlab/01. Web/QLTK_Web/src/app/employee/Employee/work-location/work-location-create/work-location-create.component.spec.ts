import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkLocationCreateComponent } from './work-location-create.component';

describe('WorkLocationCreateComponent', () => {
  let component: WorkLocationCreateComponent;
  let fixture: ComponentFixture<WorkLocationCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WorkLocationCreateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkLocationCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
