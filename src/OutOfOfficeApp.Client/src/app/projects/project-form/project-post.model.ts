export interface ProjectPostDTO {
  projectType: string;
  projectManagerId: number;
  status: string;
  comment?: string | null;
}
