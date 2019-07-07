namespace GoshoSecurity.Views
{
    using GoshoSecurity.Models;
    using System;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent();
            App.Current.MainPage = this;
            App.GoshoSecurityManager.Logout();
		}

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            var userNotEmpty = !string.IsNullOrEmpty(usernameEntry.Text);
            var passwordNotEmpty = !string.IsNullOrEmpty(passwordEntry.Text);

            if (userNotEmpty && passwordNotEmpty)
            {
                try
                {
                    await App.GoshoSecurityManager.Login(new UserLoginModel()
                    {
                        Username = usernameEntry.Text,
                        Password = passwordEntry.Text
                    });
                }
                catch (Exception ex)
                {
                    await DisplayAlert("", ex.Message, "Ok");
                }

                if (App.GoshoSecurityManager.LoggedUser != null)
                {
                   
                    App.Current.MainPage = new MainPage();
                    Navigation.RemovePage(this);
                }
            }

            messageLabel.Text = "Invalid username or password";
        }

        private bool IfUserIsAdmin()
        {
            return !string.IsNullOrEmpty(App.GoshoSecurityManager.LoggedUser.Role);
        }
    }
}