﻿using System;

namespace Graffiti.Core 
{
    [Serializable]
    public class UserRole
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string RoleName { get; set; }        
    }
}
