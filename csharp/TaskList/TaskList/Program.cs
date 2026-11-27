//Build a TaskList console app with a menu: add, list, complete, delete.
//A TaskItem class with Id , Title, IsDone, CreatedAtUtc . Always use UTC (the
//feedback from your interview).
//A TaskService class holds the logic. Program only reads input and prints output.

using TaskList;

Console.WriteLine("Welcome to the Task List App");
bool exit = false;
TaskService taskService = new TaskService();

while (!exit)
{
    Console.WriteLine("Menu:");
    Console.WriteLine("1. View all tasks.");
    Console.WriteLine("2. Add a task.");
    Console.WriteLine("3. Mark task as completed.");
    Console.WriteLine("4. Delete a task");
    Console.WriteLine("To exit type -1");

    Console.Write("\nType the number of the operation: ");
    //int userInput = int.Parse(Console.ReadLine());

    if (!int.TryParse(Console.ReadLine(), out int userInput))
    {
        Console.WriteLine("Please enter a valid number.\n");
        continue;
    }

    switch (userInput)
    {
        case 1:
            var tasks = taskService.GetAllTasks();

            Console.WriteLine("\nTasks List:");
            if (tasks.Any())
            {
                foreach (var task in tasks)
                    Console.WriteLine(task);
            }
            else
            {
                Console.WriteLine("There is no tasks.");
            }

            break;
        case 2:
            Console.Write("\nPlease enter the task title: ");
            string title = Console.ReadLine();
            taskService.AddTask(title);
            break;
        case 3:
            {
                Console.Write("\nProvide the task id: ");
                //int taskId = int.Parse(Console.ReadLine());

                if (!int.TryParse(Console.ReadLine(), out int taskId))
                {
                    Console.WriteLine("Please enter a valid task ID.");
                    break; // current switch case not the loop
                }

                if (taskService.CompleteTask(taskId))
                {
                    Console.WriteLine("Task was updated successfully.");
                }
                else
                {
                    Console.WriteLine("No such task exists");
                }

                break;
            }
        case 4:
            {
                Console.Write("\nProvide the task id: ");
                //int taskId = int.Parse(Console.ReadLine());
                if (!int.TryParse(Console.ReadLine(), out int taskId))
                {
                    Console.WriteLine("Please enter a valid task ID.");
                    break; // current switch case not the loop
                }

                if (taskService.DeleteTask(taskId))
                {
                    Console.WriteLine("Task was deleted successfully.");
                }
                else
                {
                    Console.WriteLine("No such task exists");
                }
                break;
            }
        case -1:
            exit = true;
            break;
        default:
            Console.WriteLine("Invalid operation. Please try again.");
            break;
    }
    Console.WriteLine();
}