namespace GoshoSecurity.Views
{
    using System;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChangeUserInfo : ContentPage
	{
		public ChangeUserInfo ()
		{
			InitializeComponent ();
            username.Text = App.GoshoSecurityManager.LoggedUser.Username;
            nameEntry.Text = App.GoshoSecurityManager.LoggedUser.Name;
            emailEntry.Text = App.GoshoSecurityManager.LoggedUser.Email;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var result = await App.GoshoSecurityManager.ChangeUserInformation(new Models.UserEditModel()
            {
                Id = App.GoshoSecurityManager.LoggedUser.Id,
                Name = nameEntry.Text,
                Email = emailEntry.Text

            });

            nameEntry.Text = App.GoshoSecurityManager.LoggedUser.Name;
            emailEntry.Text = App.GoshoSecurityManager.LoggedUser.Email;

            await DisplayAlert("", result, "Ok");
        }
    }
}