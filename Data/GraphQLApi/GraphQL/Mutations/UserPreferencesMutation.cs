using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace GraphQLApi.GraphQL.Mutations
{
    public partial class Mutation
    {
        [UseDbContext(typeof(AppDbContext))]
        public async Task<UserPreferencesPayload> AddUserPreferences(AddUserPreferencesInput input,
            [Service(ServiceKind.Resolver)] AppDbContext ctx, [Service] IMapper mapper,
            [Service] IValidator<AddUserPreferencesInput> validator)
        {
            ValidationResult validationRes = await validator.ValidateAsync(input);

            if (validationRes.IsValid)
            {
                UserPreferencesModel userPreferences = mapper.Map<UserPreferencesModel>(input);

                try
                {
                    await ctx.UserPreferences.AddAsync(userPreferences);
                    await ctx.SaveChangesAsync();
                }
                catch
                {
                    return new(userPreferences.UserId, "An error has occured while trying to perform user preferences addition");
                }

                return new(userPreferences.UserId);
            }

            return new(Guid.Empty, "User preferences input invalid");
        }

        [UseDbContext(typeof(AppDbContext))]
        public async Task<UserPreferencesPayload> EditUserPreferences(Guid userId, EditUserPreferencesInput input,
            [Service(ServiceKind.Resolver)] AppDbContext ctx, [Service] IMapper mapper,
            [Service] IValidator<EditUserPreferencesInput> validator)
        {
            ValidationResult validationRes = await validator.ValidateAsync(input);

            if (validationRes.IsValid)
            {
                try
                {
                    UserPreferencesModel? user = await ctx.UserPreferences.FirstOrDefaultAsync(u => u.UserId == userId);

                    if (user is null)
                    {
                        return new(userId, "No user with given Id exists");
                    }

                    mapper.Map(input, user);

                    await ctx.SaveChangesAsync();
                }
                catch
                {
                    return new(userId, "An error has occured while trying to perform user preferences edit operation");
                }

                return new(userId);
            }

            return new(Guid.Empty, "User preferences input invalid");
        }

        [UseDbContext(typeof(AppDbContext))]
        public async Task<UserPreferencesPayload> DeleteUserPreferences(DeleteUserPreferencesInput input,
            [Service(ServiceKind.Resolver)] AppDbContext ctx,
            [Service] IValidator<DeleteUserPreferencesInput> validator)
        {
            ValidationResult validationRes = await validator.ValidateAsync(input);

            if (validationRes.IsValid)
            {
                try
                {
                    UserPreferencesModel? user = await ctx.UserPreferences.FirstOrDefaultAsync(u => u.UserId == input.Id);

                    if (user is null)
                    {
                        return new(input.Id, "No user with given Id exists");
                    }

                    ctx.UserPreferences.Remove(user);

                    await ctx.SaveChangesAsync();
                }
                catch
                {
                    return new(input.Id, "An error has occured while trying to perform user preferences edit operation");
                }

                return new(input.Id);
            }

            return new(Guid.Empty, "User preferences input invalid");
        }
    }
}
