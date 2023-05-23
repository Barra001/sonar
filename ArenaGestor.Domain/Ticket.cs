using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArenaGestor.Domain
{
    public class Ticket
    {
        public Guid TicketId { get; set; }
        public TicketStatus TicketStatus { get; set; }
        [Required]
        public TicketCode TicketStatusId { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "The Email field is not a valid e-mail address.")]
        public string Email { get; set; }
        public Concert Concert { get; set; }
        [Required]
        public int ConcertId { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public List<SnackBuyId> Snacks { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Ticket ticket &&
                   TicketId.Equals(ticket.TicketId);
        }
    }
}
