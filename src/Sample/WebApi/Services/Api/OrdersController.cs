using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnowStorm.DataContext;
using SnowStorm.Users;
using WebApi.Services.Queries.Orders;

namespace WebApi.Services.Api
{
    public class OrdersController : BaseController
    {
        public OrdersController(AppDbContext dataContext, IMediator mediator) : base(dataContext, mediator)
        {
        }

        [HttpGet]
        [Route("api/orders")]
        [AllowAnonymous]
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

        [HttpPut]
        [Route("api/orders")]        
        [AllowAnonymous]
        public async Task<IActionResult> PutOrders()
        {
            return Ok();
            //try
            //{
            //    //var result = await Mediator.Send(new UpdateOrderCommand(;
            //    return Ok(result);
            //}
            //catch (System.Exception ex)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            //}

        }
    }
}
