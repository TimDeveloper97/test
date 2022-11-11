using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Course;
using NTS.Model.Employees;
using NTS.Model.Employees.EmployeeTraining;
using NTS.Model.EmployeeTraining;
using NTS.Model.HistoryVersion;
using NTS.Model.JobPosition;
using NTS.Model.Repositories;
using NTS.Model.WorkSkill;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace QLTK.Business.EmployeeTraining
{
    public class EmployeeTrainingBusiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        public SearchResultModel<EmployeeTrainingModel> SearchEmployeeTraining(EmployeeTrainingSearchModel model)
        {
            SearchResultModel<EmployeeTrainingModel> searchResult = new SearchResultModel<EmployeeTrainingModel>();

            var dataQuery = (from a in db.EmployeeTranings.AsNoTracking()
                             orderby a.Code
                             select new EmployeeTrainingModel
                             {
                                 Id = a.Id,
                                 Code = a.Code,
                                 Name = a.Name,
                                 Description = a.Description,
                                 Status = a.Status
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(model.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(model.Name.ToUpper()));
            }
            List<EmployeeTrainingCourseModel> listCourse = new List<EmployeeTrainingCourseModel>();
            List<EmployeeTrainingEmployeeModel> list;
            searchResult.TotalItem = dataQuery.Count();
            var listResult = SQLHelpper.OrderBy(dataQuery.AsQueryable(), model.OrderBy, model.OrderType).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToList();
            searchResult.ListResult = listResult;

            List<EmployeeTrainingCourseModel> cousers;

            foreach (var item in listResult)
            {
                list = new List<EmployeeTrainingEmployeeModel>();
                cousers = (from a in db.CourseTrainings.AsNoTracking()
                           join b in db.Courses.AsNoTracking() on a.CourseId equals b.Id
                           where a.EmployeeTrainingId.Equals(item.Id)
                           orderby b.Code
                           select new EmployeeTrainingCourseModel
                           {
                               Id = a.Id,
                               StartDate = a.StartDate,
                               EndDate = a.EndDate
                           }).ToList();

                item.CourseInEmployeeTraining = cousers.Count;
                listCourse.AddRange(cousers);

                if (cousers.Count > 0)
                {
                    item.StartDate = cousers.Min(i => i.StartDate);
                    item.EndDate = cousers.Max(i => i.EndDate);
                }

                item.EmployeeInEmployeeTraining = (from a in db.EmployeeCourseTrainings.AsNoTracking()
                                                   join b in db.CourseTrainings.AsNoTracking() on a.CourseTrainingId equals b.Id
                                                   where b.EmployeeTrainingId.Equals(item.Id)
                                                   group a by a.EmployeeId into g
                                                   select new
                                                   {
                                                       Id = g.Key,
                                                   }).Count();
            }

            searchResult.Status1 = listCourse.Count();
            //searchResult.Status2 = list.GroupBy(i => i.Id).ToList().Count();
            searchResult.Status3 = listResult.Where(i => i.Status).Count();
            searchResult.Status4 = listResult.Where(i => !i.Status).Count();

            return searchResult;
        }

        public List<EmployeeTrainingCourseModel> SearchCourse(CourseSearchModel modelSearch)
        {
            var dataQuery = (from a in db.Courses.AsNoTracking()
                             where !modelSearch.ListIdSelect.Contains(a.Id)
                             orderby a.Code
                             select new EmployeeTrainingCourseModel
                             {
                                 CourseId = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Description = a.Description,
                                 StudyTime = a.StudyTime,
                                 DeviceForCourse = a.DeviceForCourse,
                             }).AsQueryable();

            // Tìm kiếm theo mã khóa học
            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(t => t.Code.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }

            // Tìm kiếm theo tên khóa học
            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(t => t.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            // Tìm kiếm theo thiết bị sử dụng
            if (!string.IsNullOrEmpty(modelSearch.DeviceForCourse))
            {
                dataQuery = dataQuery.Where(t => t.DeviceForCourse.ToUpper().Contains(modelSearch.DeviceForCourse.ToUpper()));
            }

            //foreach (var item in dataQuery.ToList())
            //{
            //    var listEmployee = (from a in db.Employees.AsNoTracking()
            //                        join b in db.EmployeeSkills.AsNoTracking() on a.Id equals b.EmployeeId
            //                        join d in db.CourseSkills.AsNoTracking() on b.WorkSkillId equals d.WorkSkillId
            //                        where d.CourseId.Equals(item.Id) && d.Score > b.Grade
            //                        select new EmployeeModel()
            //                        {
            //                            Id = a.Id,
            //                            Name = a.Name,
            //                            Code = a.Code,
            //                        }).ToList();

            //    item.ListEmployees = listEmployee.GroupBy(x => new
            //    {
            //        x.Id,
            //        x.Name,
            //        x.Code
            //    })
            //        .Select(y => new EmployeeModel()
            //        {
            //            Id = y.Key.Id,
            //            Name = y.Key.Name,
            //            Code = y.Key.Code,
            //        }).ToList();
            //}

            var listResult = dataQuery.ToList();


            return listResult;
        }

        public object GetEmployeeByCourse(List<string> courseIds)
        {
            Dictionary<string, List<EmployeeTrainingEmployeeModel>> courseEmployee = new Dictionary<string, List<EmployeeTrainingEmployeeModel>>();

            List<EmployeeTrainingEmployeeModel> employees;

            foreach (var courseId in courseIds)
            {
                employees = (from c in db.CourseSkills.AsNoTracking()
                             join b in db.EmployeeSkills.AsNoTracking() on c.WorkSkillId equals b.WorkSkillId
                             join a in db.Employees.AsNoTracking() on b.EmployeeId equals a.Id
                             join d in db.Departments.AsNoTracking() on a.DepartmentId equals d.Id
                             join s in db.SBUs.AsNoTracking() on d.SBUId equals s.Id
                             where c.CourseId.Equals(courseId) && c.Score > b.Grade
                             group a by new { a.Name, a.Id, a.Code, SBUName = s.Name, DepartmentName = d.Name } into g
                             select new EmployeeTrainingEmployeeModel()
                             {
                                 EmployeeId = g.Key.Id,
                                 Name = g.Key.Name,
                                 Code = g.Key.Code,
                                 SBUName = g.Key.SBUName,
                                 DepartmentName = g.Key.DepartmentName,
                                 Status = Constants.Employee_Course_Training_Status_Not_Finsh
                             }).ToList();

                courseEmployee.Add(courseId, employees);
            }

            return courseEmployee;
        }

        public List<EmployeeTrainingEmployeeModel> GetEmployeeByCouserId(string couserId)
        {
            var employees = (from c in db.CourseSkills.AsNoTracking()
                             join b in db.EmployeeSkills.AsNoTracking() on c.WorkSkillId equals b.WorkSkillId
                             join a in db.Employees.AsNoTracking() on b.EmployeeId equals a.Id
                             join d in db.Departments.AsNoTracking() on a.DepartmentId equals d.Id
                             join s in db.SBUs.AsNoTracking() on d.SBUId equals s.Id
                             where c.CourseId.Equals(couserId) && c.Score > b.Grade
                             group a by new { a.Name, a.Id, a.Code, SBUName = s.Name, DepartmentName = d.Name } into g
                             select new EmployeeTrainingEmployeeModel()
                             {
                                 EmployeeId = g.Key.Id,
                                 Name = g.Key.Name,
                                 Code = g.Key.Code,
                                 SBUName = g.Key.SBUName,
                                 DepartmentName = g.Key.DepartmentName,
                                 Status = Constants.Employee_Course_Training_Status_Not_Finsh
                             }).ToList();

            return employees;
        }

        public List<EmployeeTrainingEmployeeModel> SearchEmployee(EmployeeSearchModel modelSearch)
        {

            var listCouserSkilId = (from h in db.EmployeeSkills.AsNoTracking()
                                    join a in db.CourseSkills.AsNoTracking() on h.WorkSkillId equals a.WorkSkillId
                                    where a.CourseId.Equals(modelSearch.CourseId)
                                    select new
                                    {
                                        h.EmployeeId,
                                        IsReach = h.Grade >= a.Score ? 1 : 2,
                                    }).ToList();

            var listCouserSkilIdGroup = listCouserSkilId.GroupBy(x => new
            {
                x.EmployeeId,
            }).Select(y => new EmployeeTrainingEmployeeModel()
            {
                EmployeeId = y.Key.EmployeeId,
                IsReach = y.Max(r => r.IsReach)
            }).ToList();

            var dataQuery = (from a in db.Employees.AsEnumerable()
                             join b in db.Departments.AsEnumerable() on a.DepartmentId equals b.Id
                             join e in db.SBUs.AsEnumerable() on b.SBUId equals e.Id
                             join g in listCouserSkilIdGroup on a.Id equals g.EmployeeId into ag
                             from ga in ag.DefaultIfEmpty()
                             join h in db.WorkTypes.AsNoTracking() on a.WorkTypeId equals h.Id
                             where !modelSearch.ListIdSelect.Contains(a.Id) && a.Status == Constants.Employee_Status_Use
                             orderby a.Code
                             select new EmployeeTrainingEmployeeModel
                             {
                                 EmployeeId = a.Id,
                                 Code = a.Code,
                                 Name = a.Name,
                                 DepartmentName = b.Name,
                                 DepartmentId = b.Id,
                                 SBUId = b.SBUId,
                                 SBUName = e.Name,
                                 IsReach = ga != null ? ga.IsReach : 2,
                                 JobPositionName = h.Name,
                                 JobPositionId = h.Id,
                                 Status = a.Status
                             }).AsQueryable();

            //if (!string.IsNullOrEmpty(modelSearch.Name))
            //{
            //    dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            //}

            //if (!string.IsNullOrEmpty(modelSearch.Code))
            //{
            //    dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()));
            //}

            if (!string.IsNullOrEmpty(modelSearch.NameCode))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.NameCode.ToUpper()) || u.Code.ToUpper().Contains(modelSearch.NameCode.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.SBUId))
            {
                dataQuery = dataQuery.Where(u => u.SBUId.Equals(modelSearch.SBUId));
            }

            if (!string.IsNullOrEmpty(modelSearch.DepartmentId))
            {
                dataQuery = dataQuery.Where(u => u.DepartmentId.Equals(modelSearch.DepartmentId));
            }

            if (modelSearch.IsReach.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.IsReach == modelSearch.IsReach);
            }
            if (!string.IsNullOrEmpty(modelSearch.WorkTypeId))
            {
                dataQuery = dataQuery.Where(u => u.JobPositionId.Equals(modelSearch.WorkTypeId));
            }

            //Tìm kiếm nhân viên có kỹ năng trong khóa học
            var listResult = dataQuery.ToList();

            return listResult;
        }

        public void AddEmployeeTraining(EmployeeTrainingModel model, string userLoginId)
        {
            CheckExistedForAdd(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    // Thêm vào trương trình đào tạo
                    EmployeeTraning employeeTraning = new EmployeeTraning()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Code = model.Code.NTSTrim(),
                        Name = model.Name.NTSTrim(),
                        Description = model.Description.NTSTrim(),
                        Status = model.Status
                    };

                    // Khóa học của chương trình đào tạo
                    List<CourseTraining> courseTrainings = new List<CourseTraining>();

                    // Nhân viên của khóa học đào tạo
                    List<EmployeeCourseTraining> employeeCourseTrainings = new List<EmployeeCourseTraining>();

                    // Danh sách khóa học truyền lên
                    if (model.ListCourse.Count > 0)
                    {
                        foreach (var item in model.ListCourse)
                        {
                            var checkStart = item.StartDate.Year;
                            var checkEnd = item.EndDate.Year;

                            if (checkStart == 1 || checkEnd == 1)
                            {
                                throw NTSException.CreateInstance("Bạn chưa chọn thời gian cho khóa học!");
                            }

                            // Thêm dữ liệu truyền lên vào khóa học của trương trình đào tạo
                            CourseTraining courseTraining = new CourseTraining()
                            {
                                Id = Guid.NewGuid().ToString(),
                                EmployeeTrainingId = employeeTraning.Id,
                                CourseId = item.CourseId,
                                StartDate = item.StartDate,
                                EndDate = item.EndDate,
                                Status = item.Status
                            };

                            // Danh sách nhân viên
                            if (item.ListEmployees.Count > 0)
                            {
                                foreach (var items in item.ListEmployees)
                                {
                                    // Thêm dữ liệu vào nhân viên của khóa học đào tạo
                                    EmployeeCourseTraining employeeCourseTraining = new EmployeeCourseTraining()
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        CourseTrainingId = courseTraining.Id,
                                        EmployeeId = items.EmployeeId,
                                        Status = Constants.Employee_Course_Training_Status_Not_Finsh,
                                    };

                                    employeeCourseTrainings.Add(employeeCourseTraining);

                                }
                            }

                            courseTrainings.Add(courseTraining);
                        }
                    }
                    if(model.ListAttachs.Count > 0)
                    {
                        List<EmployeeTrainingAttachFile> list = new List<EmployeeTrainingAttachFile>();
                        foreach (var item in model.ListAttachs)
                        {
                            EmployeeTrainingAttachFile etaf = new EmployeeTrainingAttachFile()
                            {
                                Id = Guid.NewGuid().ToString(),
                                EmployeeTrainingId = employeeTraning.Id,
                                Path = item.FilePath,
                                FileName = item.FileName,
                                FileSize = item.FileSize,
                                CreateBy = userLoginId,
                                CreateDate = DateTime.Now,
                                UpdateBy = userLoginId,
                                UpdateDate = DateTime.Now
                            };
                            list.Add(etaf);
                        }
                        db.EmployeeTrainingAttachFiles.AddRange(list);
                    }

                    db.EmployeeTranings.Add(employeeTraning);
                    db.CourseTrainings.AddRange(courseTrainings);
                    db.EmployeeCourseTrainings.AddRange(employeeCourseTrainings);

                    UserLogUtil.LogHistotyAdd(db, userLoginId, employeeTraning.Code, employeeTraning.Id, Constants.LOG_EmployeeTraining);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
            }
        }

        public void UpdateEmployeeTraining(EmployeeTrainingModel model, string userLoginId)
        {
            CheckExistedForUpdate(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var employeeTraning = db.EmployeeTranings.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                    //var jsonApter = AutoMapperConfig.Mapper.Map<EmployeeTrainingHistoryModel>(employeeTraning);

                    employeeTraning.Name = model.Name.NTSTrim();
                    employeeTraning.Code = model.Code.NTSTrim();
                    employeeTraning.Description = model.Description.NTSTrim();
                    employeeTraning.Status = model.Status;

                    List<CourseTraining> courseTrainings = new List<CourseTraining>();
                    List<EmployeeCourseTraining> employeeCourseTrainings = new List<EmployeeCourseTraining>();
                    List<CourseTrainingSkill> courseTrainingSkills = new List<CourseTrainingSkill>();
                    List<EmployeeSkill> employeeSkills = new List<EmployeeSkill>();
                    if (model.ListCourse.Count > 0)
                    {
                        CourseTraining courseTraining;
                        EmployeeCourseTraining employeeCourseTraining;
                        foreach (var item in model.ListCourse)
                        {
                            if (!string.IsNullOrEmpty(item.Id))
                            {
                                courseTraining = db.CourseTrainings.FirstOrDefault(r => r.Id.Equals(item.Id));

                                if (item.IsDelete)
                                {
                                    db.CourseTrainings.Remove(courseTraining);

                                    db.EmployeeCourseTrainings.RemoveRange(db.EmployeeCourseTrainings.Where(r => r.CourseTrainingId.Equals(item.Id)));
                                    db.CourseTrainingSkills.RemoveRange(db.CourseTrainingSkills.Where(r => r.CourseTrainingId.Equals(item.Id)));

                                    continue;
                                }
                                else
                                {
                                    courseTraining.StartDate = item.StartDate;
                                    courseTraining.EndDate = item.EndDate;
                                }
                            }
                            else
                            {
                                courseTraining = new CourseTraining()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    EmployeeTrainingId = model.Id,
                                    CourseId = item.CourseId,
                                    StartDate = item.StartDate,
                                    EndDate = item.EndDate,
                                    Status = Constants.Course_Training_Status_Not_Learn
                                };

                                courseTrainings.Add(courseTraining);
                            }

                            if (item.ListEmployees.Count > 0 && !item.IsDelete)
                            {
                                foreach (var items in item.ListEmployees)
                                {

                                    if (!string.IsNullOrEmpty(items.Id))
                                    {
                                        employeeCourseTraining = db.EmployeeCourseTrainings.FirstOrDefault(r => r.Id.Equals(items.Id));

                                        if (items.IsDelete)
                                        {
                                            db.EmployeeCourseTrainings.Remove(employeeCourseTraining);
                                            db.CourseTrainingSkills.RemoveRange(db.CourseTrainingSkills.Where(r => r.CourseTrainingId.Equals(item.Id) && r.EmployeeId.Equals(items.EmployeeId)));

                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        employeeCourseTraining = new EmployeeCourseTraining()
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            CourseTrainingId = courseTraining.Id,
                                            EmployeeId = items.EmployeeId,
                                            Status = Constants.Employee_Course_Training_Status_Not_Finsh,
                                        };

                                        employeeCourseTrainings.Add(employeeCourseTraining);
                                    }
                                }
                            }
                        }
                        if (model.ListAttachs.Count > 0)
                        {
                            List<EmployeeTrainingAttachFile> list = new List<EmployeeTrainingAttachFile>();
                            foreach (var attach in model.ListAttachs)
                            {
                                if (string.IsNullOrEmpty(attach.Id) && attach.IsDelete == false)
                                {
                                    EmployeeTrainingAttachFile etaf = new EmployeeTrainingAttachFile()
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        EmployeeTrainingId = model.Id,
                                        Path = attach.FilePath,
                                        FileName = attach.FileName,
                                        FileSize = attach.FileSize,
                                        CreateBy = userLoginId,
                                        CreateDate = DateTime.Now,
                                        UpdateBy = userLoginId,
                                        UpdateDate = DateTime.Now
                                    };
                                    list.Add(etaf);
                                }
                                else if (!string.IsNullOrEmpty(attach.Id) && attach.IsDelete == true)
                                {
                                    var attachDelete = db.EmployeeTrainingAttachFiles.FirstOrDefault(a => a.Id.Equals(attach.Id));
                                    db.EmployeeTrainingAttachFiles.Remove(attachDelete);
                                }
                                else if (!string.IsNullOrEmpty(attach.Id) && attach.IsDelete == false)
                                {
                                    var attachUpdate = db.EmployeeTrainingAttachFiles.FirstOrDefault(a => a.Id.Equals(attach.Id));
                                    if (attachUpdate != null)
                                    {
                                        attachUpdate.Path = attach.FilePath;
                                        attachUpdate.FileName = attach.FileName;
                                        attachUpdate.FileSize = attach.FileSize;
                                        attachUpdate.UpdateBy = userLoginId;
                                        attachUpdate.UpdateDate = DateTime.Now;
                                    }
                                }
                            }
                            db.EmployeeTrainingAttachFiles.AddRange(list);
                        }
                    }

                    db.CourseTrainings.AddRange(courseTrainings);
                    db.EmployeeCourseTrainings.AddRange(employeeCourseTrainings);

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<EmployeeTrainingHistoryModel>(employeeTraning);

                    //UserLogUtil.LogHistotyUpdateInfo(db, userLoginId, Constants.LOG_EmployeeTraining, employeeTraning.Id, employeeTraning.Code, jsonBefor, jsonApter);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
            }
        }

        public void DeleteEmployeeTraining(EmployeeTrainingModel model, string userLoginId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var employeeTraning = db.EmployeeTranings.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (employeeTraning == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.EmployeeTraining);
                    }

                    var courseTrainings = db.CourseTrainings.Where(i => model.Id.Equals(i.EmployeeTrainingId)).ToList();
                    if (courseTrainings.Count > 0)
                    {
                        foreach (var item in courseTrainings)
                        {
                            var employeeCourseTrainings = db.EmployeeCourseTrainings.Where(i => item.Id.Equals(i.CourseTrainingId)).ToList();
                            if (employeeCourseTrainings.Count > 0)
                            {
                                db.EmployeeCourseTrainings.RemoveRange(employeeCourseTrainings);
                            }

                            var courseTrainingSkills = db.CourseTrainingSkills.Where(i => item.Id.Equals(i.CourseTrainingId)).ToList();
                            if (courseTrainingSkills.Count > 0)
                            {
                                db.CourseTrainingSkills.RemoveRange(courseTrainingSkills);
                            }
                        }
                        db.CourseTrainings.RemoveRange(courseTrainings);
                    }

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<EmployeeTrainingHistoryModel>(employeeTraning);

                    //UserLogUtil.LogHistotyDelete(db, userLoginId, Constants.LOG_EmployeeTraining, employeeTraning.Id, employeeTraning.Code, jsonBefor);

                    db.EmployeeTranings.Remove(employeeTraning);
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
            }
        }

        public EmployeeTrainingModel GetEmployeeTrainingInfo(EmployeeTrainingModel model)
        {
            var resultInfo = db.EmployeeTranings.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new EmployeeTrainingModel
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name,
                Description = p.Description,
                Status = p.Status
            }).FirstOrDefault();

            resultInfo.ListCourse = (from a in db.CourseTrainings.AsNoTracking()
                                     join b in db.Courses.AsNoTracking() on a.CourseId equals b.Id
                                     where a.EmployeeTrainingId.Equals(model.Id)
                                     orderby b.Code
                                     select new EmployeeTrainingCourseModel
                                     {
                                         Id = a.Id,
                                         CourseId = b.Id,
                                         Name = b.Name,
                                         Code = b.Code,
                                         Description = b.Description,
                                         StudyTime = b.StudyTime,
                                         Status = a.Status,
                                         DeviceForCourse = b.DeviceForCourse,
                                         StartDate = a.StartDate,
                                         EndDate = a.EndDate
                                     }).ToList();

            if (resultInfo.ListCourse.Count > 0)
            {
                foreach (var item in resultInfo.ListCourse)
                {
                    item.ListEmployees = (from a in db.EmployeeCourseTrainings.AsNoTracking()
                                          join b in db.Employees.AsNoTracking() on a.EmployeeId equals b.Id
                                          join c in db.Departments.AsNoTracking() on b.DepartmentId equals c.Id into ac
                                          from c in ac.DefaultIfEmpty()
                                          join d in db.JobPositions.AsNoTracking() on b.JobPositionId equals d.Id into ad
                                          from d in ad.DefaultIfEmpty()
                                          join e in db.SBUs.AsNoTracking() on c.SBUId equals e.Id into ce
                                          from e in ce.DefaultIfEmpty()
                                          where a.CourseTrainingId.Equals(item.Id)
                                          orderby b.Code
                                          select new EmployeeTrainingEmployeeModel
                                          {
                                              Id = a.Id,
                                              EmployeeId = a.EmployeeId,
                                              Code = b.Code,
                                              Name = b.Name,
                                              Status = a.Status,
                                              DepartmentId = b.DepartmentId,
                                              DepartmentName = c.Name,
                                              SBUId = c.SBUId,
                                              SBUName =e.Name
                                          }).ToList();
                }

            }
            var attachs = db.EmployeeTrainingAttachFiles.Where(a => a.EmployeeTrainingId.Equals(resultInfo.Id)).Select(b => new EmployeeTrainingFileAttachModel
            {
                Id = b.Id,
                EmployeeTrainingId = b.Id,
                FilePath = b.Path,
                FileName = b.FileName,
                FileSize = b.FileSize,
                CreateName = db.Users.FirstOrDefault(a => a.Id.Equals(b.CreateBy)).UserName,
                CreateDate = b.CreateDate,

            }).ToList();
            resultInfo.ListAttachs = attachs;

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.EmployeeTraining);
            }

            return resultInfo;
        }

        private void CheckExistedForAdd(EmployeeTrainingModel model)
        {
            if (db.EmployeeTranings.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.EmployeeTraining);
            }
        }

        public void CheckExistedForUpdate(EmployeeTrainingModel model)
        {
            if (db.EmployeeTranings.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.EmployeeTraining);
            }
        }

        public List<WorkSkillResultModel> GetWorkKillEndEmployee(EmployeeTrainingWorkSkillSearchModel model)
        {
            List<WorkSkillResultModel> list = new List<WorkSkillResultModel>();

            var data = (from a in db.Courses.AsNoTracking()
                        join b in db.CourseSkills.AsNoTracking() on a.Id equals b.CourseId
                        join c in db.WorkSkills.AsNoTracking() on b.WorkSkillId equals c.Id
                        where a.Id.Equals(model.CourseId)
                        orderby c.Name
                        select new WorkSkillResultModel
                        {
                            Id = a.Id,
                            Name = c.Name,
                            WorkSkillId = c.Id,
                            Mark = 0,
                            Score = b != null ? b.Score : 0
                        }).ToList();

            list = data.Where(i => string.IsNullOrEmpty(i.EmployeeId) || i.EmployeeId.Equals(model.EmployeeId)).ToList();

            foreach (var item in list)
            {
                item.Max = 10;
                item.OldMark = 0;
                item.Grade = 0;
                var employee = db.EmployeeSkills.AsNoTracking().FirstOrDefault(i => i.WorkSkillId.Equals(item.WorkSkillId) && i.EmployeeId.Equals(model.EmployeeId));
                if (employee != null)
                {
                    item.EmployeeId = employee.EmployeeId;
                    item.Max = employee.Mark;
                    item.Grade = employee.Grade;
                }

                if (!string.IsNullOrEmpty(item.EmployeeId))
                {
                    var courseTrainingSkill = db.CourseTrainingSkills.AsNoTracking().FirstOrDefault(i => i.EmployeeId.Equals(item.EmployeeId) && i.CourseSkillId.Equals(item.Id) && i.WorkSkillId.Equals(item.WorkSkillId));
                    if (courseTrainingSkill != null)
                    {
                        item.Mark = courseTrainingSkill.Mark;
                        item.OldMark = courseTrainingSkill.OldMark;
                        item.CourseTrainingSkillId = courseTrainingSkill.Id;
                    }
                }
            }

            return list;
        }

        public EmployeeTrainingUpdatePointResultModel UpdatePointEmployee(EmployeeTrainingUpdatePointModel model)
        {
            EmployeeTrainingUpdatePointResultModel pointResult = new EmployeeTrainingUpdatePointResultModel();
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    List<CourseTrainingSkill> courseTrainingSkills = new List<CourseTrainingSkill>();
                    List<EmployeeSkill> employeeSkills = new List<EmployeeSkill>();
                    CourseTrainingSkill courseTrainingSkill;
                    foreach (var ite in model.ListWorkSkill)
                    {
                        if (!string.IsNullOrEmpty(ite.CourseTrainingSkillId))
                        {
                            courseTrainingSkill = db.CourseTrainingSkills.FirstOrDefault(r => r.Id.Equals(ite.CourseTrainingSkillId));
                            courseTrainingSkill.Mark = ite.Mark;
                        }
                        else
                        {
                            courseTrainingSkill = new CourseTrainingSkill()
                            {
                                Id = Guid.NewGuid().ToString(),
                                CourseTrainingId = model.CourseTrainingId,
                                WorkSkillId = ite.WorkSkillId,
                                Mark = ite.Mark,
                                CourseSkillId = ite.Id,
                                EmployeeId = model.EmployeeId,
                            };

                            courseTrainingSkills.Add(courseTrainingSkill);
                        }

                        if (string.IsNullOrEmpty(ite.EmployeeId))
                        {
                            EmployeeSkill employeeSkill = new EmployeeSkill()
                            {
                                Id = Guid.NewGuid().ToString(),
                                EmployeeId = model.EmployeeId,
                                WorkSkillId = ite.WorkSkillId,
                                Mark = ite.Max,
                                Grade = ite.Mark
                            };

                            employeeSkills.Add(employeeSkill);
                        }
                        else
                        {
                            var resultInfo = db.EmployeeSkills.FirstOrDefault(u => u.EmployeeId.Equals(ite.EmployeeId) && ite.WorkSkillId.Equals(u.WorkSkillId));
                            if (string.IsNullOrEmpty(ite.CourseTrainingSkillId))
                            {
                                courseTrainingSkill.OldMark = Convert.ToInt32(resultInfo.Grade);
                            }

                            if (resultInfo != null)
                            {
                                if (!model.CheckSave)
                                {
                                    resultInfo.Grade = ite.Mark;
                                }
                            }
                        }
                    }

                    var employees = db.EmployeeCourseTrainings.AsNoTracking().Where(r => r.CourseTrainingId.Equals(model.CourseTrainingId)).ToList();
                    var employeeOK = employees.Count(r => !r.EmployeeId.Equals(model.EmployeeId) && r.CourseTrainingId.Equals(model.CourseTrainingId) && r.Status == Constants.Employee_Course_Training_Status_Finsh);

                    if (employeeOK == employees.Count - 1)
                    {
                        var courseTraining = db.CourseTrainings.FirstOrDefault(a => a.Id.Equals(model.CourseTrainingId));
                        if (courseTraining == null)
                        {
                            throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Course);
                        }

                        courseTraining.Status = Constants.Employee_Course_Training_Status_Finsh;
                        pointResult.CourseStatus = courseTraining.Status;
                    }

                    var employeeCourseTraining = db.EmployeeCourseTrainings.FirstOrDefault(r => r.Id.Equals(model.EmployeeCourseTrainingId));
                    if (employeeCourseTraining == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Employee);
                    }

                    employeeCourseTraining.Status = Constants.Employee_Course_Training_Status_Finsh;
                    pointResult.EmployeeStatus = employeeCourseTraining.Status;

                    db.CourseTrainingSkills.AddRange(courseTrainingSkills);
                    db.EmployeeSkills.AddRange(employeeSkills);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
            }

            return pointResult;
        }
    }
}
