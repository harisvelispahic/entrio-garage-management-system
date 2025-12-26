import { api } from "./api";
import { mockAnalytics } from "./eventService";

const openClosedColors: Record<string, string> = {
  Open: "hsl(185, 70%, 50%)",
  Closed: "hsl(220, 15%, 35%)",
};

const sourceColors: Record<string, string> = {
  // from backend
  Remote: "hsl(35, 90%, 55%)",
  System: "hsl(145, 70%, 45%)",
  LocalRfid: "hsl(185, 70%, 50%)",

  // fallback / older mock variants
  RFID: "hsl(185, 70%, 50%)",
  "Web App": "hsl(35, 90%, 55%)",
  Web: "hsl(35, 90%, 55%)",
  Schedule: "hsl(145, 70%, 45%)",
  Manual: "hsl(220, 15%, 55%)",

  default: "hsl(0, 0%, 55%)",
};

export interface AnalyticsResponse {
  opensPerDay: { day: string; opens: number }[];
  openVsClosed: { name: string; value: number; color: string }[];
  eventSources: { name: string; value: number; color: string }[];
}

export const analyticsService = {
  async get(token: string): Promise<AnalyticsResponse> {
    try {
      const raw = await api.get<AnalyticsResponse>("/analytics", token);

      return {
        ...raw,

        openVsClosed: raw.openVsClosed.map((x) => ({
          name: x.name,
          value: x.value,
          color: openClosedColors[x.name] ?? "hsl(0, 0%, 55%)",
        })),

        eventSources: raw.eventSources.map((x) => ({
          name: x.name,
          value: x.value,
          color: sourceColors[x.name] ?? sourceColors.default,
        })),
      };
    } catch {
      console.log("Using mock analytics data");
      return mockAnalytics as AnalyticsResponse;
    }
  },
};
