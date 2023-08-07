#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BeltExam.Models;

    public class Activity
    {
        [Key]
        public int ActivityId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MinLength(2, ErrorMessage ="Title must be at least 2 characters.")]
        public string Title { get; set; } 

        [Required(ErrorMessage = "Time is required")]
        public DateTime? Time { get; set; } 

        [Required (ErrorMessage = "Date is required")]
        [FutureDate]
        public DateTime Date { get; set; }

        [Required (ErrorMessage = "Duration is required")]
        public int? DurationTime { get; set; }

        [Required (ErrorMessage = "Unit of time is required")]
        public string DurationUnit { get; set; }

        [Required(ErrorMessage = "Description of the item is required")]
        [MinLength(2, ErrorMessage ="Description must be at least 2 characters.")]
        public string Description { get; set; } 
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;        

        public List<Association> Associations { get; set; } = new List<Association>();

        public int UserId  { get; set; }
        public User? Creator  { get; set; }
    }


public class FutureDateAttribute : ValidationAttribute
{

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if ((DateTime)value < DateTime.Now)
        {

            return new ValidationResult("You must choose a date in the future!");
        }
        else
        {
            return ValidationResult.Success;

        }
    }
}

                // DateTime datetime = (DateOnly)value.ToDateTime((TimeOnly)value);
// DateOnly orderDate = ...
// TimeOnly orderTime = ...
// DateTime orderDateTime = orderDate.ToDateTime(orderTime);
        //TimeSpan time = GetTimeFieldData();
// dateField = dateField.Add(time);
    


