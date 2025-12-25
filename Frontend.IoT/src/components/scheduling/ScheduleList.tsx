import { Schedule, DoorCommand } from "@/config/api";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Calendar, DoorOpen, DoorClosed, OctagonX, Wind, Trash2, Clock } from "lucide-react";
import { format } from "date-fns";

interface ScheduleListProps {
  schedules: Schedule[];
  onDelete: (id: string) => Promise<void>;
  isDeleting: string | null;
}

const commandIcons: Record<DoorCommand, React.ReactNode> = {
  [DoorCommand.OPEN]: <DoorOpen className="h-4 w-4 text-success" />,
  [DoorCommand.CLOSE]: <DoorClosed className="h-4 w-4 text-muted-foreground" />,
  [DoorCommand.STOP]: <OctagonX className="h-4 w-4 text-destructive" />,
  [DoorCommand.VENT]: <Wind className="h-4 w-4 text-primary" />,
};

const commandLabels: Record<DoorCommand, string> = {
  [DoorCommand.OPEN]: "Open",
  [DoorCommand.CLOSE]: "Close",
  [DoorCommand.STOP]: "Stop",
  [DoorCommand.VENT]: "Vent",
};

export function ScheduleList({ schedules, onDelete, isDeleting }: ScheduleListProps) {
  if (schedules.length === 0) {
    return (
      <Card className="industrial-border">
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <Calendar className="h-5 w-5 text-primary" />
            Upcoming Schedules
          </CardTitle>
        </CardHeader>
        <CardContent>
          <div className="text-center py-8 text-muted-foreground">
            <Clock className="h-12 w-12 mx-auto mb-3 opacity-30" />
            <p>No schedules created yet</p>
            <p className="text-sm">Create a schedule to automate your door</p>
          </div>
        </CardContent>
      </Card>
    );
  }

  return (
    <Card className="industrial-border">
      <CardHeader>
        <CardTitle className="flex items-center gap-2">
          <Calendar className="h-5 w-5 text-primary" />
          Upcoming Schedules ({schedules.length})
        </CardTitle>
      </CardHeader>
      <CardContent>
        <div className="space-y-3">
          {schedules.map((schedule) => (
            <div
              key={schedule.id}
              className="flex items-center justify-between p-3 rounded-lg bg-secondary/30 border border-border animate-fade-in"
            >
              <div className="flex items-center gap-3">
                <div className="h-10 w-10 rounded-full bg-secondary flex items-center justify-center">
                  {commandIcons[schedule.commandType]}
                </div>
                <div>
                  <p className="font-medium">
                    {commandLabels[schedule.commandType]}
                    {schedule.commandType === DoorCommand.VENT && schedule.targetPercentage && (
                      <span className="text-primary ml-1">({schedule.targetPercentage}%)</span>
                    )}
                  </p>
                  <p className="text-sm text-muted-foreground">
                    {schedule.executeAtUtc ? format(new Date(schedule.executeAtUtc), "PPp") : "—"}
                  </p>
                </div>
              </div>
              <Button
                variant="ghost"
                size="sm"
                className="text-destructive hover:text-destructive hover:bg-destructive/10"
                onClick={() => onDelete(schedule.id)}
                disabled={isDeleting === schedule.id}
              >
                <Trash2 className="h-4 w-4" />
              </Button>
            </div>
          ))}
        </div>
      </CardContent>
    </Card>
  );
}
