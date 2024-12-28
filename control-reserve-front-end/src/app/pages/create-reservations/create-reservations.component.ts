import { Component, OnInit, ViewChild } from '@angular/core';
import { FormComponent } from "../../components/form/form.component";
import { SpacesService } from '../../services/spaces.service';
import { UserService } from '../../services/user.service';
import { Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ReservationsService } from '../../services/reservations.service';

@Component({
  selector: 'app-create-reservations',
  imports: [FormComponent],
  templateUrl: './create-reservations.component.html',
  styleUrls: ['./create-reservations.component.sass']
})
export class CreateReservationsComponent implements OnInit {
  @ViewChild(FormComponent) formComponent: FormComponent | null = null;

  formFields = [
    { name: 'spaceId', label: 'Espacio', type: 'select', required: true, options: [] },
    { name: 'userId', label: 'Usuario', type: 'select', required: true, options: [] },
    { name: 'startTime', label: 'Fecha y Hora de Inicio', type: 'datetime-local', required: true },
    { name: 'endTime', label: 'Fecha y Hora de Fin', type: 'datetime-local', required: true }
  ];

  formValidations = {
    spaceId: [Validators.required],
    userId: [Validators.required],
    startTime: [Validators.required],
    endTime: [Validators.required]
  };

  initialData = {
    spaceId: 0,
    userId: 0,
    startTime: '',
    endTime: ''
  };

  constructor(private reservationsService: ReservationsService, private toastr: ToastrService, private spacesService: SpacesService, private userService: UserService) { }

  ngOnInit() {
    this.toastr.info('Cargando datos...');

    this.spacesService.listSpaces().subscribe(spaces => {
      const spaceField = this.formFields.find(field => field.name === 'spaceId');
      if (spaceField) {
        spaceField.options = spaces;
        this.toastr.success('Datos de espacios cargados con éxito');
      }
    }, (error) => {
      this.toastr.error('Error al cargar los espacios 🙁 ' + error.error.message)
    });

    this.userService.listUsers().subscribe(users => {
      const userField = this.formFields.find(field => field.name === 'userId');
      if (userField) {
        userField.options = users;
        this.toastr.success('Datos de usuarios cargados con éxito');
      }
    }, (error) => {
      this.toastr.error('Error al cargar los usuarios 🙁 ' + error.error.message)
    });
  }

  onSubmitForm(data: any) {
    this.toastr.info('Enviando reserva...');
    this.reservationsService.createReservation(data).subscribe((response) => {
      this.toastr.success('Reserva creada con éxito');
      this.resetForm(); // Limpiar el formulario
    }, (error) => {
      this.toastr.error('Error al crear la reserva 🙁 ' + error.error.message);
    });
  }

  resetForm() {
    if (this.formComponent) {
      this.formComponent.form.reset(this.initialData);
    }
  }
}