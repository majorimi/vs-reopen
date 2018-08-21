using VSDocumentReopen.VS.ToolWindows.IconHandling.ButtonStates;
using Xunit;

namespace VSDocumentReopen.Test.VS.ToolWindows.IconHandling.ButtonStates
{
	public class ButtonDisabledStateTest : ImageButtonStateTest<ButtonDisabledState>
	{
		[StaFact]
		public void ButtonDisabledState_Handle_Disable_Null_Button()
		{
			var state = new ButtonDisabledState(null, null, null);
			state.Disable();

			var button = GetButton(state);

			Assert.Null(button);
		}

		[StaFact]
		public void ButtonDisabledState_Handle_Enable_Null_Button()
		{
			var state = new ButtonDisabledState(null, null, null);
			state.Enable();

			var button = GetButton(state);

			Assert.Null(button);
		}

		[StaFact]
		public void ButtonDisabledState_Should_Disable_No_State_Change()
		{
			_imageButtonState.Disable();

			var button = GetButton(_imageButtonState);
			var state = button.GetImageButtonState();

			Assert.False(button.IsEnabled);
			Assert.Same(_disableImage, button.Content);
			Assert.IsType<ButtonDisabledState>(state);
		}

		[StaFact]
		public void ButtonDisabledState_Should_Enable_And_Change_State()
		{
			_imageButtonState.Enable();

			var button = GetButton(_imageButtonState);
			var state = button.GetImageButtonState();

			Assert.True(button.IsEnabled);
			Assert.Same(_enableImage, button.Content);
			Assert.IsType<ButtonEnabledState>(state);
		}
	}
}