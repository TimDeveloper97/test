using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using NTS.Api.Models;
using NTS.Api;
using QLTK.Api;
using NTS.Model.Entities;
using NTS.Business;
using NTS.Model;
using NTS.Model.CacheModel;

namespace NTS.Api.Repositories
{
    /// <summary>
    /// We are depending on the “UserManager” that provides the domain logic for working with user information.
    /// The “UserManager” knows when to hash a password, how and when to validate a user, and how to manage claims
    /// </summary>
    public static class AuthRepository
    {
        private static AuthenBussiness _authen;
        public static async Task<LoginEntity> Login(string userName, string password, string clientId, string notifyToken)
        {
            try
            {
                _authen = new AuthenBussiness();
                LoginEntity loginBO = _authen.Login(userName, password, clientId, notifyToken);
                return loginBO;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static bool IsTokenAlive(string userId)
        {
            var modelLogin = new AuthenBussiness().GetLoginModelCache(userId);
            if (modelLogin != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static UserEntity GetUserAlive(string userId)
        {
            _authen = new AuthenBussiness();
            return _authen.GetLoginModelCache(userId);            
        }

        public static Client FindClient(string clientId)
        {
            _authen = new AuthenBussiness();

            var client = _authen.FindClients(clientId);

            return client;
        }
    }
}