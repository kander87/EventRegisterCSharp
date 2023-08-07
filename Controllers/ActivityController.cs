using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using BeltExam.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BeltExam.Controllers;    
public class ActivityController : Controller
{    
    private readonly ILogger<ActivityController> _logger;
    private MyContext _context;         
    public ActivityController(ILogger<ActivityController> logger, MyContext context)    
    {        
        _logger = logger;
        _context = context;    
    }  

    [SessionCheck]
    [HttpGet("/allactivities")]
    public IActionResult AllActivities(){
        MyViewModel MyModel = new MyViewModel(){
            AllActivities = _context.Activities
            .Include(creator => creator.Creator)
            .Include(act => act.Associations)
            .ThenInclude(user => user.User)
            .ThenInclude(user => user.Associations)
            .OrderBy(act=> act.Date)
            .ToList()
        };
        return View("AllActivities", MyModel);
    }

    [SessionCheck]
    [HttpGet("/activity/new")]
    public IActionResult NewActivity()
    {
        return View("NewActivity");
    }

    [SessionCheck]
    [HttpPost("/activity/create")]
    public IActionResult ActCreate(Models.Activity newActivity)
    {    
        if(ModelState.IsValid)
        { 
            newActivity.UserId =(int)HttpContext.Session.GetInt32("UserId");
            _context.Add(newActivity);    
            _context.SaveChanges();
            int actId = newActivity.ActivityId;
            return RedirectToAction("ViewAct", new{actId});
        } else {

            return View("NewActivity");
        }   
    }

    [SessionCheck]
    [HttpPost("/{activityId}/delete")]
    public IActionResult ActDelete(int activityId)
    {   
        Console.WriteLine(activityId);
        Models.Activity? ActivitytoDelete = _context.Activities.SingleOrDefault(act =>act.ActivityId == activityId);
        if (ActivitytoDelete.UserId != HttpContext.Session.GetInt32("UserId")){
            return RedirectToAction("AllActivities");
        }
        _context.Activities.Remove(ActivitytoDelete);
        _context.SaveChanges();
        return RedirectToAction("AllActivities"); 
    }

    [SessionCheck]
    [HttpGet("/{activityId}/RSVPAdd")]
    public IActionResult RSVPAdd( int activityId){
        Association newRSVP= new Association(){
            ActivityId = activityId,
            UserId = (int)HttpContext.Session.GetInt32("UserId")
        };

        _context.Associations.Add(newRSVP);
        _context.SaveChanges();
        return RedirectToAction("AllActivities");
    }

    [SessionCheck]
    [HttpGet("/{activityId}/RSVPRemove")]
    public IActionResult RSVPRemove(int? activityId )
    {
        Association? unRSVP = _context.Associations
            .SingleOrDefault(assoc => assoc.UserId == (int)HttpContext.Session.GetInt32("UserId") && assoc.ActivityId == activityId);
        _context.Associations.Remove(unRSVP);
        _context.SaveChanges();
        return RedirectToAction("AllActivities");
    }

    [SessionCheck]
    [HttpGet("/activity/{actId}")]
    public IActionResult ViewAct(int actId){
        Models.Activity? activity = _context.Activities
        .Include(activity=>activity.Creator)
        .Include(act => act.Associations)
        .ThenInclude(user => user.User)
        .SingleOrDefault(act => act.ActivityId == actId);
        if(activity ==null)
        {
            return RedirectToAction("AllActivities");
        } else {
            List<User> allAttendees = _context.Users
                .Include(user => user.Associations)
                .Where(user => user.Associations
                .Any(Association => Association.ActivityId ==activity.ActivityId) == true)
                .ToList();
            
            MyViewModel MyModel = new MyViewModel(){
                AllUsers = allAttendees,
                Association= new Association(){
                Activity = activity,
                }
            };
            return View ("ViewAct", MyModel);
        }
    }

    public class SessionCheckAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        int? userId = context.HttpContext.Session.GetInt32("UserId");
        if(userId == null)
        {
            context.Result = new RedirectToActionResult("Index", "Home", null);
        }
    }
}

}
    // [HttpGet("/ActUpdate/{activityId}")]
    // public IActionResult ActUpdate(Models.Activity newAct, int activityId)
    // {
    //     Models.Activity? OldAct = _context.Activities.SingleOrDefault(act => act.ActivityId ==activityId);
    //     if (OldAct == null){
    //         return RedirectToAction("AllActivities");
    //     }
    //     if(ModelState.IsValid)
    //     {   OldAct.Title = newAct.Title;
    //         OldAct.Time = newAct.Time;
    //         OldAct.Date = newAct.Date;
    //         OldAct.DurationTime = newAct.DurationTime;
    //         OldAct.DurationUnit = newAct.DurationUnit;
    //         OldAct.Description = newAct.Description;

    //         OldAct.UpdatedAt = DateTime.Now;
    //         _context.SaveChanges();
    //         return RedirectToAction("AllActivities");
    //         } else {
    //         return View("EditActivity", OldAct);
    //     }
    // }

    // [HttpGet("/edit/{activityId}")]
    // public IActionResult ActEdit(int activityId)
    // {
    //     Models.Activity? oneAct = _context.Activities.SingleOrDefault(act => act.ActivityId ==activityId);
    //     if (oneAct ==null){
    //             return RedirectToAction("AllActivities");
    //         } 
    //     if (oneAct.UserId == HttpContext.Session.GetInt32("UserId")){
    //         return View("EditActivity", oneAct);}
    //     return RedirectToAction("AllActivities");
    // }