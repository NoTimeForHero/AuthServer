using System.Net;
using Microsoft.AspNetCore.HttpOverrides;

namespace AuthServer.Utils.Converters
{
    public class IPAddressConverter
    {
        public static string Serialize(IPAddress input) => input.ToString();
        public static IPAddress Deserialize(string input) => IPAddress.Parse(input);
    }

    public class IPNetworkConverter
    {
        public static string Serialize(IPNetwork input)
        {
            return IPAddressConverter.Serialize(input.Prefix) + "/" + input.PrefixLength;
        }

        public static IPNetwork Deserialize(string input)
        {
            var parts = input.Split('/');
            if (parts.Length < 2) throw new ArgumentException("Missing \"/\" in IPNetwork!");
            if (parts.Length > 2) throw new ArgumentException("Too many \"/\" in IPNetwork!");
            var ip = IPAddressConverter.Deserialize(parts[0]);
            var prefix = int.Parse(parts[1]);
            if (prefix < 0 || prefix > 32) throw new ArgumentException("Invalid prefix size!");
            return new IPNetwork(ip, prefix);
        }
    }
}
