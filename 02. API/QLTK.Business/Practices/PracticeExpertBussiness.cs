using NTS.Model.Combobox;
using NTS.Model.Repositories;
using NTS.Model.Expert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Model;
using NTS.Model.PracticeExperts;
using NTS.Model.Bank;
using QLTK.Business.Users;
using NTS.Common;

namespace QLTK.Business.PracticeExperts
{
    public class PracticeExpertBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public SearchResultModel<PracticeExpertsModel> SearchPracticeExpert(PracticeExpertsSearchModel modelSearch)
        {
            SearchResultModel<PracticeExpertsModel> searchResult = new SearchResultModel<PracticeExpertsModel>();
            var dataQuery = (from k in db.PracticeExperts.AsNoTracking()
                             join p in db.Practices.AsNoTracking() on k.PracticeId equals p.Id
                             join a in db.Experts.AsNoTracking() on k.ExpertId equals a.Id
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
                             where k.PracticeId.Equals(modelSearch.PracticeId)
                             orderby a.Code
                             select new PracticeExpertsModel
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
                                 BankAccountName = ga.AccountName,
                                 Description = a.Description,
                             }).AsQueryable();
            var y = dataQuery.ToList();
            List<PracticeExpertsModel> listRs = new List<PracticeExpertsModel>();
            var lstRs = dataQuery.GroupBy(t => new { t.Id, t.Name, t.Code, t.DegreeName, t.PhoneNumber }).ToList();

            foreach (var item in lstRs)
            {
                PracticeExpertsModel rs = new PracticeExpertsModel();
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
            }
            searchResult.TotalItem = listRs.Count();

            var listResult = SQLHelpper.OrderBy(listRs.AsQueryable(), modelSearch.OrderBy, modelSearch.OrderType)
               .Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public SearchResultModel<ExpertModel> SearchExpert(ExpertSearchModel modelSearch)
        {
            SearchResultModel<ExpertModel> searchResult = new SearchResultModel<ExpertModel>();
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
                             where !modelSearch.ListIdSelect.Contains(a.Id)
                             orderby a.Code
                             select new ExpertModel
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
                                 BankAccountName = ga.AccountName,
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

            var y = dataQuery.ToList();
            List<ExpertModel> listRs = new List<ExpertModel>();
            var lstRs = dataQuery.GroupBy(t => new { t.Id, t.Name, t.Code, t.DegreeName, t.PhoneNumber }).ToList();

            foreach (var item in lstRs)
            {
                ExpertModel rs = new ExpertModel();
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

        public void AddPracticeExperts(PracticeExpertsModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var practices = db.PracticeExperts.Where(u => u.PracticeId.Equals(model.PracticeId)).ToList();
                if (practices.Count > 0)
                {
                    db.PracticeExperts.RemoveRange(practices);
                }
                try
                {
                    if (model.listSelect.Count > 0)
                    {
                        foreach (var item in model.listSelect)
                        {
                            if (item.ExpertId != null)
                            {
                                item.Id = item.ExpertId;
                            }
                            PracticeExpert practiceExpert = new PracticeExpert
                            {
                                Id = Guid.NewGuid().ToString(),
                                PracticeId = model.PracticeId,
                                ExpertId = item.Id,
                            };
                            db.PracticeExperts.Add(practiceExpert);
                        }
                    }
                    UserLogUtil.LogHistotyUpdateSub(db, model.CreateBy, Constants.LOG_Practice, model.PracticeId, string.Empty, "Chuyên gia");

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

        public SearchResultModel<PracticeExpertsModel> LoadPracticeExpert(PracticeExpertsSearchModel modelSearch)
        {
            var practiceGroupId = db.Practices.AsNoTracking().Where(a => a.Id.Equals(modelSearch.PracticeId)).Select(b => b.PracticeGroupId).FirstOrDefault();

            var querySpecicalizeId = (from a in db.SpecializeInPracticeGroups.AsNoTracking()
                                      where a.PracticeGroupId.Equals(practiceGroupId)
                                      join b in db.Specializes.AsNoTracking() on a.SpecializeId equals b.Id into ab
                                      from b in ab.DefaultIfEmpty()
                                      select b.Id).AsQueryable();

            List<string> listSpecializeId = new List<string>();
            if (querySpecicalizeId != null)
            {
                listSpecializeId = querySpecicalizeId.ToList();
            }

            List<string> listExpertId = (from a in db.SpecializeExperts.AsNoTracking()
                                         where listSpecializeId.Contains(a.SpecializeId)
                                         select a.ExpertId).ToList();

            SearchResultModel<PracticeExpertsModel> searchResult = new SearchResultModel<PracticeExpertsModel>();

            var query = (from a in db.Experts.AsNoTracking()
                         join b in db.Degrees.AsNoTracking() on a.DegreeId equals b.Id
                         join c in db.Workplaces.AsNoTracking() on a.WorkplaceId equals c.Id into ac
                         from c in ac.DefaultIfEmpty()
                         join e in db.SpecializeExperts.AsNoTracking() on a.Id equals e.ExpertId
                         join d in db.Specializes.AsNoTracking() on e.SpecializeId equals d.Id
                         where listExpertId.Contains(a.Id)
                         select new PracticeExpertsModel
                         {
                             ExpertId = a.Id,
                             Code = a.Code,
                             Name = a.Name,
                             DegreeName = b.Name,
                             WorkPlaceName = c.Name,
                             SpecializeName = d.Name
                         }).AsQueryable();

            List<PracticeExpertsModel> listRs = new List<PracticeExpertsModel>();
            var lstRs = query.GroupBy(t => new { t.ExpertId, t.Name, t.Code }).ToList();
            foreach (var item in lstRs)
            {
                PracticeExpertsModel rs = new PracticeExpertsModel();
                //rs.Id = item.Key.ExpertId;
                rs.Name = item.Key.Name;
                rs.Code = item.Key.Code;
                //rs.PracticeId = item.Key.PracticeId;
                rs.ExpertId = item.Key.ExpertId;
                List<string> lstSTemp = new List<string>();
                List<string> lstWTemp = new List<string>();
                List<string> lstDegree = new List<string>();
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
                        }
                    }
                    else
                    {
                        rs.WorkPlaceName += ite.WorkPlaceName;
                        lstWTemp.Add(ite.WorkPlaceName);
                    }
                    if (lstDegree.Count > 0)
                    {
                        if (!lstWTemp.Contains(ite.DegreeName))
                        {
                            rs.DegreeName += ", " + ite.DegreeName;
                        }
                    }
                    else
                    {
                        rs.DegreeName = ite.DegreeName;
                        lstWTemp.Add(ite.DegreeName);
                    }
                }
                listRs.Add(rs);
            }
            searchResult.TotalItem = listRs.Count();

            var listResult = SQLHelpper.OrderBy(listRs.AsQueryable(), modelSearch.OrderBy, modelSearch.OrderType)
               .Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

    }
}
