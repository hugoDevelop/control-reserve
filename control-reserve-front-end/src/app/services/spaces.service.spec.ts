import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { SpacesService } from './spaces.service';

describe('SpacesService', () => {
  let service: SpacesService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule], // Importar HttpClientTestingModule
      providers: [SpacesService]
    });
    service = TestBed.inject(SpacesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});