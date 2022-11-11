import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TiviListComponent } from './tivi-list.component';

describe('TiviListComponent', () => {
  let component: TiviListComponent;
  let fixture: ComponentFixture<TiviListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TiviListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TiviListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
