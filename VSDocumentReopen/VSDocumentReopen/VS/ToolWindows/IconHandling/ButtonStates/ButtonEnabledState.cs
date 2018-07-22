using System.Windows.Controls;

namespace VSDocumentReopen.VS.ToolWindows.IconHandling.ButtonStates
{
	public class ButtonEnabledState : ImageButtonState
	{
		public ButtonEnabledState(Button button, Image enabledImage, Image disabledImage)
			: base(button, enabledImage, disabledImage)
		{}

		public override void Disable()
		{
			_button.IsEnabled = false;
			_button.Content = _disabledImage;

			var state = new ButtonDisabledState(_button, _enabledImage, _disabledImage);
			_button.SetImageButtonState(state);
		}

		public override void Enable()
		{
			_button.IsEnabled = true;
			_button.Content = _enabledImage;
			_button.SetImageButtonState(this);
		}
	}
}