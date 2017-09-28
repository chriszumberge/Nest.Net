using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Nest.Net
{
    public class NestClient
    {
        const string BASE_API_URL = "https://developer-api.nest.com/";
        const string AUTHORIZATION_URL = "https://home.nest.com/login/oauth2?client_id={0}&state={1}";
        const string ACCESS_TOKEN_URL = "https://api.home.nest.com/oauth2/access_token";

        public NestClient(string productId, string productSecret)
        {
            _productId = productId;
            _productSecret = productSecret;

            _httpClient = new HttpClient();
        }

        HttpClient _httpClient;

        readonly string _productId;
        public string ProductId => _productId;

        readonly string _productSecret;
        public string ProductSecret => _productSecret;

        string _accessToken;
        public string AccessToken => _accessToken;

        DateTime _tokenExpiration;

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <param name="authorizationToken">The authorization token.</param>
        /// <returns></returns>
        public async Task<string> GetAccessToken (string authorizationToken)
        {
            var url = ACCESS_TOKEN_URL;

            var requestContent = new Dictionary<string, string>();
            requestContent.Add("client_id", ProductId);
            requestContent.Add("client_secret", ProductSecret);
            requestContent.Add("grant_type", "authorization_code");
            requestContent.Add("code", authorizationToken);

            var response = await _httpClient.PostAsync(url, new FormUrlEncodedContent(requestContent));
            var data = await response.Content.ReadAsStringAsync();

            var json = JObject.Parse(data);

            if (json != null)
            {
                if (json["access_token"] != null)
                {
                    _accessToken = json["access_token"].ToString();
                }

                if (json["expires_in"] != null)
                {
                    var expiresIn = json.Value<long>("expires_in");
                    _tokenExpiration = DateTime.UtcNow.AddSeconds(expiresIn);
                }
            }

            return _accessToken;
        }

        public void SetAccessToken(string accessToken, DateTime expiration)
        {
            _accessToken = accessToken;
            _tokenExpiration = expiration;
        }

        public bool IsAuthValid()
        {
            bool valid;
            try
            {
                VerifyAuthValid();
                valid = true;
            }
            catch
            {
                valid = false;
            }
            return valid;
        }

        /// <summary>
        /// Gets the authorization URL.
        /// </summary>
        /// <returns></returns>
        public string GetAuthorizationUrl()
        {
            var state = Guid.NewGuid().ToString();

            return String.Format(AUTHORIZATION_URL, ProductId, state);
        }

        public async Task<NestDataModel> GetNestData()
        {
            try
            {
                VerifyAuthValid();


                var url = BASE_API_URL + "?auth={0}";

                var data = await _httpClient.GetStringAsync(String.Format(url, AccessToken));

                NestDataModel dataModel = JsonConvert.DeserializeObject<NestDataModel>(data);

                return dataModel;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw ex;
            }
        }

        void VerifyAuthValid()
        {
            if (String.IsNullOrEmpty(_accessToken) || _tokenExpiration < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Invalid or Expired Access Token");
        }
    }
}
