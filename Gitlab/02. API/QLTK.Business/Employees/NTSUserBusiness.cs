using Newtonsoft.Json;
using NTS.Business;
using NTS.Caching;
using NTS.Common;
using NTS.Model.Combobox;
using NTS.Model.Common;
using NTS.Model.Repositories;
using NTS.Utils;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web;

namespace QLTK.Business.NTSUser
{
    public class NTSUserBusiness
    {
        QLTKEntities db = new QLTKEntities();
        string _RedisConnection = ConfigurationManager.AppSettings["RedisConnection"];
        string SecretKey = System.Configuration.ConfigurationManager.AppSettings["SecretKey"];
        string ApiFile = System.Configuration.ConfigurationManager.AppSettings["ApiFile"];
        public void ChangePass(UserModel model)
        {
            var modelChange = db.Users.Where(r => r.UserName.Equals(model.UserName)).FirstOrDefault();
            //Kiểm tra tồn tại
            if (modelChange == null)
            {
                throw new BusinessException(ErrorMessage.ERR003);
            }
            //Check mật khẩu cũ nhập
            var securityStamp = PasswordUtils.ComputeHash(model.OldPassword + modelChange.SecurityStamp);
            if (!modelChange.PasswordHash.Equals(securityStamp))
            {
                throw new BusinessException("Mật khẩu cũ không đúng");
            }
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    modelChange.PasswordHash = PasswordUtils.ComputeHash(model.NewPassword + modelChange.SecurityStamp);
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new ErrorException(ErrorMessage.ERR001, ex.InnerException);
                }
            }
        }

        public void ChangeUserInfo(UserInfoModel model, HttpFileCollection hfc)
        {
           var checkEmployees = db.Employees.AsNoTracking().FirstOrDefault(u => u.Email.ToLower().Equals(model.Email.ToLower()) && !u.Id.Equals(model.Id));
            if (checkEmployees != null)
            {
                throw new BusinessException("Email này đã tồn tại");
            }
            #region[xử lý ảnh]
            HttpContent imageStreamContent;
            if (hfc.Count > 0)
            {
                UploadModel upload = new UploadModel();
                upload.KeyAuthorize = SecretKey;
                upload.isCreateThumb = true;
                upload.FolderName = "ImgUser/";
                upload.FileName = "file.jpg";
                string json = JsonConvert.SerializeObject(upload);
                HttpContent streamContent = new StringContent(json);
                using (var client = new HttpClient())
                {
                    using (var formData = new MultipartFormDataContent())
                    {
                        imageStreamContent = new StreamContent(hfc[0].InputStream);
                        formData.Add(imageStreamContent, hfc[0].FileName, hfc[0].FileName);
                        formData.Add(streamContent, "Model");
                        var responseImage = client.PostAsync(string.Format("{0}api/HandlingImage/UploadFile", ApiFile), formData).Result;
                        if (responseImage.IsSuccessStatusCode)
                        {
                            var uploadResult = JsonConvert.DeserializeObject<UploadResultModel>(responseImage.Content.ReadAsStringAsync().Result);
                            model.ImagePath = uploadResult.LinkThumb;
                        }
                    }
                }
            }
            #endregion
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var data = db.Employees.FirstOrDefault(u => u.Id.Equals(model.Id));
                    data.DateOfBirth = model.Birthday;
                    data.Email = model.Email;
                    data.PhoneNumber = model.Phone;
                    data.Name = model.Name;
                    data.ImagePath = model.ImagePath;
                    data.Address = model.Address;
                    data.Gender = int.Parse(model.Gender);
                    data.BankAccount = model.BankAccount;
                    data.SocialInsurrance = model.SocialInsurrance;
                    data.HealtInsurrance = model.HealtInsurrance;
                    data.TaxCode = model.TaxCode;
                    data.IdentifyNum = model.IdentifyNum;
                    data.Carrer = model.Carrer;
                    db.SaveChanges();
                    trans.Commit();
                    //xóa cache
                    new AuthenBussiness().DeleteLoginModelCache(model.UserId);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new ErrorException(ErrorMessage.ERR001, ex.InnerException);
                }
            }
        }

        public UserInfoModel GetUserInfo(string UserId)
        {
            UserInfoModel model = new UserInfoModel();
            try
            {
                model = (from a in db.Users.AsNoTracking()
                         where a.Id.Equals(UserId)
                         join b in db.Employees.AsNoTracking() on a.EmployeeId equals b.Id
                         select new UserInfoModel
                         {
                             UserName = a.UserName,
                             Birthday = b.DateOfBirth,
                             Email = b.Email,
                             Phone = b.PhoneNumber,
                             Name = b.Name,
                             Id = b.Id,
                             UserId=a.Id,
                             ImagePath = b.ImagePath,
                             Address = b.Address,
                             Gender = b.Gender.ToString(),
                             BankAccount = b.BankAccount,
                             SocialInsurrance = b.SocialInsurrance,
                             HealtInsurrance = b.HealtInsurrance,
                             TaxCode = b.TaxCode,
                             IdentifyNum = b.IdentifyNum,
                             Carrer = b.Carrer,
                         }
                            ).FirstOrDefault();
            }
            catch (Exception)
            { }
            return model;
        }

        public void Forgotpassword(string email)
        {
            var forgotpassModel = new ForgotpassModel();
            var userModel = (from a in db.Employees.AsNoTracking()
                             where a.Email.ToLower().Equals(email.ToLower())
                             join b in db.Users.AsNoTracking() on a.Id equals b.EmployeeId
                             select b).FirstOrDefault();
            if (userModel != null)
            {
                try
                {

                    forgotpassModel.UserId = userModel.Id;
                    forgotpassModel.SecurityStamp = Guid.NewGuid().ToString();
                    var keyCache = Constants.Forgotpassword + userModel.Id;
                    var cache = RedisService<ForgotpassModel>.GetInstance(_RedisConnection);
                    cache.Add(keyCache, forgotpassModel, new TimeSpan(1, 0, 0));
                }
                catch (Exception)
                {
                    throw new Exception("QLTK.ErrosProcess");
                }
                string mailSend = ConfigurationManager.AppSettings["MailSend"];
                string mailPass = ConfigurationManager.AppSettings["MailPass"];
                string clientUrl = ConfigurationManager.AppSettings["ClientUrl"];
                string content = "";
                string title = "Cập nhật lại mật khẩu cho tài khoản email: " + email;
                content += "<p>Bạn vui lòng nhấn vào <a href='" + clientUrl + "authen/reset-password?UserId=" + forgotpassModel.UserId + "&SecurityStamp=" + forgotpassModel.SecurityStamp + "&Email=" + email + "'> đây </a> để cập nhật lại mật khẩu</p>";
                var resultSendMail = this.SendMail(mailSend, mailPass, email, title, content);
                if (!resultSendMail)
                {
                    throw new Exception("QLTK.ErrosSendMail");
                }
            }
            else
            {
                throw new Exception("QLTK.EmailInvalid");
            }
        }
        public void Resetpassword(ForgotpassModel model)
        {
            var keyCache = Constants.Forgotpassword + model.UserId;
            var cache = RedisService<ForgotpassModel>.GetInstance(_RedisConnection);
            var modelCache = cache.Get<ForgotpassModel>(keyCache);
            if (modelCache != null)
            {
                if (modelCache.UserId.Equals(model.UserId) && modelCache.SecurityStamp.Equals(model.SecurityStamp))
                {
                    var userModel = db.Users.FirstOrDefault(u => u.Id.Equals(model.UserId));
                    userModel.PasswordHash = PasswordUtils.ComputeHash(model.PassNew + userModel.SecurityStamp);
                    db.SaveChanges();
                    //viết code update pass mơi
                }
                else
                {
                    throw new Exception("QLTK.ErrosUser");
                }
            }
            else
            {
                throw new Exception("QLTK.ErrosResetUser");
            }
        }
        public bool SendMail(string emailSend, string passSend, string emailInbox, string title, string content)
        {
            try
            {
                MailMessage mailsend = new MailMessage();
                mailsend.To.Add(emailInbox);
                mailsend.From = new MailAddress(emailSend);
                mailsend.Subject = title;
                mailsend.Body = content;
                mailsend.IsBodyHtml = true;
                int cong = 587;
                SmtpClient client = new SmtpClient("smtp.gmail.com", cong);
                client.EnableSsl = true;
                NetworkCredential credentials = new NetworkCredential(emailSend, passSend);
                client.Credentials = credentials;
                client.Send(mailsend);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public void ValidCache(ForgotpassModel model)
        {
            var keyCache = Constants.Forgotpassword + model.UserId;
            var cache = RedisService<ForgotpassModel>.GetInstance(_RedisConnection);
            var modelCache = cache.Get<ForgotpassModel>(keyCache);
            if (modelCache == null)
            {
                throw new Exception("QLTK.ErrosResetUser");
            }
        }
    }
}
