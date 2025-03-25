using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace Services
{
    public interface ISystemAccountService
    {
        void Add(SystemAccount account);
        void Delete(SystemAccount account);
        IEnumerable<SystemAccount> GetAllAccounts();
        SystemAccount GetAccountById(int? id);
        SystemAccount Login(string email, string password);
        void Update(SystemAccount account);
        bool CheckEmailExists(string email);

    }
}
