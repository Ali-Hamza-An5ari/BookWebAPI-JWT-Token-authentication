using System.ComponentModel.DataAnnotations;
namespace BookWebAPI.Model
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string AuthorPseudoName { get; set; } // Hemingway
        public string Password { get; set; }    
    }
}
