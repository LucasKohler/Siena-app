import { apiRequest } from "./client";
import type {
  AuthResultDto,
  CreateEventRequest,
  CreateUserRequest,
  CurrentUserDto,
  EventSummaryDto,
  NextTrainingDto,
  PendingAttendanceDto,
  UpdateEventRequest,
  UpdateUserRequest,
  UserSummaryDto,
  VideoSummaryDto,
} from "./types";

export function login(phoneNumber: string) {
  return apiRequest<AuthResultDto>("/api/auth/login", {
    method: "POST",
    body: { phoneNumber },
  });
}

export function getCurrentUser(token: string) {
  return apiRequest<CurrentUserDto>("/api/auth/me", { token });
}

export function getEvents() {
  return apiRequest<EventSummaryDto[]>("/api/events");
}

export function getVideos() {
  return apiRequest<VideoSummaryDto[]>("/api/videos");
}

export function getNextTraining(token: string) {
  return apiRequest<NextTrainingDto>("/api/trainings/next", { token });
}

export function setTrainingAttendance(
  eventId: string,
  status: string,
  token: string
) {
  return apiRequest<void>(`/api/trainings/${eventId}/attendance`, {
    method: "POST",
    body: { status },
    token,
  });
}

export function listAdminUsers(token: string, includeInactive = false) {
  const query = `?includeInactive=${includeInactive ? "true" : "false"}`;
  return apiRequest<UserSummaryDto[]>(`/api/admin/users${query}`, { token });
}

export function listAdminEvents(token: string) {
  return apiRequest<EventSummaryDto[]>("/api/admin/events", { token });
}

export function createAdminEvent(token: string, body: CreateEventRequest) {
  return apiRequest<void>("/api/admin/events", {
    method: "POST",
    body,
    token,
  });
}

export function updateAdminEvent(
  token: string,
  id: string,
  body: UpdateEventRequest
) {
  return apiRequest<void>(`/api/admin/events/${encodeURIComponent(id)}`, {
    method: "PUT",
    body,
    token,
  });
}

export function deleteAdminEvent(token: string, id: string) {
  return apiRequest<void>(`/api/admin/events/${encodeURIComponent(id)}`, {
    method: "DELETE",
    token,
  });
}

export function createAdminUser(token: string, body: CreateUserRequest) {
  return apiRequest<void>("/api/admin/users", {
    method: "POST",
    body,
    token,
  });
}

export function updateAdminUser(
  token: string,
  id: string,
  body: UpdateUserRequest
) {
  return apiRequest<void>(`/api/admin/users/${encodeURIComponent(id)}`, {
    method: "PUT",
    body,
    token,
  });
}

export function setAdminUserActive(
  token: string,
  id: string,
  isActive: boolean
) {
  return apiRequest<void>(
    `/api/admin/users/${encodeURIComponent(id)}/active`,
    {
      method: "PATCH",
      body: { isActive },
      token,
    }
  );
}

export function listPendingAttendances(
  token: string,
  eventId: string
) {
  return apiRequest<PendingAttendanceDto[]>(
    `/api/admin/trainings/${eventId}/attendances/pending`,
    { token }
  );
}

export function setAttendanceApproval(
  token: string,
  eventId: string,
  userId: string,
  action: "approve" | "reject"
) {
  return apiRequest<void>(
    `/api/admin/trainings/${eventId}/attendances/${userId}/approval`,
    {
      method: "POST",
      body: { action },
      token,
    }
  );
}
