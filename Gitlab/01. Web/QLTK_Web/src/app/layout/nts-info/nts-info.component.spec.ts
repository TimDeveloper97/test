import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NtsInfoComponent } from './nts-info.component';

describe('NtsInfoComponent', () => {
  let component: NtsInfoComponent;
  let fixture: ComponentFixture<NtsInfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NtsInfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NtsInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
