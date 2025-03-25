using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories;
using BusinessObjects.Models;

namespace Services
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly ISystemAccountRepository _systemAccountRepository;
        public SystemAccountService()
        {
            _systemAccountRepository = new SystemAccountRepository();
        }
        public void Add(SystemAccount account)
        {
            _systemAccountRepository.Add(account);
        }
        public void Update(SystemAccount account)
        {
            _systemAccountRepository.Update(account);
        }
        public void Delete(SystemAccount account)
        {
            _systemAccountRepository.Delete(account);
        }
        public IEnumerable<SystemAccount> GetAllAccounts()
        {
            return _systemAccountRepository.GetAllAccounts();
        }
        public SystemAccount GetAccountById(int? id)
        {
            return _systemAccountRepository.GetAccountById(id);
        }
        public SystemAccount Login(string email, string password)
        {
            return _systemAccountRepository.Login(email, password);
        }
        public bool CheckEmailExists(string email)
        {
            return _systemAccountRepository.CheckEmailExists(email);
        }
    }
}
