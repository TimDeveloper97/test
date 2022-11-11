import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowImageErrorComponent } from './show-image-error.component';

describe('ShowImageErrorComponent', () => {
  let component: ShowImageErrorComponent;
  let fixture: ComponentFixture<ShowImageErrorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShowImageErrorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowImageErrorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
