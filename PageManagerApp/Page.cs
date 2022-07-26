using System;

namespace PageManagerApp
{
    public class Page
    {
        protected bool Equals(Page other)
        {
            return Equals(Job, other.Job);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Page) obj);
        }

        public override int GetHashCode()
        {
            return (Job != null ? Job.GetHashCode() : 0);
        }

        public static bool operator ==(Page left, Page right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Page left, Page right)
        {
            return !Equals(left, right);
        }

        public Page(Job job, int timeStamp)
        {
            Job = job ?? throw new ArgumentNullException(nameof(job));
            TimeStamp = timeStamp;
        }

        public Job Job { get; }
        public int TimeStamp { get; private set; }

       

        public void SetTime(int time)
        {
            TimeStamp = time;
        }

        public override string ToString()
        {
            return $"{nameof(Job)}: {Job}";
        }
    }
}