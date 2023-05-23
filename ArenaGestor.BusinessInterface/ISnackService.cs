using ArenaGestor.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaGestor.BusinessInterface
{
    public interface ISnackService
    {
        public void AddSnack(Snack snack);

        public void RemoveSnack(int snackId);
        public List<Snack> GetSnacks();
    }
}
