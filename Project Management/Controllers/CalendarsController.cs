using Newtonsoft.Json;   // ✅ For JSON deserialization when using fetch()
using Project_Management.Models; 
using Project_Management.Setup;
using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;

namespace Project_Management.Controllers
{
    [CheckAuthentication]
    public class CalendarsController : Controller
    {
        private readonly project_managementEntities db = new project_managementEntities();


        public ActionResult Index()
        {
            // Retrieve events from the database, sorted by CreatedDate with the latest first
            var events = db.Calendars
                .OrderByDescending(e => e.CreatedDate)
                .ToList();

            // Pass the events to the view
            return View(events);
        }

        [HttpGet]
        public JsonResult GetEvents()
        {
            var events = db.Calendars
                .OrderByDescending(e => e.CreatedDate)
                .Select(e => new
                {
                    e.EventID,
                    e.Subject,
                    e.StartDateTime,
                    e.EndDateTime,
                    e.ThemeColor
                })
                .ToList() // Execute the query and bring the data into memory
                .Select(e => new
                {
                    id = e.EventID,
                    title = e.Subject,
                    start = e.StartDateTime.HasValue ? e.StartDateTime.Value.ToString("s") : null, // ISO 8601 format
                    end = e.EndDateTime.HasValue ? e.EndDateTime.Value.ToString("s") : null, // ISO 8601 format
                    className = e.ThemeColor
                })
                .ToList();

            // Debugging: Check the events being returned
            System.Diagnostics.Debug.WriteLine("Events returned: " + JsonConvert.SerializeObject(events));

            return Json(events, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateEvent(Calendar newEvent)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Creating Event:");
                System.Diagnostics.Debug.WriteLine("EventID: " + newEvent.EventID);
                System.Diagnostics.Debug.WriteLine("Subject: " + newEvent.Subject);
                System.Diagnostics.Debug.WriteLine("StartDateTime: " + newEvent.StartDateTime);
                System.Diagnostics.Debug.WriteLine("EndDateTime: " + newEvent.EndDateTime);
                System.Diagnostics.Debug.WriteLine("ThemeColor: " + newEvent.ThemeColor);

                newEvent.EventID = Guid.NewGuid().ToString();
                newEvent.CreatedDate = DateTime.Now;
                newEvent.IsFullDay = newEvent.IsFullDay ?? false;
                db.Calendars.Add(newEvent);
                db.SaveChanges();

                return Json(new { success = true, message = "Event created successfully ✅" });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error creating event: " + ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult EditEvent(Calendar updatedEvent)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Editing Event:");
                System.Diagnostics.Debug.WriteLine("EventID: " + updatedEvent.EventID);
                System.Diagnostics.Debug.WriteLine("Subject: " + updatedEvent.Subject);
                System.Diagnostics.Debug.WriteLine("StartDateTime: " + updatedEvent.StartDateTime);
                System.Diagnostics.Debug.WriteLine("EndDateTime: " + updatedEvent.EndDateTime);
                System.Diagnostics.Debug.WriteLine("ThemeColor: " + updatedEvent.ThemeColor);

                var existingEvent = db.Calendars.FirstOrDefault(x => x.EventID == updatedEvent.EventID);
                if (existingEvent == null)
                {
                    System.Diagnostics.Debug.WriteLine("Event not found.");
                    return Json(new { success = false, message = "Event not found." });
                }

                existingEvent.Subject = updatedEvent.Subject;
                existingEvent.ThemeColor = updatedEvent.ThemeColor;
                existingEvent.StartDateTime = updatedEvent.StartDateTime;
                existingEvent.EndDateTime = updatedEvent.EndDateTime;
                existingEvent.IsFullDay = updatedEvent.IsFullDay ?? false;

                db.SaveChanges();

                return Json(new { success = true, message = "Event updated successfully ✅" });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error editing event: " + ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete]
        public JsonResult DeleteEvent(string eventId)
        {
            try
            {
                var ev = db.Calendars.FirstOrDefault(x => x.EventID == eventId);
                if (ev == null)
                    return Json(new { success = false, message = "Event not found." });

                db.Calendars.Remove(ev);
                db.SaveChanges();

                return Json(new { success = true, message = "Event deleted successfully 🗑️" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        //[HttpPost]
        //public JsonResult CreateEvent(Calendar newEvent)
        //{
        //    db.Calendars.Add(newEvent);
        //    db.SaveChanges();
        //    return Json(new { success = true, message = "Event created successfully", eventId = newEvent.EventID });
        //}


        //[HttpPost]
        //public JsonResult CreateEvent(Calendar newEvent)
        //{
        //    try
        //    {
        //        // Generate a new GUID for the EventID
        //        newEvent.EventID = Guid.NewGuid().ToString();

        //        // Set default values if not provided
        //        newEvent.IsFullDay = newEvent.IsFullDay ?? false;
        //        newEvent.CreatedDate = DateTime.Now;

        //        // Add the new event to the context
        //        db.Calendars.Add(newEvent);

        //        // Attempt to save changes
        //        db.SaveChanges();

        //        return Json(new { success = true, message = "Event created successfully", eventId = newEvent.EventID });
        //    }
        //    catch (DbEntityValidationException ex)
        //    {
        //        // Log detailed validation errors
        //        var errorMessages = ex.EntityValidationErrors
        //            .SelectMany(x => x.ValidationErrors)
        //            .Select(x => x.ErrorMessage);

        //        var fullErrorMessage = string.Join("; ", errorMessages);

        //        return Json(new { success = false, message = $"An error occurred: {fullErrorMessage}" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
        //    }
        //}
    }
}
