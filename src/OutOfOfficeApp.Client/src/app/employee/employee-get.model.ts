import {PeoplePartnerModel} from "./add-employee/people-partner.model";

export interface EmployeeGetModel{
    id: number;
    fullName: string;
    subdivision: string;
    position: string;
    status: string;
    peoplePartner: PeoplePartnerModel;
    outOfOfficeBalance: number;
}

