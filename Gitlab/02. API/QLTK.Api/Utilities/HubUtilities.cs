using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NTS.Api.Models;
using System.Data.Entity.SqlServer;
using Microsoft.AspNet.SignalR;
using NTS.Api.SignalR;

namespace NTS.Api
{
    public class HubUtilities
    {
        //public static void HubPatientCame(string appointmentId, ManagementClinicsEntities db)
        //{
        //    var hubContext = GlobalHost.ConnectionManager.GetHubContext<AppointmentHub>();
        //    hubContext.Clients.All.patientCame(GetAppointmentOnFlow(appointmentId, db));
        //}

        //public static void HubUndoFlow(string appointmentId, string currentFlowId, string toFlowId, ManagementClinicsEntities db)
        //{
        //    var hubContext = GlobalHost.ConnectionManager.GetHubContext<AppointmentHub>();
        //    hubContext.Clients.All.undoFlow(new { CurrentFlowId = currentFlowId, ToFlowId = toFlowId, Appointment = GetAppointmentOnFlow(appointmentId, db) });
        //}

        //public static void HubUndoNoFlow(string appointmentId, string currentFlowId, string toFlowId, ManagementClinicsEntities db)
        //{
        //    var hubContext = GlobalHost.ConnectionManager.GetHubContext<AppointmentHub>();
        //    hubContext.Clients.All.undoFlow(new { CurrentFlowId = currentFlowId, ToFlowId = toFlowId, Appointment = GetAppointmentNoFlow(appointmentId, db) });
        //}

        //public static void HubStartFlow(string appointmentId, string currentFlowId, string toFlowId, ManagementClinicsEntities db)
        //{
        //    var hubContext = GlobalHost.ConnectionManager.GetHubContext<AppointmentHub>();
        //    hubContext.Clients.All.startFlow(new { CurrentFlowId = currentFlowId, ToFlowId = toFlowId, Appointment = GetAppointmentOnFlow(appointmentId, db) });
        //}

        //public static void HubPatientGoToBed(string appointmentId, string currentFlowId, string toFlowId, List<AppointmentServiceModel> listService, decimal timeWork, ManagementClinicsEntities db)
        //{
        //    var hubContext = GlobalHost.ConnectionManager.GetHubContext<AppointmentHub>();
        //    hubContext.Clients.All.patientGoToBed(new
        //    {
        //        CurrentFlowId = currentFlowId,
        //        ToFlowId = toFlowId,
        //        Appointment = GetAppointmentWithAssistantOnFlow(appointmentId, db),
        //        ListService = listService,
        //        TimeWork = timeWork
        //    });
        //}

        //public static void HuChangeFlow(string appointmentId, string currentFlowId, string toFlowId, ManagementClinicsEntities db)
        //{
        //    var hubContext = GlobalHost.ConnectionManager.GetHubContext<AppointmentHub>();
        //    hubContext.Clients.All.changeFlow(new { CurrentFlowId = currentFlowId, ToFlowId = toFlowId, Appointment = GetAppointmentOnFlow(appointmentId, db) });
        //}

        //public static void HubChangeSchedule(string appointmentId, string appointmentNewId, ManagementClinicsEntities db)
        //{
        //    var hubContext = GlobalHost.ConnectionManager.GetHubContext<AppointmentHub>();
        //    if (string.IsNullOrEmpty(appointmentNewId))
        //    {
        //        hubContext.Clients.All.changeSchedule(new { AppointmentOld = GetAppointmentNoFlow(appointmentId, db), AppointmentNew = new AppointmentModel() });
        //    }
        //    else
        //    {
        //        hubContext.Clients.All.changeSchedule(new { AppointmentOld = GetAppointmentNoFlow(appointmentId, db), AppointmentNew = GetAppointmentNoFlow(appointmentNewId, db) });
        //    }
        //}

        //public static void HubEditSchedule(string appointmentId, string appointmentNewId, ManagementClinicsEntities db)
        //{
        //    var hubContext = GlobalHost.ConnectionManager.GetHubContext<AppointmentHub>();
        //    if (string.IsNullOrEmpty(appointmentNewId))
        //    {
        //        hubContext.Clients.All.editSchedule(new { AppointmentOld = GetAppointmentNoFlow(appointmentId, db), AppointmentNew = new AppointmentModel() });
        //    }
        //    else
        //    {
        //        hubContext.Clients.All.editSchedule(new { AppointmentOld = GetAppointmentNoFlow(appointmentId, db), AppointmentNew = GetAppointmentNoFlow(appointmentNewId, db) });
        //    }
        //}

        //public static void HubTherapyFinish(string therapyId, decimal delayTime)
        //{
        //    var hubContext = GlobalHost.ConnectionManager.GetHubContext<AppointmentHub>();
        //    hubContext.Clients.All.therapyFinish(new { TherapyId = therapyId, DelayTime = delayTime });
        //}

        //public static void HubFinishSupport(string therapyId, string appointmentId, string therapySupportId, decimal delayTime)
        //{
        //    var hubContext = GlobalHost.ConnectionManager.GetHubContext<AppointmentHub>();
        //    hubContext.Clients.All.finishSupport(new { TherapyId = therapyId, AppointmentId = appointmentId, TherapySupportId = therapySupportId, DelayTime = delayTime });
        //}

        //public static void HubStartCount(string appointmentId)
        //{
        //    var hubContext = GlobalHost.ConnectionManager.GetHubContext<AppointmentHub>();
        //    hubContext.Clients.All.startCount(new { AppointmentId = appointmentId, StatusCount = Constants.APPOINTMENTTIME_STATUSCOUNT_START, DelayTime = 0 });
        //}

        //public static void HubFloorChange(string appointmentId, ManagementClinicsEntities db)
        //{
        //    var hubContext = GlobalHost.ConnectionManager.GetHubContext<AppointmentHub>();
        //    hubContext.Clients.All.floorChange(new { Appointment = GetAppointmentOnFlow(appointmentId, db) });
        //}

        //public static void HubCallSupport(string therapyId, string therapyAssistantId, string therapyAssistantName, string therapySupportId, string appointmentId)
        //{
        //    var hubContext = GlobalHost.ConnectionManager.GetHubContext<AppointmentHub>();
        //    hubContext.Clients.All.callSupport(new
        //    {
        //        TherapyId = therapyId,
        //        AppointmentId = appointmentId,
        //        TherapyAssistantId = therapyAssistantId,
        //        TherapyAssistantName = therapyAssistantName,
        //        TherapySupportId = therapySupportId
        //    });
        //}

        //private static AppointmentModel GetAppointmentOnFlow(string appointmentId, ManagementClinicsEntities db)
        //{
        //    var listAppointment = (from r in db.Appointments.AsNoTracking()
        //                           where r.AppointmentId.Equals(appointmentId)
        //                           join at in db.AppointmentTypes.AsNoTracking() on r.AppointmentTypeId equals at.AppointmentTypeId
        //                           join ap in db.AppointmentTimes.AsNoTracking() on r.AppointmentId equals ap.AppointmentId
        //                           where ap.Status.Equals(Constants.AppointmentTimeStatusOnGoing)
        //                           join f in db.ProcessingFlows.AsNoTracking() on ap.ProcessingFlowId equals f.ProcessingFlowId
        //                           join p in db.Patients.AsNoTracking() on r.PatientId equals p.PatientId
        //                           join d in db.Users.AsNoTracking() on r.DoctorId equals d.UserId into rd
        //                           from rdn in rd.DefaultIfEmpty()
        //                           join u in db.Users.AsNoTracking() on r.TherapyId equals u.UserId into ru
        //                           from run in ru.DefaultIfEmpty()
        //                           join a in db.AfterTypes.AsNoTracking() on r.AfterType equals a.AfterTypeId into ra
        //                           from ran in ra.DefaultIfEmpty()
        //                           select new AppointmentModel()
        //                           {
        //                               AppointmentId = r.AppointmentId,
        //                               AppointmentTimeId = ap.AppointmentTimeId,
        //                               AppointmentDate = r.AppointmentDate,
        //                               AppointmentTime = r.AppointmentTime,
        //                               AppointmentTypeName = at.AppointmentTypeName,
        //                               BedNumber = r.BedNumber,
        //                               AfterType = r.AfterType,
        //                               DoctorId = r.DoctorId,
        //                               DelayTime = SqlFunctions.DateDiff("second", ap.StartTime, DateTime.Now),
        //                               TimeWork = ap.TimeWork,
        //                               DoctorName = "Dr. " + rdn.Aliass,
        //                               PatientAliass = p.Aliass,
        //                               PatientName = p.PatientName,
        //                               PatientId = p.PatientId,
        //                               Ratting = p.Rating,
        //                               RattingMax = Constants.RatingMax,
        //                               StartTime = ap.StartTime,
        //                               Status = r.Status,
        //                               TherapyId = r.TherapyId,
        //                               TherapyName = "KTV. " + run.Aliass,
        //                               vip = p.Vip,
        //                               ProcessingFlowId = ap.ProcessingFlowId,
        //                               TimeType = ap.AppointmentTimeType,
        //                               TimeStatus = ap.Status,
        //                               AfterImagePath = ran.ImagePath,
        //                               Id = p.ProfileCode.ToString(),
        //                               TemplatePath = f.TemplatePath,
        //                               FlowType = f.ProcessingFlowType,
        //                               FloorId = r.FloorId,
        //                               StartCountTime = ap.StartCountTime,
        //                               StatusCount = ap.StatusCount
        //                           }).ToList();

        //    if (listAppointment.Count > 0)
        //    {
        //        return listAppointment.First();
        //    }
        //    else
        //    {
        //        return new AppointmentModel() { AppointmentId = appointmentId };
        //    }
        //}

        //private static AppointmentModel GetAppointmentWithAssistantOnFlow(string appointmentId, ManagementClinicsEntities db)
        //{
        //    var listAppointment = (from r in db.Appointments.AsNoTracking()
        //                           where r.AppointmentId.Equals(appointmentId)
        //                           join at in db.AppointmentTypes.AsNoTracking() on r.AppointmentTypeId equals at.AppointmentTypeId
        //                           join ap in db.AppointmentTimes.AsNoTracking() on r.AppointmentId equals ap.AppointmentId
        //                           where ap.Status.Equals(Constants.AppointmentTimeStatusOnGoing)
        //                           join f in db.ProcessingFlows.AsNoTracking() on ap.ProcessingFlowId equals f.ProcessingFlowId
        //                           join p in db.Patients.AsNoTracking() on r.PatientId equals p.PatientId
        //                           join d in db.Users.AsNoTracking() on r.DoctorId equals d.UserId into rd
        //                           from rdn in rd.DefaultIfEmpty()
        //                           join u in db.Users.AsNoTracking() on r.TherapyId equals u.UserId into ru
        //                           from run in ru.DefaultIfEmpty()
        //                           join a in db.AfterTypes.AsNoTracking() on r.AfterType equals a.AfterTypeId into ra
        //                           from ran in ra.DefaultIfEmpty()
        //                           join t in db.TherapyAssistants.AsNoTracking() on ap.AppointmentTimeId equals t.AppointmentTimeId into apt
        //                           from aptn in apt.DefaultIfEmpty()
        //                           where (aptn.Status.Equals(Constants.THERAPY_ASSISTANT_STATUS_ONGOING) || string.IsNullOrEmpty(aptn.TherapyAssistantId))
        //                           join ut in db.Users.AsNoTracking() on aptn.TherapyId equals ut.UserId into tu
        //                           from tun in tu.DefaultIfEmpty()
        //                           select new AppointmentModel()
        //                           {
        //                               AppointmentId = r.AppointmentId,
        //                               AppointmentTimeId = ap.AppointmentTimeId,
        //                               AppointmentDate = r.AppointmentDate,
        //                               AppointmentTime = r.AppointmentTime,
        //                               AppointmentTypeName = at.AppointmentTypeName,
        //                               BedNumber = r.BedNumber,
        //                               AfterType = r.AfterType,
        //                               DoctorId = r.DoctorId,
        //                               DelayTime = SqlFunctions.DateDiff("second", ap.StartTime, DateTime.Now),
        //                               TimeWork = ap.TimeWork,
        //                               DoctorName = "Dr. " + rdn.Aliass,
        //                               PatientAliass = p.Aliass,
        //                               PatientName = p.PatientName,
        //                               PatientId = p.PatientId,
        //                               Ratting = p.Rating,
        //                               RattingMax = Constants.RatingMax,
        //                               StartTime = ap.StartTime,
        //                               Status = r.Status,
        //                               TherapyId = r.TherapyId,
        //                               TherapyName = "KTV. " + run.Aliass,
        //                               vip = p.Vip,
        //                               ProcessingFlowId = ap.ProcessingFlowId,
        //                               TimeType = ap.AppointmentTimeType,
        //                               TimeStatus = ap.Status,
        //                               AfterImagePath = ran.ImagePath,
        //                               Id = p.ProfileCode.ToString(),
        //                               TemplatePath = f.TemplatePath,
        //                               FlowType = f.ProcessingFlowType,
        //                               FloorId = r.FloorId,
        //                               TherapyAssistantName = tun.Aliass,
        //                               TherapyAssistantId = aptn.TherapyAssistantId,
        //                               TherapySupportId = aptn.TherapyId,
        //                               StartCountTime = ap.StartCountTime,
        //                               StatusCount = ap.StatusCount
        //                           }).ToList();

        //    if (listAppointment.Count > 0)
        //    {
        //        return listAppointment.First();
        //    }
        //    else
        //    {
        //        return new AppointmentModel() { AppointmentId = appointmentId };
        //    }
        //}

        //private static AppointmentModel GetAppointmentNoFlow(string appointmentId, ManagementClinicsEntities db)
        //{
        //    string currentDate = DateTime.Now.ToString(Constants.DATE_FORMAT_YYYYMMDD);
        //    var listAppointment = (from r in db.Appointments.AsNoTracking()
        //                           where r.AppointmentId.Equals(appointmentId)
        //                            && r.AppointmentDate.Equals(currentDate)
        //                           join at in db.AppointmentTypes.AsNoTracking() on r.AppointmentTypeId equals at.AppointmentTypeId
        //                           join p in db.Patients.AsNoTracking() on r.PatientId equals p.PatientId
        //                           join d in db.Users.AsNoTracking() on r.DoctorId equals d.UserId into rd
        //                           from rdn in rd.DefaultIfEmpty()
        //                           join u in db.Users.AsNoTracking() on r.TherapyId equals u.UserId into ru
        //                           from run in ru.DefaultIfEmpty()
        //                           join a in db.AfterTypes.AsNoTracking() on r.AfterType equals a.AfterTypeId into ra
        //                           from ran in ra.DefaultIfEmpty()
        //                           select new AppointmentModel()
        //                           {
        //                               AppointmentId = r.AppointmentId,
        //                               AppointmentDate = r.AppointmentDate,
        //                               AppointmentTime = r.AppointmentTime,
        //                               AppointmentTypeName = at.AppointmentTypeName,
        //                               BedNumber = r.BedNumber,
        //                               AfterType = r.AfterType,
        //                               DoctorId = r.DoctorId,
        //                               DoctorName = "Dr. " + rdn.Aliass,
        //                               PatientAliass = p.Aliass,
        //                               PatientName = p.PatientName,
        //                               PatientId = p.PatientId,
        //                               Ratting = p.Rating,
        //                               RattingMax = Constants.RatingMax,
        //                               Status = r.Status,
        //                               TherapyId = r.TherapyId,
        //                               TherapyName = "KTV. " + run.Aliass,
        //                               vip = p.Vip,
        //                               AfterImagePath = ran.ImagePath,
        //                               Id = p.ProfileCode.ToString(),
        //                               FloorId = r.FloorId
        //                           }).ToList();

        //    if (listAppointment.Count > 0)
        //    {
        //        return listAppointment.First();
        //    }
        //    else
        //    {
        //        return new AppointmentModel() { AppointmentId = appointmentId };
        //    }
        //}
    }
}