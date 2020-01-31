using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;

using NAudio.Wave;
using System.Windows.Forms;
namespace RoyMiz
{
    public partial class ForthWindow : Window
    {
        private MediaPlayer mediaPlayer = new MediaPlayer();
        string outputDir = Path.GetFullPath("TempOutput");
       static int count = 0;
        string path;
        List<string> file1name = new List<string>();
        List<string> file2name = new List<string>();
        List<int> initSpace = new List<int>();
        public ForthWindow()
        {
            InitializeComponent();
            
        }

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Close();
            PlayButton.Visibility = Visibility.Visible;
            StopButton.Visibility = Visibility.Hidden;

            string finalmp3 = finalFile();
            SaveFileDialog savepdf = new SaveFileDialog();
            savepdf.DefaultExt = ".mp3"; // Default file extension
            savepdf.Filter = "MP3 File (.mp3)|*.mp3";

            if (savepdf.ShowDialog() ==  System.Windows.Forms.DialogResult.OK)
            {
                string newDirectory = savepdf.FileName;
                if (!File.Exists(newDirectory))
                {
                    File.Copy(finalmp3, newDirectory);

                    System.Windows.MessageBox.Show("Download Completed");
                }
                else
                {
                    System.Windows.MessageBox.Show("A File with same name Exists ");
                }
            }

        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            //mediaPlayer.Close();
            string finalmp3 = finalFile();
            mediaPlayer.Open(new Uri(finalmp3));
            mediaPlayer.Play();

            StopButton.Visibility = Visibility.Visible;
            PlayButton.Visibility = Visibility.Hidden;


        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Close();
            PlayButton.Visibility = Visibility.Visible;
            StopButton.Visibility = Visibility.Hidden;
           
        }

        #region Create Final File
        public string finalFile()
        {
            List<string> mp3files = new List<string>();
            List<string> files1 = new List<string>();
            List<string> files2 = new List<string>();
          
            string filePath =path;
            string extension = ".mp3";
            string file1 = "";
            string file2 = "";
          
            for (int i=0;i<file1name.Count;i++)
            {
                file1 = filePath + file1name.ElementAt(i) + extension;
                file2 = filePath + file2name.ElementAt(i) + extension;
                files1.Add(file1);
                files2.Add(file2);
                


            }
            string finalmp3 = CreateMashup(files1, files2, initSpace);

            return finalmp3;


        }
        #endregion

        #region Join All Mp3
        public static string CreateMashup(List<string> files1, List<string> files2, List<int> initSpace)
        {
            
            try
            {
                List<string> allFiles =new List<string>();
                int pairCount = files1.Count();
                var path = Path.Combine(Path.GetFullPath("TempOutput"), "tempfile" + ".mp3");
                string permanentSpace = Path.Combine(Path.GetFullPath("Silent"), "per_silent" + ".mp3");
               // string initSpaceFile = Path.Combine(Path.GetFullPath("Silent"), "temp_silent" + ".mp3");



                for (int i = 0; i < pairCount; i++)
                    {
                        string blankfile=  blankMp3File(initSpace.ElementAt(i));    
                        allFiles.Add(files1.ElementAt(i));
                        allFiles.Add(blankfile);
                        allFiles.Add(files2.ElementAt(i));
                        allFiles.Add(permanentSpace);
                    }

           
                using (var w = new BinaryWriter(File.Create(path)))
                {
                    new List<string>(allFiles).ForEach(f => w.Write(File.ReadAllBytes(f)));
                }
                for (int i = 0; i <= allFiles.Count; i++)
                {
                    if(File.Exists(Path.Combine(Path.GetFullPath("Silent"), "temp_silent" + i + ".mp3")))
                    File.Delete(Path.Combine(Path.GetFullPath("Silent"), "temp_silent" + i + ".mp3"));
                }
                return path;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Craete Initial Space Blank Mp3 Files
        public static string blankMp3File(int initSpace)
        {
            count += 1;
            var mp3Path = Path.Combine(Path.GetFullPath("Silent"), "5_silent" + ".mp3");


            var outputPath = Path.Combine(Path.GetFullPath("Silent"), "temp_silent"+count + ".mp3");

            TrimMp3(mp3Path, outputPath, TimeSpan.FromMilliseconds(0), TimeSpan.FromMilliseconds(initSpace));
            return outputPath;
        }
       

        static void TrimMp3(string inputPath, string outputPath, TimeSpan? begin, TimeSpan? end)
        {
            if (begin.HasValue && end.HasValue && begin > end)
                throw new ArgumentOutOfRangeException("end", "end should be greater than begin");

            using (var reader = new Mp3FileReader(inputPath))
            using (var writer = File.Create(outputPath))
            {
                Mp3Frame frame;
                while ((frame = reader.ReadNextFrame()) != null)
                    if (reader.CurrentTime >= begin || !begin.HasValue)
                    {
                        if (reader.CurrentTime <= end || !end.HasValue)
                            writer.Write(frame.RawData, 0, frame.RawData.Length);
                        else break;
                    }
            }

        }

        #endregion

       public void filePath(string path,List<string> files1,List<string> files2,List<int> initSpace)
        {
            this.path = path;
            this.file1name = files1;
            this.file2name = files2;
            this.initSpace = initSpace;
        }
        
    }
}
