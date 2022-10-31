using NTS.Common;
using NTS.Model.Notify;
using NTS.Model.Repositories;
using NTS.Model.Sale.SaleProduct;
using QLTK.Business.Notify;
using QLTK.Business.RabbitMQServers;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QLTK.Business.SaleProducts
{
    public class SaleProductBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        private readonly RabbitMQServer<MessageNotify> rabbitMQServer = new RabbitMQServer<MessageNotify>();

        public void ImportFile(HttpPostedFile file, string lastModified)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            DateTime lastDate = DateTime.Now.Date;
            DateTime dateNow = DateTime.Now.Date;
            if (!extension.Equals(".xlsx") && !extension.Equals(".xls"))
            {
                throw NTSException.CreateInstance("File dữ liệu phải là excel");
            }

            if (!string.IsNullOrEmpty(lastModified))
            {
                lastDate = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(Double.Parse(lastModified) / 1000d)).ToLocalTime();
                if (lastDate.Date != dateNow)
                {
                    throw NTSException.CreateInstance("Ngày cập nhật file không phải ngày hiện tại!");
                }
            }

            string model, inventory;
            #region[doc du lieu tu excel]
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(file.InputStream);
            IWorksheet sheet = workbook.Worksheets[0];
            int rowCount = sheet.Rows.Count();
            SaleProduct itemC;
            SaleProduct check;
            List<SaleProductSendEmailModel> listSaleProduct = new List<SaleProductSendEmailModel>();
            decimal saleProductExportDetail;
            MessageNotify messageNotify;
            NotifyEmailModel notifyEmailModel;
            List<int> rowModel = new List<int>();
            List<int> rowCheckModel = new List<int>();
            List<int> rowInventory = new List<int>();
            List<int> rowCheckInventory = new List<int>();
            List<string> listPersonId;

            try
            {
                var list = db.SaleProductExportDetails.AsNoTracking().ToList();
                var saleProductExports = db.SaleProductExports.AsNoTracking().ToList();
                for (int i = 3; i <= rowCount; i++)
                {
                    itemC = new SaleProduct();
                    model = sheet[i, 2].Value;
                    inventory = sheet[i, 3].Value;

                    if (!string.IsNullOrEmpty(model))
                    {
                        itemC.Model = model;
                    }
                    else
                    {
                        rowModel.Add(i);
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(inventory))
                        {
                            itemC.Inventory = Convert.ToInt32(inventory);
                        }
                        else
                        {
                            rowInventory.Add(i);
                        }
                    }
                    catch (Exception)
                    {
                        rowCheckInventory.Add(i);
                        continue;
                    }

                    check = new SaleProduct();
                    check = db.SaleProducts.FirstOrDefault(t => t.Model.ToUpper().Equals(model.ToUpper()));
                    if (check != null)
                    {
                        check.Inventory = itemC.Inventory;
                        check.InventoryDate = DateTime.Now;

                        saleProductExportDetail = list.Where(a => a.SaleProductId.Equals(check.Id)).Sum(a => a.Quantity);
                        if (check.Inventory < saleProductExportDetail)
                        {
                            check.AvailableQuantity = 0;
                            listPersonId = new List<string>();
                            listPersonId = (from a in db.SaleProductExportDetails.AsNoTracking()
                                            join b in db.SaleProductExports.AsNoTracking() on a.SaleProductExportId equals b.Id
                                            where a.SaleProductId.Equals(check.Id)
                                            select new { b.CreateBy }).GroupBy(a => a.CreateBy).Select(a => a.Key).ToList();

                            foreach (var item in listPersonId)
                            {
                                listSaleProduct.Add(new SaleProductSendEmailModel
                                {
                                    Model = check.Model,
                                    EName = check.EName,
                                    Inventory = check.Inventory,
                                    ExportQuantity = saleProductExportDetail,
                                    ExportPerson = item
                                });
                            }
                        }
                        else
                        {
                            check.AvailableQuantity = check.Inventory - saleProductExportDetail;
                        }
                    }
                    else
                    {
                        rowCheckModel.Add(i);
                    }
                }

                #endregion

                if (rowModel.Count > 0)
                {
                    throw NTSException.CreateInstance("Model dòng <" + string.Join(", ", rowModel) + "> không được để trống!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (rowCheckModel.Count > 0)
                {
                    throw NTSException.CreateInstance("Model dòng <" + string.Join(", ", rowCheckModel) + "> không tồn tại!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (rowInventory.Count > 0)
                {
                    throw NTSException.CreateInstance("Số lượng tồn kho dòng <" + string.Join(", ", rowInventory) + "> không được để trống!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (rowCheckInventory.Count > 0)
                {
                    throw NTSException.CreateInstance("Số lượng tồn kho dòng <" + string.Join(", ", rowCheckInventory) + "> không đúng định dạng!");
                    //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
                }

                if (rowCount > 0)
                {
                    var importInventory = db.ImportInventories.FirstOrDefault(t => t.Type.Equals(Constants.ImportInventory_Type_SaleProduct));
                    if (importInventory != null)
                    {
                        importInventory.Date = DateTime.Now;
                    }
                    else
                    {
                        importInventory = new ImportInventory()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Type = Constants.ImportInventory_Type_SaleProduct,
                            Date = DateTime.Now
                        };
                        db.ImportInventories.Add(importInventory);
                    }

                    var importLastInventory = db.ImportInventories.FirstOrDefault(t => t.Type.Equals(Constants.ImportInventory_Type_LastModified));
                    if (importLastInventory != null)
                    {
                        importLastInventory.Date = lastDate;
                    }
                    else
                    {
                        importLastInventory = new ImportInventory()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Type = Constants.ImportInventory_Type_LastModified,
                            Date = lastDate
                        };
                        db.ImportInventories.Add(importLastInventory);
                    }
                }
                db.SaveChanges();

                messageNotify = new MessageNotify();
                notifyEmailModel = new NotifyEmailModel();
                NotifyEmailModel notifyEmail;
                Employee employee;
                List<SaleProductSendEmailModel> listExport;
                var listPerson = listSaleProduct.Where(i => !string.IsNullOrEmpty(i.ExportPerson)).GroupBy(i => i.ExportPerson).Select(i => i.Key).ToList();
                var template = db.EmailTemplates.FirstOrDefault(i => i.Type == 1);
                if (template != null)
                {
                    foreach (var item in listPerson)
                    {
                        notifyEmail = new NotifyEmailModel();
                        employee = new Employee();
                        listExport = new List<SaleProductSendEmailModel>();
                        employee = db.Employees.FirstOrDefault(a => a.Id.Equals(item));
                        if (employee != null && !string.IsNullOrEmpty(employee.Email))
                        {
                            listExport = listSaleProduct.Where(i => i.ExportPerson.Equals(item)).ToList();
                            notifyEmail = GetContentEmail(employee.Name, template.Content, listExport);
                            notifyEmailModel.SubjectTitle = notifyEmail.SubjectTitle;
                            notifyEmailModel.EmailTo = employee.Email;
                            notifyEmailModel.BodyContent = notifyEmail.BodyContent;

                            messageNotify.SentDate = DateTime.Now;
                            messageNotify.Type = NotifyType.Email;
                            messageNotify.MessageContent = notifyEmailModel;

                            rabbitMQServer.PushToQueue(messageNotify);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                workbook.Close();
                excelEngine.Dispose();
                throw new NTSLogException(null, ex);
            }
            workbook.Close();
            excelEngine.Dispose();
        }

        /// <summary>
        /// Replate content
        /// </summary>
        /// <param name="type"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public NotifyEmailModel GetContentEmail(string name, string template, List<SaleProductSendEmailModel> listData)
        {
            var content = template;
            StringBuilder result = new StringBuilder();
            content = content.Replace("<tennguoinhan>", name);

            foreach (var item in listData)
            {
                result = result.Append(
                @"<tr><td style='vertical-align: bottom;border-bottom: 2px solid #dee2e6;padding: 0.75rem;border: 1px solid #dee2e6'>" + item.Model +
                "</td><td style='vertical-align: bottom;border-bottom: 2px solid #dee2e6;padding: 0.75rem;border: 1px solid #dee2e6'>" + item.EName +
                "</td><td style='vertical-align: bottom;border-bottom: 2px solid #dee2e6;padding: 0.75rem;border: 1px solid #dee2e6;text-align: right;'>" + CurrencyNumber(item.Inventory) +
                "</td><td style='vertical-align: bottom;border-bottom: 2px solid #dee2e6;padding: 0.75rem;border: 1px solid #dee2e6;text-align: right;'>" + CurrencyNumber(item.ExportQuantity) +
                "</td></tr> ");
            }

            content = content.Replace("<sanpham>", result.ToString());
            string subject = "Cảnh báo số lượng tồn kho";
            NotifyEmailModel contentEmail = new NotifyEmailModel()
            {
                BodyContent = content,
                SubjectTitle = subject,
            };
            return contentEmail;
        }

        public string CurrencyNumber(decimal number)
        {
            return string.Format("{0:#,##0}", number);
        }
    }
}
