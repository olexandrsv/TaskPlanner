using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;
using olexandrsv.TaskPlanner.Domain.Models;

using olexandrsv.TaskPlanner.DataAccess.Abstractions;

namespace olexandrsv.TaskPlanner.DataAccess
{
    public class FileWorkItemsRepository: IWorkItemsRepository
    {
        private readonly Dictionary<Guid, WorkItem> workItems = new Dictionary<Guid, WorkItem>();
        private const string path = "file.json";
        public FileWorkItemsRepository()
        {
            if(File.Exists(path))
            {
                string json = File.ReadAllText(path);
                if(string.IsNullOrEmpty(json))
                {
                    return;
                }
                var list = JsonConvert.DeserializeObject<WorkItem[]>(json);
                foreach(var item in list)
                {
                    workItems[item.Id] = item;
                }
            }
        }

        public Guid Add(WorkItem workItem)
        {
            WorkItem item = workItem.Clone();
            Guid id = Guid.NewGuid();
            item.Id = id;
            workItems.Add(id, item);
            SaveChanges(); 
            return id;
        }

        public void SaveChanges()
        {
            WorkItem[] array = workItems.Values.ToArray();
            string res = JsonConvert.SerializeObject(array, Formatting.Indented);
            File.WriteAllText(path, res);
        }

        public WorkItem Get(Guid id)
        {
           if(workItems.ContainsKey(id)){
                return workItems[id];
           }
           return null;
        }

        public WorkItem[] GetAll()
        {
            return workItems.Values.ToArray();
        }

        public bool Update(WorkItem workItem)
        {
            if (workItems.ContainsKey(workItem.Id))
            {
                workItems[workItem.Id] = workItem;
                SaveChanges();
                return true;
            }
            return false;
        }

        public bool Remove(Guid id)
        {
            if (workItems.ContainsKey(id))
            {
                workItems.Remove(id);
                SaveChanges();
                return true;
            }
            return false;
        }

    }
}
