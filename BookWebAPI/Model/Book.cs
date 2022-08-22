using System.ComponentModel.DataAnnotations;

namespace BookWebAPI.Model
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        public string Title { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }
        public User Author { get; set; }
        
        public string CoverImage { get; set; }
        
        public double Price  { get; set; }
    }
}


//For countable quantities use int/short/long based on the size requirement
//For measurable quantiteis use double/float based on the size requierments. weight/money/temperature