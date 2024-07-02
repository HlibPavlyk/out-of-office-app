export interface ProjectPostDTO {
  projectType: string;
  startDate: string;
  endDate?: string | null;
  projectManagerId: number;
  comment?: string | null;
  status: string;
}
