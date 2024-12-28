import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environment';

@Injectable({
  providedIn: 'root',
})
export class ReservationsService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  // Crear reserva
  createReservation(reservation: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/Reservations`, reservation);
  }

  // Cancelar reserva
  cancelReservation(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/Reservations/${id}`);
  }

  // Actualizar reserva
  updateReservation(reservation: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/Reservations`, reservation);
  }

  // Listar reservas con filtros
  listReservations(filters: any): Observable<any> {
    let params = new HttpParams();
    if (filters.spaceId) params = params.set('spaceId', filters.spaceId);
    if (filters.userId) params = params.set('userId', filters.userId);
    if (filters.startTime) params = params.set('startTime', filters.startDate);
    if (filters.endTime) params = params.set('endTime', filters.endDate);

    return this.http.get(`${this.apiUrl}/Reservations`, { params });
  }
}