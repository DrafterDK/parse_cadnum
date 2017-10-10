using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Xml;

namespace ParseCadNum
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> Parcel = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonChangeFile_Click(object sender, RoutedEventArgs e)
        {

            //var dialog = new Microsoft.Win32.OpenFileDialog();
            //dialog.ShowDialog();
            //if (dialog.ShowDialog() == DialogResult.Cancel)
            //{
            //    listBoxChange.Items.Add(dialog.FileName);
            //}
            //return null;


            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
           // dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "XML documents (.XML)|*.xml"; // Filter files by extension
            dlg.Multiselect = true;

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string[] filename = dlg.FileNames;
                foreach (string u in filename)
                {
                     listBoxChange.Items.Add(u);
                }
             
            }
            
        }
        private void ParseFile (string FilePatch)
        {
            string FileName = "test";
            var doc = new XmlDocument();
            {
                doc.Load(FilePatch);
               
               // listBoxCreate.Items.Add(doc.);
                 foreach (XmlNode node in doc.DocumentElement)
                 {
                    
                    if (node.Name == "CadastralBlocks")
                    {
                        foreach( XmlNode n1 in node.ChildNodes)
                        {
                           
                            
                              //  sw.WriteLine("Hello");
                             //   sw.WriteLine("And");
                             //   sw.WriteLine("Welcome");
                            
                            if (n1.Name == "CadastralBlock")
                            {
                                foreach (XmlAttribute A in n1.Attributes)
                                {
                                    if (A.Name == "CadastralNumber")
                                    {
                                        FileName = Replace(A.Value, ":");                                       
                                    }
                                }
                              //  FileInfo fi1 = new FileInfo(FileName + ".txt");
                                StreamWriter ZU = new StreamWriter(FileName + "_ЗУ.txt");
                                StreamWriter OKS = new StreamWriter(FileName + "_ОКС.txt");
                                StreamWriter UN = new StreamWriter(FileName + "_НД.txt");

                                foreach (XmlNode n2 in n1.ChildNodes)
                                {
                                    if (n2.Name == "Parcels")
                                    {  int i = 0;
                                        foreach (XmlNode n3 in n2.ChildNodes)
                                        {
                                            if (n3.Name == "Parcel")
                                            {
                                               i += 1;

                                                foreach (XmlAttribute A1 in n3.Attributes)
                                                {
                                                    if (A1.Name == "CadastralNumber")
                                                    {
                                                       
                                                        //if (i <= 50)
                                                        //{
                                                            if (i == 50)
                                                            {
                                                                listBoxCreate.Items.Add(A1.Value);
                                                                ZU.WriteLine(A1.Value);
                                                            i = 0;
                                                        }
                                                            else
                                                            {
                                                                listBoxCreate.Items.Add(A1.Value);
                                                                ZU.Write(A1.Value + ";");
                                                            }
                                                        //}
                                                        //else
                                                        //{
                                                        //    listBoxCreate.Items.Add(A1.Value);
                                                        //    ZU.WriteLine(A1.Value + ";");
                                                        //    i = 0;
                                                        //}
                                                        listBoxCreate.Items.Add(A1.Value);
                                                        Parcel.Add(A1.Value);
                                                    }
                                                }
                                            }
                                        }
                                    ZU.Close();
                                    }
                                    

                                    if (n2.Name == "ObjectsRealty")
                                    {
                                        foreach (XmlNode n3 in n2.ChildNodes)
                                        {
                                            if (n3.Name == "ObjectRealty")
                                            {
                                                foreach (XmlNode n4 in n3.ChildNodes)
                                                {
                                                    if (n4.Name == "Building")
                                                    {
                                                        listBoxCreate.Items.Add("Building");
                                                        foreach (XmlAttribute A2 in n4.Attributes)
                                                        {
                                                            if (A2.Name == "CadastralNumber")
                                                            {
                                                                listBoxCreate.Items.Add(A2.Value);
                                                                OKS.Write(A2.Value + ";");
                                                            }
                                                        }
                                                    }

                                                    if (n4.Name == "Uncompleted")
                                                    {
                                                        listBoxCreate.Items.Add("Uncompleted");
                                                        foreach (XmlAttribute A2 in n4.Attributes)
                                                        {
                                                            if (A2.Name == "CadastralNumber")
                                                            {
                                                                listBoxCreate.Items.Add(A2.Value);
                                                                UN.Write(A2.Value +";");
                                                            }
                                                        }
                                                    }


                                                }








                                            }
                                        }
                                    }

                                        
                                    
                                }
                                
                            }
                        }

                    }
                    //foreach (XmlNode child in node.ChildNodes)
                    //{

                    //}
                    //Debug.WriteLine(string.Format("{0} = {1}", child.Name, child.InnerText));
                    //Debug.WriteLine("--------------");

                }
            }
        }

        private void ButtonChangeFile_Copy_Click(object sender, RoutedEventArgs e)
        {
            foreach (string name in listBoxChange.Items)
            {
                ParseFile(name);
               // for
            }
        }
        private static string Replace(string input, IEnumerable<char> except)
        {
            return new string(input.Where(c => !except.Contains(c)).ToArray());
        }
    }
}
