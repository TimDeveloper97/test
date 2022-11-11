using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Common
{
   public class ConfigModel
    {
        public string EmailPass { get; set; }
        public string EmailValue { get; set; }
    }
    public class UserModel
    {
        public string UserName { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
    public class UserInfoModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? Birthday { get; set; }
        public string ImagePath { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string BankAccount { get; set; }
        public string SocialInsurrance { get; set; }
        public string HealtInsurrance { get; set; }
        public string TaxCode { get; set; }
        public string IdentifyNum { get; set; }
        public string Carrer { get; set; }
        
    }
}
