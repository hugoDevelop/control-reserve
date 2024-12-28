import { Injectable } from '@angular/core';
import { environment } from '../../../environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SpacesService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  // Crear espacio
  createSpace(space: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/Space`, space);
  }

  // Actualizar espacio
  updateSpace(space: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/Space`, space);
  }

  // Eliminar espacio
  deleteSpace(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/Space/${id}`);
  }

  // Listar espacios
  listSpaces(): Observable<any> {
    return this.http.get(`${this.apiUrl}/Space`);
  }
}