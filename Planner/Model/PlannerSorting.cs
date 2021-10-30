using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Model
{
    public class PlannerSorting
    {

        public static ObservableCollection<Plan> ConvertToObservableC(IEnumerable<Plan> orig)
        {
            return new ObservableCollection<Plan>(orig.Cast<Plan>());
        }

        public static ObservableCollection<PlannerTask> ConvertToObservableC(IEnumerable<PlannerTask> orig)
        {
            return new ObservableCollection<PlannerTask>(orig.Cast<PlannerTask>());
        }

        public static ObservableCollection<Diary> ConvertToObservableC(IEnumerable<Diary> orig)
        {
            ObservableCollection<Diary>diaries= new ObservableCollection<Diary>(orig.Cast<Diary>());
            FilterDiaryDB(diaries);
            return diaries;
        }
        
        /// <summary>
        /// Отсеивает все типы кроме Diary
        /// </summary>
        /// <param name="_diary_col"></param>
        public static void FilterDiaryDB(ObservableCollection<Diary>_diary_col)
        {
            int count = _diary_col.Count;
            for (int j=0; j < _diary_col.Count; j++)
            {
                if ((_diary_col[j] is Plan)| (_diary_col[j] is PlannerTask))
                {
                    _diary_col.Remove(_diary_col[j]);
                }
            }
        }
        public static Plan SearchParent(ObservableCollection<Plan> tree, Plan plan)
        {
            foreach (Plan planner in tree)
            {
                if (planner.Id == plan.ParentId)
                {
                    return planner;
                }
            }
            return new Plan();
        }

        /// <summary>
        /// Найти объект в коллекции по названию
        /// </summary>
        /// <param name="_tree"></param>
        /// <param name="_id"></param>
        /// <returns></returns>
        public static Plan SearchElement(ObservableCollection<Plan> _tree, Guid _id)
        {
            Plan element = null;
            foreach (Plan plan in _tree)
            {
                if (plan.Children.Count == 0)
                {
                    if (plan.Id == _id)
                    {
                        return plan;
                    }
                }
                else
                {
                    element = SearchElement(plan.Children, _id);
                    if (element != null)
                        return element;
                }
            }
            return element;
        }
        //err
        public static ObservableCollection<Plan> SortingPlans(ObservableCollection<Plan> _plans)
        {
            ChildrenOC(_plans);
            ObservableCollection<Plan> newList = new ObservableCollection<Plan>();
            int ind = 0;
            while (ind < _plans.Count)
            {
                int child_ind = 0;
                while (child_ind < _plans.Count)
                {
                    if (_plans[child_ind].ParentId == _plans[ind].Id)
                    {
                        _plans[ind].Children.Add(_plans[child_ind]);
                    }
                    child_ind++;
                }
                ind++;
            }
            foreach (Plan tree in _plans)
            {
                if (tree.ParentId == Guid.Empty)
                    newList.Add(tree);
            }
            return newList;
        }

        /// <summary>
        /// Присваивание всем полям Children новых экземпляров
        /// </summary>
        /// <param name="_tree"></param>
        private static void ChildrenOC(ObservableCollection<Plan> _tree)
        {
            foreach (Plan plan in _tree)
            {
                plan.Children = new ObservableCollection<Plan>();
            }
        }
        /// <summary>
        /// Возвращает список идентификаторов всех планов
        /// </summary>
        /// <param name="_plan"></param>
        /// <returns></returns>
        public static List<Guid> ReturnIdChildrenPlans(Plan _plan)
        {
            List<Guid> ID = new List<Guid>();
            foreach (Plan child in _plan.Children)
            {
                if (child.Children.Count > 0)
                {
                    ID.AddRange(ReturnIdChildrenPlans(child));
                }
                ID.Add(child.Id);
            }
            return ID;
        }


    }
}
