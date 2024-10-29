using System;
using System.Collections.Generic;
using System.IO;

namespace ToDoListManager
{
    // Base class for general tasks
    public class Task
    {
        // Properties for task description and completion status
        public string Description { get; set; }
        public bool IsCompleted { get; set; }

        // Constructor to initialize task description and default to not completed
        public Task(string description)
        {
            Description = description;
            IsCompleted = false;
        }

        // Method to display the task, showing [X] if completed or [ ] if not
        public virtual void Display()
        {
            Console.WriteLine($"{(IsCompleted ? "[X]" : "[ ]")} {Description}");
        }
    }

    // Derived class for important tasks with a due date
    public class ImportantTask : Task
    {
        // Additional property for due date
        public DateTime DueDate { get; set; }

        // Constructor to initialize both description and due date
        public ImportantTask(string description, DateTime dueDate) : base(description)
        {
            DueDate = dueDate;
        }

        // Override the Display method to include the due date in the output
        public override void Display()
        {
            Console.WriteLine($"{(IsCompleted ? "[X]" : "[ ]")} {Description} (Due: {DueDate.ToShortDateString()})");
        }
    }

    // Class to manage the list of tasks and handle operations
    public class TaskManager
    {
        // List to store tasks
        private List<Task> tasks;

        // Constructor initializes the task list and loads tasks from file
        public TaskManager()
        {
            tasks = new List<Task>();
            LoadTasksFromFile();
        }

        // Method to add a new task to the list and save to file
        public void AddTask(string description)
        {
            Task newTask = new Task(description);
            tasks.Add(newTask);
            SaveTasksToFile();
        }

        // Method to remove a task by its index (user-provided, zero-based)
        public void RemoveTask(int index)
        {
            // Check if the index is valid
            if (index < 0 || index >= tasks.Count)
            {
                Console.WriteLine("Invalid task number.");
                return;
            }
            tasks.RemoveAt(index);
            SaveTasksToFile();
        }

        // Method to mark a task as completed
        public void CompleteTask(int index)
        {
            // Check if the index is valid
            if (index < 0 || index >= tasks.Count)
            {
                Console.WriteLine("Invalid task number.");
                return;
            }
            // Set the task's IsCompleted property to true and save to file
            tasks[index].IsCompleted = true;
            SaveTasksToFile();
            Console.WriteLine("Task marked as completed.");
        }

        // Method to view all tasks in the list with their current status
        public void ViewTasks()
        {
            // Check if there are any tasks to display
            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks available.");
                return;
            }
            // Display each task with its index
            for (int i = 0; i < tasks.Count; i++)
            {
                Console.Write($"{i + 1}. ");
                tasks[i].Display();
            }
        }

        // Method to save all tasks to a file ("tasks.txt") for persistence
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

        // Method to load tasks from file ("tasks.txt") if it exists
        private void LoadTasksFromFile()
        {
            if (!File.Exists("tasks.txt")) return;

            using (StreamReader reader = new StreamReader("tasks.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Split each line by "|" to separate the description and completion status
                    var parts = line.Split('|');
                    var description = parts[0];
                    var isCompleted = bool.Parse(parts[1]);
                    // Create a new task with the loaded data and add it to the list
                    Task task = new Task(description) { IsCompleted = isCompleted };
                    tasks.Add(task);
                }
            }
        }
    }

    class Program
    {
        // Entry point of the program
        static void Main(string[] args)
        {
            // Initialize the task manager and set the program to running
            TaskManager taskManager = new TaskManager();
            bool running = true;

            while (running)
            {
                // Display menu options to the user
                Console.WriteLine("\nTo-Do List Manager");
                Console.WriteLine("1. Add Task");
                Console.WriteLine("2. Remove Task");
                Console.WriteLine("3. Complete Task");
                Console.WriteLine("4. View Tasks");
                Console.WriteLine("5. Exit");
                Console.Write("Choose an option: ");

                // Get user's choice
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        // Adding a new task
                        Console.Write("Enter task description: ");
                        string description = Console.ReadLine();
                        taskManager.AddTask(description);
                        break;
                    case "2":
                        // Removing an existing task
                        taskManager.ViewTasks();
                        Console.Write("Enter task number to remove: ");
                        if (int.TryParse(Console.ReadLine(), out int removeIndex))
                        {
                            taskManager.RemoveTask(removeIndex - 1); // Convert to zero-based index
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a number.");
                        }
                        break;
                    case "3":
                        // Completing a task
                        taskManager.ViewTasks();
                        Console.Write("Enter task number to mark as complete: ");
                        if (int.TryParse(Console.ReadLine(), out int completeIndex))
                        {
                            taskManager.CompleteTask(completeIndex - 1); // Convert to zero-based index
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a number.");
                        }
                        break;
                    case "4":
                        // Viewing all tasks
                        taskManager.ViewTasks();
                        break;
                    case "5":
                        // Exiting the program
                        running = false;
                        break;
                    default:
                        // Handling invalid menu choice
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
    }
}