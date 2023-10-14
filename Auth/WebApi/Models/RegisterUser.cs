﻿namespace WebApi.Models
{
    public class RegisterUser : BaseUser
    {
        public string Password { get; set; } = default!;
        public string ConfirmPassword { get; set; } = default!;
    }
}
