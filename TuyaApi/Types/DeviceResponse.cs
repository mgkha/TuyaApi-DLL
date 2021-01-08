using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace TuyaApi.Types
{
    class DeviceResponse
    {
        public DeviceResponse_Header Header { get; set; }
        public DeviceResponse_Payload Payload { get; set; }
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

    class DeviceResponse_Header
    {
        public string Msg { get; set; }
        public string Code { get; set; }
        public int PayloadVersion { get; set; }
    }
    class DeviceResponse_Payload
    {
        public List<Devices> Devices { get; set; }
    }

    public class Devices
    {
        public Data Data { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Id { get; set; }
        public string Dev_type { get; set; }
        public string Ha_type { get; set; }
    }

    public class Data
    {
        public bool Online { get; set; }
        public bool State { get; set; }
    }
}
