using ServiceStack.Text;

namespace VSDocumentReopen.Infrastructure.Helpers
{
	public class ServiceStackJsonSerializer : IJsonSerializer
	{
		public string Serialize<T>(T obj)
		{
			return JsonSerializer.SerializeToString<T>(obj);
		}

		public T Deserialize<T>(string json)
		{
			return JsonSerializer.DeserializeFromString<T>(json);
		}
	}
}