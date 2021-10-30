using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Model
{
    [Table("Plans")]
    public class Plan : PlannerTask
    {
        public Plan() : base()
        {

        }
        public Plan(ObservableCollection<Plan> _collection) : base()
        {
            _collection.Add(this);
            this.Children = new ObservableCollection<Plan>();
        }
        public Plan(Plan _parent) : base()
        {
            this.ParentId = _parent.Id;
            _parent.Children.Add(this);
            this.Children = new ObservableCollection<Plan>();
        }

        public Plan(string _title) : base(_title)
        {
            Children = new ObservableCollection<Plan>();
        }

        [NotMapped]
        public new ObservableCollection<Plan> Children {
            get
            {
                if (children == null)
                    children = new ObservableCollection<Plan>();
                return children;
            }
            set
            {
                children = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Plan> children;
        private string target;
        private string methods;
        private string facilities;
        private Guid parentId;

        public string Target
        {
            get
            {
                if (target != null)
                    return target;
                else
                    return "purpose of the plan";
            }
            set
            {
                target = value;
                OnPropertyChanged();
            }
        }

        public string Methods
        {
            get
            {
                if (methods != null)
                    return methods;
                else
                    return "methods execution";
            }
            set
            {
                methods = value;
                OnPropertyChanged();
            }
        }

        public string Facilities
        {
            get
            {
                if (facilities != null)
                    return facilities;
                else
                    return "means for execition";
            }
            set
            {
                facilities = value;
                OnPropertyChanged();
            }
        }

        public Guid ParentId
        {
            get { return parentId; }
            set { parentId = value; }
        }

        public byte CommonStatus
        {
            get
            {
                if (Children.Count == 0)
                {
                    if (TaskStatus == State.Perfomed)
                    {
                        return 100;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return CommonStatus_Definition();
                }
            }
        }

        public new State TaskStatus
        {
            get
            {
                if (Children != null && Children.Count == 0)
                    return taskStatus;
                else
                    return State.Null;
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

        private byte CommonStatus_Definition()
        {
            float common = 0;
            float completed = 0;
            CommonStatus_Definition(this, ref common, ref completed);
            return Convert.ToByte(completed / (common / 100));
        }

        private void CommonStatus_Definition(Plan planner, ref float common, ref float completed)
        {
            foreach (Plan plan in planner.Children)
            {
                if (plan.Children.Count == 0)
                {
                    if (plan.TaskStatus == State.Perfomed)
                    {
                        completed++;
                    }
                    common++;
                }
                else
                {
                    CommonStatus_Definition(plan, ref common, ref completed);
                }
            }
        }



    }
}
