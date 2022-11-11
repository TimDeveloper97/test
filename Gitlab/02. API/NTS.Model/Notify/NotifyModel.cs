using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Notify
{
    public class NotifyModel
    {
        public long Id { get; set; }
        public string Image { get; set; }//anh neu co
        public string Title { get; set; }//tieu de thong baos
        public string Content { get; set; }//noi dung thong bao
        public string Status { get; set; }//0 chuwa xem, 1 da xem
        public string Link { get; set; }//link chuyen toi man hinh click
        public DateTime CreateDate { get; set; }//thoi gian
        public int? ObjectType { get; set; }
        public string ObjectId { get; set; }
    }
}
