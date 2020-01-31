using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;


namespace RoyMiz
{
    
    public partial class ThirdWindow : Window
    {
        #region Variable Declaration
        int c = 0;
        int initialSpace;
        string path;
        #endregion

        #region List Declaration
        List<string> tempFiles = new List<string>();
        List<string> tempFiles1 = new List<string>();
        List<string> altFileList = new List<string>();
        List<string> altFileList1 = new List<string>();
        List<string> fileName1 = new List<string>();
        List<string> fileName2 = new List<string>();
        List<string> Files = new List<string>();
        List<string> RepFiles = new List<string>();
        List<string> shuffelFiles1 = new List<string>();
        List<string> shuffelFiles2 = new List<string>();
        List<int> listInitSpace = new List<int>();
        List<int> listInitSpaceTemp = new List<int>();
        List<int> listRandom1 = new List<int>();
        List<int> listRandom2 = new List<int>();
        List<string> file1 = new List<string>();
        List<string> file2 = new List<string>();
        List<int> finalinitSpace = new List<int>();
        #endregion
        
        public ThirdWindow()
        {
            InitializeComponent();
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            #region Validations With Function Calls
            Regex nonNumericRegex = new Regex(@"\D$");
            if (txtRepetitions.Text == "" && txtInitialSpace.Text == "" && txtFinalSpace.Text == "")
            {
                MessageBox.Show("All fields must be necessary to fill", "Error");
            }
            else if (txtRepetitions.Text == "" || nonNumericRegex.IsMatch(txtRepetitions.Text))
            {
                MessageBox.Show("Number of repetition field Contains Number Only", "Error");
            }
            else if (txtInitialSpace.Text == "" || nonNumericRegex.IsMatch(txtInitialSpace.Text))
            {
                MessageBox.Show("Initial Space Field Contains Number Only", "Error");
            }
            else if (txtFinalSpace.Text == "" || nonNumericRegex.IsMatch(txtFinalSpace.Text))
            {
                MessageBox.Show("Final Space field Contains Number Only", "Error");
            }else if (System.Convert.ToInt32(txtFinalSpace.Text) > System.Convert.ToInt32(txtInitialSpace.Text))
            {
                MessageBox.Show("Final Space field Must be less then Initialspace Field", "Error");
            }else if (System.Convert.ToInt32(txtInitialSpace.Text)<1000)
            {
                MessageBox.Show("Initial Space Field Contains Minimum 1000 MiliSecond", "Error");
            }
            else if (System.Convert.ToInt32(txtRepetitions.Text)==0 )
            {
                MessageBox.Show("Number of repetition field should not be '0'", "Error");
            }else if (System.Convert.ToInt32(txtFinalSpace.Text) > 300000 && System.Convert.ToInt32(txtInitialSpace.Text)>300000)
            {
                MessageBox.Show("Initial Space Field and Final Space field Contains Maximum 3,00,000 MiliSecond", "Error");
            }
            else
            {

                int finalSpace = System.Convert.ToInt32(txtFinalSpace.Text);
                int initSpace = System.Convert.ToInt32(txtInitialSpace.Text);
                int repitation = System.Convert.ToInt32(txtRepetitions.Text);
                if (repitation > 1) 
                    initialSpace = ((initSpace - finalSpace) / (repitation - 1));
                else
                    initialSpace = ((initSpace - finalSpace) / (repitation));

                Files.Clear();
                Files.Clear();
                for (int i = 0; i < tempFiles.Count; i++)
                {
                    Files.Add(tempFiles[i]);
                    //Files.Add(tempFiles[i+1]);
                }

                for (int i = 0; i < (repitation * 2); i++)
                {
                    Files.Add(Files[i]);
                    RepFiles.Add(Files[i]);
                }

                for (int j = 0; j < repitation; j++)

                {
                    listInitSpace.Add(initSpace);
                    initSpace = initSpace - initialSpace;
                    
                }

                listInitSpaceTemp.AddRange(listInitSpace);

                Group();


                tempFiles.Remove(tempFiles[0]);
                tempFiles.Remove(tempFiles[0]);


            if (altFileList.Count <= 0)
            {
               
                randomList();
                this.Hide();
                ForthWindow forthWindow = new ForthWindow();
                forthWindow.filePath(path,file1,file2,finalinitSpace);
                forthWindow.Show();

            }
            else
            {
               
                getFilesList(altFileList,path);
                txtRepetitions.Text = null;
                txtInitialSpace.Text = null;
                txtFinalSpace.Text = null;

                }
            }
            #endregion
        }

        #region For Random List(Group wise Shuffle)
        public void randomList()
        {
          
            int f1Count = fileName1.Count();
            int f2Count = fileName2.Count();

            listRandom1=RandomNumberEven(0, f1Count);
            listRandom2 = RandomNumberEven(0, f2Count);

            for (int i=0;i< listRandom1.Count; i++)
            {
                int random = listRandom1.ElementAt(i);
                    shuffelFiles1.Add(fileName1[random]);
                    shuffelFiles1.Add(fileName1[random + 1]);
                    shuffelFiles1.Add(fileName1[random + 2]);
                  
            }

            for (int i = 0; i < listRandom2.Count; i++)
            {
                int random = listRandom2.ElementAt(i);
                shuffelFiles2.Add(fileName2[random]);
                shuffelFiles2.Add(fileName2[random + 1]);
                shuffelFiles2.Add(fileName2[random + 2]);
              
            }
          
          shuffelFiles2.AddRange(shuffelFiles1);
            
            for (int i=2;i < shuffelFiles2.Count; i = i + 3)
            {
                string fileElement = shuffelFiles2.ElementAt(i - 1);
                string initElement = shuffelFiles2.ElementAt(i );
                string temp;
                for (int j=i+3;j< shuffelFiles2.Count;j=j+3)
                {
                    if (fileElement == shuffelFiles2.ElementAt(j - 1))
                    {
                        if (Int32.Parse(shuffelFiles2.ElementAt(i)) <= Int32.Parse(shuffelFiles2.ElementAt(j)))
                        {
                            temp = shuffelFiles2.ElementAt(i);

                            shuffelFiles2.Insert(i, shuffelFiles2.ElementAt(j));
                            shuffelFiles2.RemoveAt(i+1);

                            shuffelFiles2.Insert(j, temp);
                            shuffelFiles2.RemoveAt(j+1);
                        }
                    }
                }
            }

            for(int i = 0; i < shuffelFiles2.Count;i=i+3)
            {
                file1.Add(shuffelFiles2.ElementAt(i));
                file2.Add(shuffelFiles2.ElementAt(i+1));
                finalinitSpace.Add(Int32.Parse(shuffelFiles2.ElementAt(i+2)));
               
            }

           
        }
        #endregion

        #region Logic of Random Numbers
        private List<int> RandomNumberEven(int min, int max)
        {   
            List<int> templist = new List<int>();
            int count = listInitSpaceTemp.Count;
            while ((max/3)-1 > templist.Count)
            {
                Random random = new Random();
                int ans = random.Next(min, max);
                if (ans % 3 == 0 && templist.Contains(ans)==false && ans!=0) templist.Add(ans);
              
            }
            return templist;

        }
        #endregion

        #region Getting List of files for heading
        public void getFilesList(List<string> fileName,string path)
        {
            this.path = path;
            if (fileName.Count != 0)
            {
                int filesCount = fileName.Count();
                lblFileName.Content = "For the pair " + fileName[0] + " and " + fileName[1];
                tempFiles.Add(fileName[0]);
                tempFiles.Add(fileName[1]);
                fileName.Remove(fileName[0]);
                fileName.Remove(fileName[0]);
                altFileList = fileName;
            }
        }
        #endregion

        #region getting Files List
        public void getFilesList1(List<string> fileName1)
        { 
            if (fileName1.Count != 0)
            {
                int filesCount = fileName1.Count();
                for (int i = 0; i < filesCount; i++)
                {
                    tempFiles1.Add(fileName1[i]);
                    altFileList1 = fileName1;
                }
            }
        }
        #endregion

        #region Grouping the files name
        public void Group()
        {
            
            string regExpr1 = "^[A-Za-z][\\d]{1,5}[.][\\d]{1,5}$";
            string regExpr2 = "^[\\d]{1,5}[.][\\d]{1,5}$";
            Regex RgxObject1 = new Regex(regExpr1);
            Regex RgxObject2 = new Regex(regExpr2);

            if (c == 0)
            {
                fileName1.Add(null);
                fileName1.Add(null);
                fileName1.Add(null);
                fileName2.Add(null);
                fileName2.Add(null);
                fileName2.Add(null);
                c = 1;
            }
            int cnt = 0;
            for (int i = 0; i < (listInitSpace.Count); i++)
            {
                if (RgxObject1.IsMatch(RepFiles[cnt]))
                {
                   
                    fileName1.Add(RepFiles[cnt]);
                    fileName1.Add(RepFiles[cnt + 1]);
                    fileName1.Add(listInitSpace[i].ToString());
                }
                else if (RgxObject2.IsMatch(RepFiles[cnt]))
                {
                    fileName2.Add(RepFiles[cnt]);
                    fileName2.Add(RepFiles[cnt + 1]);
                    fileName2.Add(listInitSpace[i].ToString());
                }
            }
            RepFiles.Clear();
            listInitSpace.Clear();
        }
        #endregion
    }
}
