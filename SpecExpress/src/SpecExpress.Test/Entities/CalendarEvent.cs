using System;

namespace SpecExpress.Test.Entities
{
    public class CalendarEvent
    {
        public string Subject { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is CalendarEvent)
            {
                CalendarEvent evnt = (CalendarEvent) obj;
                return Subject.Equals(evnt.Subject) &&
                       CreateDate.Equals(evnt.CreateDate) &&
                       StartDate.Equals(evnt.StartDate) &&
                       EndDate.Equals(evnt.EndDate);
            }
            else
            {
                return base.Equals(obj);
            }
        }

        public override string ToString()
        {
            return Subject;
        }
    }

    public class NullableCalendarEvent
    {
        public string Subject { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is CalendarEvent)
            {
                CalendarEvent evnt = (CalendarEvent)obj;
                return Subject.Equals(evnt.Subject) &&
                       CreateDate.Equals(evnt.CreateDate) &&
                       StartDate.Equals(evnt.StartDate) &&
                       EndDate.Equals(evnt.EndDate);
            }
            else
            {
                return base.Equals(obj);
            }
        }

        public override string ToString()
        {
            return Subject;
        }
    }

}