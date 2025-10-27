using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Contracts.DTOs.Search;

namespace Application.Contracts.Interfaces
{
    public interface ISearchService
    {
        Task<List<AvailableBusDto>> SearchAvailableBusesAsync(string from, string to, DateTime journeyDate);
    }
}