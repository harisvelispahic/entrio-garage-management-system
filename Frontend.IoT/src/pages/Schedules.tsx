import { ScheduleForm } from '@/components/scheduling/ScheduleForm';
import { ScheduleList } from '@/components/scheduling/ScheduleList';
import { useSchedules } from '@/hooks/useSchedules';
import { Skeleton } from '@/components/ui/skeleton';
import { Card, CardContent, CardHeader } from '@/components/ui/card';

export default function Schedules() {
  const { schedules, isLoading, isCreating, isDeleting, createSchedule, deleteSchedule } = useSchedules();

  return (
    <div className="space-y-6 animate-fade-in">
      {/* Page header */}
      <div>
        <h1 className="text-3xl font-bold tracking-tight">Schedules</h1>
        <p className="text-muted-foreground">
          Automate your garage door with scheduled actions
        </p>
      </div>

      {/* Main grid */}
      <div className="grid gap-6 lg:grid-cols-2">
        {/* Create form */}
        <ScheduleForm onSubmit={createSchedule} isLoading={isCreating} />

        {/* Schedule list */}
        {isLoading ? (
          <Card className="industrial-border">
            <CardHeader>
              <Skeleton className="h-6 w-48" />
            </CardHeader>
            <CardContent className="space-y-3">
              {[...Array(3)].map((_, i) => (
                <Skeleton key={i} className="h-16 w-full" />
              ))}
            </CardContent>
          </Card>
        ) : (
          <ScheduleList
            schedules={schedules}
            onDelete={deleteSchedule}
            isDeleting={isDeleting}
          />
        )}
      </div>
    </div>
  );
}
