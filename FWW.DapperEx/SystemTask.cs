using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWW.Framework.DapperEx
{
    /// <summary>
    /// Class SystemTask.
    /// </summary>
    public class SystemTask : IComparable
    {
        /// <summary>
        /// The task
        /// </summary>
        public object Task;
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the sort order.
        /// </summary>
        /// <value>
        /// The sort order.
        /// </value>
        public int SortOrder { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemTask"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public SystemTask(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings:
        /// Value
        /// Meaning
        /// Less than zero
        /// This instance is less than <paramref name="obj" />.
        /// Zero
        /// This instance is equal to <paramref name="obj" />.
        /// Greater than zero
        /// This instance is greater than <paramref name="obj" />.
        /// </returns>
        public int CompareTo(object obj)
        {
            var sort = obj as SystemTask;
            if (SortOrder > sort.SortOrder)
            {
                return 1;
            }
            else if (SortOrder == sort.SortOrder)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SystemTasks<T> : List<SystemTask>
    {
        private Dictionary<string, T> m_tasks;
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemTasks{T}"/> class.
        /// </summary>
        public SystemTasks()
        {
            m_tasks = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);
        }
        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="load">The load.</param>
        /// <param name="sort">The sort.</param>
        public void Add(string key, T load, int sort)
        {
            if (m_tasks.ContainsKey(key) == false)
            {
                SystemTask task = new SystemTask(key);
                task.Task = load;
                task.SortOrder = sort;
                m_tasks.Add(key, load);
                base.Add(task);
            }
        }
    }
}
