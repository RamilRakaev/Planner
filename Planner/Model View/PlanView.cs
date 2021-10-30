using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
    public partial class MainWindow : Window
    {
        #region Свойства
        ObservableCollection<Plan> tree = new ObservableCollection<Plan>();
        /// <summary>
        /// Дерево, содержащее планы
        /// </summary>
        ObservableCollection<Plan> Tree
        { 
            get 
            {
                if (tree == null)
                    tree = new ObservableCollection<Plan>();
                return tree;
            }
            set
            {
                tree = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Выделенный заголовок, используется чтобы 
        /// определить был ли изменён заголовок и стоит
        /// ли обращаться к базе данных для изменения
        /// </summary>
        string SelectedTitle;

        /// <summary>
        /// Режим редактирования
        /// </summary>
        bool Edit = false;

        static private Plan selectedPlanner;

        public Plan SelectedPlanner
        {
            get { return selectedPlanner; }
            set
            {
                selectedPlanner = value;
                OnPropertyChanged();
            }
        }
        #endregion

        private void Plan_TV_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (SelectedPlanner != null)
            {
                SelectedPlanner.Seeing_TBlock = Visibility.Visible;
                SelectedPlanner.Seeing_TBox = Visibility.Collapsed;
            }
            if (Plan_TV.SelectedItem != null)
            {
                SelectedPlanner = (Plan)Plan_TV.SelectedItem;
                SelectedTitle = selectedPlanner.Title;
            }
        }

        #region Сохранение изменений данных
        /// <summary>
        /// Перевод фокуса на другой элемент, чтобы изменения сохранились
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Target_TB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Shift && e.Key == Key.S)
            {
                SaveData(sender);
                Plan_TV.Focus();
            }
        }

        /// <summary>
        /// Перевод фокуса на другой элемент, чтобы изменения сохранились
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Deadlines_DP_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Shift && e.Key == Key.S)
            {
                SaveDeadlines(sender);
                Plan_TV.Focus();
            }
        }

        /// <summary>
        /// Сохранение изменений в конечном сроке плана в бд
        /// </summary>
        /// <param name="sender"></param>
        private void SaveDeadlines(object sender)
        {
            DatePicker obj = sender as DatePicker;
            using(PlannerContext db=new PlannerContext())
            {
                if (obj == Deadlines_DP)
                {
                    db.PlanDB.Find(SelectedPlanner.Id).EndDate = SelectedPlanner.EndDate;
                }
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Сохранение изменений данных в плане и в бд
        /// </summary>
        /// <param name="sender"></param>
        private void SaveData(object sender)
        {
            TextBox obj = sender as TextBox;
            using (PlannerContext db = new PlannerContext()) 
            {
                //Plan plan = db.PlanDB.Find(SelectedPlanner.Id);
                if (obj == TargetTB)
                {
                    //plan.Target = TargetTB.Text;
                    db.PlanDB.Find(SelectedPlanner.Id
                        ).Target = TargetTB.Text;
                }
                else if (obj == DescriptionTB)
                {
                    //plan.Description = DescriptionTB.Text;
                    db.PlanDB.Find(SelectedPlanner.Id
                        ).Description = DescriptionTB
                        .Text;
                    //Перезапись описания в списке задач
                    var task = to_do_list.SearchTask(SelectedPlanner.Id);
                    if (task != null)
                    {
                        task.Children[0].Title = DescriptionTB.Text;
                    }
                }
                else if (obj == MethodsTB)
                {
                    //plan.Methods = MethodsTB.Text;
                    db.PlanDB.Find(SelectedPlanner.Id).Methods = MethodsTB.Text;
                }
                else if (obj == FacilitiesTB)
                {
                    //plan.Facilities = FacilitiesTB.Text;
                    db.PlanDB.Find(SelectedPlanner.Id).Facilities = FacilitiesTB.Text;
                }
                db.SaveChanges();
            }
        }

        #endregion

        #region Изменение и сохранение заголовка в бд

        /// <summary>
        /// Открытие режима редактирования
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Plan_TV_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectedPlanner != null)
            {
                SelectedPlanner.Seeing_TBlock = Visibility.Collapsed;
                SelectedPlanner.Seeing_TBox = Visibility.Visible;
                Edit = true;
            }
        }

        /// <summary>
        /// Выход из режима редактирования
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Plan_TV_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (SelectedPlanner != null)
                {
                    //if (Edit)
                    //{
                    //    SelectedTitle = SelectedPlanner.Title;
                    //    SaveTitleDB();
                        SelectedPlanner.Seeing_TBlock = Visibility.Visible;
                        SelectedPlanner.Seeing_TBox = Visibility.Collapsed;
                        Edit = false;
                    
                }
            }
        }

        private void Plan_TV_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && SelectedPlanner!=null)
            {
                if(SelectedTitle!=SelectedPlanner.Title)
                SaveTitleDB();
            }
        }

        /// <summary>
        /// При нажатии левой кнопкой мыши выделение элемента отменяется,
        /// происходит выход из режима редактирования
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Plan_TV_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (SelectedPlanner != null)
            {
                if (Edit)
                {
                    SelectedPlanner.Seeing_TBlock = Visibility.Visible;
                    SelectedPlanner.Seeing_TBox = Visibility.Collapsed;
                    Edit = false;
                    if (SelectedTitle != SelectedPlanner.Title)
                    {
                        SelectedTitle = SelectedPlanner.Title;
                        SaveTitleDB();
                    }
                }
                else
                {
                    SelectedPlanner.IsSelected = false;
                    SelectedPlanner = null;
                    Planner_Zeroing();
                }
            }
        }

        /// <summary>
        /// Сохранение заголовка в бд
        /// </summary>
        private void SaveTitleDB()
        {
            using (PlannerContext db = new PlannerContext())
            {
                db.PlanDB.Find(SelectedPlanner.Id).Title = SelectedPlanner.Title;
                db.SaveChanges();
            }
            if (SelectedPlanner.Children.Count == 0)
                {
                var task= to_do_list.SearchTask(SelectedPlanner.Id);
                if(task!=null)
                { 
                    task.Title = SelectedPlanner.Title;
                }
                }
            
        }

        /// <summary>
        /// Выделение текста при наводке
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Plan_Title_MouseEnter(object sender, MouseEventArgs e)
        {
            TextBox t = sender as TextBox;
            t.Focus();
            t.SelectAll();
        }

        /// <summary>
        /// Очистка
        /// </summary>
        private void Planner_Zeroing()
        {
            TargetTB.Text = "";
            DescriptionTB.Text = "";
            MethodsTB.Text = "";
            FacilitiesTB.Text = "";
        }
        #endregion

        #region Добавление или удаление элемента
        /// <summary>
        /// Добавить новый план
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Plan_Click(object sender, RoutedEventArgs e)
        {
            Planner_Zeroing();
            Plan newPlan;
            if (SelectedPlanner != null)
            {
                newPlan = new Plan(SelectedPlanner);
                SaveAddPlan(newPlan);
                to_do_list.ChangeTasks(newPlan);
                SelectedPlanner.IsExpanded = true;
                SelectedPlanner.IsSelected = false;
                
            }
            else
            {
                newPlan = new Plan(Tree);
                SaveAddPlan(newPlan);
                to_do_list.Queue.Add(new PlannerTask(newPlan.Id, newPlan.Title,newPlan.Description));
            }
            Edit = true;

            SelectedPlanner = newPlan;
            SelectedPlanner.Seeing_TBlock = Visibility.Collapsed;
            SelectedPlanner.Seeing_TBox = Visibility.Visible;
            newPlan.IsSelected = true;
        }

        /// <summary>
        /// Сохранение нового плана в бд
        /// </summary>
        /// <param name="newPlan"></param>
        private void SaveAddPlan(Plan newPlan)
        {
            using(PlannerContext db=new PlannerContext())
            {
                db.PlanDB.Add(newPlan);
                db.SaveChanges();
            }
        }

        private void Remove_Plan_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPlanner != null)
            {
                to_do_list.RemoveTasks(SelectedPlanner);
                RemovePlanDB();
                SelectedPlanner.Children.Clear();
                if (SelectedPlanner.ParentId == Guid.Empty)
                {
                    Tree.Remove(SelectedPlanner);
                }
                else
                {
                    PlannerSorting.SearchParent(Tree, SelectedPlanner).Children.Remove(SelectedPlanner);
                }
            }
        }

        /// <summary>
        /// Удаление плана включая все его дочерние элементы
        /// </summary>
        /// <param name="children"></param>
        private void RemovePlanDB()
        {
            
                using (PlannerContext db = new PlannerContext())
                {
                    List<Guid> IdList = PlannerSorting.ReturnIdChildrenPlans(SelectedPlanner);
                    foreach (Guid Id in IdList)
                    {
                        db.PlanDB.Remove(db.PlanDB.Find(Id));
                    }
                    db.PlanDB.Remove(db.PlanDB.Find(SelectedPlanner.Id));
                    db.SaveChanges();
                }
            
        }
        #endregion
    }

}
