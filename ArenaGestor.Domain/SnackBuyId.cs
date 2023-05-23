using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaGestor.Domain
{
    public class SnackBuyId
    {
        public int Id { get; set; }
        [Required]
        public int SnackId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Amount { get; set; }
    }
}
