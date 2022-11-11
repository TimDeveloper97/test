import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchSimilarMaterialComponent } from './search-similar-material.component';

describe('SearchSimilarMaterialComponent', () => {
  let component: SearchSimilarMaterialComponent;
  let fixture: ComponentFixture<SearchSimilarMaterialComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SearchSimilarMaterialComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchSimilarMaterialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
