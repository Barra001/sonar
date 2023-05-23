using System.Collections.Generic;
using ArenaGestor.Domain;

namespace ArenaGestor.DataAccessInterface
{
    public interface ISnackManagement
    {
        void InsertSnack(Snack snack);
        void Save();
        void DeleteSnack(Snack snack);
        List<Snack> GetSnacks();
    }
}
