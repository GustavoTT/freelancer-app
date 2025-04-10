export interface Project{
    projectId: number;
    userId: number;
    title: string;
    description: string;
    budget: number;
    deadline: string;
    skillsRequired?: string;
    status: boolean;
    createdDate: string;
}