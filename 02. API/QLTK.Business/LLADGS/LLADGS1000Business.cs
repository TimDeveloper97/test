using NTS.Common;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.LLADGS;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Utils;
using SQLHelpper = NTS.Model.SQLHelpper;
using System.Web;

namespace QLTK.Business.LLADGS
{
    public class LLADGS1000Business
    {
        QLTKEntities db = new QLTKEntities();

        public SearchResultObject<LLADGS1000Model> GetGasStation(LLADGS1000SearchModel modelSearch)
        {
            SearchResultObject<LLADGS1000Model> searchResult = new SearchResultObject<LLADGS1000Model>();
            try
            {
                var listGasStation = db.Employees.AsNoTracking().Select(o => new LLADGS1000Model
                {
                    Id = o.Id,
                    Name = o.Name,
                    PhoneNumber = o.PhoneNumber,
                    Address = o.Address,
                }).AsQueryable();

                //checks conditions
                if (!string.IsNullOrEmpty(modelSearch.Name))
                {
                    listGasStation = listGasStation.Where(r => r.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
                }
                if (!string.IsNullOrEmpty(modelSearch.Address))
                {
                    listGasStation = listGasStation.Where(r => r.Address.ToUpper().Contains(modelSearch.Address));
                }
                searchResult.TotalItem = listGasStation.Count();
                var listResult = SQLHelpper.OrderBy(listGasStation, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
                searchResult.ListResult = listResult.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh trong quá trình xử lý" + ex);
            }
            return searchResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void DeleteGasStation(LLADGS1000Model model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception("Có lỗi trong quá trình xử lý. " + ex.Message);
                }
            }



        }

        public void AddGasStation(LLADGS1000Model model, HttpFileCollection hfc)
        {

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception("Có lỗi trong quá trình xử lý " + ex.Message);
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void UpdateGasStation(LLADGS1000Model model, HttpFileCollection hfc)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception("Có lỗi trong quá trình xử lý " + ex.Message);
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public LLADGS1000Model GetGasStationInfo(LLADGS1000Model model)
        {
            return null;
        }


    }
}
