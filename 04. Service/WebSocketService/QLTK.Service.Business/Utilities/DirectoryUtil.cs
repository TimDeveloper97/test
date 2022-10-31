using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Utilities
{
    public class DirectoryUtil
    {
        /// <summary>
        /// Tạo thư mục
        /// </summary>
        /// <param name="path">
        /// Link thư mục
        /// Phân cách thư mục bằng ký tự '/'
        /// </param>
        public static void CreateFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                List<string> pathChild = path.Split('/').ToList();

                string root = pathChild[0];

                for (int i = 1; i < pathChild.Count; i++)
                {
                    root += "/" + pathChild[i];
                    if (!Directory.Exists(root))
                    {
                        Directory.CreateDirectory(root);
                    }
                }

            }
        }
    }
}
