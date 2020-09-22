import { TestBed } from '@angular/core/testing';

import { GenreInterestService } from './genre-interest.service';

describe('GenreInterestService', () => {
  let service: GenreInterestService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GenreInterestService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
