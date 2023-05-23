using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ArenaGestor.Domain
{
    public class SnackBuy
    {
        public int Id { get; set; }
        [Required]
        public Snack Snack { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Amount { get; set; }
    }
}
