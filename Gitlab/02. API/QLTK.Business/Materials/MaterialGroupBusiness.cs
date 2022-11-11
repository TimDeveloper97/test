using NTS.Model.MaterialGroup;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.MaterialGroups
{
    public class MaterialGroupBusiness
    {
        QLTKEntities db = new QLTKEntities();
        public List<MaterialGroupFromDBModel> GetListMaterialGroup()
        {
            List<MaterialGroupFromDBModel> searchResult = new List<MaterialGroupFromDBModel>();
            searchResult = (from a in db.MaterialGroups.AsNoTracking()
                            orderby a.Code
                            select new MaterialGroupFromDBModel
                            {
                                Id = a.Id,
                                Name = a.Name,
                                Code = a.Code,
                                MaterialGroupTPAId = a.MaterialGroupTPAId,
                                ParentId = a.ParentId,
                            }).ToList();
            return searchResult;
        }

        public List<MaterialGroupModel> GetMaterialGroups()
        {
            List<MaterialGroupModel> listModel = new List<MaterialGroupModel>();
            listModel = (from a in db.MaterialGroups.AsNoTracking()
                         orderby a.Code
                         select new MaterialGroupModel()
                         {
                             Id = a.Id,
                             Name = a.Name,
                             Code = a.Code,
                             ParentId = a.ParentId
                         }).ToList();
            if (listModel.Count > 0)
            {
                foreach (var item in listModel)
                {
                    var groups = db.MaterialGroups.AsNoTracking().Where(t => item.Id.Equals(t.ParentId)).ToList();
                    if (groups.Count > 0)
                    {
                        foreach (var ite in groups)
                        {
                            item.ListChildId.Add(ite.Id);
                        }
                    }
                }
            }
            //if (!string.IsNullOrEmpty(modelSearch.Id))
            //{
            //    var check = db.MaterialGroups.Where(i => modelSearch.Id.Equals(i.Id)).FirstOrDefault().ParentId;
            //    if (check == null)
            //    {
            //        var name = db.MaterialGroups.Where(i => modelSearch.Id.Equals(i.Id)).Select(i => i.Name).ToList();

            //        Ids.AddRange(name);
            //        Id = db.MaterialGroups.Where(i => i.ParentId.Equals(modelSearch.Id)).Select(i => i.Id).ToList();
            //        Ids.AddRange(db.MaterialGroups.Where(i => i.ParentId.Equals(modelSearch.Id)).Select(i => i.Name).ToList());
            //        var List = GetListParentId(Id);
            //        Id.AddRange(List.ListParentId);
            //        Ids.AddRange(List.ListParentName);
            //    }
            //    else
            //    {
            //        var name = db.MaterialGroups.Where(i => modelSearch.Id.Equals(i.Id)).Select(i => i.Name).ToList();
            //        Ids.AddRange(name);
            //        Id.Add(modelSearch.Id);
            //        var List = GetId(check);
            //        Id.AddRange(List.ListParentId);
            //        Ids.AddRange(List.ListParentName);
            //        Id = db.MaterialGroups.Where(i => i.ParentId.Equals(modelSearch.Id)).Select(i => i.Id).ToList();
            //        Ids.AddRange(db.MaterialGroups.Where(i => i.ParentId.Equals(modelSearch.Id)).Select(i => i.Name).ToList());
            //        var Lists = GetListParentId(Id);
            //        Id.AddRange(Lists.ListParentId);
            //        Ids.AddRange(Lists.ListParentName);
            //    }
            //}
            //var id = db.MaterialGroups.Where(i => modelSearch.Id.Equals(i.Id)).Select(i => i.Id).ToList();
            //Id.AddRange(id);
            //searchResult.ListParentId = Id;
            //searchResult.ListParentName = Ids;
            return listModel;
        }
    }
}
