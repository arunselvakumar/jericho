using System;
using Microsoft.AspNet.Identity;

namespace Jericho.Identity.Models
{
    public class MongoUserLogin : IEquatable<MongoUserLogin>, IEquatable<UserLoginInfo>
    {
        public MongoUserLogin(UserLoginInfo loginInfo)
        {
            if (loginInfo == null)
            {
                throw new ArgumentNullException(nameof(loginInfo));
            }

            LoginProvider = loginInfo.LoginProvider;
            ProviderKey = loginInfo.ProviderKey;
        }


        public string LoginProvider { get; private set; }

        public string ProviderKey { get; private set; }

        public bool Equals(MongoUserLogin other)
        {
            return other.LoginProvider.Equals(LoginProvider) && other.ProviderKey.Equals(ProviderKey);
        }

        public bool Equals(UserLoginInfo other)
        {
            return other.LoginProvider.Equals(LoginProvider) && other.ProviderKey.Equals(ProviderKey);
        }
    }
}