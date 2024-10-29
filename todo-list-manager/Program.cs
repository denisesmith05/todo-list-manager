using System;
using System.Collections.Generic;
using System.IO;

namespace ToDoListManager
{
    // Base class for tasks
    public class Task
    {
        public string Description { get; set; }
        public bool IsCompleted { get; set; }

        public Task(string description)
        {
            Description = description;
            IsCompleted = false;
        }

        public virtual void Display()
        {
            Console.WriteLine($"{(IsCompleted ? "[X]" : "[ ]")} {Description}");
        }
    }

    // Inherited class for special tasks (if needed)
    public class ImportantTask : Task
    {
        public DateTime DueDate { get; set; }

        public ImportantTask(string description, DateTime dueDate) : base(description)
        {
            DueDate = dueDate;
        }

        public override void Display()
        {
            Console.WriteLine($"{(IsCompleted ? "[X]" : "[ ]")} {Description} (Due: {DueDate.ToShortDateString()})");
        }
    }

    public class TaskManager
    {
        private List<Task> tasks;

        public TaskManager()
        {
            tasks = new List<Task>();
            LoadTasksFromFile();
        }

        public void AddTask(string description)
        {
            Task newTask = new Task(description);
            tasks.Add(newTask);
            SaveTasksToFile();
        }

        public void RemoveTask(int index)
        {
            if (index < 0 || index >= tasks.Count)
            {
                Console.WriteLine("Invalid task number.");
                return;
            }
            tasks.RemoveAt(index);
            SaveTasksToFile();
        }

        public void ViewTasks()
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks available.");
                return;
            }
            for (int i = 0; i < tasks.Count; i++)
            {
                Console.Write($"{i + 1}. ");
                tasks[i].Display();
            }
        }

        private void SaveTasksToFile()
        {
            using (StreamWriter writer = new StreamWriter("tasks.txt"))
            {
                foreach (var task in tasks)
                {
                    writer.WriteLine($"{task.Description}|{task.IsCompleted}");
                }
            }
        }

        private void LoadTasksFromFile()
        {
            if (!File.Exists("tasks.txt")) return;

            using (StreamReader reader = new StreamReader("tasks.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split('|');
                    var description = parts[0];
                    var isCompleted = bool.Parse(parts[1]);
                    Task task = new Task(description) { IsCompleted = isCompleted };
                    tasks.Add(task);
                }
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            TaskManager taskManager = new TaskManager();
            bool running = true;

            while (running)
            {
                Console.WriteLine("\nTo-Do List Manager");
                Console.WriteLine("1. Add Task");
                Console.WriteLine("2. Remove Task");
                Console.WriteLine("3. View Tasks");
                Console.WriteLine("4. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.Write("Enter task description: ");
                        string description = Console.ReadLine();
                        taskManager.AddTask(description);
                        break;
                    case "2":
                        taskManager.ViewTasks();
                        Console.Write("Enter task number to remove: ");
                        if (int.TryParse(Console.ReadLine(), out int taskNumber))
                        {
                            taskManager.RemoveTask(taskNumber - 1); // Convert to zero-based index
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a number.");
                        }
                        break;
                    case "3":
                        taskManager.ViewTasks();
                        break;
                    case "4":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
    }
}
