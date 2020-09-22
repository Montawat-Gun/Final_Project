import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SuggestItemComponent } from './suggest-item.component';

describe('SuggestItemComponent', () => {
  let component: SuggestItemComponent;
  let fixture: ComponentFixture<SuggestItemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SuggestItemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SuggestItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
