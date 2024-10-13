using System;

namespace SysgamingApi.Src.Application.Users.Command.RegisterUser
{
    public class RegisterRequest
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}