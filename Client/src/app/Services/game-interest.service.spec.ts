import { TestBed } from '@angular/core/testing';

import { GameInterestService } from './game-interest.service';

describe('GameInterestService', () => {
  let service: GameInterestService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GameInterestService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
