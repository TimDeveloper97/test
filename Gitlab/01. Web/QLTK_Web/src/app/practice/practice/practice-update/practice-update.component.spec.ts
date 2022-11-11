import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PracticeUpdateComponent } from './practice-update.component';

describe('PracticeUpdateComponent', () => {
  let component: PracticeUpdateComponent;
  let fixture: ComponentFixture<PracticeUpdateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PracticeUpdateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PracticeUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
