﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectSetupKit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.vm = new MainWindowVM(this);
            DataContext = this.vm;

            this.PreviewKeyDown += new KeyEventHandler(HandleEscapeKey);
            this.PreviewKeyDown += new KeyEventHandler(HandleEnterKey);

            Loaded += (sender, e) => MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        private void HandleEscapeKey(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        private void HandleEnterKey(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                if (vm.installNewProject())
                {
                    Close();
                }
                else
                {
                    MessageBox.Show("blabla", "Error", MessageBoxButton.OK);
                }
            }
        }

        MainWindowVM vm;
    }
}
