export type AuthResultDto = {
  token: string;
  expiresAt: string;
  displayName: string;
  role: string;
};

export type CurrentUserDto = {
  id: string;
  displayName: string;
  role: string;
};

export type EventSummaryDto = {
  id: string;
  title: string;
  type: string;
  category: string;
  startsAt: string;
  location: string;
  opponent: string | null;
};

export type VideoSummaryDto = {
  id: string;
  title: string;
  url: string;
  durationSeconds: number;
  publishedAt: string;
  views: number;
};

export type ConfirmedAttendeeDto = {
  displayName: string;
  position: string | null;
};

export type NextTrainingDto = {
  eventId: string;
  title: string;
  category: string;
  startsAt: string;
  location: string;
  myStatus: string | null;
  myApprovalStatus: string | null;
  confirmed: ConfirmedAttendeeDto[];
};

export type UserSummaryDto = {
  id: string;
  phoneNumber: string;
  displayName: string;
  role: string;
  position: string | null;
  isActive: boolean;
};

export type PendingAttendanceDto = {
  userId: string;
  displayName: string;
  position: string | null;
  response: string;
  approvalStatus: string;
};

export type CreateEventRequest = {
  id: string;
  title: string;
  type: string;
  category: string;
  startsAt: string;
  location: string;
  opponent: string | null;
  description: string | null;
};

export type ApiProblem = {
  title?: string;
  detail?: string;
  status?: number;
};
