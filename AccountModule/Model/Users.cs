using System.ComponentModel.DataAnnotations;

namespace AccountModule.Model
{
    public class Users
    {
        internal string username;

        [Key]
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public char Gender { get; set; }
        public string Address { get; set; }
        public int Phone { get; set; }
        public string Email { get; set; }
        public string PAN { get; set; }
        public string UID { get; set; }
        public string Status { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        

    }
}
