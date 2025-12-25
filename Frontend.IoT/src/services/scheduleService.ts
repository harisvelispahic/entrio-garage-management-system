import { api } from "./api";
import { Schedule, DoorCommand } from "@/config/api";

export interface CreateScheduleRequest {
  deviceId: string;
  command: DoorCommand;
  percentage?: number;
  scheduledAt: string; // ISO
}

// {
//   "deviceId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
//   "commandType": 0,
//   "targetPercentage": 0,
//   "executeAtUtc": "2025-12-26T18:14:00.439Z"
// }

export const scheduleService = {
  async getSchedules(token: string): Promise<Schedule[]> {
    return api.get<Schedule[]>("/schedules", token);
  },

  async createSchedule(schedule: CreateScheduleRequest, token: string): Promise<Schedule> {
    return api.post<Schedule>(
      "/schedules",
      {
        deviceId: "0f8fad5b-d9cb-469f-a165-70867728950e",
        commandType: schedule.command,
        targetPercentage: schedule.percentage ?? 0,
        executeAtUtc: schedule.scheduledAt,
      },
      token
    );
  },

  async deleteSchedule(id: string, token: string): Promise<void> {
    return api.delete(`/schedules/${id}`, token);
  },
};
