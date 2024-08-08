﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ZoidUpAPI.Models
{
    [Table(name: "Users", Schema = "ath")]
    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;

    }
}
