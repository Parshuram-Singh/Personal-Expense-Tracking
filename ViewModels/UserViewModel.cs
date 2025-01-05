using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using Personal_Expense_Tracking.Models;


namespace Personal_Expense_Tracking.ViewModels
{
    public class UserViewModel
    {

        public User User { get; set; } = new User();

        [Required]
        [Compare("User.Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        // Property to hold the message to display in the UI
        public string StatusMessage { get; set; }

        public async Task HandleValidSubmit()
        {
            if (User != null && User.Password == ConfirmPassword)
            {
                // Simulate saving the user (replace with actual logic)
                StatusMessage = "User registration successful!";
                Console.WriteLine(StatusMessage);

            }
            else
            {
                StatusMessage = "Password and confirm password do not match.";
                Console.WriteLine(StatusMessage);
            }
        }
    }
}
