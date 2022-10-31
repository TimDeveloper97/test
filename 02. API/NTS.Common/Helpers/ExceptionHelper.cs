using NTS.Common.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Common.Helpers
{
    public static class ExceptionHelper
    {
        public static ExceptionInfoModel GetExceptionInfo(this Exception ex)
        {
            ExceptionInfoModel exceptionInfoModel = new ExceptionInfoModel();
            StackTrace st = new StackTrace(ex, true);

            bool isFile = false;
            List<string> router = new List<string>();
            StackFrame sf;
            string fileName;

            for (int i = 0; i < st.FrameCount; i++)
            {
                sf = st.GetFrame(i);
                fileName = sf.GetFileName();

                if (CheckFileName(fileName))
                {
                    if (!isFile)
                    {
                        isFile = true;

                        exceptionInfoModel.MethodName = GetMethodName(sf.GetMethod());
                        exceptionInfoModel.LineNumber = sf.GetFileLineNumber();
                        exceptionInfoModel.FileName = fileName;

                        router.Add(exceptionInfoModel.MethodName);
                    }
                    else
                    {
                        router.Insert(0, GetMethodName(sf.GetMethod()));
                    }
                }
                else if (isFile)
                {
                    break;
                }
            }

            exceptionInfoModel.Router = string.Join("/", router);

            if (ex is DbEntityValidationException)
            {
                StringBuilder errors = new StringBuilder();
                var entityError = ex as DbEntityValidationException;
                foreach (var eve in entityError.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        errors.AppendLine(string.Format("- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                            ve.PropertyName,
                            eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                            ve.ErrorMessage));
                    }
                }

                exceptionInfoModel.Message = errors.ToString();
            }

            Debug.WriteLine(string.Format("File: {0}", exceptionInfoModel.FileName));
            Debug.WriteLine(string.Format("MethodName: {0}", exceptionInfoModel.MethodName));
            Debug.WriteLine(string.Format("Line: {0}", exceptionInfoModel.LineNumber));
            Debug.WriteLine(string.Format("Router: {0}", exceptionInfoModel.Router));

            return exceptionInfoModel;
        }

        /// <summary>
        /// Kiểm tra tên file xem có đúng trong danh sách project không
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static bool CheckFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return false;
            }

            if (fileName.Contains("QLTK.Api") || fileName.Contains("NTS.Model") || fileName.Contains("QLTK.Business") || fileName.Contains("NTS.Utils")
                || fileName.Contains("NTS.Common") || fileName.Contains("NTS.Caching"))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Lấy methodname theo thông tin method
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        private static string GetMethodName(System.Reflection.MethodBase method = null)
        {
            string methodName = string.Empty;
            if (method.Name.Contains("MoveNext"))
            {
                methodName = method.DeclaringType.Name.Substring(1, method.DeclaringType.Name.IndexOf(">") - 1);
            }
            else
            {
                methodName = method.Name;
            }

            return methodName;
        }
    }
}
