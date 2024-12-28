import { Injectable } from '@angular/core';
import { environment } from '../../../environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  // Crear usuario
  createUser(user: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/User`, user);
  }

  // Actualizar usuario
  updateUser(user: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/User`, user);
  }

  // Eliminar usuario
  deleteUser(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/User/${id}`);
  }

  // Listar usuarios
  listUsers(): Observable<any> {
    return this.http.get(`${this.apiUrl}/User`);
  }
}