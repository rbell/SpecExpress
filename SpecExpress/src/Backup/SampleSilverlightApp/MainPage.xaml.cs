using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SpecExpress;
using SpecExpress.Test.Domain.Entities;

namespace SampleSilverlightApp
{
    public partial class MainPage : UserControl
    {
        private MainPageModel _model;

        public MainPage()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Initialize the Validaiton Container
            ValidationCatalog.Scan(x => x.AddAssembly(typeof (Project).Assembly));

            _model = new MainPageModel(new Project(){ProjectName = ""});

           LayoutRoot.DataContext = _model;
        }

        private void Save1Button_Click(object sender, RoutedEventArgs e)
        {
            _model.Save();
        }
    }
}
