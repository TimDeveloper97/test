using NTS.Model.Combobox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Subjects
{
    public class SubjectsSearchModel : SearchCommonModel
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public string ClassRoomId { get; set; }
        public string ClassRoomName { get; set; }
        public string DegreeId { get; set; }
        public string DegreeName { get; set; }
        public string Description { get; set; }
        public bool IsExport { get; set; }


        public Nullable<int> TotalLearningTime { get; set; } // Tổng time học
        public Nullable<int> LearningTheoryTime { get; set; } // time lý thuyết
        public Nullable<int> LearningPracticeTime { get; set; } // time thực hành
    }
}
