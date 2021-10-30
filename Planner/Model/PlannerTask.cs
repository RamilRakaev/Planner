using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Planner.Model
{
    [Table("PlannerTask")]
    public class PlannerTask : Diary
    {
        public PlannerTask() : base()
        {

        }

        public PlannerTask(string _title) : base(_title)
        {

        }

        /// <summary>
        /// Main construct
        /// </summary>
        /// <param name="_title"></param>
        /// <param name="_children"></param>
        public PlannerTask(Guid _id, string _title, string _children) : base(_title)
        {
            Id = _id;
            Title = _title;
            Children = new ObservableCollection<PlannerTask>() { new PlannerTask(_children) };
        }

        [NotMapped]
        public ObservableCollection<PlannerTask> Children { get; set; }

        protected State taskStatus;
        private DateTime endDate;
        private bool isExpanded;
        private Difficult taskDifficult;

        [Column("Expanded,", TypeName = "bit")]
        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                if (value != isExpanded)
                {
                    isExpanded = value;
                    OnPropertyChanged();
                }
            }
        }

        public State TaskStatus
        {
            get
            {
                return taskStatus;
            }
            set
            {
                if (value != taskStatus)
                {
                    taskStatus = value;
                    OnPropertyChanged();
                }
            }
        }

        [Column("EndDate", TypeName = "date")]
        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                OnPropertyChanged();
            }
        }

        public Difficult TaskDifficult
        {
            get { return taskDifficult; }
            set
            {
                taskDifficult = value;
                OnPropertyChanged();
            }
        }

    }

    public enum State { Queue, Waiting, InProgress, Perfomed, Null }
    public enum Difficult { VeryEasy, Easy, Average, Difficult, VeryDifficult }

}
