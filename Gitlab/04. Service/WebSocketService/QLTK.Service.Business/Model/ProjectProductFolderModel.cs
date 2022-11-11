using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Model
{
    public class ProjectProductFolderModel
    {
        public string Name { get; set; }
        public List<ProjectProductFolderDocumentModel> ListProjectProductFolder { get; set; }

        public ProjectProductFolderModel()
        {
            ListProjectProductFolder = new List<ProjectProductFolderDocumentModel>();
        }
    }

    public class ProjectProductFolderDocumentModel
    {
        public string Name { get; set; }
        public List<ProjectProductFileDocumentModel> ListHDSD { get; set; }
        public List<ProjectProductFileDocumentModel> ListHDTH { get; set; }
        public List<ProjectProductFileDocumentModel> ListDataSheet { get; set; }
        public ProjectProductFileDocumentModel FileExcel { get; set; }
        public ProjectProductFolderDocumentModel()
        {
            ListHDSD = new List<ProjectProductFileDocumentModel>();
            ListHDTH = new List<ProjectProductFileDocumentModel>();
            ListDataSheet = new List<ProjectProductFileDocumentModel>();
            FileExcel = new ProjectProductFileDocumentModel();
        }
    }

    public class ProjectProductFileDocumentModel
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
