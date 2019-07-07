using GoshoSecurity.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GoshoSecurity.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PhotoPage : ContentPage
	{
		public PhotoPage ()
		{
			InitializeComponent ();
		}

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            var photo = (Photo)BindingContext;
            await App.GoshoSecurityManager.DeletePhoto(photo);
            await Navigation.PopAsync();
        }
    }
}