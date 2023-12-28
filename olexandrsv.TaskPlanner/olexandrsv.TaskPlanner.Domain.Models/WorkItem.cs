using olexandrsv.TaskPlanner.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace olexandrsv.TaskPlanner.Domain.Models
{
    public class WorkItem
    {
        public DateTime CreationDate { get; set; }
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }
        public Complexity Complexity { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public Guid Id { get; set; }
        public WorkItem()
        {
            Title = "laundry";
            Description = "";
            Priority = Priority.High;
            CreationDate = DateTime.Now;
            DueDate = CreationDate.AddDays(5);
            Id = Guid.NewGuid();
        }

        public WorkItem Clone()
        {
            return new WorkItem
            {
                CreationDate = CreationDate,
                DueDate = DueDate,
                Priority = Priority,
                Complexity = Complexity,
                Title = Title,
                Description = Description,
                IsCompleted = IsCompleted,
                Id = Guid.NewGuid(),
            };
        }

        public override string ToString()
        {
            string p = Priority.ToString().ToLower();
            string d = DueDate.ToString("dd.MM.yyyy");
            return $"{Id}: Do {Title} : due {d}, {p} priority";
        }
    }
}
