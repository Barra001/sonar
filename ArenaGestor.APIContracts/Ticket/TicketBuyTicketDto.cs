using System.Collections.Generic;
using ArenaGestor.Domain;


namespace ArenaGestor.APIContracts.Ticket
{
    public class TicketBuyTicketDto
    {
        public int ConcertId { get; set; }

        public int Amount { get; set; }

        public List<SnackBuy> Snacks { get; set; }
    }
}
