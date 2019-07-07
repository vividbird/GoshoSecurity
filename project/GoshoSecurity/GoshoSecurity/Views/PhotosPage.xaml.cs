using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GoshoSecurity.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PhotosPage : ContentPage
	{
		public PhotosPage()
		{
			InitializeComponent ();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            deleteButton.IsVisible = listView.ItemsCount == 0 ? false : true;
            listView.ItemsSource = await App.GoshoSecurityManager.GetPhotosForUser();
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PhotoPage
            {
                BindingContext = listView.SelectedItem
            });
        }
    }
}