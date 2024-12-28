import { Component, OnInit, ViewChild } from '@angular/core';
import { TableComponent } from "../../components/table/table.component";
import { FormComponent } from "../../components/form/form.component";
import { ReservationsService } from '../../services/reservations.service';
import { SpacesService } from '../../services/spaces.service';
import { UserService } from '../../services/user.service';
import { ToastrService } from 'ngx-toastr';
import { Validators } from '@angular/forms';
import * as bootstrap from 'bootstrap';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [TableComponent, FormComponent],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.sass']
})
export class HomeComponent implements OnInit {
  @ViewChild(FormComponent) formComponent: FormComponent | null = null;

  reservations: any[] = [];
  columns = [
    { key: 'id', label: 'ID' },
    { key: 'space', label: 'Espacio' },
    { key: 'user', label: 'Usuario' },
    { key: 'startTime', label: 'Fecha de Inicio' },
    { key: 'endTime', label: 'Fecha de Fin' }
  ];

  formFields = [
    { name: 'id', label: 'ID', type: 'text', required: true, hidden: true },
    { name: 'spaceId', label: 'Espacio', type: 'select', required: true, options: [] },
    { name: 'userId', label: 'Usuario', type: 'select', required: true, options: [] },
    { name: 'startTime', label: 'Fecha y Hora de Inicio', type: 'datetime-local', required: true },
    { name: 'endTime', label: 'Fecha y Hora de Fin', type: 'datetime-local', required: true }
  ];

  formValidations = {
    id: [Validators.required],
    spaceId: [Validators.required],
    userId: [Validators.required],
    startTime: [Validators.required],
    endTime: [Validators.required]
  };

  selectedReservation: any = {
    id: 0,
    spaceId: 0,
    userId: 0,
    startTime: '',
    endTime: ''
  };

  constructor(
    private reservationsService: ReservationsService,
    private toastr: ToastrService,
    private spacesService: SpacesService,
    private userService: UserService
  ) {}

  ngOnInit() {
    this.loadReservations();
    this.loadOptions();
  }

  loadReservations() {
    this.toastr.info('Cargando reservas...');
    this.reservationsService.listReservations({}).subscribe(
      (response) => {
        this.toastr.success('Reservas cargadas con éxito');
        this.reservations = response;
      },
      (error) => {
        console.error('Error al cargar las reservas', error);
        this.toastr.error('Error al cargar las reservas 🙁 ' + error.error.message);
      }
    );
  }

  loadOptions() {
    this.spacesService.listSpaces().subscribe(spaces => {
      const spaceField = this.formFields.find(field => field.name === 'spaceId');
      if (spaceField) {
        spaceField.options = spaces;
      }
    }, (error) => {
      this.toastr.error('Error al cargar los espacios 🙁 ' + error.error.message);
    });

    this.userService.listUsers().subscribe(users => {
      const userField = this.formFields.find(field => field.name === 'userId');
      if (userField) {
        userField.options = users;
      }
    }, (error) => {
      this.toastr.error('Error al cargar los usuarios 🙁 ' + error.error.message);
    });
  }

  onEdit(reserva: any) {
    console.log('Edit', reserva);
    console.log('Form fields', this.formFields);
    this.selectedReservation.id = reserva.id;
    this.selectedReservation.startTime = this.formatDateToLocalISO(reserva.startTime);
    this.selectedReservation.endTime = this.formatDateToLocalISO(reserva.endTime);
    // buscar por nombre de espacio en las opciones
    const spaceField = this.formFields.find(field => field.name === 'spaceId');
    if (spaceField && spaceField.options) {
      const spaceOption = spaceField.options.find((option: any) => option.name === reserva.space);
      if (spaceOption) {
        this.selectedReservation.spaceId = (spaceOption as { id: number }).id;
      }
    }
    // buscar por nombre de usuario en las opciones
    const userField = this.formFields.find(field => field.name === 'userId');
    if (userField && userField.options) {
      const userOption = userField.options.find((option: any) => option.name === reserva.user);
      if (userOption) {
        this.selectedReservation.userId = (userOption as { id: number }).id;
      }
    }
    if (this.formComponent) {
      this.formComponent.form.patchValue(this.selectedReservation);
    }
    const modal = new bootstrap.Modal(document.getElementById('editReservationModal')!);
    modal.show();
  }

  formatDateToLocalISO(date: string): string {
    const localDate = new Date(date);
    return localDate.toISOString().slice(0, 16);
  }

  onSubmitEditForm(data: any) {
    this.toastr.info('Actualizando reserva...');
    console.log('Data', data);
    this.reservationsService.updateReservation(data).subscribe(
      (response) => {
        this.toastr.success('Reserva actualizada con éxito');
        this.loadReservations(); // Recargar la lista de reservas
        const modal = bootstrap.Modal.getInstance(document.getElementById('editReservationModal')!);
        if (modal) {
          modal.hide();
        }
      },
      (error) => {
        this.toastr.error('Error al actualizar la reserva 🙁 ' + error.error.message);
      }
    );
  }

  onDelete(id: number) {
    this.toastr.info('Eliminando reserva...');
    this.reservationsService.cancelReservation(id).subscribe(
      (response) => {
        this.toastr.success('Reserva eliminada con éxito');
        this.loadReservations(); // Recargar la lista de reservas
      },
      (error) => {
        this.toastr.error('Error al eliminar la reserva 🙁 ' + error.error.message);
      }
    );
  }
}