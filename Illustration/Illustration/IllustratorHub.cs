using Illustration.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace Illustration
{
    public class IllustratorHub:Hub
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _acessor;

        public IllustratorHub(UserManager<AppUser> userManager, IHttpContextAccessor acessor)
        {
            _userManager = userManager;
            _acessor = acessor;
        }
        public override Task OnConnectedAsync()
        {
            if (_acessor.HttpContext.User.Identity.IsAuthenticated && _acessor.HttpContext.User.IsInRole("Member"))
            {
                var user = _userManager.FindByNameAsync(_acessor.HttpContext.User.Identity.Name).Result;

                if (user != null)
                {
                    //user.ConnectionId = Context.ConnectionId;
                    //user.LastConnectedAt = DateTime.UtcNow.AddHours(4);

                    //var result = _userManager.UpdateAsync(user).Result;

                    //Clients.All.SendAsync("SetAsOnline", user.Id);

                }


            }



            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            if (_acessor.HttpContext.User.Identity.IsAuthenticated && _acessor.HttpContext.User.IsInRole("Member"))
            {
                var user = _userManager.FindByNameAsync(_acessor.HttpContext.User.Identity.Name).Result;

                if (user != null)
                {
                    //user.ConnectionId = null;
                    //user.LastConnectedAt = DateTime.UtcNow.AddHours(4);

                    //var result = _userManager.UpdateAsync(user).Result;

                    //Clients.All.SendAsync("SetAsOffline", user.Id);

                }


            }



            return base.OnDisconnectedAsync(exception);
        }


        public async Task SendMessage(string name, string message)
        {
            if (_acessor.HttpContext.User.Identity.IsAuthenticated)
            {
                await Clients.All.SendAsync("RecieveMessagee", name, message);
                await Clients.All.SendAsync("RecievMessage", name, message);
            }
        }

        public async Task SendMessageUser(string name, string message)
        {
            if (_acessor.HttpContext.User.Identity.IsAuthenticated)
            {
                await Clients.All.SendAsync("RecieveMessage", name, message);
                await Clients.All.SendAsync("RecievMessage2", name, message);
            }
        }

    }
}
