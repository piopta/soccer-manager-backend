namespace WebApi.Tests.DataGenerators;

internal class LockUserGenerator : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        //admin user trying to perform operations on someone else
        yield return new object[] { new LoginUser() {
            Email = TestConstants.validUser.Email,
            Password = TestConstants.validUser.Password},TestConstants.validUser2.Email, HttpStatusCode.NoContent, HttpStatusCode.NoContent, HttpStatusCode.OK };
        //non-admin user trying to perform operations
        yield return new object[] {new LoginUser() {
            Email = TestConstants.validUser2.Email,
            Password = TestConstants.validUser2.Password},TestConstants.validUser2.Email, HttpStatusCode.Forbidden, HttpStatusCode.Forbidden, HttpStatusCode.Forbidden };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
