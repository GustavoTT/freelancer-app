import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-projects',
  imports: [CommonModule],
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.css']
})
export class ProjectsComponent implements OnInit {
  projects: any[] = [];

  constructor(private http: HttpClient, private router: Router, private authService: AuthService, private toastr: ToastrService) {}

  ngOnInit(): void {
    this.loadProjects();
  }

  loadProjects() {
    this.http.get<any[]>('http://localhost:5104/api/Projects')
      .subscribe({
        next: (data) => {
          this.projects = data;
          this.toastr.success('Projetos carregados com sucesso!');
        },
        error: () => {
          this.toastr.error('Erro ao carregar projetos. Tente novamente mais tarde.');
        }
      });
    
  }

  editProject(id: string) {
    this.router.navigate(['/projects/edit', id]);
  }  

  deleteProject(id: string) {
    if (confirm('Tem certeza que deseja deletar este projeto?')) {
      this.http.delete(`http://localhost:5104/api/Projects/${id}`).subscribe({
        next: () => {
          this.toastr.success('Projeto deletado com sucesso!');
          this.loadProjects();
        },
        error: () => {
          this.toastr.error('Erro ao deletar projeto. Tente novamente.');
        }
      });
    }
  }

  logout() {
    this.authService.logout();
  }

  createProject() {
    this.router.navigate(['/projects/create']);
  }
}
