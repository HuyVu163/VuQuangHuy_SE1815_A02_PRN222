using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using System.Linq;

namespace DataAccessLayer
{
    public class SystemAccountDAO
    {
        public static void Add(SystemAccount account)
        {
            try
            {
                using (var context = new NmsContext())
                {
                    // Lấy ID cao nhất và tăng lên 1
                    short maxId = (short)(context.SystemAccounts.Max(a => (short?)a.AccountId) ?? 0);
                    account.AccountId = (short)(maxId + 1);

                    context.SystemAccounts.Add(account);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm tài khoản", ex);
            }
        }

        public static void Update(SystemAccount account)
        {
            try
            {
                using (var context = new NmsContext())
                {
                    context.SystemAccounts.Update(account);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            
            }
        }
        public static void Delete(SystemAccount account)
        {
            try
            {
                using (var context = new NmsContext())
                {
                    context.SystemAccounts.Remove(account);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static SystemAccount GetAccountById(int? id)
        {
            try
            {
                using (var context = new NmsContext())
                {
                    return context.SystemAccounts.FirstOrDefault(x => x.AccountId == id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IEnumerable<SystemAccount> GetAllAccounts()
        {
            try
            {
                using (var context = new NmsContext())
                {
                    IEnumerable<SystemAccount> accounts = from account in context.SystemAccounts
                                                          orderby account.AccountName ascending
                                                          select account;
                    return accounts.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // Login
        public static SystemAccount Login(string email, string password)
        {
            try
            {
                using (var context = new NmsContext())
                {
                    return context.SystemAccounts.FirstOrDefault(x => x.AccountEmail == email && x.AccountPassword == password);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // Check email exists
        public static bool CheckEmailExists(string email)
        {
            try
            {
                using (var context = new NmsContext())
                {
                    return context.SystemAccounts.Any(x => x.AccountEmail == email);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
