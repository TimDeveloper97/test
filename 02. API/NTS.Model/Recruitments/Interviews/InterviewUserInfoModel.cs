using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Recruitments.Interviews
{
    public class InterviewUserInfoModel
    {
        public string Id { get; set; }
        public string InterviewUserId { get; set; }
        public string UserId { get; set; }
        public string InterviewId { get; set; }
        public int Status { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string DepartmentName { get; set; }
        public bool IsNew { get; set; }
        public string ImagePath { get; set; }
    }
}
