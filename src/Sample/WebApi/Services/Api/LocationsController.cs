using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnowStorm.Domain;
using SnowStorm.Users;
using WebApi.Services.Commands.Locations;
using WebApi.Services.Domain;
using WebApi.Services.Queries.Locations;
using WebApi.Services.Queries.Orders;
using WebApi.Shared.Dto;

namespace WebApi.Services.Api
{
    public class LocationsController : BaseController
    {
        public LocationsController(AppDbContext dataContext, IMediator mediator, ICurrentUser currentUser) : base(dataContext, mediator, currentUser)
        {
        }

        [HttpGet]
        [Route("api/locations/regions")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken()]
        public async Task<IActionResult> GetRegions()
        {
            try
            {
                var result = await DataContext.Get<Region, RegionDto>(new GetRegionsQuery());
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                //Log.Error(ex, "GetOrders ERROR");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/locations/regions/{id:int}")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken()]
        public async Task<IActionResult> GetRegion(int id)
        {
            try
            {
                var result = await DataContext.Get(new GetRegionQuery(id));
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                //Log.Error(ex, "GetOrders ERROR");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("api/locations/regions")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken()]
        public async Task<IActionResult> PostRegion([FromBody] RegionDto data)
        {
            try
            {
                var result = await Mediator.Send(new RegionSaveCommand(data));
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                //Log.Error(ex, "GetOrders ERROR");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
