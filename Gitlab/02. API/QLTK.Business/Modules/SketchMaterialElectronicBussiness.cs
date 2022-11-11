using NTS.Model.Repositories;
using NTS.Model.SketchMaterialElectronic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Model.Materials;
using NTS.Model.QLTKMODULE;
using static NTS.Model.SketchMaterialElectronic.SketchMaterialElectronicModel;
using System.IO;
using Syncfusion.XlsIO;
using NTS.Model.Combobox;
using NTS.Model;
using NTS.Model.SketchMaterialMechanical;
using NTS.Common;
using NTS.Common.Resource;

namespace QLTK.Business.SketchMaterialElectronic
{
    public class SketchMaterialElectronicBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        public ImportSketchesMaterialElectronic ImportFile(string userId, string file, string moduleId)
        {
            ImportSketchesMaterialElectronic rs = new ImportSketchesMaterialElectronic();
            rs.ListExist = new List<SketchMaterialElectronicModel>();
            string materialName, materialCode, leadtime, quantity, note;
            var materials = (from a in db.Materials.AsNoTracking()
                             join b in db.MaterialGroups.AsNoTracking() on a.MaterialGroupId equals b.Id

                             select new MaterialElectronicMechanical
                             {
                                 Code = a.Code,
                                 MaterialId = a.Id,
                                 Leadtime = a.DeliveryDays,
                                 Type = b.Type,
                             }).AsQueryable();

            #region[doc du lieu tu excel]

            using (Stream fs = File.OpenRead(file))
            {
                ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                IWorkbook workbook = application.Workbooks.Open(fs);
                IWorksheet sheet = workbook.Worksheets[0];
                int rowCount = sheet.Rows.Count();
                List<NTS.Model.Repositories.SketchMaterialElectronic> listElectronic = new List<NTS.Model.Repositories.SketchMaterialElectronic>();
                List<NTS.Model.Repositories.SketchMaterialMechanical> listMechanical = new List<NTS.Model.Repositories.SketchMaterialMechanical>();
                NTS.Model.Repositories.SketchMaterialElectronic itemC;
                NTS.Model.Repositories.SketchMaterialMechanical itemD;
                List<int> rowModuleErrors = new List<int>();
                List<int> rowMaterialErrors = new List<int>();
                List<int> rowMaterialEmpty = new List<int>();
                List<int> rowModuleEmpty = new List<int>();
                List<int> rowNoteEmpty = new List<int>();
                List<int> rowQuantityEmpty = new List<int>();
                List<int> rowLeadtimeEmpty = new List<int>();

                try
                {
                    for (int i = 7; i <= rowCount; i++)
                    {
                        itemC = new NTS.Model.Repositories.SketchMaterialElectronic();
                        itemD = new NTS.Model.Repositories.SketchMaterialMechanical();
                        itemD.Id = Guid.NewGuid().ToString();
                        itemC.Id = Guid.NewGuid().ToString();
                        itemD.ModuleId = moduleId;
                        itemC.ModuleId = moduleId;
                        materialName = sheet[i, 2].Value;
                        materialCode = sheet[i, 4].Value;
                        leadtime = sheet[i, 7].Value;
                        quantity = sheet[i, 6].Value;
                        note = sheet[i, 12].Value;

                        if (string.IsNullOrEmpty(materialCode))
                        {
                            continue;
                        }

                        var _itemC = materials.FirstOrDefault(t => t.Code.Equals(materialCode));
                        if (_itemC != null)
                        {
                            if (_itemC.Type.Equals("1"))
                            {
                                itemC.MaterialId = _itemC.MaterialId;

                                if (!string.IsNullOrEmpty(quantity))
                                {
                                    itemC.Quantity = int.Parse(quantity);
                                }
                                else
                                {
                                    rowQuantityEmpty.Add(i);
                                }

                                if (!string.IsNullOrEmpty(leadtime))
                                {
                                    itemC.Leadtime = int.Parse(leadtime);
                                }
                                else
                                {
                                    rowLeadtimeEmpty.Add(i);
                                }

                                if (!string.IsNullOrEmpty(note))
                                {
                                    itemC.Note = note;

                                }
                                else
                                {
                                    rowNoteEmpty.Add(i);
                                }
                            }
                            else
                            {
                                itemD.MaterialId = _itemC.MaterialId;
                                if (!string.IsNullOrEmpty(quantity))
                                {
                                    itemD.Quantity = int.Parse(quantity);
                                }
                                else
                                {
                                    rowQuantityEmpty.Add(i);
                                }

                                if (!string.IsNullOrEmpty(leadtime))
                                {
                                    itemD.Leadtime = int.Parse(leadtime);
                                }
                                else
                                {
                                    rowLeadtimeEmpty.Add(i);
                                }

                                if (!string.IsNullOrEmpty(note))
                                {
                                    itemD.Note = note;
                                }
                                else
                                {
                                    rowNoteEmpty.Add(i);
                                }
                            }
                        }
                        else
                        {
                            rs.RowMaterialEmpty.Add(i);
                            continue;
                        }


                        if (!string.IsNullOrEmpty(itemC.MaterialId))
                        {
                            var checkE = db.SketchMaterialElectronics.FirstOrDefault(t => t.MaterialId.Equals(itemC.MaterialId) && (t.ModuleId.Equals(itemC.ModuleId)));
                            if (checkE != null)
                            {
                                checkE.MaterialId = itemC.MaterialId;
                                checkE.ModuleId = moduleId;
                                checkE.Leadtime = itemC.Leadtime;
                                checkE.Quantity = itemC.Quantity;
                                checkE.Note = itemC.Note;
                            }
                            else
                            {
                                listElectronic.Add(itemC);
                            }

                        }

                        if (!string.IsNullOrEmpty(itemD.MaterialId))
                        {
                            var checkM = db.SketchMaterialMechanicals.FirstOrDefault(t => t.MaterialId.Equals(itemD.MaterialId) && (t.ModuleId.Equals(itemD.ModuleId)));
                            if (checkM != null)
                            {
                                checkM.MaterialId = itemD.MaterialId;
                                checkM.ModuleId = moduleId;
                                checkM.Leadtime = itemD.Leadtime;
                                checkM.Quantity = itemD.Quantity;
                                checkM.Note = itemD.Note;
                            }
                            else
                            {
                                listMechanical.Add(itemD);
                            }
                        }

                    }
                    #endregion

                    db.SketchMaterialElectronics.AddRange(listElectronic);
                    db.SketchMaterialMechanicals.AddRange(listMechanical);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    fs.Close();
                    workbook.Close();
                    excelEngine.Dispose();
                    throw (ex);
                }

                fs.Close();
                workbook.Close();
                excelEngine.Dispose();
            }

            return rs;
        }

        public object SearchSketchMaterialElectronic(SketchMaterialElectronicModel modelSearch, string moduleId)
        {
            SearchResultModel<SketchMaterialElectronicModel> searchResult = new SearchResultModel<SketchMaterialElectronicModel>();

            var dataQuery = (from a in db.SketchMaterialElectronics.AsNoTracking()
                             join b in db.Modules.AsNoTracking() on a.ModuleId equals b.Id into ab
                             from b in ab.DefaultIfEmpty()
                             join c in db.Materials.AsNoTracking() on a.MaterialId equals c.Id into ac
                             from c in ac.DefaultIfEmpty()
                             where a.ModuleId.Equals(moduleId)
                             select new SketchMaterialElectronicModel
                             {
                                 Id = a.Id,
                                 MaterialId = c.Id,
                                 Note = a.Note,
                                 Quantity = a.Quantity,
                                 Leadtime = a.Leadtime,
                                 Code = c.Code,
                                 Name = c.Name,
                             }).AsQueryable();


            searchResult.TotalItem = dataQuery.Count();
            // listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            searchResult.ListResult = dataQuery.ToList();
            return searchResult;
        }

        public object SearchSketchMaterialMechanical(SketchMaterialMechanicalModel modelSearch, string moduleId)
        {
            SearchResultModel<SketchMaterialMechanicalModel> searchResult = new SearchResultModel<SketchMaterialMechanicalModel>();

            var dataQuery = (from a in db.SketchMaterialMechanicals.AsNoTracking()
                             join b in db.Modules.AsNoTracking() on a.ModuleId equals b.Id into ab
                             from b in ab.DefaultIfEmpty()
                             join c in db.Materials.AsNoTracking() on a.MaterialId equals c.Id into ac
                             from c in ac.DefaultIfEmpty()
                             where a.ModuleId.Equals(moduleId)
                             select new SketchMaterialMechanicalModel
                             {
                                 Id = a.Id,
                                 MaterialId = c.Id,
                                 Note = a.Note,
                                 Quantity = a.Quantity,
                                 Leadtime = a.Leadtime,
                                 Code = c.Code,
                                 Name = c.Name,
                             }).AsQueryable();


            searchResult.TotalItem = dataQuery.Count();
            // listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            searchResult.ListResult = dataQuery.ToList();
            return searchResult;
        }

        public void DeleteSketchMaterialMechanical(SketchMaterialMechanicalModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var _mechanical = db.SketchMaterialMechanicals.FirstOrDefault(a => a.Id.Equals(model.Id));

                    if (_mechanical == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.MaterialMechanical);
                    }

                    db.SketchMaterialMechanicals.Remove(_mechanical);
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

        public void DeleteSketchMaterialElectronic(SketchMaterialElectronicModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var _electronic = db.SketchMaterialElectronics.FirstOrDefault(a => a.Id.Equals(model.Id));
                    if (_electronic == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.MaterialElectronic);
                    }

                    db.SketchMaterialElectronics.Remove(_electronic);
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
