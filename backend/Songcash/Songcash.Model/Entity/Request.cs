namespace Songcash.Model
{
    public class Request
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string SpotifyLink { get; set; }
        public DateTime RequestStartDate { get; set; }
        public DateTime? EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public short Status { get; set; }
        public short StepInFlow { get; set; }
        public int AutoEstimatedIncome { get; set; }
        public int AutoEstimatedPaymentToRecover { get; set; }
        public int CalculatedIncome { get; set; }
        public int FinalPaymentToRecover { get; set; }
    }
}
