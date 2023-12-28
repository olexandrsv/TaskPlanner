using olexandrsv.TaskPlanner.DataAccess.Abstractions;
using olexandrsv.TaskPlanner.Domain.Models;
using Moq;

namespace olexandrsv.TaskPlanner.Domain.Logic.Tests
{
    public class SimpleTaskPlannerTests
    {
        [Fact]
        public void TestCreatePlan()
        {
            var mockRepo = new Mock<IWorkItemsRepository>();
            var item1 = new WorkItem
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(1),
                Title = "Football",
                Priority = Models.Enums.Priority.High,
                Complexity = Models.Enums.Complexity.Hours,
                IsCompleted = false,
            };
            var item2 = new WorkItem
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(3),
                Title = "Books",
                Priority = Models.Enums.Priority.Low,
                Complexity = Models.Enums.Complexity.Days,
                IsCompleted = true,
            };
            var item3 = new WorkItem
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(7),
                Title = "Lessons",
                Priority = Models.Enums.Priority.Urgent,
                Complexity = Models.Enums.Complexity.Minutes,
                IsCompleted = false,
            };
            WorkItem[] items = { item1, item2, item3 };

            mockRepo.Setup(repo => repo.GetAll()).Returns(items);

            var planner = new SimpleTaskPlanner(mockRepo.Object);
            var plan = planner.CreatePlan();

            Assert.Equal(plan[0].Id, item3.Id);
            Assert.Equal(plan[1].Id, item1.Id);

            Assert.Equal(plan.Length, 2);

            foreach (var item in plan)
            {
                Assert.True(items.Contains(item) && !item.IsCompleted);
            }
        }
    }
}