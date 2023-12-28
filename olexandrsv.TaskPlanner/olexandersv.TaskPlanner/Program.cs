using olexandrsv.TaskPlanner.Domain.Logic;
using olexandrsv.TaskPlanner.Domain.Models;
using olexandrsv.TaskPlanner.Domain.Models.Enums;
using System;
using System.Collections;

using olexandrsv.TaskPlanner.DataAccess;
using olexandrsv.TaskPlanner.DataAccess.Abstractions;
using System.Numerics;
internal static class Program
{
    private static IWorkItemsRepository repo = new FileWorkItemsRepository();
    public static void Main(string[] args)
    {
        Console.WriteLine("[A]dd work item;\r\n[B]uild a plan;\r\n[M]ark work item as completed;\r\n[R]emove a work item;\r\n[Q]uit the app.");
        bool finish = false;
        while (!finish)
        {
            Console.Write("Input command: ");
            string command = Console.ReadLine();
            switch (command)
            {
                case "A":
                    {
                        WorkItem workItem = ReadWorkItem();
                        repo.Add(workItem);
                        break;
                    }
                case "B":
                    {
                        SimpleTaskPlanner taskPlanner = new SimpleTaskPlanner(repo);
                        var plan = taskPlanner.CreatePlan();
                        foreach (var item in plan)
                        {
                            Console.WriteLine(item);
                        }
                        break;
                    }
                case "M":
                    {
                        if (Guid.TryParse(Console.ReadLine(), out Guid id))
                        {
                            var item = repo.Get(id);
                            item.IsCompleted = true;
                            var ok = repo.Update(item);
                            if (ok)
                            {
                                Console.WriteLine("ok");
                            }
                            else
                            {
                                Console.WriteLine("error");
                            }
                        }
                        break;
                    }
                case "R":
                    {
                        if (Guid.TryParse(Console.ReadLine(), out Guid id))
                        {
                            var ok = repo.Remove(id);
                            if (ok)
                            {
                                Console.WriteLine("ok");
                            }
                            else
                            {
                                Console.WriteLine("error");
                            }
                        }
                        break;
                    }
                case "P":
                    {
                        var items = repo.GetAll();
                        foreach (var i in items)
                        {
                            Console.WriteLine(i);
                        }
                        break;
                    }
                case "Q":
                    {
                        finish = true;
                        break;
                    }
            }
        }
    }

    private static WorkItem ReadWorkItem()
    {
        WorkItem workItem = new WorkItem();

        Console.Write("CreationDate: ");
        bool ok = DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime creationDate);
        if (!ok) { return workItem; }

        Console.Write("Due Date: ");
        ok = DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dueDate);
        if (!ok) { return workItem; }

        Console.Write("Priority: ");
        ok = Enum.TryParse<Priority>(Console.ReadLine(), ignoreCase: true, out Priority priority);
        if (!ok) { return workItem; }

        Console.Write("Complexity: ");
        ok = Enum.TryParse<Complexity>(Console.ReadLine(), ignoreCase: true, out Complexity complexity);
        if (!ok) { return workItem; }

        Console.Write("Title: ");
        string title = Console.ReadLine();

        Console.Write("Description: ");
        string description = Console.ReadLine();

        Console.Write("IsCompleted: ");
        ok = bool.TryParse(Console.ReadLine(), out bool isCompleted);
        if (!ok) { return workItem; }

        workItem.CreationDate = creationDate;
        workItem.DueDate = dueDate;
        workItem.Priority = priority;
        workItem.Complexity = complexity;
        workItem.Title = title;
        workItem.Description = description; 
        workItem.IsCompleted = isCompleted;

        return workItem;
    }
}
