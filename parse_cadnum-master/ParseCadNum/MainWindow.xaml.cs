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
        List<string> LZu = new List<string>();
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
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();         
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
            try
            {
                doc.Load(FilePatch);

                foreach (XmlNode node in doc.DocumentElement)
                {

                    if (node.Name == "CadastralBlocks")
                    {
                        foreach (XmlNode n1 in node.ChildNodes)
                        {
                            if (n1.Name == "CadastralBlock")
                            {
                                foreach (XmlAttribute A in n1.Attributes)
                                {
                                    if (A.Name == "CadastralNumber")
                                    {
                                        FileName = Replace(A.Value, ":");
                                    }
                                }
                                foreach (XmlNode n2 in n1.ChildNodes)
                                {
                                    if (n2.Name == "Parcels")
                                    {
                                        int i = 0;
                                        foreach (XmlNode n3 in n2.ChildNodes)
                                        {
                                            if (n3.Name == "Parcel")
                                            {
                                                i += 1;

                                                foreach (XmlAttribute A1 in n3.Attributes)
                                                {
                                                    if (A1.Name == "CadastralNumber")
                                                    {
                                                        LZu.Add(A1.Value);
                                                        Count++;
                                                    }
                                                }
                                            }
                                        }
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
                                                                LOks.Add(A2.Value);
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
                                                        foreach (XmlAttribute A2 in n4.Attributes)
                                                        {
                                                            if (A2.Name == "CadastralNumber")
                                                            {
                                                                LFlat.Add(A2.Value);
                                                                Count++;
                                                            }
                                                        }
                                                    }
                                                    if (n4.Name == "Construction")                                                    {
                                                       
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
            catch
            {
                listBoxCreate.Items.Add("Файл "+ FilePatch + " имеет не верный формат.");
                LOks.Clear();
                LUn.Clear();
                LFlat.Clear();
                LCnst.Clear();
                LZu.Clear();
                Count = 0;
            }
            
        }

        private void ButtonChangeFile_Copy_Click(object sender, RoutedEventArgs e)
        {
          
            foreach (string name in listBoxChange.Items)
            {
                listBoxCreate.Items.Add("*** Результаты разбора файла : " + name + " в " + DateTime.Now +" ***");
                ParseFile(name);
                #region ZU
                if (LZu.Count != 0)
               {
                    int i = 0;
                    int j = 0;
                    int n = 1;
                    StreamWriter FZu = new StreamWriter(FileName + "_ЗУ_"+n+".txt");
                    listBoxCreate.Items.Add(FileName + "_ЗУ_"+n+".txt");
                    foreach (string ZU in LZu)
                    {
                        if (i == 50)
                        {
                            FZu.WriteLine(ZU);
                            if(CheckBoxVal.IsChecked == true)
                            {
                                FZu.Close();
                                n++;
                                FZu = new StreamWriter(FileName + "_ЗУ_" + n + ".txt");
                                listBoxCreate.Items.Add(FileName + "_ЗУ_" + n + ".txt");
                                
                            }
                            i = 0;
                        }
                        else
                        {
                            if (j == LZu.Count() - 1)
                            {
                                FZu.Write(ZU);
                            }
                            else
                            {
                                FZu.Write(ZU + ";");
                            }
                        }

                        i++;
                        j++;
                    }
                    FZu.Close();
                }
                #endregion ZU

                if (CheckBoxDel.IsChecked == true)
                {

                    if (LOks.Count != 0)
                    {
                        int i = 0;
                        int j = 0;
                        int n = 1;
                        StreamWriter FOks = new StreamWriter(FileName + "_ОКС_" + n + ".txt");
                        listBoxCreate.Items.Add(FileName + "_ОКС_" + n + ".txt");
                        foreach (string OKS in LOks)
                        {
                            if (i == 50)
                            {
                                FOks.WriteLine(OKS);
                                if (CheckBoxVal.IsChecked == true)
                                {
                                    FOks.Close();
                                    n++;
                                    FOks = new StreamWriter(FileName + "_ОКС_" + n + ".txt");
                                    listBoxCreate.Items.Add(FileName + "_ОКС_" + n + ".txt");

                                }
                                i = 0;
                            }
                            else
                            {
                                if (j == LOks.Count() - 1)
                                {
                                    FOks.Write(OKS);
                                }
                                else
                                {
                                    FOks.Write(OKS + ";");
                                }

                            }

                            i++;
                            j++;
                        }
                        FOks.Close();
                    }
                    if (LUn.Count != 0)
                    {
                        int i = 0;
                        int j = 0;
                        int n = 1;
                        StreamWriter Fun = new StreamWriter(FileName + "_НД_" + n + ".txt");
                        listBoxCreate.Items.Add(FileName + "_НД_" + n + ".txt");
                        foreach (string Un in LUn)
                        {
                            if (i == 50)
                            {
                                Fun.WriteLine(Un);
                                if (CheckBoxVal.IsChecked == true)
                                {
                                    Fun.Close();
                                    n++;
                                    Fun = new StreamWriter(FileName + "_НД_" + n + ".txt");
                                    listBoxCreate.Items.Add(FileName + "_НД_" + n + ".txt");

                                }
                                i = 0;
                            }
                            else
                            {
                                if (j == LUn.Count() - 1)
                                {
                                    Fun.Write(Un);
                                }
                                else
                                {
                                    Fun.Write(Un + ";");
                                }
                            }

                            i++;
                            j++;
                        }
                        Fun.Close();
                    }
                    if (LFlat.Count != 0)
                    {
                        int i = 0;
                        int j = 0;
                        int n = 1;
                        StreamWriter FFlat = new StreamWriter(FileName + "_КВ_" + n + ".txt");
                        listBoxCreate.Items.Add(FileName + "_КВ_" + n + ".txt");
                        foreach (string Flat in LFlat)
                        {
                            if (i == 50)
                            {
                                FFlat.WriteLine(Flat);
                                if (CheckBoxVal.IsChecked == true)
                                {
                                    FFlat.Close();
                                    n++;
                                    FFlat = new StreamWriter(FileName + "_КВ_" + n + ".txt");
                                    listBoxCreate.Items.Add(FileName + "_КВ_" + n + ".txt");

                                }
                                i = 0;
                            }
                            else
                            {
                                if (j == LFlat.Count() - 1)
                                {
                                    FFlat.Write(Flat);
                                }
                                else
                                {
                                    FFlat.Write(Flat + ";");
                                }
                            }

                            i++;
                            j++;
                        }
                        FFlat.Close();
                    }
                    if (LCnst.Count != 0)
                    {
                        int i = 0;
                        int j = 0;
                        int n = 1;
                        StreamWriter FCnstr = new StreamWriter(FileName + "_СР_" + n + ".txt");
                        listBoxCreate.Items.Add(FileName + "_СР_" + n + ".txt");
                        foreach (string Cnstr in LCnst)
                        {
                            if (i == 50)
                            {
                                FCnstr.WriteLine(Cnstr);
                                if (CheckBoxVal.IsChecked == true)
                                {
                                    FCnstr.Close();
                                    n++;
                                    FCnstr = new StreamWriter(FileName + "_СР_" + n + ".txt");
                                    listBoxCreate.Items.Add(FileName + "_СР_" + n + ".txt");

                                }
                                i = 0;
                            }
                            else
                            {
                                if (j == LCnst.Count() - 1)
                                {
                                    FCnstr.Write(Cnstr);
                                }
                                else
                                {
                                    FCnstr.Write(Cnstr + ";");
                                }
                            }

                            i++;
                            j++;
                        }
                        FCnstr.Close();
                    }
                }
                else
                {
                    int i = 0;
                    int j = 0;
                    int n = 1;
                    StreamWriter FOks = new StreamWriter(FileName + "_ОКС_" + n + ".txt");
                    listBoxCreate.Items.Add(FileName + "_ОКС_" + n + ".txt");
                    foreach (string OKS in LOks)
                    {
                        if (i == 50)
                        {
                            FOks.WriteLine(OKS);
                            if (CheckBoxVal.IsChecked == true)
                            {
                                FOks.Close();
                                n++;
                                FOks = new StreamWriter(FileName + "_ОКС_" + n + ".txt");
                                listBoxCreate.Items.Add(FileName + "_ОКС_" + n + ".txt");

                            }
                            i = 0;
                        }
                        else
                        {
                            if (j == LOks.Count() - 1)
                            {
                                FOks.Write(OKS);
                            }
                            else
                            {
                                FOks.Write(OKS + ";");
                            }

                        }

                        i++;
                        j++;
                    }
                    foreach (string Un in LUn)
                    {
                        if (i == 50)
                        {
                            FOks.WriteLine(Un);
                            if (CheckBoxVal.IsChecked == true)
                            {
                                FOks.Close();
                                n++;
                                FOks = new StreamWriter(FileName + "_ОКС_" + n + ".txt");
                                listBoxCreate.Items.Add(FileName + "_ОКС_" + n + ".txt");

                            }
                            i = 0;
                        }
                        else
                        {
                            if (j == LUn.Count() - 1)
                            {
                                FOks.Write(Un);
                            }
                            else
                            {
                                FOks.Write(Un + ";");
                            }
                        }

                        i++;
                        j++;
                    }
                    foreach (string Flat in LFlat)
                    {
                        if (i == 50)
                        {
                            FOks.WriteLine(Flat);
                            if (CheckBoxVal.IsChecked == true)
                            {
                                FOks.Close();
                                n++;
                                FOks = new StreamWriter(FileName + "_ОКС_" + n + ".txt");
                                listBoxCreate.Items.Add(FileName + "_ОКС_" + n + ".txt");

                            }
                            i = 0;
                        }
                        else
                        {
                            if (j == LFlat.Count() - 1)
                            {
                                FOks.Write(Flat);
                            }
                            else
                            {
                                FOks.Write(Flat + ";");
                            }
                        }

                        i++;
                        j++;
                    }
                    foreach (string Cnstr in LCnst)
                    {
                        if (i == 50)
                        {
                            FOks.WriteLine(Cnstr);
                            if (CheckBoxVal.IsChecked == true)
                            {
                                FOks.Close();
                                n++;
                                FOks = new StreamWriter(FileName + "_ОКС_" + n + ".txt");
                                listBoxCreate.Items.Add(FileName + "_ОКС_" + n + ".txt");

                            }
                            i = 0;
                        }
                        else
                        {
                            if (j == LCnst.Count() - 1)
                            {
                                FOks.Write(Cnstr);
                            }
                            else
                            {
                                FOks.Write(Cnstr + ";");
                            }
                        }

                        i++;
                        j++;
                    }

                    FOks.Close();

                }

               


                LOks.Clear();
                LUn.Clear();
                LFlat.Clear();
                LCnst.Clear();
                LZu.Clear();
            }
            CountCad.Visibility = Visibility.Visible;
            CountCad.Content = "Получено кадастровых номеров: " + Count;
            Count = 0;
        }
        private static string Replace(string input, IEnumerable<char> except)
        {
            return new string(input.Where(c => !except.Contains(c)).ToArray());
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            listBoxChange.Items.Clear();
        }
    }
}
