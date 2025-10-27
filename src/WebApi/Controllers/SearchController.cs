using Application.Contracts.DTOs.Search;
using Application.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet("available-buses")]
        public async Task<ActionResult<List<AvailableBusDto>>> SearchAvailableBuses(
            [FromQuery] string from,
            [FromQuery] string to,
            [FromQuery] DateTime journeyDate)
        {
            try
            {
                var buses = await _searchService.SearchAvailableBusesAsync(from, to, journeyDate);
                return Ok(buses);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}