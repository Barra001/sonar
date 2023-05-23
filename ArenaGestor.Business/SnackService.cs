using System;
using System.Collections.Generic;
using ArenaGestor.DataAccessInterface;
using ArenaGestor.Domain;
using ArenaGestor.BusinessInterface;

namespace ArenaGestor.Business
{
    public class SnackService : ISnackService
    {
        private ISnackManagement snackManagement;
        public SnackService(ISnackManagement snackManagement)
        {
            this.snackManagement = snackManagement;
        }

        private static bool ValidateSnack(Snack snack)
        {
            if (snack == null)
                throw new NullReferenceException("snack received was null");
            if (string.IsNullOrEmpty(snack.Description))
                throw new ArgumentException("description received was null");
            if (snack.Price <= 0)
                throw new ArgumentException("price was invalid");
            return true;
        }
        public void AddSnack(Snack snack)
        {
            ValidateSnack(snack);
            snackManagement.InsertSnack(snack);
            snackManagement.Save();
        }

        public void RemoveSnack(int snackId)
        {
            if(snackId <= 0)
                throw new ArgumentException("snackId was invalid");
            Snack mockSnackForDb = new Snack { Id = snackId };
            snackManagement.DeleteSnack(mockSnackForDb);
            snackManagement.Save();
        }

        public List<Snack> GetSnacks()
        {
           return snackManagement.GetSnacks();
        }
    }
}
