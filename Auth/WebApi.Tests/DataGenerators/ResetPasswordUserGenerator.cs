namespace WebApi.Tests.DataGenerators;

internal class ResetPasswordUserGenerator : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        //test for invalid token
        yield return new object[] { new ResetPasswordUser() { Email = TestConstants.validUser.Email, Password = "newPasswd123!@#!", ConfirmedPassword = "newPasswd123!@#!", Token = "invalid-token" }, HttpStatusCode.BadRequest };
        //test for non-existing user
        yield return new object[] { new ResetPasswordUser() { Email = "max.stokes33@ethereal.email", Password = "newPasswd123!@#!", ConfirmedPassword = "newPasswd123!@#!", Token = "invalid-token" }, HttpStatusCode.BadRequest };
        //test for password that doesn't meet requirements
        yield return new object[] { new ResetPasswordUser() { Email = TestConstants.invalidUser.Email, Password = "newPasswd123", ConfirmedPassword = "newPasswd123", Token = "invalid-token" }, HttpStatusCode.BadRequest };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
