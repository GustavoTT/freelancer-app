import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProjectService } from '../../services/project.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-project-form',
  imports: [CommonModule, FormsModule],
  templateUrl: './project-form.component.html',
  styleUrls: ['./project-form.component.css']
})
export class ProjectFormComponent implements OnInit {
  project: any = {
    title: '',
    description: '',
    budget: 0,
    deadline: '',
    skillsRequired: ''
  };
  isEditMode = false;
  projectId: string | null = null;

  constructor(private toastr: ToastrService, private route: ActivatedRoute, private router: Router, private projectService: ProjectService) {}

  ngOnInit(): void {
    this.projectId = this.route.snapshot.paramMap.get('id');
    if (this.projectId) {
      this.isEditMode = true;
      this.projectService.getProjectById(+this.projectId).subscribe(data => {
        this.project = data;
      });
    }
  }
  
  saveProject() {
    if (this.isEditMode) {
      this.projectService.updateProject(+this.projectId!, {
        ...this.project,
        status: true
      }).subscribe({
        next: () => {
          this.toastr.success('Projeto atualizado com sucesso!');
          this.router.navigate(['/projects']);
        },
        error: () => {
          this.toastr.error('Erro ao atualizar o projeto.');
        }
      });
    } else {
      this.projectService.createProject(this.project).subscribe({
        next: () => {
          this.toastr.success('Projeto criado com sucesso!');
          this.router.navigate(['/projects']);
        },
        error: () => {
          this.toastr.error('Erro ao criar o projeto.');
        }
      });
    }
  }  

  cancel() {
    this.router.navigate(['/projects']);
  }
}
