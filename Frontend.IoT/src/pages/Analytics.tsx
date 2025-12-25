import { useEffect, useState } from 'react';
import { EventsTable } from '@/components/analytics/EventsTable';
import { AnalyticsCharts } from '@/components/analytics/AnalyticsCharts';
import { eventService, mockEvents, mockAnalytics } from '@/services/eventService';
import { DoorEvent } from '@/config/api';
import { useAuth } from '@/contexts/AuthContext';
import { Skeleton } from '@/components/ui/skeleton';
import { Card, CardContent, CardHeader } from '@/components/ui/card';
import { Alert, AlertDescription } from '@/components/ui/alert';
import { Info } from 'lucide-react';

export default function Analytics() {
  const { token } = useAuth();
  const [events, setEvents] = useState<DoorEvent[]>([]);
  const [analytics, setAnalytics] = useState(mockAnalytics);
  const [isLoading, setIsLoading] = useState(true);
  const [usingMockData, setUsingMockData] = useState(false);

  useEffect(() => {
    async function fetchData() {
      if (!token) return;

      setIsLoading(true);
      try {
        const eventsData = await eventService.getEvents(token);
        const analyticsData = await eventService.getAnalytics(token);
        
        setEvents(eventsData);
        setAnalytics(analyticsData as typeof mockAnalytics);
        
        // Check if we got mock data
        if (eventsData === mockEvents) {
          setUsingMockData(true);
        }
      } catch {
        setEvents(mockEvents);
        setAnalytics(mockAnalytics);
        setUsingMockData(true);
      } finally {
        setIsLoading(false);
      }
    }

    fetchData();
  }, [token]);

  return (
    <div className="space-y-6 animate-fade-in">
      {/* Page header */}
      <div>
        <h1 className="text-3xl font-bold tracking-tight">Analytics</h1>
        <p className="text-muted-foreground">
          View door activity history and usage statistics
        </p>
      </div>

      {/* Mock data notice */}
      {usingMockData && (
        <Alert className="border-primary/30 bg-primary/5">
          <Info className="h-4 w-4 text-primary" />
          <AlertDescription className="text-sm">
            Displaying sample data. Connect the analytics API endpoints to see real data.
          </AlertDescription>
        </Alert>
      )}

      {/* Loading state */}
      {isLoading ? (
        <div className="space-y-6">
          <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
            {[...Array(3)].map((_, i) => (
              <Card key={i} className="industrial-border">
                <CardHeader>
                  <Skeleton className="h-6 w-32" />
                </CardHeader>
                <CardContent>
                  <Skeleton className="h-[200px] w-full" />
                </CardContent>
              </Card>
            ))}
          </div>
          <Card className="industrial-border">
            <CardHeader>
              <Skeleton className="h-6 w-40" />
            </CardHeader>
            <CardContent>
              <Skeleton className="h-[300px] w-full" />
            </CardContent>
          </Card>
        </div>
      ) : (
        <>
          {/* Charts */}
          <AnalyticsCharts
            opensPerDay={analytics.opensPerDay}
            openVsClosed={analytics.openVsClosed}
            eventSources={analytics.eventSources}
          />

          {/* Events table */}
          <EventsTable events={events} />
        </>
      )}
    </div>
  );
}
