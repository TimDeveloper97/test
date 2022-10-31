import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowEditContenComponent } from './show-edit-conten.component';

describe('ShowEditContenComponent', () => {
  let component: ShowEditContenComponent;
  let fixture: ComponentFixture<ShowEditContenComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShowEditContenComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowEditContenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
