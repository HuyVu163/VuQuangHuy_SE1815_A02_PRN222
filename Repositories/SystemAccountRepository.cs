using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using DataAccessLayer;

namespace Repositories
{
    public class SystemAccountRepository : ISystemAccountRepository
    {
        public void Add(SystemAccount account) => SystemAccountDAO.Add(account);


        public void Delete(SystemAccount account) => SystemAccountDAO.Delete(account);


        public IEnumerable<SystemAccount> GetAllAccounts() => SystemAccountDAO.GetAllAccounts();

        public SystemAccount GetAccountById(int? id) => SystemAccountDAO.GetAccountById(id);

        public SystemAccount Login(string email, string password) => SystemAccountDAO.Login(email, password);

        public void Update(SystemAccount account) => SystemAccountDAO.Update(account);
        public bool CheckEmailExists(string email) => SystemAccountDAO.CheckEmailExists(email);

    }
}
