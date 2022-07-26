namespace PageManagerApp
{
    public class Job
    {
        protected bool Equals(Job other)
        {
            return JobNumber == other.JobNumber && PageNumber == other.PageNumber;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Job) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (JobNumber * 397) ^ PageNumber;
            }
        }

        public static bool operator ==(Job left, Job right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Job left, Job right)
        {
            return !Equals(left, right);
        }

        public Job(int jobNumber, int pageNumber)
        {
            JobNumber = jobNumber;
            PageNumber = pageNumber;
        }

        public int JobNumber { get; }
        public int PageNumber { get; }

        public override string ToString()
        {
            return $"#: {JobNumber}, Page: {PageNumber}";
        }
    }
}