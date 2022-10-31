import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PracticeContentComponent } from './practice-content.component';

describe('PracticeContentComponent', () => {
  let component: PracticeContentComponent;
  let fixture: ComponentFixture<PracticeContentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PracticeContentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PracticeContentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
