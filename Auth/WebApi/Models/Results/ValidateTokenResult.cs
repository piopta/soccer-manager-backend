namespace WebApi.Models.Results
{
    public class ValidateTokenResult
    {
        public ValidateTokenState State { get; set; }
    }

    public enum ValidateTokenState
    {
        VALID,
        INVALID
    }
}
