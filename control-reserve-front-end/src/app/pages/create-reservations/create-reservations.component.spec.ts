import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CreateReservationsComponent } from './create-reservations.component';
import { ReservationsService } from '../../services/reservations.service';
import { SpacesService } from '../../services/spaces.service';
import { UserService } from '../../services/user.service';

describe('CreateReservationsComponent', () => {
  let component: CreateReservationsComponent;
  let fixture: ComponentFixture<CreateReservationsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, ToastrModule.forRoot(), BrowserAnimationsModule, CreateReservationsComponent], // Importar BrowserAnimationsModule
      providers: [ReservationsService, SpacesService, UserService]
    }).compileComponents();

    fixture = TestBed.createComponent(CreateReservationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});