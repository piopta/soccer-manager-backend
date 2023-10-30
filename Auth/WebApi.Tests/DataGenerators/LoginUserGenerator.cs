namespace WebApi.Tests.DataGenerators;

internal class LoginUserGenerator : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        //valid user (confirmed email)
        yield return new object[] { new LoginUser() {
            Email = TestConstants.validUser2.Email,
            Password = TestConstants.validUser2.Password}, HttpStatusCode.OK };
        //not confirmed user
        yield return new object[] {new LoginUser() {
            Email = TestConstants.invalidUser.Email,
            Password = TestConstants.invalidUser.Password}, HttpStatusCode.BadRequest };
        //not-existing user
        yield return new object[] { new LoginUser() {
            Email = "tess.stokes99@ethereal.email",
            Password = "myPassword123!"}, HttpStatusCode.BadRequest };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
