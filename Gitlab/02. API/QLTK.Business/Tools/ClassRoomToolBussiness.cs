using NTS.Common;
using NTS.Model.ClassRoomTool;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.ClassRoomTools
{
    public class ClassRoomToolBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public PracticeAndSkillModel GetPracticeAndSkill(ClassRoomToolPracticeAndSkillModel model)
        {
            List<string> ListSkillName = new List<string>();
            foreach (var item in model.ListPractice)
            {
                item.ListCheckPracticeSkill = new List<ClassRoomToolPracticeSkillModel>();
            }

            var listPracticeSkills = (from a in db.PracticeSkills.AsEnumerable()
                                      join b in model.ListPractice.AsEnumerable() on a.PracticeId equals b.Id
                                      join c in model.ListSkill.AsEnumerable() on a.SkillId equals c.Id
                                      select new
                                      {
                                          a.PracticeId,
                                          a.SkillId
                                      }).ToList();

            foreach (var item in model.ListSkill)
            {
                ListSkillName.Add(item.Code);
                foreach (var ite in model.ListPractice)
                {
                    var check = listPracticeSkills.FirstOrDefault(i => i.PracticeId.Equals(ite.Id) && i.SkillId.Equals(item.Id));
                    ite.ListCheckPracticeSkill.Add(new ClassRoomToolPracticeSkillModel
                    {
                        PracticeId = ite.Id,
                        SkillId = item.Id,
                        Check = check != null ? true : false
                    });
                }
            }

            return new PracticeAndSkillModel
            {
                ListSkillName = ListSkillName,
                ListPractices = model.ListPractice
            };
        }
        public PracticeAndProductModel GetPracticeAndProduct(ClassRoomToolPracticeAndProductModel model)
        {
            List<string> ListPraticeName = new List<string>();
            foreach (var item in model.ListProduct)
            {
                item.ListCheckPracticeProduct = new List<ClassRoomToolPracticeProductModel>();
            }

            var listPracticInProducts = (from a in db.PracticInProducts.AsEnumerable()
                                         join b in model.ListPractice.AsEnumerable() on a.PracticeId equals b.Id
                                         join c in model.ListProduct.AsEnumerable() on a.ProductId equals c.Id
                                         select new
                                         {
                                             a.PracticeId,
                                             a.ProductId
                                         }).ToList();

            foreach (var item in model.ListPractice)
            {
                ListPraticeName.Add(item.Code);
                foreach (var ite in model.ListProduct)
                {
                    var check = listPracticInProducts.FirstOrDefault(i => i.PracticeId.Equals(item.Id) && i.ProductId.Equals(ite.Id));
                    ite.ListCheckPracticeProduct.Add(new ClassRoomToolPracticeProductModel
                    {
                        PracticeId = item.Id,
                        ProductId = ite.Id,
                        Check = check != null ? true : false
                    });
                }
            }

            return new PracticeAndProductModel
            {
                ListPraticeName = ListPraticeName,
                ListProducts = model.ListProduct
            };
        }
        public void AddClassRoomTool(ClassRoomToolModel model, string userId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var classRoom = db.ClassRoomTools.FirstOrDefault(i => i.CreateBy.Equals(userId));
                if (classRoom != null)
                {
                    var classRoomToolSkills = db.ClassRoomToolSkills.Where(i => i.ClassRoomToolId.Equals(classRoom.Id)).ToList();
                    if (classRoomToolSkills.Count > 0)
                    {
                        db.ClassRoomToolSkills.RemoveRange(classRoomToolSkills);
                    }
                    var classRoomToolPractices = db.ClassRoomToolPractices.Where(i => i.ClassRoomToolId.Equals(classRoom.Id)).ToList();
                    if (classRoomToolPractices.Count > 0)
                    {
                        db.ClassRoomToolPractices.RemoveRange(classRoomToolPractices);
                    }
                    var classRoomToolProducts = db.ClassRoomToolProducts.Where(i => i.ClassRoomToolId.Equals(classRoom.Id)).ToList();
                    if (classRoomToolProducts.Count > 0)
                    {
                        db.ClassRoomToolProducts.RemoveRange(classRoomToolProducts);
                    }
                    db.ClassRoomTools.Remove(classRoom);
                }

                try
                {
                    ClassRoomTool classRoomTool = new ClassRoomTool()
                    {
                        Id = Guid.NewGuid().ToString(),
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                        UpdateBy = userId,
                        UpdateDate = DateTime.Now
                    };
                    db.ClassRoomTools.Add(classRoomTool);

                    foreach (var item in model.ListSkill)
                    {
                        ClassRoomToolSkill classRoomToolSkill = new ClassRoomToolSkill()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ClassRoomToolId = classRoomTool.Id,
                            SkillId = item.Id
                        };
                        db.ClassRoomToolSkills.Add(classRoomToolSkill);
                    }

                    foreach (var item in model.ListPractice)
                    {
                        ClassRoomToolPractice classRoomToolPractice = new ClassRoomToolPractice()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ClassRoomToolId = classRoomTool.Id,
                            PracticeId = item.Id
                        };
                        db.ClassRoomToolPractices.Add(classRoomToolPractice);
                    }

                    foreach (var item in model.ListProduct)
                    {
                        ClassRoomToolProduct classRoomToolProduct = new ClassRoomToolProduct()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ClassRoomToolId = classRoomTool.Id,
                            ProductId = item.Id
                        };
                        db.ClassRoomToolProducts.Add(classRoomToolProduct);
                    }

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
        public object GetClassRoomToolInfo(string userId)
        {
            ClassRoomToolPracticeAndSkillModel classRoomToolPracticeAndSkillModel = new ClassRoomToolPracticeAndSkillModel();
            ClassRoomToolPracticeAndProductModel classRoomToolPracticeAndProductModel = new ClassRoomToolPracticeAndProductModel();
            PracticeAndSkillModel practiceAndSkillModel = new PracticeAndSkillModel();
            PracticeAndProductModel practiceAndProductModel = new PracticeAndProductModel();
            List<ClassRoomToolSkillModel> ListSkill = new List<ClassRoomToolSkillModel>();
            var classRoomTool = db.ClassRoomTools.AsNoTracking().FirstOrDefault(i => i.CreateBy.Equals(userId));
            if (classRoomTool != null)
            {
                var classRoomToolSkills = (from a in db.ClassRoomToolSkills.AsNoTracking()
                                           join b in db.Skills.AsNoTracking() on a.SkillId equals b.Id
                                           select new ClassRoomToolSkillModel
                                           {
                                               Id = b.Id,
                                               Name = b.Name,
                                               Code = b.Code
                                           }).ToList();
                var classRoomToolPractices = (from a in db.ClassRoomToolPractices.AsNoTracking()
                                              join b in db.Practices.AsNoTracking() on a.PracticeId equals b.Id
                                              select new ClassRoomToolPracticeModel
                                              {
                                                  Id = b.Id,
                                                  Name = b.Name,
                                                  Code = b.Code
                                              }).ToList();
                var classRoomToolProducts = (from a in db.ClassRoomToolProducts.AsNoTracking()
                                             join b in db.Products.AsNoTracking() on a.ProductId equals b.Id
                                             select new ClassRoomToolProductModel
                                             {
                                                 Id = b.Id,
                                                 Name = b.Name,
                                                 Code = b.Code
                                             }).ToList();

                ListSkill = classRoomToolSkills.ToList();

                classRoomToolPracticeAndSkillModel.ListSkill = classRoomToolSkills;
                classRoomToolPracticeAndSkillModel.ListPractice = classRoomToolPractices;
                practiceAndSkillModel = GetPracticeAndSkill(classRoomToolPracticeAndSkillModel);

                classRoomToolPracticeAndProductModel.ListPractice = classRoomToolPractices;
                classRoomToolPracticeAndProductModel.ListProduct = classRoomToolProducts;
                practiceAndProductModel = GetPracticeAndProduct(classRoomToolPracticeAndProductModel);
            }
            return new
            {
                ListSkill = ListSkill,
                ListSkillName = practiceAndSkillModel.ListSkillName,
                ListPractices = practiceAndSkillModel.ListPractices,
                ListPraticeName = practiceAndProductModel.ListPraticeName,
                ListProducts = practiceAndProductModel.ListProducts
            };
        }
        public PracticeAndSkillModel GetAutoPracticeWithSkill(ClassRoomToolPracticeAndSkillModel model)
        {
            var listId = (from a in db.PracticeSkills.AsEnumerable()
                          join b in model.ListSkill.AsEnumerable() on a.SkillId equals b.Id
                          group a by new { a.PracticeId } into g
                          select new
                          {
                              g.Key.PracticeId,
                          }).ToList();

            var practice = (from a in db.Practices.AsEnumerable()
                            join b in listId.AsEnumerable() on a.Id equals b.PracticeId
                            select new ClassRoomToolPracticeModel
                            {
                                Id = a.Id,
                                Name = a.Name,
                                Code = a.Code
                            }).ToList();

            model.ListPractice = practice;
            var practiceAndSkillModel = GetPracticeAndSkill(model);

            return new PracticeAndSkillModel
            {
                ListSkillName = practiceAndSkillModel.ListSkillName,
                ListPractices = practiceAndSkillModel.ListPractices
            };
        }

        public PracticeAndProductModel GetAutoProductWithPractice(ClassRoomToolPracticeAndProductModel model)
        {
            var listId = (from a in db.PracticInProducts.AsEnumerable()
                          join b in model.ListPractice.AsEnumerable() on a.PracticeId equals b.Id
                          group a by new { a.ProductId } into g
                          select new
                          {
                              g.Key.ProductId,
                          }).ToList();

            var product = (from a in db.Products.AsEnumerable()
                           join b in listId.AsEnumerable() on a.Id equals b.ProductId
                           select new ClassRoomToolProductModel
                           {
                               Id = a.Id,
                               Name = a.Name,
                               Code = a.Code
                           }).ToList();

            model.ListProduct = product;
            var practiceAndProductModel = GetPracticeAndProduct(model);

            return new PracticeAndProductModel
            {
                ListPraticeName = practiceAndProductModel.ListPraticeName,
                ListProducts = practiceAndProductModel.ListProducts
            };
        }


    }
}
