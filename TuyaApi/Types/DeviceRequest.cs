using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TuyaApi.Types
{
    class DeviceRequest
    {
        public DeviceRequest_Header Header { get; set; }
        public DeviceRequest_Payload Payload { get; set; }
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
    class DeviceRequest_Header
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public int PayloadVersion { get; set; }
    }
    class DeviceRequest_Payload
    {
        public string AccessToken { get; set; }
        public string DevId { get; set; }
        public int Value { get; set; }
    }
}
