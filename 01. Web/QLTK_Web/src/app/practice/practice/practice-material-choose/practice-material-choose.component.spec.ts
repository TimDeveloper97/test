import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PracticeMaterialChooseComponent } from './practice-material-choose.component';

describe('PracticeMaterialChooseComponent', () => {
  let component: PracticeMaterialChooseComponent;
  let fixture: ComponentFixture<PracticeMaterialChooseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PracticeMaterialChooseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PracticeMaterialChooseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
