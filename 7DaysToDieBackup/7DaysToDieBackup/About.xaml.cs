using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _7DaysToDieBackup
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
            AddAppInformation();
        }

        private void AddAppInformation()
        {
            StringBuilder sb = new StringBuilder();
            var info = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
            sb.AppendLine(info.ProductName);
            sb.AppendLine("Version: " + info.ProductVersion);
            sb.AppendLine(info.LegalCopyright);
            sb.AppendLine(info.CompanyName);
            textBlock.Text = sb.ToString();
        }
    }
}
