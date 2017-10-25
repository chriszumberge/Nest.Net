using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Nest.Net
{
    public class NestClient : INestClient
    {
        const string BASE_API_URL = "https://developer-api.nest.com/";
        const string AUTHORIZATION_URL = "https://home.nest.com/login/oauth2?client_id={0}&state={1}";
        const string ACCESS_TOKEN_URL = "https://api.home.nest.com/oauth2/access_token";

        public NestClient(string productId, string productSecret)
        {
            _productId = productId;
            _productSecret = productSecret;
        }

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
        public async Task<string> GetAccessToken(string authorizationToken)
        {
            HttpClient _httpClient = new HttpClient();
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

        public void VerifyAuthValid()
        {
            if (String.IsNullOrEmpty(_accessToken) || _tokenExpiration < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Invalid or Expired Access Token");
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

        public async Task<NestDataModel> GetNestDataAsync()
        {
            try
            {
                VerifyAuthValid();

                HttpClient httpClient = new HttpClient();

                var url = BASE_API_URL + "?auth={0}";

                var data = await httpClient.GetStringAsync(String.Format(url, AccessToken));

                NestDataModel dataModel = JsonConvert.DeserializeObject<NestDataModel>(data);

                return dataModel;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw ex;
            }
        }

        public async Task<Devices> GetDevicesAsync()
        {
            try
            {
                VerifyAuthValid();

                HttpClient httpClient = new HttpClient();

                var url = BASE_API_URL + "devices.json?auth={0}";

                var data = await httpClient.GetStringAsync(String.Format(url, AccessToken));

                Devices devices = JsonConvert.DeserializeObject<Devices>(data);

                return devices;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw ex;
            }
        }

        public async Task<Dictionary<string, Camera>> GetCamerasAsync()
        {
            try
            {
                VerifyAuthValid();

                HttpClient httpClient = new HttpClient();

                var url = BASE_API_URL + "devices/cameras/.json?auth={0}";

                var data = await httpClient.GetStringAsync(String.Format(url, AccessToken));

                Dictionary<string, Camera> cameras = JsonConvert.DeserializeObject<Dictionary<string, Camera>>(data);

                return cameras;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw ex;
            }
        }

        public async Task<Dictionary<string, Thermostat>> GetThermostatsAsync()
        {
            try
            {
                VerifyAuthValid();

                HttpClient httpClient = new HttpClient();

                var url = BASE_API_URL + "devices/thermostats/.json?auth={0}";

                var data = await httpClient.GetStringAsync(String.Format(url, AccessToken));

                Dictionary<string, Thermostat> thermostats = JsonConvert.DeserializeObject<Dictionary<String, Thermostat>>(data);

                return thermostats;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw ex;
            }
        }

        public async Task<Dictionary<string, SmokeCoAlarm>> GetSmokeCoAlarms()
        {
            try
            {
                VerifyAuthValid();

                HttpClient httpClient = new HttpClient();

                var url = BASE_API_URL + "devices/smoke_co_alarms/.json?auth={0}";

                var data = await httpClient.GetStringAsync(String.Format(url, AccessToken));

                Dictionary<string, SmokeCoAlarm> smokeCoAlarms = JsonConvert.DeserializeObject<Dictionary<string, SmokeCoAlarm>>(data);

                return smokeCoAlarms;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw ex;
            }
        }


        /// <summary>
        /// Occurs when [stream event occurs]. 
        /// This is every single stream event that the client receives.
        /// More often than not, this is mostly just connection keep-alive events so it's extremely noisy.
        /// </summary>
        public event EventHandler<string> OnNestEventStream_StreamEventOccured;
        /// <summary>
        /// Occurs when [device event occurs].
        /// This is only events that are triggered from devices.
        /// </summary>
        public event EventHandler<NestDataModel> OnNestEventStream_DeviceEventOccured;

        public bool IsStreamingEvents => (_eventStream == null) && (_streamClient == null);

        HttpClient _streamClient;
        Stream _eventStream;
        /// <summary>
        /// Begins the nest event stream.
        /// Subscribe to the OnNestEventStream_* EventHandlers to do work on events.
        /// </summary>
        /// <returns></returns>
        public async Task BeginNestEventStream()
        {
            using (var stream = await this.GetNestEventStreamAsync())
            using (var reader = new StreamReader(stream))
            {
                while (reader != null && !reader.EndOfStream)
                {
                    var currentLine = await reader.ReadLineAsync();
                    if (!String.IsNullOrEmpty(currentLine) && currentLine.StartsWith("data"))
                    {
                        try
                        {
                            // Object needs to be wrapped in curly braces to it's deserialized correctly
                            var model = $"{{{currentLine}}}";
                            var dataModelDict = JsonConvert.DeserializeObject<Dictionary<string, NestStreamEventDataModel>>(model);
                            NestDataModel dataModel = dataModelDict["data"].Data;
                            this.OnNestEventStream_DeviceEventOccured?.Invoke(this, dataModel);
                            }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex);
                        }
                    }
                    else
                    {
                        this.OnNestEventStream_StreamEventOccured?.Invoke(this, currentLine);
                    }
                }
            }
        }

        /// <summary>
        /// Ends the active nest event stream.
        /// </summary>
        public void EndNestEventStream()
        {
            if (_eventStream != null)
            {
                _eventStream.Dispose();
                _eventStream = null;
            }
            if (_streamClient != null)
            {
                _streamClient.CancelPendingRequests();
                _streamClient.Dispose();
                _streamClient = null;
            }
        }

        /// <summary>
        /// Gets and returns the nest event stream, instead of handling it internally.
        /// </summary>
        /// <returns></returns>
        public async Task<Stream> GetNestEventStreamAsync()
        {
            try
            {
                VerifyAuthValid();

                _streamClient = new HttpClient();

                var url = BASE_API_URL + "?auth={0}";

                _streamClient.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);

                _streamClient.DefaultRequestHeaders.Accept.Clear();
                _streamClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/event-stream"));

                _eventStream = await _streamClient.GetStreamAsync(String.Format(url, AccessToken));

                return _eventStream;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Closes the nest event stream.
        /// </summary>
        public void CloseNestEventStream()
        {
            this.EndNestEventStream();
        }
    }
}
