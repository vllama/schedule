# Schedule
A schedule library with similar schedule settings as a task scheduler.

Scheduling resolution options are with the lowest being Day.:
  Once
  Daily
  Weekly
  Monthly
  And for each one can schedule more granular such as every x (day of week) etch
  
  Sample usage :
  
```c#
            var start = new DateTime(2010, 6, 3);
            var end = new DateTime(2018, 8, 13);
            var days = new[] { DayOfWeek.Sunday, DayOfWeek.Saturday };
            var weekendDays = new WeeklyOccurrence(start, end, days, 2);
```
