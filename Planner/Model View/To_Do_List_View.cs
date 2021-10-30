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
using Planner.Model;
namespace Planner
{

    public partial class MainWindow : Window
    {
        /// <summary>
        /// Объект, извлекающий в при иницилизации список подзадач из планов и распределяющий их
        /// </summary>
        To_do_list to_do_list;

        static private PlannerTask selectedTask;

        public PlannerTask SelectedTask
        {
            get { return selectedTask; }
            set
            {
                selectedTask = value;
                OnPropertyChanged();
            }
        }



        private void Add_Executable_But_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTask != null)
            {
                Check();
                using (PlannerContext db = new PlannerContext())
                {
                    db.PlanDB.Find(SelectedTask.Id).TaskStatus = State.InProgress;
                }
                PlannerSorting.SearchElement(Tree, SelectedTask.Id).TaskStatus = State.InProgress;
                    to_do_list.InProgress.Add(SelectedTask);
            }
        }

        private void Add_Waiting_But_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTask != null)
            {
                Check();
                using (PlannerContext db = new PlannerContext())
                {
                    db.PlanDB.Find(SelectedTask.Id).TaskStatus = State.Waiting;
                }
                PlannerSorting.SearchElement(Tree, SelectedTask.Id).TaskStatus = State.Waiting;
                to_do_list.Waiting.Add(SelectedTask);
            }
        }

        private void Add_Perfomed_But_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTask != null)
            {
                Check();
                using (PlannerContext db = new PlannerContext())
                {
                    db.PlanDB.Find(SelectedTask.Id).TaskStatus = State.Perfomed;
                }
                PlannerSorting.SearchElement(Tree, SelectedTask.Id).TaskStatus = State.Perfomed;
                to_do_list.Perfomed.Add(SelectedTask);
            }
        }

        /// <summary>
        /// Удалить из всех списков кроме очереди
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Remove_Task_But_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTask != null)
            {
                Check();
            }
        }

        /// <summary>
        /// Проверяет входит ли задача, в другие списки (не считая очередь)
        /// и удаляет из них
        /// </summary>
        private void Check()
        {
            if (to_do_list.Waiting.Contains(SelectedTask))
            {
                to_do_list.Waiting.Remove(SelectedTask);
                CleanStatus();
            }

            if (to_do_list.InProgress.Contains(SelectedTask))
            {
                to_do_list.InProgress.Remove(SelectedTask);
                CleanStatus();
            }

            if (to_do_list.Perfomed.Contains(SelectedTask))
            {
                to_do_list.Perfomed.Remove(SelectedTask);
                CleanStatus();
            }
            
        }

        /// <summary>
        /// Изменение статуса плана
        /// </summary>
        private void CleanStatus()
        {
            PlannerSorting.SearchElement(Tree, SelectedTask.Id).TaskStatus = State.Queue;
            using (PlannerContext db = new PlannerContext())
            {
                db.PlanDB.Find(SelectedTask.Id).TaskStatus = State.Queue;
            }
        }

        private void Queue_TV_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedItemChanged(Queue_TV);
        }

        private void Pending_TV_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedItemChanged(Waiting_TV);
        }

        private void Executable_TV_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedItemChanged(InProgress_TV);
        }

        /// <summary>
        /// Изменение выделенного элемента
        /// </summary>
        /// <param name="tree"></param>
        private void SelectedItemChanged(TreeView tree)
        {
            if (SelectedTask != null)
            {
                SelectedTask.IsSelected = false;
                SelectedTask.IsExpanded = false;
            }

            if (tree.SelectedItem != null)
            { 
                SelectedTask = (PlannerTask)tree.SelectedItem;
                SelectedTask.IsSelected = true;
            }
        }

        //private void Executable_TV_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    if (SelectedTask != null)
        //    {
        //        SelectedTask.Seeing_TBlock = Visibility.Collapsed;
        //        SelectedTask.Seeing_TBox = Visibility.Visible;
        //        SelectedTask.IsExpanded = false;
        //    }
        //}

        private void InProgress_TV_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (SelectedTask != null)
            {
                SelectedTask.IsSelected = false;
            }
            SelectedTask = null;
        }
    }
}
