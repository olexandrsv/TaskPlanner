using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using olexandrsv.TaskPlanner.DataAccess.Abstractions;
using olexandrsv.TaskPlanner.Domain.Models;

namespace olexandrsv.TaskPlanner.Domain.Logic
{
    public class SimpleTaskPlanner
    {
        private IWorkItemsRepository repo;
        public SimpleTaskPlanner(IWorkItemsRepository repo)
        {
            this.repo = repo;
        }


        public WorkItem[] CreatePlan()
        {
            WorkItem[] items = repo.GetAll();
            var itemsAsList = items.ToList();
            itemsAsList = itemsAsList.Where(item => !item.IsCompleted).ToList();
            itemsAsList.Sort(CompareWorkItems);
            return itemsAsList.ToArray();
        }

        private static int CompareWorkItems(WorkItem firstItem, WorkItem secondItem)
        {
            int p = secondItem.Priority.CompareTo(firstItem.Priority);
            if (p != 0) {
                return p;
            }
            int d = firstItem.DueDate.CompareTo(secondItem.DueDate);
            if (d != 0) {
                return d;
            }
            return string.Compare(firstItem.Title, secondItem.Title, StringComparison.OrdinalIgnoreCase);
        }
    }
}
