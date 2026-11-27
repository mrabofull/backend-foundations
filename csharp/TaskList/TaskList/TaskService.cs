using System;
using System.Collections.Generic;
using System.Text;

namespace TaskList
{
    internal class TaskService
    {
        // methods:
        //Build a TaskList console app with a menu: add, list, complete, delete.

        private List<TaskItem> tasks = new List<TaskItem>();
        private int nextId = 1;

        public void AddTask(string title)
        {
            tasks.Add(new TaskItem (nextId, title));
        }
        public List<TaskItem> GetAllTasks()
        {
            return tasks;
        }

        public bool CompleteTask(int id)
        {
            foreach (var task in tasks)
            {
                if (task.GetId() == id)
                {
                    task.SetIsDone(true);
                    return true;
                }
            }

            return false;
        }

        public bool DeleteTask(int id)
        {
            foreach (var task in tasks)
            {
                if (task.GetId() == id)
                {
                    tasks.Remove(task);
                    return true;
                }
            }

            return false;
        }

    }
}
