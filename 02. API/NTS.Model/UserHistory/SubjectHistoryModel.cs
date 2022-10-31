using NTS.Model.ClassRoom;
using NTS.Model.Subjects;
using NTS.Model.SubjectsAttach;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.HistoryVersion
{
    public class SubjectHistoryModel
    {
        public string Id { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public string Documents { get; set; }
        public string Description { get; set; }

        public string ClassRoomId { get; set; }
        public string DegreeId { get; set; }

        public Nullable<int> TotalLearningTime { get; set; } // Tổng time học
        public Nullable<int> LearningTheoryTime { get; set; } // time lý thuyết
        public Nullable<int> LearningPracticeTime { get; set; } // time thực hành

        public List<ClassRoomModel> ListClassRoom { get; set; }

        public List<SubjectsAttachModel> ListFile { get; set; }

        public List<SubjectSkillModel> ListSkill { get; set; }
        public SubjectHistoryModel()
        {
            ListSkill = new List<SubjectSkillModel>();
            ListFile = new List<SubjectsAttachModel>();
        }
    }
}
