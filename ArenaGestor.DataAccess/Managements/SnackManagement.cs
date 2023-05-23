using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using ArenaGestor.DataAccessInterface;
using ArenaGestor.Domain;
using Microsoft.EntityFrameworkCore;

namespace ArenaGestor.DataAccess.Managements
{
    public class SnackManagement : ISnackManagement
    {
        private readonly DbSet<Snack> snacks;
        private readonly DbContext context;
        public SnackManagement(DbContext context)
        {
            this.snacks = context.Set<Snack>();
            this.context = context;
        }
        public void InsertSnack(Snack snack)
        {
            snacks.Add(snack);
            Save();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void DeleteSnack(Snack snack)
        {
            try
            {
                snacks.Remove(snack);
                Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new KeyNotFoundException("The snack was not in the data base");
            }
            
        }

        public List<Snack> GetSnacks()
        {
            return snacks.ToList();
        }
    }
}
