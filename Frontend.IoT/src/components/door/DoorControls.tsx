import { useState } from 'react';
import { Button } from '@/components/ui/button';
import { Slider } from '@/components/ui/slider';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { DoorOpen, DoorClosed, OctagonX, Wind, Loader2 } from 'lucide-react';
import { DoorCommand } from '@/config/api';
import { cn } from '@/lib/utils';

interface DoorControlsProps {
  onCommand: (command: DoorCommand, percentage?: number) => Promise<void>;
  isLoading: boolean;
  activeCommand: DoorCommand | null;
}

export function DoorControls({ onCommand, isLoading, activeCommand }: DoorControlsProps) {
  const [ventPercentage, setVentPercentage] = useState(50);

  const handleCommand = async (command: DoorCommand, percentage?: number) => {
    await onCommand(command, percentage);
  };

  const isCommandActive = (command: DoorCommand) => 
    isLoading && activeCommand === command;

  return (
    <Card className="industrial-border">
      <CardHeader>
        <CardTitle className="flex items-center gap-2">
          <Wind className="h-5 w-5 text-primary" />
          Door Controls
        </CardTitle>
      </CardHeader>
      <CardContent className="space-y-6">
        {/* Main control buttons */}
        <div className="grid grid-cols-3 gap-3">
          <Button
            size="lg"
            className={cn(
              "h-20 flex-col gap-2 transition-all",
              "bg-success/20 hover:bg-success/30 text-success border border-success/30",
              isCommandActive(DoorCommand.OPEN) && "glow-success"
            )}
            onClick={() => handleCommand(DoorCommand.OPEN)}
            disabled={isLoading}
          >
            {isCommandActive(DoorCommand.OPEN) ? (
              <Loader2 className="h-6 w-6 animate-spin" />
            ) : (
              <DoorOpen className="h-6 w-6" />
            )}
            <span className="text-xs font-medium">OPEN</span>
          </Button>

          <Button
            size="lg"
            className={cn(
              "h-20 flex-col gap-2 transition-all",
              "bg-destructive/20 hover:bg-destructive/30 text-destructive border border-destructive/30"
            )}
            onClick={() => handleCommand(DoorCommand.STOP)}
            disabled={isLoading && activeCommand !== DoorCommand.STOP}
          >
            {isCommandActive(DoorCommand.STOP) ? (
              <Loader2 className="h-6 w-6 animate-spin" />
            ) : (
              <OctagonX className="h-6 w-6" />
            )}
            <span className="text-xs font-medium">STOP</span>
          </Button>

          <Button
            size="lg"
            className={cn(
              "h-20 flex-col gap-2 transition-all",
              "bg-secondary hover:bg-secondary/80 text-foreground border border-border"
            )}
            onClick={() => handleCommand(DoorCommand.CLOSE)}
            disabled={isLoading}
          >
            {isCommandActive(DoorCommand.CLOSE) ? (
              <Loader2 className="h-6 w-6 animate-spin" />
            ) : (
              <DoorClosed className="h-6 w-6" />
            )}
            <span className="text-xs font-medium">CLOSE</span>
          </Button>
        </div>

        {/* Vent control */}
        <div className="space-y-4 pt-4 border-t border-border">
          <div className="flex items-center justify-between">
            <span className="text-sm font-medium flex items-center gap-2">
              <Wind className="h-4 w-4 text-primary" />
              Ventilation
            </span>
            <span className="text-sm font-mono text-primary">{ventPercentage}%</span>
          </div>

          <Slider
            value={[ventPercentage]}
            onValueChange={(value) => setVentPercentage(value[0])}
            min={1}
            max={99}
            step={1}
            disabled={isLoading}
            className="py-2"
          />

          <Button
            className={cn(
              "w-full transition-all",
              "bg-primary/20 hover:bg-primary/30 text-primary border border-primary/30",
              isCommandActive(DoorCommand.VENT) && "glow-primary"
            )}
            onClick={() => handleCommand(DoorCommand.VENT, ventPercentage)}
            disabled={isLoading}
          >
            {isCommandActive(DoorCommand.VENT) ? (
              <>
                <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                Setting Vent...
              </>
            ) : (
              <>
                <Wind className="mr-2 h-4 w-4" />
                Set Vent to {ventPercentage}%
              </>
            )}
          </Button>
        </div>
      </CardContent>
    </Card>
  );
}
