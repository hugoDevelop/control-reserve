import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-table',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.sass']
})
export class TableComponent {
  @Input() data: any[] = [];
  @Input() columns: { key: string, label: string }[] = [];
  @Input() title: string = 'Tabla';
  @Input() showActions: boolean = false;
  @Input() editButtonLabel: string = 'Editar';
  @Input() deleteButtonLabel: string = 'Eliminar';

  @Output() edit = new EventEmitter<any>();
  @Output() delete = new EventEmitter<number>();

  constructor(private toastr: ToastrService) {}

  cancelarReserva(id: number) {
    this.delete.emit(id);
  }

  editarReserva(reserva: any) {
    this.edit.emit(reserva);
  }
}