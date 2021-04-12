using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Lab_2.Models.Collections;
using Lab_2.Models;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        private V5MainCollection Main = new V5MainCollection();
        public MainWindow(){
            InitializeComponent();
            DataContext = Main;
        }
        private void ButtonNew(object sender, RoutedEventArgs e)
        {
            if (Main.Change == true) {
                UnsavedChanges();
            }
            Main = new V5MainCollection();
            DataContext = Main;
            ErrorMsg();
        }

        private void ButtonOpen(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Main.Change == true) {
                    UnsavedChanges();
                }
                Microsoft.Win32.OpenFileDialog fd = new Microsoft.Win32.OpenFileDialog();
                if ((bool)fd.ShowDialog() == true) {
                    Main = new V5MainCollection();
                    Main.Load(fd.FileName);
                    DataContext = Main;
                }
            }
            catch (Exception) {
                MessageBox.Show("Error");
            }
            finally
            {
                ErrorMsg();
            }
        }

        private void ButtonSave(object sender, RoutedEventArgs e)
        {
            try{
                Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
                if ((bool)dialog.ShowDialog() == true)
                    Main.Save(dialog.FileName);
            }
            catch (Exception) {
                MessageBox.Show("Error");
            }
            finally{
                ErrorMsg();
            }
        }

        private void ButtonAddElement(object sender, RoutedEventArgs e)
        {
            try{
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                if ((bool)dlg.ShowDialog() == true) Main.AddFromFile(dlg.FileName);
            }
            catch (Exception){
                MessageBox.Show("Add error");
            }
            finally{
                ErrorMsg();
            }
        }

        private void ButtonV5DataCollection(object sender, RoutedEventArgs e)
        {
            Main.AddDefaultDataCollection();
            ErrorMsg();
        }

        private void ButtonV5MainCollection(object sender, RoutedEventArgs e)
        {
            Main.AddDefaults();
            DataContext = Main;
            ErrorMsg();
        }

        private void ButtonV5DataOnGrid(object sender, RoutedEventArgs e)
        {
            Main.AddDefaultDataOnGrid();
            ErrorMsg();
        }

        private bool UnsavedChanges()
        {
            MessageBoxResult msg = MessageBox.Show("Save Changes?", "Save", MessageBoxButton.YesNoCancel);
            if (msg == MessageBoxResult.Cancel){
                return true;
            }
            else if (msg == MessageBoxResult.Yes) {
                Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
                if ((bool)dialog.ShowDialog() == true) Main.Save(dialog.FileName);
            }
            return false;
        }

        private void AppClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Main.Change == true){
                e.Cancel = UnsavedChanges();
            }
            ErrorMsg();
        }

        private void ButtonRemove(object sender, RoutedEventArgs e)
        {
            var selectedLB = lisBox_Main.SelectedItems;
            List<V5Data> Items = new List<V5Data>();
            Items.AddRange(selectedLB.Cast<V5Data>());
            foreach (V5Data item in Items){
                Main.Remove(item.info, item.date);
            }
            ErrorMsg();
        }

        private void FilterDataCollection(object sender, FilterEventArgs args)
        {
            var item = args.Item;
            if (item != null == true){
                if (item.GetType() == typeof(V5DataCollection)) args.Accepted = true;
                else args.Accepted = false;
            }
        }

        private void FilterDataOnGrid(object sender, FilterEventArgs args)
        {
            var item = args.Item;
            if (item != null == true){
                if (item.GetType() == typeof(V5DataOnGrid)) args.Accepted = true;
                else args.Accepted = false;
            }
        }

        public void ErrorMsg()
        {
            if (Main.ErrorMessage != null){
                MessageBox.Show(Main.ErrorMessage, "Error");
                Main.ErrorMessage = null;
            }
        }
    }
}
