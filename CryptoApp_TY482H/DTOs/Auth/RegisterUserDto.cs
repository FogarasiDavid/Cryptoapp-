﻿using System.ComponentModel.DataAnnotations;

namespace CryptoApp_TY482H.DTOs.Auth
{
    public class RegisterUserDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

}
