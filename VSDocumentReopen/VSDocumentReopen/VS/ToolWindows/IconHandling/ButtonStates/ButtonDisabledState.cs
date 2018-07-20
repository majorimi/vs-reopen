using System.Windows.Controls;

namespace VSDocumentReopen.VS.ToolWindows.IconHandling.ButtonStates
{
	public class ButtonDisabledState : ImageButtonState
	{
		public ButtonDisabledState(Button button, Image enabledImage, Image disabledImage)
			: base(button, enabledImage, disabledImage)
		{}

		public override void Disable()
		{
			_button.IsEnabled = false;
			_button.Content = _disabledImage;

			_button.SetImageButtonState(this);
		}

		public override void Enable()
		{
			_button.IsEnabled = true;
			_button.Content = _enabledImage;

			var state = new ButtonEnabledState(_button, _enabledImage, _disabledImage);
			_button.SetImageButtonState(state);
		}
	}
}