using System.Text.Json;

namespace AuthServer.Utils
{
    public class IndexFile
    {
        private readonly Dictionary<string, object> jsonObjects = new();
        private readonly string filename = Constants.IndexFile;

        public void Add(string name, object value) => jsonObjects.Add(name, value);

        private string BuildText(string text)
        {
            if (jsonObjects.Count == 0) return text;
            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var json = JsonSerializer.Serialize(jsonObjects, jsonOptions);
            json = Environment.NewLine + "<script>" + Environment.NewLine +
                   $"window.{Constants.WindowTag} = {json}" + Environment.NewLine +
                   "</script>";
            return Inject(text, json);
        }

        private string Inject(string inputText, string payload, string tag = "</title>")
        {
            var injectPosition = inputText.IndexOf(tag, StringComparison.Ordinal);
            if (injectPosition < 0) throw new ApplicationException($"Cannot find {tag} tag in document!");
            return inputText.Insert(injectPosition + tag.Length + 1, payload);
        }

        public async Task Write(HttpResponse response)
        {
            response.StatusCode = 200;
            response.ContentType = "text/html";
            var text = await File.ReadAllTextAsync(filename);
            text = BuildText(text);
            await response.WriteAsync(text);
        }

    }
}
