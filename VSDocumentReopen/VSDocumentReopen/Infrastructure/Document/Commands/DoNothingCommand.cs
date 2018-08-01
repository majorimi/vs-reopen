namespace VSDocumentReopen.Infrastructure.Document.Commands
{
	public class DoNothingCommand : IDocumentCommand
	{
		public static DoNothingCommand Instance { get; }

		static DoNothingCommand()
		{
			Instance = new DoNothingCommand();
		}

		private DoNothingCommand()
		{}

		public void Execute()
		{}
	}
}