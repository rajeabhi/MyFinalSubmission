export class Task {
    TaskId: number;
    ProjectId: number;
    Task: string;
    ParentTaskId: number;
    Priority: string;
    StartDate: Date;
    EndDate: Date;
    UserId: number;
    IsEnded: boolean;
}

export class ViewTask extends Task {
    ParentTaskName: string;
    UserName: string;
    ProjectName: string;
}
