#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace BeltExam.Models;

// using statements and namespace removed for brevity
public class Association
{
    [Key]    
    public int AssociationId { get; set; } 
    public int UserId { get; set; }    
    public int ActivityId { get; set; }
    
    public User? User { get; set; }    
    public Activity? Activity { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
