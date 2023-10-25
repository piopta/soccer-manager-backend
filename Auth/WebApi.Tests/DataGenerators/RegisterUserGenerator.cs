using Bogus;

namespace WebApi.Tests.DataGenerators;

public class RegisterUserGenerator : IEnumerable<object[]>
{
    Faker<RegisterUser> validUsers = new Faker<RegisterUser>()
                                            .RuleFor(u => u.Email, g => g.Internet.Email(provider: "ethereal.email"))
                                            .RuleFor(u => u.Password, g => RandomPasswordGenerator(9, 12))
                                            .RuleFor(u => u.ConfirmPassword, (Faker _, RegisterUser u) => u.Password);

    Faker<RegisterUser> invalidUsers = new Faker<RegisterUser>()
                                            .RuleFor(u => u.Email, g => g.Internet.Email(provider: "ethereal.email"))
                                            .RuleFor(u => u.Password, g => RandomPasswordGenerator(8, 12))
                                            .RuleFor(u => u.ConfirmPassword, g => RandomPasswordGenerator(6, 12));

    public IEnumerator<object[]> GetEnumerator()
    {
        List<RegisterUser> vu = validUsers.GenerateBetween(3, 3);
        List<RegisterUser> iu = invalidUsers.GenerateBetween(3, 3);

        foreach (var v in vu)
        {
            yield return new object[] { v, HttpStatusCode.OK };
        }

        foreach (var i in iu)
        {
            yield return new object[] { i, HttpStatusCode.BadRequest };
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private static string RandomPasswordGenerator(int minLen, int maxLen)
    {
        string passwdFragment = string.Join("", Enumerable.Range(0, Random.Shared.Next(minLen, maxLen) - 3).Select(_ => Convert.ToChar(Random.Shared.Next('a', 'z'))));
        return $"!{passwdFragment}1A";
    }
}
