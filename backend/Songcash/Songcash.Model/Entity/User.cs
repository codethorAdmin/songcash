namespace Songcash.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public short Type { get; set; } // flat = 0, admin = 1
        public string Name { get; set; }
        public string SpotifyLink { get; set; }
    }
}
