using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Common.Resource
{
    public static class ResourceUtil
    {

        /// <summary>
        /// Lấy thông tin Resource
        /// </summary>
        /// <param name="msgKey">Mã resource</param>
        /// <returns></returns>
        public static string GetResourcesNoLag(string key)
        {
            return GetResource(null, key, null);
        }

        /// <summary>
        /// Lấy thông tin Resource
        /// </summary>
        /// <param name="lagKey">mã nước</param>
        /// <param name="msgKey">Mã resource</param>
        /// <returns></returns>
        public static string GetResource(string lagKey, string key)
        {
            return GetResource(lagKey, key, null);
        }

        /// <summary>
        /// Lấy thông tin Resource
        /// </summary>
        /// <param name="key">Mã resource</param>
        /// <param name="parameter">Thông số để fomart resource</param>
        /// <returns></returns>
        public static string GetResourcesNoLag(string key, params object[] parameter)
        {
            return GetResource(null, key, parameter);
        }

        public static string GetResourcesNoLag(object eRR0001)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Lấy thông tin Resource
        /// </summary>
        /// <param name="lagKey">Mã nước</param>
        /// <param name="key">Mã resource</param>
        /// <param name="parameter">Thông số để fomart resource</param>
        /// <returns></returns>
        public static string GetResource(string lagKey, string key, params object[] parameter)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("The resource key input must be not null.");
            }

            if (string.IsNullOrEmpty(lagKey))
            {
                lagKey = "vi";
            }

            // Make a resx reader.
            ResourceManager rm = MessageResource.ResourceManager;
            ResourceSet rs = rm.GetResourceSet(new CultureInfo(lagKey), true, true);

            try
            {
                if (parameter != null)
                {
                    return string.Format(rs.GetString(key, true), GetResourceParameter(lagKey, parameter));
                }
                else
                {
                    return rs.GetString(key, true);
                }
            }
            catch
            {
                return key;
            }
        }   
        private static object[] GetResourceParameter(string lagKey, params object[] parameter)
        {
            ResourceManager rm = TextResource.ResourceManager;
            ResourceSet rs = rm.GetResourceSet(new CultureInfo(lagKey), true, true);
            object[] paramR = new object[parameter.Length];
            string resource;
            for (int i = 0; i < parameter.Length; i++)
            {
                try
                {
                    resource = rs.GetString(parameter[i].ToString(), true);

                    paramR[i] = string.IsNullOrEmpty(resource) ? parameter[i] : resource;
                }
                catch
                {
                    paramR[i] = parameter[i];
                }
            }

            return paramR;
        }

        public static string GetTextResource(string key)
        {
            return GetTextResource(null, key);
        }

        public static string GetTextResource(string lagKey, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("The resource key input must be not null.");
            }

            if (string.IsNullOrEmpty(lagKey))
            {
                lagKey = "vi";
            }

            // Make a resx reader.
            ResourceManager rm = TextResource.ResourceManager;
            ResourceSet rs = rm.GetResourceSet(new CultureInfo(lagKey), true, true);

            return rs.GetString(key, true);
        }
    }
}
