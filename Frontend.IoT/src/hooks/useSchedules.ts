import { useState, useEffect, useCallback } from 'react';
import { scheduleService, CreateScheduleRequest } from '@/services/scheduleService';
import { Schedule, DoorCommand } from '@/config/api';
import { useAuth } from '@/contexts/AuthContext';
import { toast } from '@/hooks/use-toast';

export function useSchedules() {
  const { token } = useAuth();
  const [schedules, setSchedules] = useState<Schedule[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const [isCreating, setIsCreating] = useState(false);
  const [isDeleting, setIsDeleting] = useState<string | null>(null);

  const fetchSchedules = useCallback(async () => {
    if (!token) return;

    setIsLoading(true);
    try {
      const data = await scheduleService.getSchedules(token);
      setSchedules(data);
    } catch (err) {
      console.error('Failed to fetch schedules:', err);
    } finally {
      setIsLoading(false);
    }
  }, [token]);

  useEffect(() => {
    fetchSchedules();
  }, [fetchSchedules]);

  const createSchedule = useCallback(async (
    command: DoorCommand,
    scheduledAt: string,
    percentage?: number
  ) => {
    if (!token) return;

    setIsCreating(true);
    try {
      const request: CreateScheduleRequest = {
        command,
        scheduledAt,
        percentage,
      };
      const newSchedule = await scheduleService.createSchedule(request, token);
      setSchedules((prev) => [...prev, newSchedule].sort(
        (a, b) => new Date(a.scheduledAt).getTime() - new Date(b.scheduledAt).getTime()
      ));
      toast({
        title: 'Schedule Created',
        description: 'Your schedule has been created successfully.',
      });
    } catch (err) {
      const message = err instanceof Error ? err.message : 'Failed to create schedule';
      toast({
        title: 'Error',
        description: message,
        variant: 'destructive',
      });
    } finally {
      setIsCreating(false);
    }
  }, [token]);

  const deleteSchedule = useCallback(async (id: string) => {
    if (!token) return;

    setIsDeleting(id);
    try {
      await scheduleService.deleteSchedule(id, token);
      setSchedules((prev) => prev.filter((s) => s.id !== id));
      toast({
        title: 'Schedule Deleted',
        description: 'Your schedule has been deleted.',
      });
    } catch (err) {
      const message = err instanceof Error ? err.message : 'Failed to delete schedule';
      toast({
        title: 'Error',
        description: message,
        variant: 'destructive',
      });
    } finally {
      setIsDeleting(null);
    }
  }, [token]);

  return {
    schedules,
    isLoading,
    isCreating,
    isDeleting,
    createSchedule,
    deleteSchedule,
    refreshSchedules: fetchSchedules,
  };
}
