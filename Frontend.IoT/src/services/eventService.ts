import { api } from './api';
import { DoorEvent } from '@/config/api';

// Mock data for analytics when API is not available
export const mockEvents: DoorEvent[] = [
  { id: '1', eventType: 'Door Opened', source: 'RFID', user: 'John', timestamp: new Date(Date.now() - 1000 * 60 * 5).toISOString() },
  { id: '2', eventType: 'Door Closed', source: 'Schedule', timestamp: new Date(Date.now() - 1000 * 60 * 30).toISOString() },
  { id: '3', eventType: 'Door Opened', source: 'Web', user: 'Admin', timestamp: new Date(Date.now() - 1000 * 60 * 60).toISOString() },
  { id: '4', eventType: 'Vent 50%', source: 'Web', user: 'Admin', timestamp: new Date(Date.now() - 1000 * 60 * 120).toISOString() },
  { id: '5', eventType: 'Door Closed', source: 'RFID', user: 'Jane', timestamp: new Date(Date.now() - 1000 * 60 * 180).toISOString() },
  { id: '6', eventType: 'Door Opened', source: 'Manual', timestamp: new Date(Date.now() - 1000 * 60 * 240).toISOString() },
  { id: '7', eventType: 'Emergency Stop', source: 'Web', user: 'Admin', timestamp: new Date(Date.now() - 1000 * 60 * 300).toISOString() },
  { id: '8', eventType: 'Door Opened', source: 'Schedule', timestamp: new Date(Date.now() - 1000 * 60 * 60 * 24).toISOString() },
];

// Mock analytics data
export const mockAnalytics = {
  opensPerDay: [
    { day: 'Mon', opens: 12 },
    { day: 'Tue', opens: 8 },
    { day: 'Wed', opens: 15 },
    { day: 'Thu', opens: 10 },
    { day: 'Fri', opens: 18 },
    { day: 'Sat', opens: 5 },
    { day: 'Sun', opens: 3 },
  ],
  openVsClosed: [
    { name: 'Open', value: 35, color: 'hsl(185, 70%, 50%)' },
    { name: 'Closed', value: 65, color: 'hsl(220, 15%, 35%)' },
  ],
  eventSources: [
    { name: 'RFID', value: 45, color: 'hsl(185, 70%, 50%)' },
    { name: 'Web App', value: 30, color: 'hsl(35, 90%, 55%)' },
    { name: 'Schedule', value: 20, color: 'hsl(145, 70%, 45%)' },
    { name: 'Manual', value: 5, color: 'hsl(220, 15%, 55%)' },
  ],
};

export const eventService = {
  async getEvents(token: string): Promise<DoorEvent[]> {
    try {
      return await api.get<DoorEvent[]>('/events', token);
    } catch {
      // Return mock data if API fails
      console.log('Using mock event data');
      return mockEvents;
    }
  },

  async getAnalytics(token: string) {
    try {
      return await api.get('/analytics', token);
    } catch {
      // Return mock data if API fails
      console.log('Using mock analytics data');
      return mockAnalytics;
    }
  },
};
