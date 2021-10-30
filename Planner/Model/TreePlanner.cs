using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
namespace Planner.Model
{
    //public class TreePlanner:Plan
    //{

        
    //    #region 
    //    /// <summary>
    //    /// Create a new tree element with a parent
    //    /// </summary>
    //    /// <param name = "_title" ></ param >
    //    /// < param name="_parent"></param>
    //    /// <param name = "_target" ></ param >
    //    /// < param name="_description"></param>
    //    /// <param name = "_methods" ></ param >
    //    /// < param name="_facilities"></param>
    //    /// <param name = "_endDate" ></ param >
    //    /// < param name="_startDate"></param>
    //    public TreePlanner(
    //        string _title,
    //        TreePlanner _parent,
    //        string _target = "",
    //        string _description = "",
    //        string _methods = "",
    //        string _facilities = "",
    //        DateTime _endDate = new DateTime(),
    //        DateTime _startDate = new DateTime())
    //    {
    //        this.Id = ++Count;
    //        this.Title = _title;
    //        this.Target = _target;
    //        this.Description = _description;
    //        this.Methods = _methods;
    //        this.Facilities = _facilities;
    //        this.Children = new ObservableCollection<TreePlanner>();

    //        if (_parent != null)
    //        {
    //            this.ParentId = _parent.Id;
    //            _parent.Children.Add(this);
    //        }
    //        this.StartDate = _startDate;
    //        this.EndDate = _endDate;

    //    }

    //    /// <summary>
    //    /// Create a new tree element without a parent
    //    /// </summary>
    //    /// <param name="_title"></param>
    //    /// <param name="_collection"></param>
    //    /// <param name="_target"></param>
    //    /// <param name="_description"></param>
    //    /// <param name="_methods"></param>
    //    /// <param name="_facilities"></param>
    //    /// <param name="_endDate"></param>
    //    /// <param name="_startDate"></param>
    //    public TreePlanner(
    //        string _title,
    //        ObservableCollection<TreePlanner> _collection,
    //        string _target = "",
    //        string _description = "",
    //        string _methods = "",
    //        string _facilities = "",
    //        DateTime _endDate = new DateTime(),
    //        DateTime _startDate = new DateTime())
    //    {
    //        this.Id = ++Count;
    //        this.Title = _title;
    //        this.Target = _target;
    //        this.Description = _description;
    //        this.Methods = _methods;
    //        this.Facilities = _facilities;
    //        this.Children = new ObservableCollection<TreePlanner>();

    //        if (_collection != null)
    //        {
    //            this.ParentId = 0;
    //            _collection.Add(this);
    //        }
    //        this.StartDate = _startDate;
    //        this.EndDate = _endDate;
    //    }

    //    /// <summary>
    //    /// Create item on load from database
    //    /// </summary>
    //    /// <param name="_id"></param>
    //    /// <param name="_parentId"></param>
    //    /// <param name="_title"></param>
    //    /// <param name="_target"></param>
    //    /// <param name="_description"></param>
    //    /// <param name="_methods"></param>
    //    /// <param name="_facilities"></param>
    //    public TreePlanner(
    //        int _id,
    //        int _parentId,
    //        string _title,
    //        string _target,
    //        string _description,
    //        string _methods,
    //        string _facilities)
    //    {
    //        Count++;
    //        this.Id = _id;
    //        this.ParentId = _parentId;
    //        this.Title = _title;
    //        this.Target = _target;
    //        this.Description = _description;
    //        this.Methods = _methods;
    //        this.Facilities = _facilities;
    //        this.Children = new ObservableCollection<TreePlanner>();
    //    }
    //    #endregion

    //    #region Fields
    //    public new ObservableCollection<TreePlanner> Children;
    //    #endregion




        
        
        
    //}
}
