#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace BeltExam.Models;

public class User
{
    [Key]
    public int UserId { get; set; }

    [Required(ErrorMessage = "First name is required")]
    [MinLength(2, ErrorMessage = " First name must be at least 2 characters.")]
    [Name]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    [MinLength(2, ErrorMessage = " Last name must be at least 2 characters.")]
    [Name]
    public string LastName { get; set; }


    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    [UniqueEmail]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [Password]
    // [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    public string Password { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [NotMapped]
    // There is also a built-in attribute for comparing two fields we can use!
    [Compare("Password")]
    [DataType(DataType.Password)]
    public string PasswordConfirm { get; set; }

    public List<Activity> AllActivities { get; set; } = new List<Activity>();
    public List<Association> Associations { get; set; } = new List<Association>();
}


public class UniqueEmailAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("Email is required!");
        }
        // This will connect us to our database since we are not in our Controller
        MyContext _context = (MyContext)validationContext.GetService(typeof(MyContext));
        // Check to see if there are any records of this email in our database
        if (_context.Users.Any(e => e.Email == value.ToString()))
        {
            return new ValidationResult("Email must be unique!");
        }
        else
        {
            return ValidationResult.Success;
        }
    }
}


public class PasswordAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {

        if (value == null)
        {
            return new ValidationResult("Password is required!");
        }

            string? input = value.ToString();
            ErrorMessage = string.Empty;

    if (string.IsNullOrWhiteSpace(input))
    {
        return new ValidationResult("Password should not be empty");
    }

    var hasNumber = new Regex(@"[0-9]+");
    var hasUpperChar = new Regex(@"[A-Z]+");
    var hasMiniMaxChars = new Regex(@".{8,20}");
    var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

    if (!hasUpperChar.IsMatch(input))
    {
        return new ValidationResult("Password should contain at least one uppercase letter");
    }
    else if (!hasMiniMaxChars.IsMatch(input))
    {
        return new ValidationResult("Password should not be less than 8 characters");
    }
    else if (!hasNumber.IsMatch(input))
    {
        return new ValidationResult("Password should contain at least one number");
    }
    else if (!hasSymbols.IsMatch(input))
    {
        return new ValidationResult("Password should contain at least one special character");
    }
    else
    {
        return ValidationResult.Success;
    }

    }
}




public class NameAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {

        if (value == null)
        {
            return new ValidationResult("Name is required!");
        }

            string? input = value.ToString();
            ErrorMessage = string.Empty;

    if (string.IsNullOrWhiteSpace(input))
    {
        return new ValidationResult("Password should not be empty");
    }

    var hasNumber = new Regex(@"[0-9]+");
    var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

    if (hasNumber.IsMatch(input))
    {
        return new ValidationResult("Name should not contain numbers");
    }
    else if (hasSymbols.IsMatch(input))
    {
        return new ValidationResult("Name should not contain symbols");
    }
    else
    {
        return ValidationResult.Success;
    }
    }
}


