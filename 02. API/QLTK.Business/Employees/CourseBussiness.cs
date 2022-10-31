using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Course;
using NTS.Model.CourseAttach;
using NTS.Model.CourseSkill;
using NTS.Model.Employees;
using NTS.Model.Repositories;
using NTS.Model.UserHistory;
using NTS.Model.WorldSkill;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.Courses
{
    public class CourseBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        /// <summary>
        ///  Tìm kiếm khóa học.
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<CourseModel> SearchCourse(CourseSearchModel modelSearch)
        {
            SearchResultModel<CourseModel> searchResult = new SearchResultModel<CourseModel>();

            var dataQuery = (from a in db.Courses.AsNoTracking()
                             orderby a.Code
                             select new CourseModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Description = a.Description,
                                 Status = a.Status,
                                 StudyTime = a.StudyTime,
                                 DeviceForCourse = a.DeviceForCourse,
                                 ParentId = a.ParentId,
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

            // Tìm kiếm theo trạng thái
            if (!string.IsNullOrEmpty(modelSearch.Status.ToString()))
            {
                if (modelSearch.Status == true)
                {
                    dataQuery = dataQuery.Where(t => t.Status == true);
                }
                else
                {
                    dataQuery = dataQuery.Where(t => t.Status == false);
                }
            }

            // Tìm kiếm theo thiết bị sử dụng
            if (!string.IsNullOrEmpty(modelSearch.DeviceForCourse))
            {
                dataQuery = dataQuery.Where(t => t.DeviceForCourse.ToUpper().Contains(modelSearch.DeviceForCourse.ToUpper()));
            }

            searchResult.TotalItem = dataQuery.Count();
            var listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType).ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public List<CourseModel> GetListParentCourse()
        {
            var dataQuery = (from a in db.Courses.AsNoTracking()
                             orderby a.Code
                             select new CourseModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Description = a.Description,
                                 Status = a.Status,
                                 StudyTime = a.StudyTime,
                                 DeviceForCourse = a.DeviceForCourse,
                                 ParentId =a.ParentId
                             }).ToList();
            return dataQuery;
        }

        /// <summary>
        /// Thêm mới khóa học.
        /// </summary>
        /// <param name="model"></param>
        public void CreateCourse(CourseModel model, string userId)
        {
            model.Code = Util.RemoveSpecialCharacter(model.Code);

            if (db.Courses.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Course);
            }

            // check mã đã tồn tại chưa
            if (db.Courses.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Course);
            }
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    Course course = new Course()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = model.Name,
                        Code = model.Code,
                        Status = model.Status,
                        StudyTime = model.StudyTime,
                        Description = model.Description,
                        DeviceForCourse = model.DeviceForCourse,
                        ParentId = model.ParentId
                    };
                    db.Courses.Add(course);

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, course.Code, course.Id, Constants.LOG_Course);

                    // thêm list file
                    if (model.ListFile.Count > 0)
                    {
                        List<CourseAttachFile> listCourseAttach = new List<CourseAttachFile>();
                        foreach (var item in model.ListFile)
                        {
                            if (item.Path != null)
                            {

                                CourseAttachFile fileEntity = new CourseAttachFile()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    CourseId = course.Id,
                                    Path = item.Path,
                                    FileName = item.FileName,
                                    FileSize = item.FileSize,
                                    CreateDate = DateTime.Now,
                                    CreateBy = userId,
                                    UpdateDate = DateTime.Now,
                                    UpdateBy = userId,

                                };
                                listCourseAttach.Add(fileEntity);
                            }
                        }
                        db.CourseAttachFiles.AddRange(listCourseAttach);
                    }
                    // thêm list kĩ năng
                    var listAllWorkSkill = (from a in db.WorkSkills.AsNoTracking()
                                            orderby a.Name
                                            select new WorkSkillModel()
                                            {
                                                Id = a.Id,
                                                Name = a.Name,
                                                ParentId = a.WorkSkillGroupId,
                                                WorkSkillGroupId = a.WorkSkillGroupId,
                                            }).AsQueryable();

                    var queryWorkSkillGroup = (from a in db.WorkSkillGroups.AsNoTracking()
                                               orderby a.Name
                                               select new WorkSkillModel()
                                               {
                                                   Id = a.Id,
                                                   Name = a.Name,
                                                   ParentId = a.ParentId,
                                               }).AsQueryable();
                    List<WorkSkillModel> listAllWorkSkillGroup = queryWorkSkillGroup.ToList();
                    List<WorkSkillModel> listAllChild = queryWorkSkillGroup.Where(a => !string.IsNullOrEmpty(a.ParentId)).ToList();
                    if (model.ListCourseSkill.Count > 0)
                    {
                        CourseSkill works;
                        List<CourseSkill> listCourseSkills = new List<CourseSkill>();

                        foreach (var it in model.ListCourseSkill)
                        {
                            if (!string.IsNullOrEmpty(it.WorkSkillGroupId))
                            {
                                works = new CourseSkill()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    CourseId = course.Id,
                                    WorkSkillId = it.Id,
                                    Score = it.Score,
                                };
                                listCourseSkills.Add(works);
                            }
                            else
                            {
                                var listWorkSkillGroupParentChild = GetWorkSkillGroupChild(it.Id, listAllChild);
                                var parentWorkSkillGroup = listAllWorkSkillGroup.Where(a => a.Id.Equals(it.Id)).FirstOrDefault();
                                listWorkSkillGroupParentChild.Add(parentWorkSkillGroup);
                                foreach (var group in listWorkSkillGroupParentChild)
                                {
                                    var listAdd = listAllWorkSkill.Where(a => a.WorkSkillGroupId.Equals(group.Id)).ToList();
                                    foreach (var item in listAdd)
                                    {
                                        works = new CourseSkill()
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            CourseId = course.Id,
                                            WorkSkillId = item.Id,
                                            Score = it.Score,
                                        };
                                        listCourseSkills.Add(works);
                                    }
                                }
                            }
                        }
                        db.CourseSkills.AddRange(listCourseSkills);
                    }
                    db.SaveChanges();
                    trans.Commit();

                }
                catch (Exception x)
                {
                    trans.Rollback();
                    throw x;
                }
            }
        }

        public List<WorkSkillModel> GetWorkSkillGroupChild(string parentId, List<WorkSkillModel> listAll)
        {
            List<WorkSkillModel> result = new List<WorkSkillModel>();
            var listChild = listAll.Where(a => a.ParentId.Equals(parentId)).ToList();
            List<WorkSkillModel> listChildChild = new List<WorkSkillModel>();
            foreach (var item in listChild)
            {
                listChildChild = GetWorkSkillGroupChild(item.Id, listAll);
                result.Add(item);
                result.AddRange(listChildChild);
            }
            return result;
        }


        /// <summary>
        /// Load danh sách skill.
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<WorkSkillModel> SearchCourseSkill(WorkSkillModel modelSearch)
        {
            SearchResultModel<WorkSkillModel> searchResult = new SearchResultModel<WorkSkillModel>();

            //List<WorkSkillModel> list = new List<WorkSkillModel>();
            // danh sách 
            var listIdWorkSkill = (from a in db.Courses.AsNoTracking()
                                   join b in db.CourseSkills.AsNoTracking() on a.Id equals b.CourseId
                                   //join c in db.WorkSkills.AsNoTracking() on b.WorkSkillId equals c.Id
                                   where a.Id.Equals(modelSearch.WorkSkillId)
                                   select new CourseSkillModel()
                                   {
                                       //Id = c.Id,
                                       //Name = c.Name,
                                       //ParentId = c.WorkSkillGroupId,
                                       //WorkSkillGroupId = c.WorkSkillGroupId,
                                       WorkSkillId = b.WorkSkillId,
                                       Score = b.Score,
                                   }).AsQueryable();

            var dataQuery = (from a in db.WorkSkills.AsNoTracking()
                             join b in listIdWorkSkill on a.Id equals b.WorkSkillId into ab
                             from ba in ab.DefaultIfEmpty()
                             orderby a.Name
                             select new WorkSkillModel()
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 ParentId = a.WorkSkillGroupId,
                                 WorkSkillGroupId = a.WorkSkillGroupId,
                                 Score = ba!=null ? ba.Score:0,
                             }).AsQueryable();

            //list.AddRange(listIdWorkSkill);
            //list.AddRange(dataQuery);

            var queryWorkSkillGroup = (from a in db.WorkSkillGroups.AsNoTracking()
                                       orderby a.Name
                                       select new WorkSkillModel()
                                       {
                                           Id = a.Id,
                                           Name = a.Name,
                                           ParentId = a.ParentId,
                                       }).AsQueryable();

            // Tìm kiếm theo mã khóa học
            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(t => modelSearch.Name.Equals(t.Name));
            }

            searchResult.TotalItem = dataQuery.Count();
            searchResult.ListResult = dataQuery.ToList();
            searchResult.ListResult.AddRange(queryWorkSkillGroup.ToList());

            return searchResult;
        }

        /// <summary>
        /// Lấy thông tin dữ liệu theo id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public object GetInfoCourse(CourseModel model)
        {
            try
            {
                var result = db.Courses.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new CourseModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    StudyTime = p.StudyTime,
                    Code = p.Code,
                    Status = p.Status,
                    DeviceForCourse = p.DeviceForCourse,
                    Description = p.Description,
                    ParentId = p.ParentId
                }).FirstOrDefault();




                if (result == null)
                {
                    throw new Exception("Khóa học này đã bị xóa bởi người dùng khác");
                }

                result.ListFile = db.CourseAttachFiles.AsNoTracking().Where(u => u.CourseId.Equals(model.Id)).Select(a => new CourseAttachModel
                {
                    Id = a.Id,
                    FileName = a.FileName,
                    CourseId = a.CourseId,
                    FileSize = a.FileSize,
                    Path = a.Path,
                }).ToList();

                // Lấy dữ liệu kĩ năng theo khóa học
                var listCourseSkill = (from a in db.CourseSkills.AsNoTracking()
                                       where model.Id.Equals(a.CourseId)
                                       select new CourseSkillModel()
                                       {
                                           Id = a.WorkSkillId,
                                           //Name = c.Name,
                                       }).ToList();

                result.ListId = listCourseSkill.Select(i => i.Id).ToList();
                result.ListCourseSkill = listCourseSkill;
                return result;
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Cập nhật khóa học
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateCourse(CourseModel model, string userId)
        {
            bool cs = false;
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            using (var trans = db.Database.BeginTransaction())
            {
                if (db.Courses.AsNoTracking().Where(o => !model.Id.Equals(o.Id) && model.Name.Equals(o.Name)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Course);
                }
                if (db.Courses.AsNoTracking().Where(o => !model.Id.Equals(o.Id) && model.Code.Equals(o.Code)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Course);
                }
                try
                {
                    var newCourse = db.Courses.Where(u => u.Id.Equals(model.Id)).FirstOrDefault();

                    //var jsonApter = AutoMapperConfig.Mapper.Map<CourseHistoryModel>(newCourse);

                    newCourse.Name = model.Name;
                    newCourse.Code = model.Code;
                    newCourse.StudyTime = model.StudyTime;
                    newCourse.Status = model.Status;
                    newCourse.Description = model.Description;
                    newCourse.ParentId = model.ParentId;
                    newCourse.DeviceForCourse = model.DeviceForCourse;

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<CourseHistoryModel>(newCourse);

                    //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_Course, newCourse.Id, newCourse.Code, jsonBefor, jsonApter);

                    // Lấy list file theo Id khóa học
                    var fileCourse = db.CourseAttachFiles.Where(t => t.CourseId.Equals(model.Id));
                    if (fileCourse != null)
                    {
                        // Xóa list file theo Id khóa học
                        db.CourseAttachFiles.RemoveRange(fileCourse);
                        if (model.ListFile.Count > 0)
                        {
                            List<CourseAttachFile> listCourseAttach = new List<CourseAttachFile>();
                            foreach (var item in model.ListFile)
                            {
                                if (item.Path != null)
                                {
                                    CourseAttachFile fileEntity = new CourseAttachFile()
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        CourseId = newCourse.Id,
                                        Path = item.Path,
                                        FileName = item.FileName,
                                        FileSize = item.FileSize,
                                        CreateDate = DateTime.Now,
                                        CreateBy = userId,
                                        UpdateBy = userId,
                                        UpdateDate = DateTime.Now,
                                    };
                                    listCourseAttach.Add(fileEntity);
                                }

                            }
                            db.CourseAttachFiles.AddRange(listCourseAttach);
                        }

                    }
                    //Lấy list kĩ năng theo Id khóa học
                    var courseSkill = db.CourseSkills.Where(t => t.CourseId.Equals(model.Id));
                    if (courseSkill != null)
                    {
                        // Xóa list kĩ năng theo Id
                        db.CourseSkills.RemoveRange(courseSkill);
                        var listAllWorkSkill = (from a in db.WorkSkills.AsNoTracking()
                                                orderby a.Name
                                                select new WorkSkillModel()
                                                {
                                                    Id = a.Id,
                                                    Name = a.Name,
                                                    ParentId = a.WorkSkillGroupId,
                                                    WorkSkillGroupId = a.WorkSkillGroupId,
                                                }).AsQueryable();
                        var queryWorkSkillGroup = (from a in db.WorkSkillGroups.AsNoTracking()
                                                   orderby a.Name
                                                   select new WorkSkillModel()
                                                   {
                                                       Id = a.Id,
                                                       Name = a.Name,
                                                       ParentId = a.ParentId,
                                                   }).AsQueryable();
                        List<WorkSkillModel> listAllWorkSkillGroup = queryWorkSkillGroup.ToList();
                        List<WorkSkillModel> listAllChild = queryWorkSkillGroup.Where(a => !string.IsNullOrEmpty(a.ParentId)).ToList();
                        if (model.ListCourseSkill.Count > 0)
                        {
                            List<CourseSkill> listCourseSkills = new List<CourseSkill>();
                            CourseSkill works;
                            foreach (var it in model.ListCourseSkill)
                            {
                                if (!string.IsNullOrEmpty(it.WorkSkillGroupId))
                                {
                                    works = new CourseSkill()
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        CourseId = newCourse.Id,
                                        WorkSkillId = it.Id,
                                        Score = it.Score,
                                    };
                                    listCourseSkills.Add(works);
                                }
                                else
                                {
                                    var listWorkSkillGroupParentChild = GetWorkSkillGroupChild(it.Id, listAllChild);
                                    var parentWorkSkillGroup = listAllWorkSkillGroup.Where(a => a.Id.Equals(it.Id)).FirstOrDefault();
                                    listWorkSkillGroupParentChild.Add(parentWorkSkillGroup);
                                    foreach (var group in listWorkSkillGroupParentChild)
                                    {
                                        var listAdd = listAllWorkSkill.Where(a => a.WorkSkillGroupId.Equals(group.Id)).ToList();
                                        foreach (var item in listAdd)
                                        {
                                            works = new CourseSkill()
                                            {
                                                Id = Guid.NewGuid().ToString(),
                                                CourseId = newCourse.Id,
                                                WorkSkillId = item.Id,
                                                Score = it.Score,
                                            };
                                            listCourseSkills.Add(works);
                                        }
                                    }
                                }
                            }
                            db.CourseSkills.AddRange(listCourseSkills);
                        }
                    }
                    db.SaveChanges();
                    trans.Commit();
                    cs = true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
            }
            return cs;
        }

        /// <summary>
        /// Delete khóa học
        /// </summary>
        /// <param name="model"></param>
        public void DeleteCourse(CourseModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var course = db.Courses.Where(m => m.Id.Equals(model.Id)).FirstOrDefault();
                if (course == null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Course);
                }
                var courseCheck = db.Courses.Where(m => m.ParentId.Equals(course.Id)).FirstOrDefault();
                if (courseCheck != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Course);
                }
                var courseEmployeeTraning = db.CourseTrainings.Where(m => m.CourseId.Equals(model.Id)).FirstOrDefault();
                if (courseEmployeeTraning != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Course);
                }
                try
                {
                    var courseFile = db.CourseAttachFiles.Where(m => m.CourseId.Equals(model.Id)).ToList();
                    if (courseFile.Count() > 0)
                    {
                        db.CourseAttachFiles.RemoveRange(courseFile);
                    }

                    var courseSkill = db.CourseSkills.Where(m => m.CourseId.Equals(model.Id)).ToList();
                    if (courseSkill.Count() > 0)
                    {
                        db.CourseSkills.RemoveRange(courseSkill);
                    }

                    db.Courses.Remove(course);

                    var NameOrCode = course.Code;

                    //var jsonApter = AutoMapperConfig.Mapper.Map<CourseHistoryModel>(course);
                    //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_Course, course.Id, NameOrCode, jsonApter);
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
    }
}
