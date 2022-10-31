using NTS.Model.Guideline;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.Guidelines
{
    public class GuidelineBusiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        public GuidelineModel GetGuidelineInfo(GuidelineModel model)
        {
            GuidelineModel guideline = new GuidelineModel();
            var result = db.Guidelines.FirstOrDefault();
            if(result != null)
            {
                guideline.Id = result.Id;
                guideline.Content = result.Content;
            }
            return guideline;
        }

        public void UpdateGuidelineInfo(GuidelineModel model)
        {
            var result = db.Guidelines.FirstOrDefault(a => a.Id.Equals(model.Id));
            result.Content = model.Content;
            db.SaveChanges();
        }
    }
}
