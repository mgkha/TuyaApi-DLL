using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace TuyaApi.Types
{
    class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string CountryCode { get; set; }
        public string BizType { get; set; }
        public string From { get; set; }

        public Dictionary<string, string> GetFormData()
        {
            return new Dictionary<string, string>
            {
                { "userName", this.UserName },
                { "password", this.Password  },
                { "countryCode", this.CountryCode  },
                { "bizType", this.BizType  },
                { "from", this.From  }
            };
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Formatting = Formatting.Indented
            });
        }
    }

    class LoginResponse
    {
        // Success
        public string Access_token { get; set; }
        public string Refresh_token { get; set; }
        public string Token_type { get; set; }
        public int Expires_in { get; set; }

        // Error
        public string ResponseStatus { get; set; }
        public string ErrorMsg { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Formatting = Formatting.Indented
            });
        }
    }

    class RefreshTokenRequest
    {
        public string Grant_type { get; set; }
        public string Refresh_token { get; set; }

        public Dictionary<string, string> GetFormData()
        {
            return new Dictionary<string, string>
            {
                { "grant_type", this.Grant_type },
                { "refresh_token", this.Refresh_token }
            };
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Formatting = Formatting.Indented
            });
        }
    }

    class RefreshTokenResponse
    {
        // Success
        public string Access_token { get; set; }
        public string Refresh_token { get; set; }
        public string Token_type { get; set; }
        public int Expires_in { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Formatting = Formatting.Indented
            });
        }
    }
}
