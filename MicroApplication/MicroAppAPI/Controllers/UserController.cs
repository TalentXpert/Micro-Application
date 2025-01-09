using BaseLibrary.Services;
using MicroAppAPI.Services.Interfaces;

namespace MicroAppAPI.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UserController : BaseController
    {
        IUserService _userService;
        public UserController(IServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory.CreateLogger<UserController>())
        {
            _userService = SF.UserService;
        }

        [HttpGet("GetUsers")]
        public IActionResult GetUsers([FromBody] UserFilterVM model)
        {
            try
            {
                var users = _userService.GetUsers(model, LoggedInUser);
                return Ok(users.Select(u=>new ApplicationUserListVM(u)));
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        //[HttpPost]
        //public IActionResult Post([FromBody] ApplicationUserVM model)
        //{
        //    try
        //    {
        //        var matchingUsers = SF.BaseLibraryServiceFactory.UserService.GetUserAfterValidatingUserCredential(model.Email);

        //        var user = _userService.SaveUpdate(model, LoggedInUser);
        //        CommitTransaction();
        //        return Ok(new ApplicationUserListVM(user));
        //    }
        //    catch (Exception exception)
        //    {
        //        RollbackTransaction();
        //        return HandleException(exception, CodeHelper.CallingMethodInfo());
        //    }
        //}

        [HttpDelete("Delete/{id}")]

        public IActionResult Delete(Guid id)
        {
            try
            {
                var users = _userService.Delete(id);
                CommitTransaction();
                return Ok(true);
            }
            catch (Exception exception)
            {
                RollbackTransaction();
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }
    }
}