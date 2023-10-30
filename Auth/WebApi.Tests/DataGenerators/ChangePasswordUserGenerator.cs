namespace WebApi.Tests.DataGenerators;

internal class ChangePasswordUserGenerator : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        //happy path
        yield return new object[] { new ChangePasswordUser() { Email = TestConstants.validUser.Email, OldPassword = TestConstants.validUser.Password, NewPassword = "myPassword123!!!", ConfirmedNewPassword = "myPassword123!!!" }, HttpStatusCode.OK };
        //user doesn't exist
        yield return new object[] { new ChangePasswordUser() { Email = "user.stokes123@ethereal.email", OldPassword = "myPassword123!", NewPassword = "myPassword123!!", ConfirmedNewPassword = "myPassword123!!" }, HttpStatusCode.BadRequest };
        //user is locked
        yield return new object[] { new ChangePasswordUser() { Email = TestConstants.invalidUser.Email, OldPassword = TestConstants.invalidUser.Password, NewPassword = "myPassword123!!", ConfirmedNewPassword = "myPassword123!!" }, HttpStatusCode.BadRequest };
        //passwords don't match
        yield return new object[] { new ChangePasswordUser() { Email = TestConstants.validUser.Email, OldPassword = TestConstants.validUser.Password, NewPassword = "myPassword123!!", ConfirmedNewPassword = "myPassword1234!!" }, HttpStatusCode.BadRequest };
        //invalid password
        yield return new object[] { new ChangePasswordUser() { Email = TestConstants.validUser.Email, OldPassword = "myPassword123!23", NewPassword = "myPassword123!!", ConfirmedNewPassword = "myPassword1234!" }, HttpStatusCode.BadRequest };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
