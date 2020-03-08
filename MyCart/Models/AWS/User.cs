﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DigiFyy.Models.AWS
{
    public class User
    {
        public int UserID { get; set; } = 0;
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string Token { get; set; } = "";
    }

    public class UserResponse
    {
        public int Status { get; set; } = -1;
        public string Message { get; set; } = "";
        public User User { get; set; } = null;
    }
}
