namespace Songcash.Model
{
    public class Contract
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public bool Signed { get; set; }
        public string Path { get; set; }
    }
}
