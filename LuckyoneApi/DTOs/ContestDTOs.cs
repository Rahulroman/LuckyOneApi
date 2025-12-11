namespace LuckyoneApi.DTOs
{
    public class ContestDTOs
    {
        public class CreateContestRequest
        {
            public string ContestName { get; set; }
            public string Description { get; set; }
            public decimal EntryFee { get; set; }
            public int MaxParticipants { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public decimal PrizePool { get; set; }
        }

        public class ContestResponse
        {
            public int ContestId { get; set; }
            public string ContestName { get; set; }
            public string ContestCode { get; set; }
            public string Description { get; set; }
            public decimal EntryFee { get; set; }
            public int MaxParticipants { get; set; }
            public int CurrentParticipants { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public string Status { get; set; }
            public decimal PrizePool { get; set; }
            public bool IsDrawCompleted { get; set; }
        }


        public class JoinContestRequest
        {
            public int ContestId { get; set; }
            public int SlotNumber { get; set; }
        }

    }
}
