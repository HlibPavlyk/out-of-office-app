export interface LeaveRequestPostModel {
  absenceReason: string; // Assuming AbsenceReason is defined elsewhere
  startDate: string;
  endDate: string;
  comment?: string | null;
}
