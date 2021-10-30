using Planner.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Planner
{
    public partial class MainWindow : Window
    {
        #region Свойства
        static ObservableCollection<Diary> DiaryRecords = new ObservableCollection<Diary>();

        static private Diary selectedRecord;
        public Diary SelectedRecord
        {
            get { return selectedRecord; }
            set
            {
                selectedRecord = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Выделенный заголовок
        /// </summary>
        string DiarySelectedTitle;

        bool DiaryEdit = false;
        #endregion

        #region Выделение
        private void TableTV_SelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedRecord = (Diary)Table_TV.SelectedItem;
            if (SelectedRecord != null) 
            {
                Diary_Zeroing(SelectedRecord.Description);
            }
        }

        private void Records_RTB_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (SelectedRecord != null)
            {
                string f = new TextRange(Records_RTB.Document.ContentStart, Records_RTB.Document.ContentEnd).Text;
                SelectedRecord.Description = f;
            }
        }
        #endregion

        #region Сохранение изменений записей в дневнике

        private void Diary_Zeroing(string _description)
        {
            Records_RTB.Document.Blocks.Clear();
            Records_RTB.Document.Blocks.Add(new Paragraph(new Run(_description)));
        }

        /// <summary>
        /// Сохранение записи в дневнике
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Records_RTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Shift && e.Key == Key.S)
            {
                using (PlannerContext db = new PlannerContext())
                {
                    db.DiaryDB.Find(SelectedRecord.Id).Description = SelectedRecord.Description;
                }
            }
        }
        #endregion

        #region Изменение и сохранение заголовка в бд

        private void Table_LV_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectedRecord != null)
            {
                SelectedRecord.Seeing_TBlock = Visibility.Collapsed;
                SelectedRecord.Seeing_TBox = Visibility.Visible;
            }
        }

        private void Table_TV_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (SelectedRecord != null)
                {
                    SelectedRecord.Seeing_TBlock = Visibility.Visible;
                    SelectedRecord.Seeing_TBox = Visibility.Collapsed;
                    DiaryEdit = false;
                }
            }
        }

        private void Table_LV_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && SelectedPlanner != null)
            {
                if (DiarySelectedTitle != SelectedPlanner.Title)
                    DiarySaveTitleDB();
            }
        }

        private void DiarySaveTitleDB()
        {
            if (DiaryEdit)
            {
                using (PlannerContext db = new PlannerContext())
                {
                    db.DiaryDB.Find(SelectedRecord.Id).Title = SelectedRecord.Title;
                    db.SaveChanges();
                }
                
                DiaryEdit = false;
            }
        }

        private void Table_LV_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (SelectedRecord != null)
            {
                if (DiaryEdit)
                {
                    SelectedRecord.Seeing_TBlock = Visibility.Visible;
                    SelectedRecord.Seeing_TBox = Visibility.Collapsed;
                    DiaryEdit = false;
                    if (DiarySelectedTitle != SelectedRecord.Title) 
                    {
                        DiarySaveTitleDB();
                        DiarySelectedTitle = SelectedRecord.Title;
                    }
                }
                else
                {
                    SelectedRecord.IsSelected = false;
                    SelectedRecord = null;
                    Records_RTB.Document.Blocks.Clear();
                }
                
            }
        }

        #endregion

        #region Добавление и удаление записей в дневнике
        private void AddRecord_But_Click(object sender, RoutedEventArgs e)
        {
            Diary newRecord = new Diary();
            SaveAddRecord(newRecord);
            DiaryRecords.Add(newRecord);
            SelectedRecord = newRecord;
            SelectedRecord.Seeing_TBlock = Visibility.Collapsed;
            SelectedRecord.Seeing_TBox = Visibility.Visible;
            SelectedRecord.IsSelected = true;
        }

        private void SaveAddRecord(Diary newPlan)
        {
            using (PlannerContext db = new PlannerContext())
            {
                db.DiaryDB.Add(newPlan);
                db.SaveChanges();
            }
        }

        private void RemoveRecord_But_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedRecord != null)
            {
                RemoveRecordDB();
                Records_RTB.Document.Blocks.Clear();
                DiaryRecords.Remove(SelectedRecord);
                SelectedRecord = null;
            }
        }

        private void RemoveRecordDB()
        { 
            using(PlannerContext db=new PlannerContext())
            {
                db.DiaryDB.Remove(db.DiaryDB.Find(SelectedRecord.Id));
                db.SaveChanges();
            }
        }
        #endregion
    }
}
