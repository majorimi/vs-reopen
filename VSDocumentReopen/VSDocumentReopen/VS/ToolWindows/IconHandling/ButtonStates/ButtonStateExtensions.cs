using System.Windows;
using System.Windows.Controls;

namespace VSDocumentReopen.VS.ToolWindows.IconHandling.ButtonStates
{
	public static class ButtonStateExtensions
	{
		public static readonly DependencyProperty ImageButtonStateProperty =
			DependencyProperty.RegisterAttached("MenuButtonState", typeof(ImageButtonState), typeof(Button));

		public static void SetImageButtonState(this Button element, ImageButtonState state)
		{
			element.SetValue(ImageButtonStateProperty, state);
		}

		public static ImageButtonState GetImageButtonState(this Button element)
		{
			return (ImageButtonState)element.GetValue(ImageButtonStateProperty);
		}
	}
}