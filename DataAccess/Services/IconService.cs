using Core.Cache;
using Core.Dto;
using Core.Models;
using System.Collections.Generic;
using System.Linq;
using static Common.Enums.SD;

namespace DataAccess.Services
{
    public class IconService
    {
        private readonly ApplicationDbContext _context;
        private readonly CacheManager _cacheManager;

        public IconService(ApplicationDbContext context, CacheManager cacheManager)
        {
            _context = context;
            _cacheManager = cacheManager;
        }

        public Icon GetByName(string name)
        {
            return _context.Icons.FirstOrDefault(x => x.Name == name);
        } 

        public void UpdateListCache()
        {
            var icons = (from a in _context.Icons
                        select new IconsListDto
                        {
                             IconId = a.Id,
                              IconName= a.Name,
                               Svg = a.Svg,
                        }).ToList();

            var iconsSelect = (from i in icons
                               select new SelectDto
                               {
                                   label = i.IconName,
                                   value = i.IconId.ToString()
                               }).ToList();
            
            _cacheManager.Set<List<IconsListDto>>(CacheKeysEnum.IconsList.ToString(), icons);
            _cacheManager.Set<List<SelectDto>>(CacheKeysEnum.IconsListSelect.ToString(), iconsSelect);
        }
    }
}
