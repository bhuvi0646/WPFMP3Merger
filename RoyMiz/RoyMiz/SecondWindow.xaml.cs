using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;


namespace RoyMiz
{

    public partial class SecondWindow : Window
    {
        List<string> templist = new List<string>();
        List<string> templist1 = new List<string>();
        string path;
        public SecondWindow()
        {
            


            InitializeComponent();
        }
        
        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            ThirdWindow thirdWindow = new ThirdWindow();
            thirdWindow.getFilesList1(templist1);
            thirdWindow.getFilesList(templist,path);
            
            
            thirdWindow.Show();

        }

        public void getFilesList(List<string> fileName,string path)
            {
            this.path = path;
            fileName.Sort();
            templist = fileName;
            //templist1 = fileName;
            int filesCount = fileName.Count();
            for (int loop = 0; loop < filesCount; loop++)
            {
                
                ListAudio.Items.Add(fileName[loop]);
            }
            
        }


        public void getFilesList1(List<string> fileName1)
        {

            fileName1.Sort();
            //templist = fileName;
            templist1 = fileName1;
            
        }

    }
}
