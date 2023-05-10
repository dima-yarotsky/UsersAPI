﻿using System.ComponentModel.DataAnnotations;

namespace UsersAPI.Models
{
    public class UserGroup
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        public string Description { get; set; }
    }
}
