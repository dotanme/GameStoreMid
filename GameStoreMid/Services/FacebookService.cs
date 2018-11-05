using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStoreMid.Services
{
    public class FacebookService
    {

            private readonly FacebookClient _facebookClient;

            public FacebookService(FacebookClient facebookClient)
            {
                _facebookClient = facebookClient;
            }

            public async Task<Account> GetAccountAsync(string accessToken)
            {
                var result = await _facebookClient.GetAsync<dynamic>(
                    accessToken, "me", "fields=id,name,email,first_name,last_name");

                if (result == null)
                {
                    return new Account();
                }

            var account = new Account
            {
                Email = result.email,
                Name = result.name,
                UserName = result.username,
                FirstName = result.first_name,
                LastName = result.last_name,
                Locale = result.location.name
                };

                return account;
            }

            public async Task PostOnWallAsync(string accessToken, string message)
                => await _facebookClient.PostAsync(accessToken, "me/feed", new { message });
        }
}
