import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ReservationsService } from './reservations.service';

describe('ReservationsService', () => {
  let service: ReservationsService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule], // Importar HttpClientTestingModule
      providers: [ReservationsService]
    });
    service = TestBed.inject(ReservationsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});