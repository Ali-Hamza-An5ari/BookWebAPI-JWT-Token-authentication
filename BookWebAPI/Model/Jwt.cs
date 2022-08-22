namespace BookWebAPI.Model
{

    public class Jwt
    {
        public string Key { get; set; }
        public string Issuer { get; set; } // Hemingway
        public string Audience { get; set; } // Hemingway
        public string Subject { get; set; }
    }
}
