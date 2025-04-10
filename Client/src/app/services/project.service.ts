import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Project } from '../models/project.model';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {

  private API_URL = 'http://localhost:5104/api/Projects';

  constructor(private http: HttpClient) {}

  getProjects(): Observable<any[]> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.get<any[]>(this.API_URL, { headers });
  }

  createProject(project: Project): Observable<any> {
    return this.http.post(this.API_URL, project);
  }

  updateProject(id: number, project: Project): Observable<any> {
    return this.http.put(`${this.API_URL}/${id}`, project);
  }

  deleteProject(id: number): Observable<any> {
    return this.http.delete(`${this.API_URL}/${id}`);
  }

  getProjectById(id: number): Observable<Project> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.get<Project>(`${this.API_URL}/${id}`, { headers });
  }
  
}
