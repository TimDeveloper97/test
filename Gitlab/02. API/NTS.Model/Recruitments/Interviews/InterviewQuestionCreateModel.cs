using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Recruitments.Interviews
{
    public class InterviewQuestionCreateModel
    {
        public string Id { get; set; }
        public string InterviewId { get; set; }
        public int QuestionType { get; set; }
        public int QuestionScore { get; set; }
        public string QuestionContent { get; set; }
        public string QuestionAnswer { get; set; }
    }
}
