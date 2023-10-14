namespace WebApi;

public static class ApplicationConstants
{
    public const string DbConnectionStringName = "DbConn";
    public const string JwtOptionsSectionName = "JwtOptions";
    public const string MailOptionsSectionName = "MailOptions";
    public const string FrontendOptionsSectionName = "FrontendOptions";
    public const string AdminAuthorizationPolicy = "AdminAuthzPolicy";
    public const string RoleClaimName = "Role";
    public const string UserNameClaimName = "UserName";
    public const string MailParameterPrefixSuffix = "@@";

    public static class Validation
    {
        public const int MinimumPasswordLength = 9;
    }

    public static class Roles
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }
}
