using System.ComponentModel.DataAnnotations;

namespace ArenaGestor.Domain
{
    public class Snack
    {
        public int Id { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public string Description { get; set; }

    }
}
