using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;


namespace RoyMiz
{
  
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
          
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            openFileDlg.Multiselect = true;
            openFileDlg.Filter = "mp3 files(*.mp3)|*.mp3";
            int flag = 0;
            int loop;
            List<string> finalFilesList = new List<string>();

            Nullable<bool> result = openFileDlg.ShowDialog();
            
            if (result == true)
            {
                string fullPath = openFileDlg.FileName;
                string fileName = openFileDlg.SafeFileName;
                string path = fullPath.Replace(fileName, "");

                string filename;
                List<string> files = new List<string>();
                int i = 0;
                try
                {
                    if ((openFileDlg.OpenFile()) != null)
                    {
                        foreach (string file in openFileDlg.FileNames)
                        {

                          
                            filename = Path.GetFileNameWithoutExtension(file);
                            if (IsValidFileName(filename))
                            {
                               
                                files.Add(filename);
                              
                            }
                           
                            i = i + 1;

                        }
                    }

                    int filesCount= files.Count();
                        if (filesCount%2==0)
                        {
                           
                            for (loop=0;loop<filesCount/2; loop++)
                            {
                                string tempFileName = files[0];
                                string fileNameBeforeDot = tempFileName.Split('.')[0];
                                string fileNameAfterDot = tempFileName.Split('.')[1];
                                if (fileNameAfterDot == "1")
                                {
                                     string tempFileName2 = fileNameBeforeDot + ".2";
                                    if (files.Contains(tempFileName2))
                                    {
                                        finalFilesList.Add(tempFileName);
                                        finalFilesList.Add(tempFileName2);
                                        files.Remove(tempFileName);
                                        files.Remove(tempFileName2);
                                    }
                                    else
                                    {
                                    flag = 1;
                                    MessageBox.Show("Pair of"+tempFileName+" is Missing");
                                    break;
                                }
                                }
                                else if(fileNameAfterDot == "2")
                                {
                                    string tempFileName2 = fileNameBeforeDot + ".1";
                                    if (files.Contains(tempFileName2))
                                    {
                                        finalFilesList.Add(tempFileName);
                                        finalFilesList.Add(tempFileName2);
                                        files.Remove(tempFileName);
                                        files.Remove(tempFileName2);
                                }
                                    else
                                    {
                                    flag = 1;
                                    MessageBox.Show("Pair of " + tempFileName + " is Missing");
                                    break;

                                }

                                }

                        }

                        }
                        else
                        {
                            flag = 1;
                            MessageBox.Show("Pair is missing!","error");
                        }

                   
                }
                catch (Exception ex)
                {
                    flag = 1;
                    MessageBox.Show("Error: Could not read file from disk." ,ex.Message);
                }

                if (flag == 0)
                {
                    this.Hide();
                    SecondWindow secondWindow = new SecondWindow();
                   
                    secondWindow.getFilesList(finalFilesList, path);
                    secondWindow.getFilesList1(finalFilesList);
                    secondWindow.Show();

                }
            }

           
        }

        public static bool IsValidFileName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;

            string regExpr1 = "^[A-Za-z][\\d]{1,5}[.][\\d]{1,5}$";
            string regExpr2 = "^[\\d]{1,5}[.][\\d]{1,5}$";
            Regex RgxObject1 = new Regex(regExpr1);
            Regex RgxObject2 = new Regex(regExpr2);
            if (RgxObject1.IsMatch(name))
                return true;
            else if (RgxObject2.IsMatch(name))
                return true;
            else
                return false;




        }
    }
}
