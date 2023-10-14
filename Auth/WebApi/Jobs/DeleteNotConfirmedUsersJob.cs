using Coravel.Invocable;
using WebApi.Data;

namespace WebApi.Jobs
{
    public class DeleteNotConfirmedUsersJob : IInvocable
    {
        private readonly ApplicationDbContext _ctx;

        public DeleteNotConfirmedUsersJob(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task Invoke()
        {
            List<ApplicationUser> notConfirmedUsers = _ctx.Users.Where(u => !u.EmailConfirmed).ToList();

            if (notConfirmedUsers.Any())
            {
                _ctx.Users.RemoveRange(notConfirmedUsers);

                await _ctx.SaveChangesAsync();
            }
        }
    }
}
