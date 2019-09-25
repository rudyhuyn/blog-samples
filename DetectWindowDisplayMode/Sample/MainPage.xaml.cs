using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WindowHelper
{

    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void DetectWindowType_Click(object sender, RoutedEventArgs e)
        {
            await new MessageDialog(Huyn.WindowDisplayInfo.GetForCurrentView().ToString()).ShowAsync();
        }
    }
}
