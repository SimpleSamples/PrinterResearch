using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Xml;
using System.Xml.Serialization;

namespace PrinterResearch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // ObservableCollection<printersPrinter> PrinterList = new ObservableCollection<printersPrinter>();
        printers plist = new printers();

        public MainWindow()
        {
            InitializeComponent();
            //ColumnMake.Binding = new Binding("Make");
            //ColumnModel.Binding = new Binding("Model");
            //ColumnYield.Binding = new Binding("Yield");
            //ColumnInkPrice.Binding = new Binding("InkCost");
            //ColumnListPrice.Binding = new Binding("ListPrice");
            //ColumnOtherPrice.Binding = new Binding("OtherPrice");
            //ColumnWidth.Binding = new Binding("Width");
            //ColumnDepth.Binding = new Binding("Depth");
            //ColumnHeight.Binding = new Binding("Height");
            //ColumnScreen.Binding = new Binding("TouchScreen");
            //ColumnVolume.Binding = new Binding("Volume");
            //ColumnNotes.Binding = new Binding("Notes");
        }

        private void MenuOpen_Click(object sender, RoutedEventArgs e)
        {
            StatusMessage.Content = string.Empty;
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.FileName = Properties.Settings.Default.LastFilenameSetting;
            ofd.DefaultExt = ".xml";
            ofd.Filter = "xml Files|*.xml";
            if (ofd.ShowDialog(this) == false)
                return;
            Properties.Settings.Default.LastFilenameSetting = ofd.FileName;
            Properties.Settings.Default.Save();
            //
            plist = new printers();
            XmlSerializer serializer = new XmlSerializer(typeof(printers));
            serializer.UnknownNode += Serializer_UnknownNode;
            serializer.UnknownAttribute += Serializer_UnknownAttribute;
            // A FileStream is needed to read the XML document.  
            FileStream fs = new FileStream(ofd.FileName, FileMode.Open);
            // Uses the Deserialize method to restore the object's state   
            // with data from the XML document. */  
            plist = (printers)serializer.Deserialize(fs);
            fs.Dispose();
            MainGrid.ItemsSource = plist.printer;
            StatusMessage.Content = "Done";
        }

        private void Serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            System.Xml.XmlAttribute attr = e.Attr;
            System.Diagnostics.Debug.WriteLine("Unknown attribute " + attr.Name + "='" + attr.Value + "'");
        }

        private void Serializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Unknown Node:" + e.Name + "\t" + e.Text);
        }

        private void MenuSave_Click(object sender, RoutedEventArgs e)
        {
            StatusMessage.Content = string.Empty;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(printers));
                FileStream fs = new FileStream(Properties.Settings.Default.LastFilenameSetting, FileMode.Open);
                XmlWriter writer = new XmlTextWriter(fs, Encoding.Unicode);
                // Serialize using the XmlTextWriter.
                serializer.Serialize(writer, plist);
                writer.Close();
            }
            catch (Exception ex)
            {
                StatusMessage.Content = "Save error: " + ex.Message;
                return;
            }
            StatusMessage.Content = "Saved";
        }

        private void MenuDebug_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
