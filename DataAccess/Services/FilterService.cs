using Core.Dto;
using System;
using System.Collections.Generic;
using static Common.Enums.SD;

namespace DataAccess.Services
{
    public class FilterService
    {
        public List<SelectDto> GetFiltersSelect()
        {
            List<SelectDto> filters = new List<SelectDto>();
            foreach (var i in Enum.GetValues(typeof(FiltersEnum)))
            {
                filters.Add(new SelectDto
                {
                    label = i.ToString(),
                    value = i.ToString(),
                });
            }
            return filters;
        }

    }
}
