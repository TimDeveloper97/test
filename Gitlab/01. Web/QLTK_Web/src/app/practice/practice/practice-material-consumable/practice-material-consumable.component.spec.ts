import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PracticeMaterialConsumableComponent } from './practice-material-consumable.component';

describe('PracticeMaterialConsumableComponent', () => {
  let component: PracticeMaterialConsumableComponent;
  let fixture: ComponentFixture<PracticeMaterialConsumableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PracticeMaterialConsumableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PracticeMaterialConsumableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
