import {PeoplePartnerModel} from "../employee/add-employee/people-partner.model";

export interface LeaveRequestGetModel {
  id: number;
  employee: PeoplePartnerModel;
  absenceReason: string;
  startDate: string;
  endDate: string;
  comment?: string | null;
  status: string;
}
