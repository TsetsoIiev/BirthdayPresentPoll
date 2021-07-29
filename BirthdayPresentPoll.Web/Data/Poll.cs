using BirthdayPresentPoll.Web.Areas.Identity.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BirthdayPresentPoll.Web.Data
{
    public class Poll
    {
        public int Id { get; set; }
        public string InitiatorId { get; set; }
        public string CelebrantId { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("InitiatorId")]
        public virtual ApplicationUser Initiator { get; set; }
        [ForeignKey("CelebrantId")]
        public virtual ApplicationUser Celebrant { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }
    }
}
