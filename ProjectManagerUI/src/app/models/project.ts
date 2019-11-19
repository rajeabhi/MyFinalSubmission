export class Project {
    ProjectId: number;
    ProjectName: string;
    Priority: string;
    StartDate: Date;
    EndDate: Date;
    ManagerId: number;
    IsSuspended: boolean;
}

export class ViewProject extends Project {
    ManagerName: string;
    TotalTasks: number;
    CompletedTasks: number;
}