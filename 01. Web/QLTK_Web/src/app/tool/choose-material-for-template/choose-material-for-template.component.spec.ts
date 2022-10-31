import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseMaterialForTemplateComponent } from './choose-material-for-template.component';

describe('ChooseMaterialForTemplateComponent', () => {
  let component: ChooseMaterialForTemplateComponent;
  let fixture: ComponentFixture<ChooseMaterialForTemplateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChooseMaterialForTemplateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseMaterialForTemplateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
