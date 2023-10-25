namespace WebApi.Tests.DataGenerators;

public class LoginUserGenerator : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { new LoginUser() {
            Email = "tess.stokes12@ethereal.email",
            Password = "myPassword123!"}, HttpStatusCode.OK };
        yield return new object[] {new LoginUser() {
            Email = "tess.stokes13@ethereal.email",
            Password = "myPassword123!"}, HttpStatusCode.BadRequest };
        yield return new object[] { new LoginUser() {
            Email = "tess.stokes99@ethereal.email",
            Password = "myPassword123!"}, HttpStatusCode.BadRequest };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
