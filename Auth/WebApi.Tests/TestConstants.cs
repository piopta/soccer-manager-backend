namespace WebApi.Tests
{
    internal static class TestConstants
    {
        internal static RegisterUser validUser = new()
        {
            Email = "tom.stokes12@ethereal.email",
            Password = "myPassword123!",
            ConfirmPassword = "myPassword123!",
        };

        internal static RegisterUser validUser2 = new()
        {
            Email = "jack.stokes22@ethereal.email",
            Password = "myPassword123!",
            ConfirmPassword = "myPassword123!",
        };

        internal static RegisterUser invalidUser = new()
        {
            Email = "tess.stokes13@ethereal.email",
            Password = "myPassword123!",
            ConfirmPassword = "myPassword123!",
        };
    }
}