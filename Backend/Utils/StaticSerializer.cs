using AspNet.Security.OAuth.MailRu;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AuthServer
{
    internal class StaticSerializer
    {
        private static readonly IDeserializer deserializer = new DeserializerBuilder().Build();
        private static readonly ISerializer serializer = new SerializerBuilder().Build();

        public static T Deserialize<T>(string input) => deserializer.Deserialize<T>(input);
        public static string Serialize<T>(T input) => serializer.Serialize(input);
    }
}
