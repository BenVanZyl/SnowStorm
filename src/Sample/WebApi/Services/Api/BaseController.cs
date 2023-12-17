using MediatR;
using Microsoft.AspNetCore.Mvc;
using SnowStorm.DataContext;
using SnowStorm.Users;

namespace WebApi.Services.Api
{
    public class BaseController : Controller
    {
        public BaseController(AppDbContext dataContext, IMediator mediator) //, ICurrentUser currentUser
        {
            DataContext = dataContext;
            Mediator = mediator;
            //CurrentUser = currentUser;
        }


        public AppDbContext DataContext { get; set; }
        public IMediator Mediator { get; set; }
        //public ICurrentUser CurrentUser { get; set; }
    }
}
