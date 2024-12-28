import { Component, OnInit, ViewChild } from '@angular/core';
import { TableComponent } from '../../components/table/table.component';
import { FormComponent } from '../../components/form/form.component';
import { UserService } from '../../services/user.service';
import { Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import * as bootstrap from 'bootstrap';

@Component({
  selector: 'app-manage-user',
  imports: [TableComponent, FormComponent],
  templateUrl: './manage-user.component.html',
  styleUrls: ['./manage-user.component.sass']
})
export class ManageUserComponent implements OnInit {
  @ViewChild('createUserForm') createUserFormComponent: FormComponent | null = null;
  @ViewChild('editUserForm') editUserFormComponent: FormComponent | null = null;

  users: any[] = [];
  columns = [
    { key: 'id', label: 'ID' },
    { key: 'name', label: 'Nombre' },
    { key: 'email', label: 'Correo Electrónico' }
  ];

  formFields = [
    { name: 'id', label: 'ID', type: 'text', required: false, hidden: true },
    { name: 'name', label: 'Nombre', type: 'text', required: true },
    { name: 'email', label: 'Correo Electrónico', type: 'email', required: true }
  ];

  formValidations = {
    name: [Validators.required],
    email: [Validators.required, Validators.email]
  };

  selectedUser: any = {
    id: 0,
    name: '',
    email: ''
  };

  initialData = {
    id: 0,
    name: '',
    email: ''
  };

  constructor(private userService: UserService, private toastr: ToastrService) {}

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    this.userService.listUsers().subscribe(
      (response) => {
        this.users = response;
      },
      (error) => {
        this.toastr.error('Error al cargar los usuarios 🙁 ' + error.error.message);
      }
    );
  }

  onSubmitForm(data: any) {
    this.toastr.info('Enviando usuario...');
    this.userService.createUser(data).subscribe(
      (response) => {
        this.toastr.success('Usuario creado con éxito');
        this.resetForm(); // Limpiar el formulario
        this.loadUsers(); // Recargar la lista de usuarios
      },
      (error) => {
        this.toastr.error('Error al crear el usuario 🙁 ' + error.error.message);
      }
    );
  }

  onEdit(user: any) {
    console.log('Editar usuario', user);
    this.selectedUser.id = user.id;
    this.selectedUser.name = user.name;
    this.selectedUser.email = user.email;
    if (this.editUserFormComponent) {
      this.editUserFormComponent.form.patchValue(this.selectedUser);
    }
    const modal = new bootstrap.Modal(document.getElementById('editUserModal')!);
    modal.show();
  }

  onSubmitEditForm(data: any) {
    this.toastr.info('Actualizando usuario...');
    this.userService.updateUser(data).subscribe(
      (response) => {
        this.toastr.success('Usuario actualizado con éxito');
        this.loadUsers(); // Recargar la lista de usuarios
        const modal = bootstrap.Modal.getInstance(document.getElementById('editUserModal')!);
        if (modal) {
          modal.hide();
        }
      },
      (error) => {
        this.toastr.error('Error al actualizar el usuario 🙁 ' + error.error.message);
      }
    );
  }

  onDelete(id: number) {
    this.toastr.info('Eliminando usuario...');
    this.userService.deleteUser(id).subscribe(
      (response) => {
        this.toastr.success('Usuario eliminado con éxito');
        this.loadUsers(); // Recargar la lista de usuarios
      },
      (error) => {
        this.toastr.error('Error al eliminar el usuario 🙁 ' + error.error.message);
      }
    );
  }

  resetForm() {
    if (this.createUserFormComponent) {
      this.createUserFormComponent.form.reset(this.initialData);
    }
  }
}