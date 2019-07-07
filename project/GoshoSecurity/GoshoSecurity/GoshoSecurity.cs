using GoshoSecurity.Data;
using GoshoSecurity.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace GoshoSecurity
{
	public class App : Application
	{
		public static GoshoSecurityManager GoshoSecurityManager { get; private set; }

		public App ()
		{
            GoshoSecurityManager = new GoshoSecurityManager (new RestService ());
            MainPage = new LoginPage();
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

