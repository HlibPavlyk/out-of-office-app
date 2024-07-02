import {PeoplePartnerModel} from "../employee/add-employee/people-partner.model";

export interface ProjectGetModel{
  id: number;
  projectType: string;
  startDate: string;
  endDate?: string | null;
  projectManager: PeoplePartnerModel;
  comment?: string | null;
  status: string;
}
