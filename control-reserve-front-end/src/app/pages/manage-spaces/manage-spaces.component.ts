import { Component, OnInit, ViewChild } from '@angular/core';
import { TableComponent } from '../../components/table/table.component';
import { FormComponent } from '../../components/form/form.component';
import { SpacesService } from '../../services/spaces.service';
import { Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import * as bootstrap from 'bootstrap';

@Component({
  selector: 'app-manage-spaces',
  imports: [TableComponent, FormComponent],
  templateUrl: './manage-spaces.component.html',
  styleUrls: ['./manage-spaces.component.sass']
})
export class ManageSpacesComponent implements OnInit {
  @ViewChild('createSpaceForm') createSpaceFormComponent: FormComponent | null = null;
  @ViewChild('editSpaceForm') editSpaceFormComponent: FormComponent | null = null;

  spaces: any[] = [];
  columns = [
    { key: 'id', label: 'ID' },
    { key: 'name', label: 'Nombre' },
    { key: 'description', label: 'Descripción' }
  ];

  formFields = [
    { name: 'id', label: 'ID', type: 'text', required: false, hidden: true },
    { name: 'name', label: 'Nombre', type: 'text', required: true },
    { name: 'description', label: 'Descripción', type: 'text', required: true }
  ];

  formValidations = {
    name: [Validators.required],
    description: [Validators.required]
  };

  selectedSpace: any = {
    id: 0,
    name: '',
    description: ''
  };

  initialData = {
    id: 0,
    name: '',
    description: ''
  };

  constructor(private spacesService: SpacesService, private toastr: ToastrService) {}

  ngOnInit() {
    this.loadSpaces();
  }

  loadSpaces() {
    this.spacesService.listSpaces().subscribe(
      (response) => {
        this.spaces = response;
      },
      (error) => {
        this.toastr.error('Error al cargar los espacios 🙁 ' + error.error.message);
      }
    );
  }

  onSubmitForm(data: any) {
    this.toastr.info('Enviando espacio...');
    this.spacesService.createSpace(data).subscribe(
      (response) => {
        this.toastr.success('Espacio creado con éxito');
        this.resetForm(); // Limpiar el formulario
        this.loadSpaces(); // Recargar la lista de espacios
      },
      (error) => {
        this.toastr.error('Error al crear el espacio 🙁 ' + error.error.message);
      }
    );
  }

  onEdit(space: any) {
    console.log('Editar espacio', space);
    this.selectedSpace.id = space.id;
    this.selectedSpace.name = space.name;
    this.selectedSpace.description = space.description;
    if (this.editSpaceFormComponent) {
      this.editSpaceFormComponent.form.patchValue(this.selectedSpace);
    }
    const modal = new bootstrap.Modal(document.getElementById('editSpaceModal')!);
    modal.show();
  }

  onSubmitEditForm(data: any) {
    this.toastr.info('Actualizando espacio...');
    this.spacesService.updateSpace(data).subscribe(
      (response) => {
        this.toastr.success('Espacio actualizado con éxito');
        this.loadSpaces(); // Recargar la lista de espacios
        const modal = bootstrap.Modal.getInstance(document.getElementById('editSpaceModal')!);
        if (modal) {
          modal.hide();
        }
      },
      (error) => {
        this.toastr.error('Error al actualizar el espacio 🙁 ' + error.error.message);
      }
    );
  }

  onDelete(id: number) {
    this.toastr.info('Eliminando espacio...');
    this.spacesService.deleteSpace(id).subscribe(
      (response) => {
        this.toastr.success('Espacio eliminado con éxito');
        this.loadSpaces(); // Recargar la lista de espacios
      },
      (error) => {
        this.toastr.error('Error al eliminar el espacio 🙁 ' + error.error.message);
      }
    );
  }

  resetForm() {
    if (this.createSpaceFormComponent) {
      this.createSpaceFormComponent.form.reset(this.initialData);
    }
  }
}