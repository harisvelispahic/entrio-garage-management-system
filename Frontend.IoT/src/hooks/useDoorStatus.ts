import { useState, useEffect, useCallback, useRef } from "react";
import { doorService } from "@/services/doorService";
import { DoorStatus, DoorCommand, DOOR_STATUS_POLL_INTERVAL, DoorState } from "@/config/api";
import { useAuth } from "@/contexts/AuthContext";
import { toast } from "@/hooks/use-toast";

export function useDoorStatus() {
  const { token, logout } = useAuth();
  const [status, setStatus] = useState<DoorStatus>({
    position: 0,
    state: DoorState.Error,
  });
  const [isLoading, setIsLoading] = useState(false);
  const [activeCommand, setActiveCommand] = useState<DoorCommand | null>(null);
  const [error, setError] = useState<string | null>(null);
  const intervalRef = useRef<NodeJS.Timeout | null>(null);

  const fetchStatus = useCallback(async () => {
    if (!token) return;

    try {
      const newStatus = await doorService.getStatus(token);
      setStatus(newStatus);
      setError(null);
    } catch (err) {
      const message = err instanceof Error ? err.message : "Failed to fetch status";
      if (message.includes("401")) {
        logout();
      } else {
        setError(message);
      }
    }
  }, [token, logout]);

  // Start polling
  useEffect(() => {
    if (!token) return;

    fetchStatus(); // Initial fetch

    intervalRef.current = setInterval(fetchStatus, DOOR_STATUS_POLL_INTERVAL);

    return () => {
      if (intervalRef.current) {
        clearInterval(intervalRef.current);
      }
    };
  }, [token, fetchStatus]);

  const sendCommand = useCallback(
    async (command: DoorCommand, percentage?: number) => {
      if (!token) return;

      setIsLoading(true);
      setActiveCommand(command);

      try {
        await doorService.sendCommand(command, percentage ?? null, token);

        const commandNames = {
          [DoorCommand.OPEN]: "Opening door...",
          [DoorCommand.CLOSE]: "Closing door...",
          [DoorCommand.STOP]: "Door stopped",
          [DoorCommand.VENT]: `Setting vent to ${percentage}%`,
        };

        toast({
          title: "Command Sent",
          description: commandNames[command],
        });

        // Refresh status after command
        await fetchStatus();
      } catch (err) {
        const message = err instanceof Error ? err.message : "Command failed";
        toast({
          title: "Error",
          description: message,
          variant: "destructive",
        });
      } finally {
        setIsLoading(false);
        setActiveCommand(null);
      }
    },
    [token, fetchStatus]
  );

  return {
    status,
    isLoading,
    activeCommand,
    error,
    sendCommand,
    refreshStatus: fetchStatus,
  };
}
