using System;
using System.Collections.Generic;
using ArenaGestor.Domain;

namespace ArenaGestor.APIContracts.Ticket
{
    public class TicketBuyTicketResultDto
    {
        public Guid TicketId { get; set; }
        public TicketStatusDto TicketStatus { get; set; }
        public string Email { get; set; }
        public int ConcertId { get; set; }
        public List<SnackBuyId> Snacks { get; set; }
    }
}
