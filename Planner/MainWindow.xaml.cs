using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Planner.Model;

namespace Planner
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public static readonly DependencyProperty PhoneProperty;

        public MainWindow()
        {
            
            InitializeComponent();
            AssignmentValues();
        }

        private void AssignmentValues()
        {
            using(PlannerContext db=new PlannerContext())
            {
                Tree=PlannerSorting.ConvertToObservableC(db.PlanDB);
                Tree=PlannerSorting.SortingPlans(Tree);
                Plan_TV.ItemsSource = Tree;

                to_do_list = new To_do_list(Tree);
                Queue_TV.ItemsSource = to_do_list.Queue;
                InProgress_TV.ItemsSource = to_do_list.InProgress;
                Waiting_TV.ItemsSource = to_do_list.Waiting;
                Perfomed_TV.ItemsSource = to_do_list.Perfomed;

                DiaryRecords = PlannerSorting.ConvertToObservableC(db.DiaryDB);
                Table_TV.ItemsSource = DiaryRecords;
            }
        }

        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            if (tg_but.IsChecked == true)
            {
                tt_planner.Visibility = Visibility.Collapsed;
                tt_to_do_list.Visibility = Visibility.Collapsed;
                tt_Ideas_Notes.Visibility = Visibility.Collapsed;
                tt_diary.Visibility = Visibility.Collapsed;
                tt_statistics.Visibility = Visibility.Collapsed;
            }
            else
            {
                tt_planner.Visibility = Visibility.Visible;
                tt_to_do_list.Visibility = Visibility.Visible;
                tt_Ideas_Notes.Visibility = Visibility.Visible;
                tt_diary.Visibility = Visibility.Visible;
                tt_statistics.Visibility = Visibility.Visible;
            }
        }

        private void Tg_but_Unchecked(object sender, RoutedEventArgs e)
        {
            img_bg.Opacity = 1;
        }

        private void Tg_but_Checked(object sender, RoutedEventArgs e)
        {
            img_bg.Opacity = 0.3;
        }

        private void BG_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            tg_but.IsChecked = false;
        }

        private void Close_But_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Выделенная на данный момент сетка
        /// </summary>
        Grid Dedicated_Grid;

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Dedicated_Grid != null)
            {
                Dedicated_Grid.Visibility = Visibility.Collapsed;
            }

            StackPanel button = (StackPanel)sender;
            if (button == Plan_SP)
            {
                Dedicated_Grid = Planner_Grid;
            }
            else if (button == To_do_list_SP)
            {
                Dedicated_Grid = To_do_list_Grid;
            }
            else if (button == Diary_SP)
            {
                Dedicated_Grid = Diary_Grid;
            }
            else if (button == Ideas_Notes_SP)
            {
                Dedicated_Grid = Ideas_Notes_Grid;
            }
            else if (button == Statistics_SP)
            {
                Dedicated_Grid = Statistics_Grid;
            }

            Dedicated_Grid.Visibility = Visibility.Visible;

        }

        private void Menu_MoueDown(object sender, MouseButtonEventArgs e)
        {
            if (Dedicated_Grid != null)
            {
                Dedicated_Grid.Visibility = Visibility.Collapsed;
            }
            ListViewItem button = (ListViewItem)sender;
            if (button == Planner_LVI)
            {
                Dedicated_Grid = Planner_Grid;
            }
            else if (button == To_do_list_LVI)
            {
                Dedicated_Grid = To_do_list_Grid;
            }

            Dedicated_Grid.Visibility = Visibility.Visible;

        }

    }
}
