using NTS.Common;
using NTS.Model.HistoryVersion;
using NTS.Model.Holiday;
using NTS.Model.Repositories;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.Holidays
{
    public class HolidayBussiness
    {
        QLTKEntities db = new QLTKEntities();
        public List<HolidayModel> GetCalendarOfYear(HolidayModel modelSearch)
        {
            List<HolidayModel> listCalendar = new List<HolidayModel>();
            HolidayModel holidayModel;
            for (int i = 1; i <= 12; i++)
            {
                holidayModel = new HolidayModel();
                holidayModel.Month = i;
                listCalendar.Add(holidayModel);
            }
            DateTime dateFrom = DateTimeHelper.ConvertDateFromStr("01/01/" + modelSearch.Year.ToString());
            DateTime dateTo = DateTimeHelper.ConvertDateFromStr("31/12/" + modelSearch.Year.ToString());
            List<DateTime> daysOfYear = Enumerable.Range(0, dateTo.Subtract(dateFrom).Days + 1).Select(d => dateFrom.AddDays(d)).ToList();

            List<int> dayOfWeek = new List<int>();
            var listHolidayDataBase = db.Holidays.AsNoTracking().ToList();
            foreach (var month in listCalendar)
            {
                var dayOfMonth = daysOfYear.Where(a => a.Month == month.Month);
                foreach (var item in dayOfMonth)
                {
                    holidayModel = new HolidayModel();
                    holidayModel.DayOfWeek = ((int)item.DayOfWeek + 1);
                    holidayModel.Month = item.Month;
                    holidayModel.Year = item.Year;
                    holidayModel.FullDate = item;
                    if (listHolidayDataBase.FirstOrDefault(a => a.HolidayDate == item) != null)
                    {
                        holidayModel.IsChecked = true;
                    }
                    month.ListDayOfMonth.Add(holidayModel);
                }
            }

            return listCalendar;
        }

        public void CreateHoliday(HolidayModel model)
        {
            var listHolidayOldOfYear = db.Holidays.Where(a => a.Year == model.Year).ToList();
            if (listHolidayOldOfYear.Count > 0)
            {
                db.Holidays.RemoveRange(listHolidayOldOfYear);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    if (model.ListDayOfMonth.Count > 0)
                    {
                        Holiday holiday;
                        foreach (var item in model.ListDayOfMonth)
                        {
                            holiday = new Holiday();
                            holiday.Year = item.Year;
                            holiday.HolidayDate = item.FullDate;
                            db.Holidays.Add(holiday);
                        }
                    }

                    db.SaveChanges();

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, model.Year.ToString(), null, Constants.LOG_Holiday);

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
            }
        }
    }
}
