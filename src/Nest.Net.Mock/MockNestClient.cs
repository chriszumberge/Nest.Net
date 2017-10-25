using Nest.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Nest.Net.Mock
{
    public class MockNestClient : INestClient
    {
        public MockNestClient(string productId, string productSecret, string validAuthToken, DateTime? accessTokenExpiration =  null)
        {
            _productId = productId;
            _productSecret = productSecret;
            _expectedAuthToken = validAuthToken;

            if (accessTokenExpiration.HasValue) _expectedTokenExpiration = accessTokenExpiration.Value;
            else _expectedTokenExpiration = DateTime.UtcNow.AddSeconds(36000);
        }

        readonly string _expectedAuthToken;
        readonly DateTime _expectedTokenExpiration;

        readonly string _productId;
        public string ProductId => _productId;

        readonly string _productSecret;
        public string ProductSecret => _productSecret;

        string _accessToken;
        public string AccessToken => _accessToken;

        DateTime _tokenExpiration;

        public async Task<string> GetAccessToken(string authorizationToken)
        {
            if (authorizationToken.Equals(_expectedAuthToken))
            {
                _accessToken = Guid.NewGuid().ToString();
                _tokenExpiration = _expectedTokenExpiration;
            }
            else
            {
                // TODO what does happen if the auth token is wrong?
                throw new UnauthorizedAccessException();
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

        const string AUTHORIZATION_URL = "https://home.nest.com/login/oauth2?client_id={0}&state={1}";

        public event EventHandler<NestDataModel> OnNestEventStream_DeviceEventOccured;
        public event EventHandler<string> OnNestEventStream_StreamEventOccured;

        public string GetAuthorizationUrl()
        {
            var state = Guid.NewGuid().ToString();

            return String.Format(AUTHORIZATION_URL, ProductId, state);
        }

        public bool IsStreamingEvents => _isStreamingEvents;
        bool _isStreamingEvents = false;

        public async Task BeginNestEventStream()
        {
            await Task.Run(() => _isStreamingEvents = true);
        }

        public void CloseNestEventStream()
        {
            this.EndNestEventStream();
        }

        public void EndNestEventStream()
        {
            _isStreamingEvents = false;
        }

        public void TriggerDeviceEvent(NestDataModel data)
        {
            if (_isStreamingEvents)
            {
                this.OnNestEventStream_DeviceEventOccured?.Invoke(this, data);
            }
        }

        public void TriggerStreamEvent(string data)
        {
            if (_isStreamingEvents)
            {
                this.OnNestEventStream_StreamEventOccured?.Invoke(this, data);
            }
        }

        public Task<Dictionary<string, Camera>> GetCamerasAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Devices> GetDevicesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<NestDataModel> GetNestDataAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Stream> GetNestEventStreamAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, SmokeCoAlarm>> GetSmokeCoAlarms()
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, Thermostat>> GetThermostatsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
