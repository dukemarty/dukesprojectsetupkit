﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ProjectSetupKit.Properties;

namespace ProjectSetupKit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            vm = new MainWindowViewModel(this);
            DataContext = new
            {
                vm,
                settings = Settings.Default
            };

            PreviewKeyDown += HandleEscapeKey;
            PreviewKeyDown += HandleEnterKey;
            PreviewKeyDown += HandleCombinationKeys;

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
                object focusObj = FocusManager.GetFocusedElement(this);
                if (focusObj != null && focusObj is TextBox)
                {
                    var binding = (focusObj as TextBox).GetBindingExpression(TextBox.TextProperty);

                    if (binding != null)
                    {
                        binding.UpdateSource();
                    }
                }

                if (vm.InstallNewProject())
                {
                    Close();
                }
                else
                {
                    MessageBox.Show("Template could not be installed with the given parameters.", "Error", MessageBoxButton.OK);
                }
            }
        }

        private void HandleCombinationKeys(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Alt)
            {
                var newIndex = -1;
                if (e.SystemKey >= Key.D1 && e.SystemKey <= Key.D9)
                {
                    newIndex = e.SystemKey - Key.D1;
                }
                else if (e.SystemKey == Key.D0)
                {
                    newIndex = 9;
                    e.Handled = true;
                }
                if (newIndex >= 0 && newIndex < ProjectTypeListBox.Items.Count)
                {
                    ProjectTypeListBox.SelectedIndex = newIndex;
                }
            }
        }

        public void ExitWithError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK);
            Close();
        }

        MainWindowViewModel vm;
    }
}
