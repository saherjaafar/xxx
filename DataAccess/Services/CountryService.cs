using Core.Cache;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataAccess.Services
{
    public class CountryService
    {
        private readonly ApplicationDbContext _context;
        private readonly CacheManager _cacheManager;

        public CountryService(ApplicationDbContext context, CacheManager cacheManager)
        {
            _context = context;
            _cacheManager = cacheManager;
        }

        public Country GetById(long Id)
        {
            return _context.Countries.Include(i => i.Icon).FirstOrDefault(i => i.Id== Id);
        }
    }
}
