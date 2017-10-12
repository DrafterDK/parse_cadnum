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
        List<string> LOks = new List<string>();
        List<string> LUn = new List<string>();
        List<string> LFlat = new List<string>();
        List<string> LCnst = new List<string>();
        string FileName = "test";
        int Count;

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
                                listBoxCreate.Items.Add(FileName + "_ЗУ.txt");

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
                                                        if (i == 50)
                                                        {                                                          
                                                            ZU.WriteLine(A1.Value);
                                                            i = 0;
                                                        }
                                                        else
                                                        {                                                          
                                                            ZU.Write(A1.Value + ";");
                                                        }
                                                        Count++;                                                       
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
                                                        foreach (XmlAttribute A2 in n4.Attributes)
                                                        {
                                                            if (A2.Name == "CadastralNumber")
                                                            {
                                                                LOks.Add(A2.Value );
                                                                Count++;
                                                            }
                                                        }
                                                    }

                                                    if (n4.Name == "Uncompleted")
                                                    {                                                       
                                                        foreach (XmlAttribute A2 in n4.Attributes)
                                                        {
                                                            if (A2.Name == "CadastralNumber")
                                                            {
                                                                LUn.Add(A2.Value);
                                                                Count++;
                                                            }
                                                        }
                                                    }
                                                    if (n4.Name == "Flat")
                                                    {
                                                        listBoxCreate.Items.Add("Flat");
                                                        foreach (XmlAttribute A2 in n4.Attributes)
                                                        {
                                                            if (A2.Name == "CadastralNumber")
                                                            {
                                                                LFlat.Add(A2.Value);
                                                                Count++;
                                                            }
                                                        }
                                                    }
                                                    if (n4.Name == "Construction")
                                                    {
                                                        listBoxCreate.Items.Add("Construction");
                                                        foreach (XmlAttribute A2 in n4.Attributes)
                                                        {
                                                            if (A2.Name == "CadastralNumber")
                                                            {
                                                                LCnst.Add(A2.Value);
                                                                Count++;
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
                }
            }
        }

        private void ButtonChangeFile_Copy_Click(object sender, RoutedEventArgs e)
        {
            foreach (string name in listBoxChange.Items)
            {
               ParseFile(name);
               if (LOks.Count != 0)
                {
                    int i = 0;
                     StreamWriter FOks = new StreamWriter(FileName + "_ОКС.txt");
                    listBoxCreate.Items.Add(FileName + "_ОКС.txt");
                    foreach (string OKS in LOks)
                    {
                        if (i == 50)
                        {
                            FOks.WriteLine(OKS);
                            i = 0;
                        }
                        else
                        {
                            FOks.Write(OKS + ";");
                        }

                        i++;
                    }
                    FOks.Close();
                }
                if (LUn.Count != 0)
                {
                    int i = 0;
                    StreamWriter Fun = new StreamWriter(FileName + "_НД.txt");
                    listBoxCreate.Items.Add(FileName + "_НД.txt");
                    foreach (string Un in LUn)
                    {
                        if (i == 50)
                        {
                            Fun.WriteLine(Un);
                            i = 0;
                        }
                        else
                        {
                            Fun.Write(Un + ";");
                        }

                        i++;
                    }
                    Fun.Close();                    
                }
                if (LFlat.Count != 0)
                {
                    int i = 0;
                    StreamWriter FFlat = new StreamWriter(FileName + "_КВ.txt");
                    listBoxCreate.Items.Add(FileName + "_КВ.txt");
                    foreach (string Flat in LFlat)
                    {
                        if (i == 50)
                        {
                            FFlat.WriteLine(Flat);
                            i = 0;
                        }
                        else
                        {
                            FFlat.Write(Flat + ";");
                        }

                        i++;
                    }
                    FFlat.Close();
                }
                if (LCnst.Count != 0)
                {
                    int i = 0;
                    StreamWriter FCnstr = new StreamWriter(FileName + "_СР.txt");
                    listBoxCreate.Items.Add(FileName + "_СР.txt");
                    foreach (string Cnstr in LCnst)
                    {
                        if (i == 50)
                        {
                            FCnstr.WriteLine(Cnstr);
                            i = 0;
                        }
                        else
                        {
                            FCnstr.Write(Cnstr + ";");
                        }

                        i++;
                    }
                    FCnstr.Close();
                }

                LOks.Clear();
                LUn.Clear();
                LFlat.Clear();
                LCnst.Clear();
            }
            CountCad.Visibility = Visibility.Visible;
            CountCad.Content = "Получено кадастровых номеров: " + Count;
        }
        private static string Replace(string input, IEnumerable<char> except)
        {
            return new string(input.Where(c => !except.Contains(c)).ToArray());
        }
    }
}
