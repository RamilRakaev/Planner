using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Planner.Model
{
    public class Diary : INotifyPropertyChanged
    {
        public Diary()
        {

        }

        public Diary(string _title)
        {
            //Id = ++Count;
            Title = _title;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        private string title;
        private string description;
        private DateTime startDate;
        private bool isSelected = false;
        private Visibility? seeing_TBlock;
        private Visibility? seeing_TBox = Visibility.Collapsed;

        public string Title
        {
            get
            {
                if (title != null)
                    return title;
                else
                    return "new plan";
            }
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get
            {
                if (description != null)
                    return description;
                else
                    return "description of the plan";
            }
            set
            {
                description = value;
                OnPropertyChanged();
            }
        }

        [Column("StartDate", TypeName = "date")]
        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        [Column("IsSelected", TypeName = "bit")]
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (value != isSelected)
                {
                    isSelected = value;
                    OnPropertyChanged();
                }
            }
        }
        [NotMapped]
        public Visibility? Seeing_TBox
        {
            get
            {
                return seeing_TBox;
            }
            set
            {
                seeing_TBox = value;
                OnPropertyChanged();
            }
        }

        [NotMapped]
        public Visibility? Seeing_TBlock
        {
            get
            {
                return seeing_TBlock;
            }
            set
            {
                seeing_TBlock = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
