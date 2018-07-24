using System.Drawing;
using VSDocumentReopen.Domain.Documents;

namespace VSDocumentReopen.Infrastructure.FileIcons
{
	public interface IFileExtensionIconResolver
	{
		Icon GetIcon(IClosedDocument document);
	}
}