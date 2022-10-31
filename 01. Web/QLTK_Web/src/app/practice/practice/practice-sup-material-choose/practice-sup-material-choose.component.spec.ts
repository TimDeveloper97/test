import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PracticeSupMaterialChooseComponent } from './practice-sup-material-choose.component';

describe('PracticeSupMaterialChooseComponent', () => {
  let component: PracticeSupMaterialChooseComponent;
  let fixture: ComponentFixture<PracticeSupMaterialChooseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PracticeSupMaterialChooseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PracticeSupMaterialChooseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
