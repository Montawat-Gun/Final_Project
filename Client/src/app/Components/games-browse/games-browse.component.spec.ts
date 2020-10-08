import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GamesBrowseComponent } from './games-browse.component';

describe('GamesBrowseComponent', () => {
  let component: GamesBrowseComponent;
  let fixture: ComponentFixture<GamesBrowseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GamesBrowseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GamesBrowseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
