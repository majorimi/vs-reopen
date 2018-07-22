using System.Windows.Controls;

namespace VSDocumentReopen.VS.ToolWindows.IconHandling.ButtonStates
{
	public abstract class ImageButtonState
	{
		protected readonly Button _button;
		protected readonly Image _enabledImage;
		protected readonly Image _disabledImage;

		protected ImageButtonState(Button button, Image enabledImage, Image disabledImage)
		{
			_button = button;
			_enabledImage = enabledImage;
			_disabledImage = disabledImage;
		}

		public abstract void Enable();

		public abstract void Disable();
	}
}