using GoshoSecurity.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GoshoSecurity.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PasswordChangePage : ContentPage
	{
		public PasswordChangePage ()
		{
			InitializeComponent ();
		}

        private async void ChangePasswordButton_Clicked(object sender, EventArgs e)
        {
            var response = await App.GoshoSecurityManager.ChangePasswordAsync(new PasswordChangeModel()
            {
                UserId = App.GoshoSecurityManager.LoggedUser.Id,
                CurrentPassword = currentPasswordEntry.Text,
                NewPassword = newPasswordEntry.Text,
                ConfirmPassword = confirmNewPasswordEntry.Text
            });

            this.ResetEntryValues();
            await DisplayAlert("", response , "Ok");
        }

        private void ResetEntryValues()
        {
            currentPasswordEntry.Text = string.Empty;
            newPasswordEntry.Text = string.Empty;
            confirmNewPasswordEntry.Text = string.Empty;
        }
    }
}