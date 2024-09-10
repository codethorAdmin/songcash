namespace Songcash.Dto
{
    public class CreateRequestDto
    {
        public string SpotifyLink { get; set; }
        public int AutoEstimatedIncome { get; set; }
        public int AutoEstimatedPaymentToRecover { get; set; }
    }
}
