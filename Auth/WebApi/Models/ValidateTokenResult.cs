﻿namespace WebApi.Models
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
