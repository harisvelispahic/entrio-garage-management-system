import { api } from './api';
import { DoorStatus, CommandRequest, DoorCommand } from '@/config/api';

export const doorService = {
  async getStatus(token: string): Promise<DoorStatus> {
    return api.get<DoorStatus>('/door/status', token);
  },

  async sendCommand(command: DoorCommand, percentage: number | null, token: string): Promise<void> {
    const request: CommandRequest = {
      command,
      percentage,
    };
    return api.post('/door/command', request, token);
  },

  async open(token: string): Promise<void> {
    return this.sendCommand(DoorCommand.OPEN, null, token);
  },

  async close(token: string): Promise<void> {
    return this.sendCommand(DoorCommand.CLOSE, null, token);
  },

  async stop(token: string): Promise<void> {
    return this.sendCommand(DoorCommand.STOP, null, token);
  },

  async vent(percentage: number, token: string): Promise<void> {
    return this.sendCommand(DoorCommand.VENT, percentage, token);
  },
};
