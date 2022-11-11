import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SBUCreateComponent } from './sbucreate.component';

describe('SBUCreateComponent', () => {
  let component: SBUCreateComponent;
  let fixture: ComponentFixture<SBUCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SBUCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SBUCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
