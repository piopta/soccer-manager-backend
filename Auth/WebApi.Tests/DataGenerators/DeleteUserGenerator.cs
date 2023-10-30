namespace WebApi.Tests.DataGenerators;

internal class DeleteUserGenerator : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        //correct deleting
        yield return new object[] { new LoginUser() {
            Email = TestConstants.validUser2.Email,
            Password = TestConstants.validUser2.Password}, TestConstants.validUser2.Email ,HttpStatusCode.NoContent };
        //non-admin user trying to delete someone who doesn't exist
        yield return new object[] { new LoginUser() {
            Email = TestConstants.validUser.Email,
            Password = TestConstants.validUser.Password}, "tess.stokes17@ethereal.email" ,HttpStatusCode.BadRequest};
        //non-admin user trying to delete someone else
        yield return new object[] { new LoginUser() {
            Email = "tom.stokes15@ethereal.email",
            Password = "myPassword123!"}, "tess.stokes13@ethereal.email" ,HttpStatusCode.Unauthorized };
        //non-existing user trying to delete someone
        yield return new object[] { new LoginUser() {
            Email = "tess.stokes99@ethereal.email",
            Password = "myPassword123!"}, "tess.stokes99@ethereal.email" ,HttpStatusCode.Unauthorized };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
