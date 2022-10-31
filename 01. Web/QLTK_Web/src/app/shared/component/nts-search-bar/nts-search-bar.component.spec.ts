import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NTSSearchBarComponent } from './nts-search-bar.component';

describe('NTSSearchBarComponent', () => {
  let component: NTSSearchBarComponent;
  let fixture: ComponentFixture<NTSSearchBarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NTSSearchBarComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NTSSearchBarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
