import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NtsTextMoreComponent } from './nts-text-more.component';

describe('NtsTextMoreComponent', () => {
  let component: NtsTextMoreComponent;
  let fixture: ComponentFixture<NtsTextMoreComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NtsTextMoreComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NtsTextMoreComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
