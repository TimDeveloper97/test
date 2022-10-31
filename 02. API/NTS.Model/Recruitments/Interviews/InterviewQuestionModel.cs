using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Recruitments.Interviews
{
    public class InterviewQuestionModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string QuestionCode { get; set; }
        public int Score { get; set; }
        public string Answer { get; set; }
        public string QuestionContent { get; set; }
        public string QuestionAnswer { get; set; }
        public int QuestionScore { get; set; }
        public int QuestionType { get; set; }
        public int Status { get; set; }
        public string InterviewId { get; set; }
        public int Type { get; set; }
        public string Content { get; set; }
        public string QuestionGroupName { get; set; }
        public string QuestionTypeName { get; set; }
    }
}
