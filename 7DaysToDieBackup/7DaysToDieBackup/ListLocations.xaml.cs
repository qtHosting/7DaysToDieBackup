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
using System.Windows.Shapes;
using _7DaysToDieBackup.DAL;
using _7DaysToDieBackup.Models;

namespace _7DaysToDieBackup
{
    /// <summary>
    /// Interaction logic for ListLocations.xaml
    /// </summary>
    public partial class ListLocations : Window
    {
        public ListLocations()
        {
            InitializeComponent();
            GetLocations();
        }

        private void GetLocations()
        {
            List<Games> Locations = new List<Games>();
            Database db = new Database();
            Locations = db.GetAllLocations();
            foreach(var game in Locations)
            {
                ContextMenu rightClick = new ContextMenu();
                MenuItem item = new MenuItem();
                TreeViewItem parent = new TreeViewItem();
                TreeViewItem child = new TreeViewItem();

                parent.Header = game.strGameLocation;
                child.Header = game.strBackupLocation;
                parent.Items.Add(child);
                item.Header = "Remove";
                item.Click += delegate { DeleteNode(parent, child); };
                rightClick.Items.Add(item);
                parent.ContextMenu = rightClick;
                gamesTreeView.Items.Add(parent);
            }
        }

        private void DeleteNode(TreeViewItem parent, TreeViewItem child)
        {
            Games game = new Games();
            game.strGameLocation = parent.Header.ToString();
            game.strBackupLocation = child.Header.ToString();
            Database db = new Database();
            db.Delete(game);
            gamesTreeView.Items.Remove(parent);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach(TreeViewItem parent in gamesTreeView.Items)
            {
                foreach(MenuItem menu in parent.ContextMenu.Items)
                {
                    foreach(TreeViewItem child in parent.Items)
                    {
                        menu.Click -= delegate { DeleteNode(parent, child);  };
                    }
                }
            }
        }
    }
}
