import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OverallProjectComponent } from './overall-project.component';

describe('OverallProjectComponent', () => {
  let component: OverallProjectComponent;
  let fixture: ComponentFixture<OverallProjectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OverallProjectComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OverallProjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
