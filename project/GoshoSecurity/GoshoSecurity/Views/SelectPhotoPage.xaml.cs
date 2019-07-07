using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GoshoSecurity.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectPhotoPage : ContentPage
    {
        private MediaFile photo;

        public SelectPhotoPage()
        {
            InitializeComponent();
            uploadButton.IsVisible = false;
        }

        private async void SelectImageButton_Clicked(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("No upload", "Picking a photo is not supported", "ok");
                return;
            }

            photo = await CrossMedia.Current.PickPhotoAsync();
            if (photo == null)
                return;

            uploadButton.IsVisible = true;
            selectedImage.Source = ImageSource.FromStream(() => photo.GetStream());
        }

        private async void UploadPhoto_Clicked(object sender, EventArgs e)
        {
            var succeeded = await App.GoshoSecurityManager.UploadPhoto(photo);

            if (succeeded)
            {
                await DisplayAlert("", "Photo was successfully uploaded", "Ok");
            }
            else
            {
                await DisplayAlert("", "An error occured. Please try again", "Ok");
            }

            uploadButton.IsVisible = false;
            selectedImage.Source = null;
        }

        private async void TakePhotoButton_Clicked(object sender, EventArgs e)
        {
            var photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions() { });

            if (photo == null)
                return;

            uploadButton.IsVisible = true;
            selectedImage.Source = ImageSource.FromStream(() => photo.GetStream());
        }
    }
}