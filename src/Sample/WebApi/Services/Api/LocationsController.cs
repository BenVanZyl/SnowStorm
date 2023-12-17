using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnowStorm.DataContext;
using SnowStorm.Users;
using WebApi.Services.Commands.Locations;
using WebApi.Services.Domain;
using WebApi.Services.Queries.Locations;
using WebApi.Shared.Dto.Locations;
using WebApi.Shared.Dto.Regions;

namespace WebApi.Services.Api
{
    [AllowAnonymous]
    public class LocationsController : BaseController
    {
        public LocationsController(AppDbContext dataContext, IMediator mediator) : base(dataContext, mediator)
        {
        }

        [HttpGet]
        [Route("api/locations/regions")]
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
        [Route("api/locations/regions/no-query-class")]
        public async Task<IActionResult> GetRegionsNoQueryClass()
        {
            try
            {
                var result = await DataContext.GetAll<Region>();
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

        [HttpGet]
        [Route("api/locations/regions/{id:int}/no-query-class")]
        public async Task<IActionResult> GetRegionNoQueryClass(int id)
        {
            try
            {
                var result = await DataContext.GetById<Region>(id);
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

        [HttpPatch]
        [Route("api/locations/regions")]
        public async Task<IActionResult> PatchRegion([FromBody] RegionPatchDto data)
        {
            try
            {
                var result = await Mediator.Send(new RegionPatchCommand(data));
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
