import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PracticeFileComponent } from './practice-file.component';

describe('PracticeFileComponent', () => {
  let component: PracticeFileComponent;
  let fixture: ComponentFixture<PracticeFileComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PracticeFileComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PracticeFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
