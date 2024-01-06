using System;
using System.Data;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System.Collections.ObjectModel;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BDFF
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void NavigationViewItem_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var item = sender as NavigationViewItem;
            switch(item.Name)
            {
                case "ha": ContentFrame.Navigate(typeof(Views.GridHistoriqueAchat));
                    break;
                case "ad": ContentFrame.Navigate(typeof(Views.AjoutDonnees));
                    break;
            }
            
        }
    }
}
