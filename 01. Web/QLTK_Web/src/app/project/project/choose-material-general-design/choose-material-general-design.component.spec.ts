import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseMaterialGeneralDesignComponent } from './choose-material-general-design.component';

describe('ChooseMaterialGeneralDesignComponent', () => {
  let component: ChooseMaterialGeneralDesignComponent;
  let fixture: ComponentFixture<ChooseMaterialGeneralDesignComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChooseMaterialGeneralDesignComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseMaterialGeneralDesignComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
