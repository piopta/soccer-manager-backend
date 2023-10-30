namespace WebApi.Tests.DataGenerators;

internal class ValidateTokenGenerator : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        //user trying to validate login token
        yield return new object[] { new LoginUser() {
            Email = TestConstants.validUser.Email,
            Password = TestConstants.validUser.Password}, string.Empty ,HttpStatusCode.OK };

        //user trying to validate invalid JWT token
        yield return new object[] { new LoginUser() {
            Email = TestConstants.validUser2.Email,
            Password = TestConstants.validUser2.Password}, "another-token" ,HttpStatusCode.Unauthorized };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
