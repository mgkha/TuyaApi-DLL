using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TuyaApi.Types;

namespace TuyaApi
{
    public class TuyaApi
    {
        public static string tokenPath = Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile) + @"\tuya\credentials.txt";
        public static string devicesListPath = Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile) + @"\tuya\devices.txt";
        public static string loginUri = "https://px1.tuyaus.com/homeassistant/auth.do";
        public static string refreshTokenUri = "https://px1.tuyaus.com/homeassistant/access.do";
        public static string deviceUri = "https://px1.tuyaus.com/homeassistant/skill";

        private Session SessionData { get; set; }
        public List<Devices> DevicesList { get; set; }

        public TuyaApi()
        {
            try
            {
                string token = File.ReadAllText(tokenPath);
                SessionData = JsonConvert.DeserializeObject<Session>(token);
            }
            catch (Exception)
            { }

            try
            {
                string devicesList = File.ReadAllText(devicesListPath);
                DevicesList = JsonConvert.DeserializeObject<List<Devices>>(devicesList);
            }
            catch (Exception)
            {
                DevicesList = new List<Devices>();
            }
        }

        public async Task Login(string userName, string password, string countryCode = "95", string bizType = "tuya", string from = "tuya")
        {
            var reqData = new LoginRequest
            {
                UserName = userName,
                Password = password,
                CountryCode = countryCode,
                BizType = bizType,
                From = from,
            };
            var res = await ApiServices.PostFormData<LoginResponse>(loginUri, reqData.GetFormData());

            if (!string.IsNullOrEmpty(res.Access_token))
            {
                SessionData = new Session
                {
                    Access_token = res.Access_token,
                    Refresh_token = res.Refresh_token,
                    Token_type = res.Token_type,
                    Expires_in = res.Expires_in
                };
                if (!Directory.Exists(Path.GetDirectoryName(tokenPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(tokenPath));
                }
                File.WriteAllText(tokenPath, SessionData.ToString());
            }
        }

        public async Task RefreshToken()
        {
            var reqData = new RefreshTokenRequest
            {
                Grant_type = "refresh_token",
                Refresh_token = SessionData.Refresh_token
            };
            var res = await ApiServices.PostFormData<RefreshTokenResponse>(refreshTokenUri, reqData.GetFormData());
            if (!string.IsNullOrEmpty(res.Access_token))
            {
                SessionData = new Session
                {
                    Access_token = res.Access_token,
                    Refresh_token = res.Refresh_token,
                    Token_type = res.Token_type,
                    Expires_in = res.Expires_in
                };
                if (!Directory.Exists(Path.GetDirectoryName(tokenPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(tokenPath));
                }
                File.WriteAllText(tokenPath, SessionData.ToString());
            }
        }

        public async Task DiscoverDevices()
        {
            var reqData = new DeviceRequest
            {
                Header = new DeviceRequest_Header
                {
                    Name = "Discovery",
                    Namespace = "discovery",
                    PayloadVersion = 1,
                },
                Payload = new DeviceRequest_Payload
                {
                    AccessToken = SessionData.Access_token,
                    DevId = null
                }
            };
            var res = await ApiServices.PostJsonData<DeviceResponse>(deviceUri, reqData.ToString());

            if (res.Header.Code == "SUCCESS")
            {
                DevicesList = res.Payload.Devices;
                if (!Directory.Exists(Path.GetDirectoryName(devicesListPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(devicesListPath));
                }
                File.WriteAllText(devicesListPath, JsonConvert.SerializeObject(DevicesList));
            }
        }

        public void SaveDeviceData()
        {
            File.WriteAllText(devicesListPath, JsonConvert.SerializeObject(DevicesList));
        }


        public async void ControlDevicesAsync(string deviceId, int state)
        {
            var reqData = new DeviceRequest
            {
                Header = new DeviceRequest_Header
                {
                    Name = "turnOnOff",
                    Namespace = "control",
                    PayloadVersion = 1,
                },
                Payload = new DeviceRequest_Payload
                {
                    AccessToken = SessionData.Access_token,
                    DevId = deviceId,
                    Value = state,
                }
            };
            await ApiServices.PostJsonData<DeviceResponse>(deviceUri, reqData.ToString());
        }

    }

    class Session
    {
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
