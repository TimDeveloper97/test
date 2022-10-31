import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExpertCreateComponent } from './expert-create.component';

describe('ExpertCreateComponent', () => {
  let component: ExpertCreateComponent;
  let fixture: ComponentFixture<ExpertCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExpertCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExpertCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
