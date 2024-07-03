import {PeoplePartnerModel} from "../employee/add-employee/people-partner.model";
import {LeaveRequestGetModel} from "../leave-requests/leave-request-get.model";

export interface ApprovalRequestGetModel {
  id: number;
  approver: PeoplePartnerModel;
  leaveRequestId: number;
  status: string;
  comment?: string | null;
}
