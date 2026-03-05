using System;

namespace GenericFunctions.Models
{
    public class CreateTrackableEntity
    {
        public virtual DateTime CreatedOn { get; set; } = DateTime.Now;
        public virtual string CreatedBy { get; set; }
    }

    public class TrackableEntity : CreateTrackableEntity
    {
        public virtual DateTime? UpdatedOn { get; set; }
        public virtual string UpdatedBy { get; set; }
    }
}
