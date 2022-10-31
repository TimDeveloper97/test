using NTS.Model.Bank;
using NTS.Model.Expert;
using NTS.Model.Specialize;
using NTS.Model.WorkPlace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.PracticeExperts
{
    public class PracticeExpertsModel : BaseModel
    {
        public string Id { get; set; }
        public string PracticeId { get; set; }
        public string ExpertId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public string DegreeName { get; set; }
        public string WorkPlaceName { get; set; }
        public string SpecializeName { get; set; }
        public string PhoneNumber { get; set; }
        public string DegreeId { get; set; }
        public string SpecializeId { get; set; }
        public string WorkPlaceId { get; set; }
        public string BankAccountName { get; set; }
        public string Description { get; set; }
        public List<ExpertModel> listSelect { get; set; }
        public List<SpecializeModel> ListSpecialize { get; set; }
        public List<WorkPlaceModel> ListWorkPlace { get; set; }
        public List<BankModel> ListBank { get; set; }
    }
}
