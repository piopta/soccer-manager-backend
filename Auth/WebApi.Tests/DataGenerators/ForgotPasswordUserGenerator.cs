namespace WebApi.Tests.DataGenerators;

internal class ForgotPasswordUserGenerator : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        //valid user - email exists
        yield return new object[] { new ForgotPasswordUser() { Email = TestConstants.validUser.Email }, HttpStatusCode.OK };
        //invalid user - email doesn't exist
        yield return new object[] { new ForgotPasswordUser() { Email = "max.stokes33@ethereal.email" }, HttpStatusCode.BadRequest };
        //invalid user - not confirmed account
        yield return new object[] { new ForgotPasswordUser() { Email = TestConstants.invalidUser.Email }, HttpStatusCode.BadRequest };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
