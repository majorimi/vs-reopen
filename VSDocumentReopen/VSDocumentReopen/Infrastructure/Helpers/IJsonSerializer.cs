namespace VSDocumentReopen.Infrastructure.Helpers
{
	public interface IJsonSerializer
	{
		string Serialize<T>(T obj);

		T Deserialize<T>(string s);
	}
}