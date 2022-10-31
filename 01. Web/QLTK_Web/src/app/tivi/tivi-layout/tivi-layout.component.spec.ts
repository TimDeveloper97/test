import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TiviLayoutComponent } from './tivi-layout.component';

describe('TiviLayoutComponent', () => {
  let component: TiviLayoutComponent;
  let fixture: ComponentFixture<TiviLayoutComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TiviLayoutComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TiviLayoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
