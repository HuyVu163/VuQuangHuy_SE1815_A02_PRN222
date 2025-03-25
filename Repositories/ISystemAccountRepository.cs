using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace Repositories
{
    public interface ISystemAccountRepository
    {
        void Add(SystemAccount account);
        void Update(SystemAccount account);
        void Delete(SystemAccount account);
        SystemAccount GetAccountById(int? id);
        IEnumerable<SystemAccount> GetAllAccounts();
        SystemAccount Login(string email, string password);
        bool CheckEmailExists(string email);
    }
}
