using eTicket.Data.Base;
using System.ComponentModel.DataAnnotations;

namespace eTicket.Models
{
    public class Actor : IEntityBase
    {
        //this is new branc
        [Key]
        public int Id { get; set; }
        
        [Display(Name = "Profile Picture")]
        [Required(ErrorMessage ="Profile Picture is required")]
        public string? ProfilePictureURL { get; set; }
        
        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage ="Full Name must be between 3-50 characters!")]
        public string? FullName { get; set; }
        
        [Display(Name = "Biography")]
        [Required(ErrorMessage = "Biograpy is required")]
        public string? Bio { get; set; }

        //Relationships
        public List<Actor_Movie>? Actors_Movies { get; set; }

    }
}
