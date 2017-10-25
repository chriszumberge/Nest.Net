using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Nest.Net
{
    public interface INestClient
    {
        string AccessToken { get; }
        bool IsStreamingEvents { get; }
        string ProductId { get; }
        string ProductSecret { get; }

        event EventHandler<NestDataModel> OnNestEventStream_DeviceEventOccured;
        event EventHandler<string> OnNestEventStream_StreamEventOccured;

        Task BeginNestEventStream();
        void CloseNestEventStream();
        void EndNestEventStream();
        Task<string> GetAccessToken(string authorizationToken);
        string GetAuthorizationUrl();
        Task<Dictionary<string, Camera>> GetCamerasAsync();
        Task<Devices> GetDevicesAsync();
        Task<NestDataModel> GetNestDataAsync();
        Task<Stream> GetNestEventStreamAsync();
        Task<Dictionary<string, SmokeCoAlarm>> GetSmokeCoAlarms();
        Task<Dictionary<string, Thermostat>> GetThermostatsAsync();
        bool IsAuthValid();
        void SetAccessToken(string accessToken, DateTime expiration);
        void VerifyAuthValid();
    }
}