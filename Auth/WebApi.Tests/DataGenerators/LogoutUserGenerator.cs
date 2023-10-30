namespace WebApi.Tests.DataGenerators;

internal class LogoutUserGenerator : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        //user trying to invalidate login token
        yield return new object[] { new LoginUser() {
            Email = TestConstants.validUser.Email,
            Password = TestConstants.validUser.Password}, string.Empty ,HttpStatusCode.OK };

        //non-existing user trying to invalidate login token
        yield return new object[] { new LoginUser() {
            Email = "tom.stokes15@ethereal.email",
            Password = "myPassword123!"}, string.Empty ,HttpStatusCode.Unauthorized };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
