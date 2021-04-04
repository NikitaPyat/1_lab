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
        public MainWindow()
        {
            InitializeComponent();
        }

        public void ButtonV5MainCollection(object sender, RoutedEventArgs e) {
            if (TryFindResource("main_collection") is V5MainCollection coll) { coll.AddDefaults(); }
        }

        public void ButtonV5DataCollection(object sender, RoutedEventArgs e) {
            V5DataCollection datacollection = new V5DataCollection();
            datacollection.InitRandom(3, (float)0.5, (float)0.5, (float)0.1, (float)1.0);
            if (TryFindResource("main_collection") is V5MainCollection coll) {
                coll.Add(datacollection);
            }
        }

        public void ButtonV5DataOnGrid(object sender, RoutedEventArgs e) {
            Grid2D g = new Grid2D((float)0.3, 3, (float)0.3, 3);
            V5DataOnGrid dataongrid = new V5DataOnGrid("", DateTime.Now, g);
            dataongrid.InitRandom((float)0.1, (float)1.0);
            if (TryFindResource("main_collection") is V5MainCollection coll) { coll.Add(dataongrid); }
        }

        public void ButtonRemove(object sender, RoutedEventArgs e) {
            if (TryFindResource("main_collection") is V5MainCollection coll) {
                try { coll.RemoveAt(lisBox_Main.SelectedIndex); }
                catch (Exception) { MessageBox.Show("Choose element"); }
            }
        }

        public void ButtonNew(object sender, RoutedEventArgs e) {
            V5MainCollection res = TryFindResource("main_collection") as V5MainCollection;
            if ((res != null) && (res.Change == true)) {
                MessageBoxResult result = MessageBox.Show("Do you want to save changes?", " ", MessageBoxButton.YesNo);
                switch (result) {
                    case MessageBoxResult.Yes:
                        Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                        Microsoft.Win32.SaveFileDialog s = saveFileDialog;
                        s.Filter = "Documents (.txt)";
                        s.CreatePrompt = true;
                        s.OverwritePrompt = true;
                        if (s.ShowDialog() == true){
                            string filename = s.FileName;
                            if (res != null) {res.Save(filename);}
                        }
                        break;
                    case MessageBoxResult.No: break;
                }
            }
            if (res != null){res.RemoveAll();}
        }

        public void ButtonOpen(object sender, RoutedEventArgs e) {
            V5MainCollection res = TryFindResource("main_collection") as V5MainCollection;
            if ((res != null) && (res.Change == true)) {
                MessageBoxResult result = MessageBox.Show("Do you want to save changes?", " ", MessageBoxButton.YesNo);
                switch (result) {
                    case MessageBoxResult.Yes:
                        Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog {
                            Filter = "Text documents (.txt)|*.txt",
                            CreatePrompt = true,
                            OverwritePrompt = true
                        };
                        if (dlg.ShowDialog() == true) {
                            string filename = dlg.FileName;
                            if (res != null){ res.Save(filename);}
                        }
                        break;
                    case MessageBoxResult.No: break;
                }
            }
            try {
                Microsoft.Win32.OpenFileDialog dlg1 = new Microsoft.Win32.OpenFileDialog{Filter = "Documents (.txt)"};
                if (dlg1.ShowDialog() == true) {
                    string filename = dlg1.FileName;
                    if (res != null) {
                        res.Load(filename);
                        res.Change = false;
                    }

                }
            }
            catch (Exception) {MessageBox.Show("Uncorrect file");}
        }

        public void ButtonSave(object sender, RoutedEventArgs e) {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog{
                Filter = "Text documents (.txt)|*.txt",
                CreatePrompt = true,
                OverwritePrompt = true
            };
            if (dlg.ShowDialog() == true){
                string filename = dlg.FileName;
                if (TryFindResource("main_collection") is V5MainCollection coll){coll.Save(filename);}
            }
        }
    }
}
