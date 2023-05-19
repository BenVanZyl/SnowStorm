using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnowStorm.Domain;
using SnowStorm.Users;
using WebApi.Services.Domain;
using WebApi.Services.Queries;
using WebApi.Shared.Dto;

namespace WebApi.Services.Api
{
    public class OrdersController : BaseController
    {
        public OrdersController(AppDbContext dataContext, IMediator mediator, ICurrentUser currentUser) : base(dataContext, mediator, currentUser)
        {
        }

        [HttpGet]
        [Route("api/orders")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken()]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                var result = await DataContext.Get(new GetOrdersQuery());
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
