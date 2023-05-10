using System.ComponentModel.DataAnnotations;

namespace UsersAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        public DateTime CreatedDate { get; set; }

        public int UserGroupId { get; set; }
        public UserGroup UserGroup { get; set; }

        public int UserStateId { get; set; }
        public UserState UserState { get; set; }
    }
}