using NTS.Common;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.UserHistory
{
    public class UserHistoryBusiness
    {
        private QLTKEntities db = new QLTKEntities();
        public void Add(string UserId, int ObjectType, string ObjectId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var dateNow = DateTime.Now;
                    NTS.Model.Repositories.UserHistory adduser = new NTS.Model.Repositories.UserHistory
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = UserId,
                        ObjectType = ObjectType,
                        ObjectId = ObjectId,

                    };
                    db.UserHistories.Add(adduser);


                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(null, ex);
                }
            }
        }
    }
}
