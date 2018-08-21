using System;
using System.Reflection;
using System.Windows.Controls;
using VSDocumentReopen.VS.ToolWindows.IconHandling.ButtonStates;
using Xunit;

namespace VSDocumentReopen.Test.VS.ToolWindows.IconHandling.ButtonStates
{
	public abstract class ImageButtonStateTest<T> where T : ImageButtonState
	{
		protected readonly ImageButtonState _imageButtonState;
		protected readonly Image _enableImage;
		protected readonly Image _disableImage;
		protected readonly Button _button;

		protected ImageButtonStateTest()
		{
			_button = new Button();
			_enableImage = new Image();
			_disableImage = new Image();

			var genericType = typeof(T);

			_imageButtonState  = (ImageButtonState)Activator.CreateInstance(genericType, _button, _enableImage, _disableImage);
		}

		protected Button GetButton(ImageButtonState imageButtonState)
		{
			var member = GetMemberValue(imageButtonState, "_button") as Button;
			return member;
		}
		protected Image GetEnableImage(ImageButtonState imageButtonState)
		{
			var member = GetMemberValue(imageButtonState, "_enabledImage") as Image;
			return member;
		}
		protected Image GetDisableImage(ImageButtonState imageButtonState)
		{
			var member = GetMemberValue(imageButtonState, "_disabledImage") as Image;
			return member;
		}

		private object GetMemberValue(ImageButtonState imageButtonState, string name)
		{
			return typeof(ImageButtonState).GetField(name, BindingFlags.NonPublic|BindingFlags.Instance).GetValue(imageButtonState);
		}

		[StaFact]
		public void ItShould_Set_Values()
		{
			Assert.Same(_button, GetButton(_imageButtonState));
			Assert.Same(_enableImage, GetEnableImage(_imageButtonState));
			Assert.Same(_disableImage, GetDisableImage(_imageButtonState));
		}
	}
}