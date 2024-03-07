using System;
using System.ComponentModel.DataAnnotations;

namespace ChatWebApp.Models
{
    public class UserModel
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public DateTime CreateDate { get; set; }
        public string Token { get; set; }
    }
    public class SignInModel
    {
        [Required(ErrorMessage = "Please Enter Email")]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z]+(\\.[a-z]+)*\\.([a-z]{2,4})$",
        ErrorMessage = "Invalid Email Format")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Please Enter Password")]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$",
        ErrorMessage = "Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
        public string Password { get; set; }
    }
    public class SignUpModel
    {
        [Required(ErrorMessage = "First Name is required")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "First Name should contain only letters")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Last Name should contain only letters")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please Enter Email")]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z]+(\\.[a-z]+)*\\.([a-z]{2,4})$",
        ErrorMessage = "Invalid Email Format")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Please Enter Password")]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$",
        ErrorMessage = "Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please Enter Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        public string ConfirmPassword { get; set; }
    }
    public class UserListWithLastMessageModel
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string MessageText { get; set; }
        public string GroupName { get; set; }
        public DateTime CreateDate { get; set; }
        
    }

    public class getUserMessage
    {
        public Guid SId { get; set; }
        public Guid RId { get; set; }
    }
}
