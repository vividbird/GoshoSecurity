using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GoshoSecurity.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WelcomePage : ContentPage
	{
		public WelcomePage ()
		{
           
			InitializeComponent ();
            welcomeBackLabel.Text = $"Welcome back to Gosho Security, {App.GoshoSecurityManager.LoggedUser.Name}";
        }
	}
}