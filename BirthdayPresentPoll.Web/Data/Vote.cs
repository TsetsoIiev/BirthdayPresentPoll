using BirthdayPresentPoll.Web.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace BirthdayPresentPoll.Web.Data
{
    public class Vote
    {
        public int Id { get; set; }
        public string VoterId { get; set; }
        public int PollId { get; set; }
        public int? PresentId { get; set; }
        public bool HasVoted => PresentId.HasValue;

        [ForeignKey("PollId")]
        public virtual Poll Poll { get; set; }

        [ForeignKey("VoterId")]
        public virtual ApplicationUser Voter { get; set; }

        [ForeignKey("PresentId")]
        public virtual Present Present { get; set; }
    }
}
