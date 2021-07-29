using BirthdayPresentPoll.Web.Data;
using System.Collections.Generic;

namespace BirthdayPresentPoll.Web.Models
{
    public class PollModel
    {
        public PollModel()
        {
            Votes = new();
        }

        public int Id { get; set; }
        public bool IsActive { get; set; }
        public bool HasVoteCast { get; set; }
        public string InitiatorName { get; set; }
        public string CelebrantName { get; set; }
        public int PresentId { get; set; }
        public int VotesNotCastCount { get; set; }
        public Dictionary<string, List<Vote>> Votes { get; set; }
    }
}
