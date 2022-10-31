using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.QLTKEP
{
    public class QLTKEPModel : BaseModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string JobPositionId { get; set; }
        public string JobPositionName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ImagePath { get; set; }
        public string Address { get; set; }
        public int Gender { get; set; }
        public string Qualification { get; set; }
        public string BankAccount { get; set; }
        public string SocialInsurrance { get; set; }
        public string HealtInsurrance { get; set; }
        public string TaxCode { get; set; }
        public string IdentifyNum { get; set; }
        public Nullable<System.DateTime> StartWorking { get; set; }
        public Nullable<System.DateTime> EndWorking { get; set; }
        public string Carrer { get; set; }
      
    }
}
