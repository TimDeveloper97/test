using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DashboardEmployee
{
    public class ObjectEmployeeModel
    {
        public string Name { get; set; }
        //Công việc hoàn thành, mô hình, module
        public int FinishParadigmNewDesign { get; set; }
        public int FinishParadigmUpdateDesign { get; set; }
        public int FinishParadigmUseDesign { get; set; }
        public int FinishModuleNewDesign { get; set; }
        public int FinishModuleUpdateDesign { get; set; }
        public int FinishModuleUseDesign { get; set; }

        //Công việc chưa hoàn thành: mô hình, module
        public int MakeParadigmNewDesign { get; set; }
        public int MakeParadigmUpdateDesign { get; set; }
        public int MakeParadigmUseDesign { get; set; }
        public int MakeModuleNewDesign { get; set; }
        public int MakeModuleUpdateDesign { get; set; }
        public int MakeModuleUseDesign { get; set; }

        /*
        public decimal MediumTypeDesign { get; set; }
        public decimal MediumTypeDoc { get; set; }
        public decimal MediumTypeTransfer { get; set; }
        public decimal TimeStandardDesign { get; set; }
        public decimal TimeStandardDoc { get; set; }
        public decimal TimeStandardTransfer { get; set; }
        */
        public string TaskName { get; set; }

        public List<ListTime> ListTime { get; set; }
        public ObjectEmployeeModel()
        {
            ListTime = new List<ListTime>();
        }
    }

    public class ListTime
    {
        public string ModuleGroupCode { get; set; }
        public decimal Avg { get; set; }
        public decimal TimeStandard { get; set; }
    }
}
