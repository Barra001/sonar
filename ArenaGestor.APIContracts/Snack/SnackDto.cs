using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaGestor.APIContracts.Snack
{
    public class SnackDto
    {
        public int Price { get; set; }
        public string Description { get; set; }
    }
}
