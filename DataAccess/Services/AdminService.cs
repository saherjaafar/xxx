using Common.Utilities;
using Core.Models;
using System;
using System.Linq;

namespace DataAccess.Services
{
    public class AdminService
    {
        private readonly ApplicationDbContext _context;

        public AdminService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void UpdateVerificationTokenExpiry(long adminId)
        {
            var verification = _context.AdminVerification.FirstOrDefault(i => i.AdminId == adminId);
            if (verification is null)
                verification.EmailVerificationTokenExpiry = DateTime.Now.AddMinutes(15);
        }

        public bool CheckIfEmailExist(string email)
        {
            var admin = _context.Admins.FirstOrDefault(i => i.Email == email);
            if(admin is null) return false;
            else return true;
        }

        public Admin GetByEmail(string email)
        {
            return _context.Admins.FirstOrDefault(x => x.Email == UtilClass.EncryptBackByAES(email));
        }

        public AdminVerification GetVerification(long adminId)
        {
            return _context.AdminVerification.FirstOrDefault(i => i.AdminId == adminId);
        }


    }
}
