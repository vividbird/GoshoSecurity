namespace GoshoSecurity.CS
{
    using GoshoSecurity.CS.Models;
    using GoshoSecurity.Views;
    using System.Collections.Generic;
    using Xamarin.Forms;

    public class MasterPageCS : ContentPage
    {
        public ListView ListView { get { return listView; } }

        readonly ListView listView;

        public MasterPageCS()
        {
            var masterPageItems = new List<MasterPageItem>();
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Main Page",
                IconSource = "contacts.png",
                TargetType = typeof(WelcomePage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Photos",
                IconSource = "todo.png",
                TargetType = typeof(Image)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Change password",
                IconSource = "contacts.png",
                TargetType = typeof(PasswordChangePage)
            });


            if (App.GoshoSecurityManager.LoggedUser.Role == "Administrator")
            {
                masterPageItems.Add(new MasterPageItem
                {
                    Title = "Change password",
                    IconSource = "contacts.png",
                    TargetType = typeof(PasswordChangePage)
                });
            }

            listView = new ListView
            {
                ItemsSource = masterPageItems,
                ItemTemplate = new DataTemplate(() =>
                {
                    var grid = new Grid { Padding = new Thickness(5, 10) };
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

                    var image = new Image();
                    image.SetBinding(Image.SourceProperty, "IconSource");
                    var label = new Label { VerticalOptions = LayoutOptions.FillAndExpand };
                    label.SetBinding(Label.TextProperty, "Title");

                    grid.Children.Add(image);
                    grid.Children.Add(label, 1, 0);

                    return new ViewCell { View = grid };
                }),
                SeparatorVisibility = SeparatorVisibility.None
            };

            Title = "Gosho Security";
            Content = new StackLayout
            {
                Children = { listView }
            };
        }
    }
}
