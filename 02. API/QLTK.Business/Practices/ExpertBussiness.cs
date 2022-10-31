using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Combobox;
using NTS.Model.Expert;
using NTS.Model.Function;
using NTS.Model.Specialize;
using NTS.Model.WorkPlace;
using NTS.Utils;
using Syncfusion.Pdf.Graphics;
using Syncfusion.XlsIO;
using System.Web.Hosting;
using System.Web;
using NTS.Model.Bank;
using QLTK.Business.Users;
using QLTK.Business.AutoMappers;
using NTS.Model.ExpertHistory;

namespace QLTK.Business.Expert
{
    public class Expert
    {
        private QLTKEntities db = new QLTKEntities();

        public SearchResultModel<ExpertResultModel> SearchExpert(ExpertSearchModel modelSearch)
        {
            SearchResultModel<ExpertResultModel> searchResult = new SearchResultModel<ExpertResultModel>();

            var dataQuery = (from a in db.Experts.AsNoTracking()
                             join b in db.Degrees.AsNoTracking() on a.DegreeId equals b.Id into ab
                             from ba in ab.DefaultIfEmpty()
                             join c in db.ExpertWorkplaces.AsNoTracking() on a.Id equals c.ExpertId into ac
                             from ca in ac.DefaultIfEmpty()
                             join e in db.SpecializeExperts.AsNoTracking() on a.Id equals e.ExpertId into ae
                             from ea in ae.DefaultIfEmpty()
                             join d in db.Specializes.AsNoTracking() on ea.SpecializeId equals d.Id into ad
                             from da in ad.DefaultIfEmpty()
                             join f in db.Workplaces.AsNoTracking() on ca.WorkplaceId equals f.Id into af
                             from fa in af.DefaultIfEmpty()
                             join g in db.Banks.AsNoTracking() on a.Id equals g.ExpertId into ag
                             from ga in ag.DefaultIfEmpty()
                             orderby a.Name
                             select new ExpertResultModel()
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Email = a.Email,
                                 PhoneNumber = a.PhoneNumber,
                                 DegreeId = ba.Id,
                                 DegreeName = ba.Name,
                                 SpecializeName = da.Name,
                                 WorkPlaceName = fa.Name,
                                 SpecializeId = da.Id,
                                 WorkPlaceId = fa.Id,
                                 BankAccountName = ga.AccountName,
                             }).AsQueryable();
            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.BankAccountName))
            {
                dataQuery = dataQuery.Where(u =>
                    u.BankAccountName.ToUpper().Contains(modelSearch.BankAccountName.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.SpecializeId))
            {
                dataQuery = dataQuery.Where(u => u.SpecializeId.ToUpper().Equals(modelSearch.SpecializeId.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.WorkPlaceId))
            {
                dataQuery = dataQuery.Where(u => u.WorkPlaceId.ToUpper().Equals(modelSearch.WorkPlaceId.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.DegreeId))
            {
                dataQuery = dataQuery.Where(u => u.DegreeId.ToUpper().Equals(modelSearch.DegreeId.ToUpper()));
            }
            var y = dataQuery.ToList();
            List<ExpertResultModel> listRs = new List<ExpertResultModel>();
            var lstRs = dataQuery.GroupBy(t => new { t.Id, t.Name, t.Code, t.DegreeName, t.PhoneNumber }).ToList();

            foreach (var item in lstRs)
            {
                ExpertResultModel rs = new ExpertResultModel();
                rs.Id = item.Key.Id;
                rs.Name = item.Key.Name;
                rs.Code = item.Key.Code;
                rs.DegreeName = item.Key.DegreeName;
                rs.PhoneNumber = item.Key.PhoneNumber;
                List<string> lstSTemp = new List<string>();
                List<string> lstWTemp = new List<string>();
                foreach (var ite in item.ToList())
                {
                    if (lstSTemp.Count > 0)
                    {
                        if (!lstSTemp.Contains(ite.SpecializeName))
                        {
                            rs.SpecializeName += ", " + ite.SpecializeName;
                            lstSTemp.Add(ite.SpecializeName);
                        }
                    }
                    else
                    {
                        rs.SpecializeName += ite.SpecializeName;
                        lstSTemp.Add(ite.SpecializeName);
                    }

                    if (lstWTemp.Count > 0)
                    {
                        if (!lstWTemp.Contains(ite.WorkPlaceName))
                        {
                            rs.WorkPlaceName += ", " + ite.WorkPlaceName;
                            lstWTemp.Add(ite.WorkPlaceName);
                        }
                    }
                    else
                    {
                        rs.WorkPlaceName += ite.WorkPlaceName;
                        lstWTemp.Add(ite.WorkPlaceName);
                    }
                }
                listRs.Add(rs);
            }
            foreach (var item in listRs)
            {
                item.ListBank = db.Banks.AsNoTracking().Where(t => t.ExpertId.Equals(item.Id)).Select(m => new BankModel
                {
                    Id = m.Id,
                    //NameBank =m.Account + m.Name + m == null ? "Chi nhánh" : m.AccountName,
                    Name = m.Name,// tên ngân hàng
                    Account = m.Account, // Số tài khoản
                    AccountName = m.AccountName, // Chi nhánh ngân hàng
                }).ToList();
            }
            searchResult.TotalItem = listRs.Count();

            var listResult = SQLHelpper.OrderBy(listRs.AsQueryable(), modelSearch.OrderBy, modelSearch.OrderType)
               .Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }
        public SearchResultModel<SpecializeResultModel> SearchSqecialize(SpecializeSearchModel modelSearch)
        {
            SearchResultModel<SpecializeResultModel> searchResult = new SearchResultModel<SpecializeResultModel>();
            var dataQuery = (from a in db.Specializes.AsNoTracking()
                             where !modelSearch.ListIdSelect.Contains(a.Id)
                             orderby a.Name
                             select new SpecializeResultModel()
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Description = a.Description,
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }

            searchResult.TotalItem = dataQuery.Count();
            searchResult.ListResult = dataQuery.ToList();
            return searchResult;
        }
        public SearchResultModel<WorkPlaceResultModel> SearchworkPlace(WorkPlaceSearchModel modelSearch)
        {
            SearchResultModel<WorkPlaceResultModel> searchResult = new SearchResultModel<WorkPlaceResultModel>();
            var dataQuery = (from a in db.Workplaces.AsNoTracking()
                             where !modelSearch.ListIdSelect.Contains(a.Id)
                             orderby a.Name
                             select new WorkPlaceResultModel()
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Description = a.Description,
                                 Status = "1"
                             }).AsQueryable();
            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }
            searchResult.TotalItem = dataQuery.Count();
            searchResult.ListResult = dataQuery.ToList();
            return searchResult;
        }
        public ExpertModel GetIdExpert(ExpertModel model)
        {
            var resultInfo = db.Experts.AsNoTracking().Where(u => u.Id.Equals(model.Id)).Select(p => new ExpertModel()
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                Description = p.Description,
                Email = p.Email,
                //BankAccount = p.BankAccount,
                //BankName = p.BankName,
                PhoneNumber = p.PhoneNumber,
                // BankAccountName = p.BankAccountName,
                DegreeId = p.DegreeId,
            }).FirstOrDefault();

            var ListWorkplaces = (from a in db.ExpertWorkplaces.AsNoTracking()
                                  where a.ExpertId.Equals(model.Id)
                                  join b in db.Workplaces.AsNoTracking() on a.WorkplaceId equals b.Id
                                  orderby b.Code
                                  select new WorkPlaceModel()
                                  {
                                      Id = b.Id,
                                      Code = b.Code,
                                      Name = b.Name,
                                      Description = b.Description,
                                      Status = a.Status,
                                  }).ToList();
            resultInfo.ListWorkPlace = ListWorkplaces;

            //var status = db.ExpertWorkplaces.AsNoTracking().Where(a => a.ExpertId.Equals(model.Id)).FirstOrDefault();
            //resultInfo.Status = status.Status;

            var ListSpecialize = (from a in db.SpecializeExperts.AsNoTracking()
                                  where a.ExpertId.Equals(model.Id)
                                  join b in db.Specializes.AsNoTracking() on a.SpecializeId equals b.Id
                                  orderby b.Code
                                  select new SpecializeModel()
                                  {
                                      Id = b.Id,
                                      Code = b.Code,
                                      Name = b.Name,
                                      Description = b.Description,
                                  }).ToList();
            resultInfo.ListSpecialize = ListSpecialize;

            // Khởi tạo 1 tk ngân hàng để thêm mới 
            var ListBank = (from a in db.Banks.AsNoTracking()
                            where a.ExpertId.Equals(model.Id)
                            select new BankModel()
                            {
                                Id = a.Id,
                                Name = a.Name,
                                Account = a.Account,
                                AccountName = a.AccountName,
                            }).ToList();
            resultInfo.ListBank = ListBank;

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Expert);
            }
            return resultInfo;
        }
        public void AddExpert(ExpertModel model)
        {
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForAdd(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    NTS.Model.Repositories.Expert newExpert = new NTS.Model.Repositories.Expert()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Code = model.Code.Trim(),
                        Name = model.Name.Trim(),
                        Description = model.Description.Trim(),
                        Email = model.Email.Trim(),
                        PhoneNumber = model.PhoneNumber.Trim(),
                        DegreeId = model.DegreeId,

                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now,
                    };

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, newExpert.Code, newExpert.Id, Constants.LOG_Expert);

                    foreach (var item in model.ListSpecialize)
                    {
                        SpecializeExpert specialize = new SpecializeExpert()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ExpertId = newExpert.Id,
                            SpecializeId = item.Id,

                        };
                        db.SpecializeExperts.Add(specialize);
                    }

                    foreach (var item in model.ListWorkPlace)
                    {
                        ExpertWorkplace workplace = new ExpertWorkplace()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ExpertId = newExpert.Id,
                            WorkplaceId = item.Id,
                            Status = item.Status,

                        };
                        db.ExpertWorkplaces.Add(workplace);
                    }
                    db.Experts.Add(newExpert);

                    foreach (var item in model.ListBank)
                    {
                        NTS.Model.Repositories.Bank bank = new NTS.Model.Repositories.Bank()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ExpertId = newExpert.Id,
                            Name = item.Name,
                            Account = item.Account,
                            AccountName = item.AccountName,
                        };
                        db.Banks.Add(bank);
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
        public void UpdateExpert(ExpertModel model)
        {
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForUpdate(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var newExpert = db.Experts.Where(u => u.Id.Equals(model.Id)).FirstOrDefault();

                    //var jsonApter = AutoMapperConfig.Mapper.Map<ExpertHistoryModel>(newExpert);

                    newExpert.Name = model.Name.Trim();
                    newExpert.Code = model.Code.Trim();
                    newExpert.Description = model.Description.NTSTrim();
                    newExpert.Email = model.Email.Trim();
                    //newExpert.BankAccount = model.BankAccount;
                    // newExpert.BankName = model.BankName;
                    newExpert.PhoneNumber = model.PhoneNumber.Trim();
                    // newExpert.BankAccountName = model.BankAccountName;
                    newExpert.DegreeId = model.DegreeId;
                    newExpert.UpdateBy = model.UpdateBy;
                    newExpert.UpdateDate = DateTime.Now;

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<ExpertHistoryModel>(newExpert);

                    //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_Expert, newExpert.Id, newExpert.Code, jsonBefor, jsonApter);

                    var listSpecialize = db.SpecializeExperts.Where(a => a.ExpertId.Equals(model.Id)).ToList();
                    if (listSpecialize.Count > 0)
                    {
                        db.SpecializeExperts.RemoveRange(listSpecialize);
                    }
                    if (model.ListSpecialize.Count > 0)
                    {
                        foreach (var item in model.ListSpecialize)
                        {
                            SpecializeExpert specialize = new SpecializeExpert()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ExpertId = newExpert.Id,
                                SpecializeId = item.Id,

                            };
                            db.SpecializeExperts.Add(specialize);
                        }
                    }

                    var listWorkplace = db.ExpertWorkplaces.Where(a => a.ExpertId.Equals(model.Id)).ToList();
                    if (listWorkplace.Count > 0)
                    {
                        db.ExpertWorkplaces.RemoveRange(listWorkplace);
                    }
                    if (model.ListWorkPlace.Count > 0)
                    {
                        foreach (var item in model.ListWorkPlace)
                        {
                            ExpertWorkplace workplace = new ExpertWorkplace()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ExpertId = newExpert.Id,
                                WorkplaceId = item.Id,
                                Status = item.Status,
                            };
                            db.ExpertWorkplaces.Add(workplace);
                        }
                    }

                    var listBank = db.Banks.Where(a => a.ExpertId.Equals(model.Id)).ToList();
                    if (listBank.Count > 0)
                    {
                        db.Banks.RemoveRange(listBank);
                    }
                    if (model.ListBank.Count > 0)
                    {
                        foreach (var item in model.ListBank)
                        {
                            NTS.Model.Repositories.Bank bank = new NTS.Model.Repositories.Bank()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ExpertId = newExpert.Id,
                                Account = item.Account,
                                Name = item.Name,
                                AccountName = item.AccountName,
                            };
                            db.Banks.Add(bank);
                        }
                    }

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw;
                }
            }
        }
        public void DeleteExpert(ExpertModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {

                var practiceExpert = db.PracticeExperts.AsNoTracking().Where(m => m.ExpertId.Equals(model.Id)).FirstOrDefault();
                if (practiceExpert != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Expert);
                }
                try
                {
                    var _expert = db.Experts.FirstOrDefault(a => a.Id.Equals(model.Id));
                    if (_expert == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Expert);
                    }

                    var _specialize = db.SpecializeExperts.Where(b => b.ExpertId.Equals(model.Id)).ToList();
                    db.SpecializeExperts.RemoveRange(_specialize);

                    var _workPkace = db.ExpertWorkplaces.Where(c => c.ExpertId.Equals(model.Id)).ToList();
                    db.ExpertWorkplaces.RemoveRange(_workPkace);

                    var _bank = db.Banks.Where(b => b.ExpertId.Equals(model.Id)).ToList();
                    db.Banks.RemoveRange(_bank);

                    db.Experts.Remove(_expert);

                    var NameOrCode = _expert.Code;

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<ExpertHistoryModel>(_expert);
                    //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_Expert, _expert.Id, NameOrCode, jsonBefor);

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
        private void CheckExistedForAdd(ExpertModel model)
        {
            //if (db.Experts.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
            //{
            //    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Expert);
            //}

            if (db.Experts.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Expert);
            }
        }
        public void CheckExistedForUpdate(ExpertModel model)
        {
            //if (db.Experts.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Name.Equals(model.Name)).Count() > 0)
            //{
            //    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Expert);
            //}

            if (db.Experts.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Expert);
            }
        }
        public void CheckDeleteBank(string expertId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var bank = db.Banks.FirstOrDefault(a => a.Id.Equals(expertId));
                    if (bank == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Bank);
                    }

                    db.Banks.Remove(bank);
                    db.SaveChanges();
                    trans.Commit();
                }

                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(null, ex);
                }
            }
        }
        public string ExportExcelExpert(ExpertSearchModel model)
        {
            model.IsExport = true;
            var dataQuery = (from a in db.Experts.AsNoTracking()
                             join b in db.Degrees.AsNoTracking() on a.DegreeId equals b.Id
                             join c in db.ExpertWorkplaces.AsNoTracking() on a.Id equals c.ExpertId into ac
                             from c in ac.DefaultIfEmpty()
                             join e in db.SpecializeExperts.AsNoTracking() on a.Id equals e.ExpertId into ae
                             from e in ae.DefaultIfEmpty()
                             join d in db.Specializes.AsNoTracking() on e.SpecializeId equals d.Id into ad
                             from d in ad.DefaultIfEmpty()
                             join f in db.Workplaces.AsNoTracking() on c.WorkplaceId equals f.Id into af
                             from f in af.DefaultIfEmpty()
                             join g in db.Banks.AsNoTracking() on a.Id equals g.ExpertId into ag
                             from g in ag.DefaultIfEmpty()
                             orderby a.Name
                             select new ExpertResultModel()
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Email = a.Email,
                                 PhoneNumber = a.PhoneNumber,
                                 DegreeId = b.Id,
                                 DegreeName = b.Name,
                                 SpecializeName = d.Name,
                                 WorkPlaceName = f.Name,
                                 SpecializeId = d.Id,
                                 WorkPlaceId = f.Id,
                                 BankAccountName = g.AccountName,
                             }).AsQueryable();
            if (!string.IsNullOrEmpty(model.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(model.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(model.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(model.Code.ToUpper()));
            }

            if (!string.IsNullOrEmpty(model.BankAccountName))
            {
                dataQuery = dataQuery.Where(u =>
                    u.BankAccountName.ToUpper().Contains(model.BankAccountName.ToUpper()));
            }

            if (!string.IsNullOrEmpty(model.SpecializeId))
            {
                dataQuery = dataQuery.Where(u => u.SpecializeId.ToUpper().Equals(model.SpecializeId.ToUpper()));
            }

            if (!string.IsNullOrEmpty(model.WorkPlaceId))
            {
                dataQuery = dataQuery.Where(u => u.WorkPlaceId.ToUpper().Equals(model.WorkPlaceId.ToUpper()));
            }

            if (!string.IsNullOrEmpty(model.DegreeId))
            {
                dataQuery = dataQuery.Where(u => u.DegreeId.ToUpper().Equals(model.DegreeId.ToUpper()));
            }

            List<ExpertResultModel> listRs = new List<ExpertResultModel>();
            var lstRs = dataQuery.GroupBy(t => new { t.Id, t.Name, t.Code, t.DegreeName, t.PhoneNumber }).ToList();

            foreach (var item in lstRs)
            {
                ExpertResultModel rs = new ExpertResultModel();
                rs.Id = item.Key.Id;
                rs.Name = item.Key.Name;
                rs.Code = item.Key.Code;
                rs.DegreeName = item.Key.DegreeName;
                rs.PhoneNumber = item.Key.PhoneNumber;
                List<string> lstSTemp = new List<string>();
                List<string> lstWTemp = new List<string>();
                foreach (var ite in item.ToList())
                {
                    if (lstSTemp.Count > 0)
                    {
                        if (!lstSTemp.Contains(ite.SpecializeName))
                        {
                            rs.SpecializeName += ", " + ite.SpecializeName;
                            lstSTemp.Add(ite.SpecializeName);
                        }
                    }
                    else
                    {
                        rs.SpecializeName += ite.SpecializeName;
                        lstSTemp.Add(ite.SpecializeName);
                    }

                    if (lstWTemp.Count > 0)
                    {
                        if (!lstWTemp.Contains(ite.WorkPlaceName))
                        {
                            rs.WorkPlaceName += ", " + ite.WorkPlaceName;
                            lstWTemp.Add(ite.WorkPlaceName);
                        }
                    }
                    else
                    {
                        rs.WorkPlaceName += ite.WorkPlaceName;
                        lstWTemp.Add(ite.WorkPlaceName);
                    }
                }
                listRs.Add(rs);
            }
            foreach (var item in listRs)
            {
                item.ListBank = db.Banks.AsNoTracking().Where(t => t.ExpertId.Equals(item.Id)).Select(m => new BankModel
                {
                    Id = m.Id,
                    Name = m.Name,// tên ngân hàng
                    Account = m.Account, // Số tài khoản
                    AccountName = m.AccountName, // Chi nhánh ngân hàng
                }).ToList();
                //if (item.ListBank.Count() > 0)
                //{
                //    item.BankName = db.Banks.Where(u => u.ExpertId.Equals(item.Id)).FirstOrDefault().Name;
                //    item.BankAccount = db.Banks.Where(u => u.ExpertId.Equals(item.Id)).FirstOrDefault().Account;
                //    item.BankAccountName = db.Banks.Where(u => u.ExpertId.Equals(item.Id)).FirstOrDefault().AccountName;
                //}
            }
            if (listRs.Count == 0)
            {
                throw new Exception("Không có dữ liệu!");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/expert.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = listRs.Count;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                //var listExport = listRs.OrderBy(i => i.Code).Select((a, i) => new
                //{
                //    Index = i + 1,
                //    a.Code,
                //    a.Name,
                //    a.WorkPlaceName,
                //    a.DegreeName,
                //    a.SpecializeName,
                //    a.PhoneNumber,
                //    a.BankAccount,
                //    a.BankName,
                //    a.BankAccountName,
                //});
                var list = listRs.OrderBy(i=>i.Code).ToList();
                int index = 1;
                List<ExpertExcelModel> listExport = new List<ExpertExcelModel>();
                foreach (var item in list)
                {
                   if(item.ListBank.Count> 1)
                    {
                        listExport.Add(new ExpertExcelModel()
                        {
                            Index = index++.ToString(),
                            Code = item.Code,
                            Name = item.Name,
                            WorkPlaceName = item.WorkPlaceName,
                            DegreeName = item.DegreeName,
                            SpecializeName = item.SpecializeName,
                            PhoneNumber = item.PhoneNumber,
                        });
                        foreach(var it in item.ListBank)
                        {
                            listExport.Add(new ExpertExcelModel()
                            {
                                BankName = it.Name,
                                BankAccount = it.Account,
                                BankAccountName = it.AccountName
                            });
                        }
                        
                    }
                    else
                    {
                        listExport.Add(new ExpertExcelModel()
                        {
                            Index = index++.ToString(),
                            Code = item.Code,
                            Name = item.Name,
                            WorkPlaceName = item.WorkPlaceName,
                            DegreeName = item.DegreeName,
                            SpecializeName = item.SpecializeName,
                            PhoneNumber = item.PhoneNumber,
                            BankAccount = item.BankAccount,
                            BankName = item.BankName,
                            BankAccountName = item.BankAccountName,
                        });
                    }
                }

                if (listExport.Count() > 1)
                {
                    sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }
                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 10].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 10].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 10].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 10].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 10].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 10].CellStyle.WrapText = true;

                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách chuyên gia" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách chuyên gia" + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                //Log.LogError(ex);
                throw new Exception("Có lỗi trong quá trình xử lý" + ex.Message);
            }

        }
    }
}
