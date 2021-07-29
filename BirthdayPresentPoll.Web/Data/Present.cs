using System.Collections.Generic;

namespace BirthdayPresentPoll.Web.Data
{
    public class Present
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }
    }
}
