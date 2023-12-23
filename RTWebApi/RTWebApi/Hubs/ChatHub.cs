using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RTWebApi.Data;

namespace RTWebApi.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatDbContext _dbContext;
        public ChatHub(ChatDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize]
        public async Task Send(string message)
        {
            var userId = Context.User.FindFirst("Id")?.Value;


            await _dbContext.Messages.AddAsync(new Message
            {
                UserId = Convert.ToInt32(userId),
                Text = message,
                Date = DateTime.Now,
            });
            await _dbContext.SaveChangesAsync();

            await Clients.All.SendAsync("Receive", Context.User.Identity.Name, DateTime.Now.ToString("dd.MM HH:mm"), message);
        }
    }
}
