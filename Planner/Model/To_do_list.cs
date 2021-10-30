using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Planner.Model
{
    class To_do_list
    {
        public To_do_list()
        {
            Queue = new ObservableCollection<PlannerTask>();
            Waiting = new ObservableCollection<PlannerTask>();
            InProgress = new ObservableCollection<PlannerTask>();
            Perfomed = new ObservableCollection<PlannerTask>();
        }

        public To_do_list(ObservableCollection<Plan> tree)
        {
            Queue = new ObservableCollection<PlannerTask>();
            Waiting = new ObservableCollection<PlannerTask>();
            InProgress = new ObservableCollection<PlannerTask>();
            Perfomed = new ObservableCollection<PlannerTask>();
            Filter_tree(tree);
            Distribution();
        }

        public To_do_list(IEnumerable<PlannerTask> Queue)
        {
            Waiting = new ObservableCollection<PlannerTask>();
            InProgress = new ObservableCollection<PlannerTask>();
            Perfomed = new ObservableCollection<PlannerTask>();
            Distribution();
        }

        public ObservableCollection<PlannerTask> Queue { get; set; }

        public ObservableCollection<PlannerTask> Waiting { get; set; }

        public ObservableCollection<PlannerTask> InProgress { get; set; }

        public ObservableCollection<PlannerTask> Perfomed { get; set; }


        /// <summary>
        /// Breaks the plan down into small subtasks
        /// </summary>
        /// <param name="tree"></param>
        private void Filter_tree(ObservableCollection<Plan> tree)
        {
            foreach (Plan item in tree)
            {
                if (item.Children.Count == 0)
                {
                    PlannerTask exe = new PlannerTask(item.Id, item.Title, item.Description);
                    Queue.Add(exe);
                }
                else
                {
                    Filter_tree(item.Children);
                }
            }
        }

        /// <summary>
        /// Распределение задач по коллекциям
        /// </summary>
        private void Distribution()
        {
            foreach (PlannerTask task in Queue)
            {
                if (task.TaskStatus == State.Waiting)
                {
                    Waiting.Add(task);
                }
                if (task.TaskStatus == State.InProgress)
                {
                    InProgress.Add(task);
                }
                if (task.TaskStatus == State.Perfomed)
                {
                    Perfomed.Add(task);
                }
            }
        }

        /// <summary>
        /// Добавление новой задачи
        /// </summary>
        /// <param name="newPlan"></param>
        public void ChangeTasks(Plan newPlan)
        {
            if (newPlan.Children.Count == 0)
            {
                SearchTask(newPlan.ParentId, true);
                PlannerTask newTask =
                    new PlannerTask(newPlan.Id,
                    newPlan.Title,
                    newPlan.Description);
                Queue.Add(newTask);
            }
        }

        /// <summary>
        /// Поиск задачи по идентификатору, и удаление из очереди
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="_delete">Удалять или нет</param>
        /// <returns></returns>
        public PlannerTask SearchTask(Guid _id, bool _delete = false)
        {
            PlannerTask output = null;
            foreach (PlannerTask task in this.Queue)
            {
                if (task.Id == _id)
                    output = task;

            }

            if (_delete)
            {
                if (output != null)
                {
                    Queue.Remove(output);
                    Waiting.Remove(output);
                    InProgress.Remove(output);
                    Perfomed.Remove(output);
                }
            }
            return output;
        }

        public void RemoveTasks(Plan plan)
        {
            foreach (Plan item in plan.Children)
            {
                if (item.Children.Count == 0)
                {
                    SearchTask(item.Id, true);
                }
                else
                {
                    RemoveTasks(item);
                }
            }
        }
    }
}
