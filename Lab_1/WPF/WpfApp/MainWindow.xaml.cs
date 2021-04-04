using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Lab_2.Models.Collections;
using Lab_2.Models;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void ButtonNew(object sender, RoutedEventArgs e)
        {
            V5MainCollection coll = TryFindResource("key_main_collection") as V5MainCollection;
            if ((coll != null) && (coll.Change == true))
            {
                MessageBoxResult result = MessageBox.Show("Изменения будут потеряны. Сохранить изменения?", "", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                        dlg.Filter = "Text documents (.txt)|*.txt";
                        dlg.CreatePrompt = true;
                        dlg.OverwritePrompt = true;
                        if (dlg.ShowDialog() == true)
                        {
                            string filename = dlg.FileName;
                            if (coll != null)
                            {
                                coll.Save(filename);
                            }
                        }
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }

            if (coll != null)
            {
                coll.RemoveAll();
            }
        }

        public void ButtonOpen(object sender, RoutedEventArgs e)
        {
            V5MainCollection coll = TryFindResource("key_main_collection") as V5MainCollection;
            if ((coll != null) && (coll.Change == true))
            {
                MessageBoxResult result = MessageBox.Show("Изменения будут потеряны. Сохранить изменения?", "", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                        dlg.Filter = "Text documents (.txt)|*.txt";
                        dlg.CreatePrompt = true;
                        dlg.OverwritePrompt = true;
                        if (dlg.ShowDialog() == true)
                        {
                            string filename = dlg.FileName;
                            if (coll != null)
                            {
                                coll.Save(filename);
                            }
                        }
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }

            try
            {
                Microsoft.Win32.OpenFileDialog dlg1 = new Microsoft.Win32.OpenFileDialog();
                dlg1.Filter = "Text documents (.txt)|*.txt";
                if (dlg1.ShowDialog() == true)
                {
                    string filename = dlg1.FileName;
                    if (coll != null)
                    {
                        coll.Load(filename);
                        coll.Change = false;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Некорректный файл");
            }

        }

        public void ButtonSave(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Filter = "Text documents (.txt)|*.txt";
            dlg.CreatePrompt = true;
            dlg.OverwritePrompt = true;
            if (dlg.ShowDialog() == true)
            {
                string filename = dlg.FileName;
                V5MainCollection coll = TryFindResource("key_main_collection") as V5MainCollection;
                if (coll != null)
                {
                    coll.Save(filename);
                }
            }
        }

        public void ButtonV5MainCollection(object sender, RoutedEventArgs e)
        {
            V5MainCollection coll = TryFindResource("key_main_collection") as V5MainCollection;
            if (coll != null)
            {
                coll.AddDefaults();
            }
        }

        public void ButtonV5DataCollection(object sender, RoutedEventArgs e)
        {
            V5DataCollection datacollection = new V5DataCollection();
            datacollection.InitRandom(3, (float)0.5, (float)0.5, (float)0.1, (float)1.0);

            V5MainCollection coll = TryFindResource("key_main_collection") as V5MainCollection;
            if (coll != null)
            {
                coll.Add(datacollection);
            }
        }

        public void ButtonV5DataOnGrid(object sender, RoutedEventArgs e)
        {
            Grid2D g = new Grid2D((float)0.3, 3, (float)0.3, 3);
            V5DataOnGrid dataongrid = new V5DataOnGrid("", DateTime.Now, g);
            dataongrid.InitRandom((float)0.1, (float)1.0);

            V5MainCollection coll = TryFindResource("key_main_collection") as V5MainCollection;
            if (coll != null)
            {
                coll.Add(dataongrid);
            }
        }

        /*private void Button_AddFromFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Text documents (.txt)|*.txt";
            try
            {
                if (dlg.ShowDialog() == true)
                {
                    string filename = dlg.FileName;
                    V5DataOnGrid dataongrid = new V5DataOnGrid(filename);

                    V5MainCollection coll = TryFindResource("key_main_collection") as V5MainCollection;
                    if (coll != null)
                    {
                        coll.Add(dataongrid);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }*/

        public void ButtonRemove(object sender, RoutedEventArgs e)
        {
            V5MainCollection coll = TryFindResource("key_main_collection") as V5MainCollection;
            if (coll != null)
            {
                try
                {
                    coll.RemoveAt(lisBox_Main.SelectedIndex);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Сначала выберите элемент");
                }

            }
        }
    }
}
