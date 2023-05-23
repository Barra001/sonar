using ArenaGestor.APIContracts.Ticket;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArenaGestor.APIContracts.Snack;
using ArenaGestor.Domain;

namespace ArenaGestor.APIContracts
{
    public interface ISnacksAppService
    {
        IActionResult PostSnacks(SnackDto snackDto);
        IActionResult DeleteSnacks(int snackId);
        IActionResult GetSnacks();
    }
}
