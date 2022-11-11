import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TiviTopbarComponent } from './tivi-topbar.component';

describe('TiviTopbarComponent', () => {
  let component: TiviTopbarComponent;
  let fixture: ComponentFixture<TiviTopbarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TiviTopbarComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TiviTopbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
