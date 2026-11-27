using System;

namespace TaskList
{
    internal class TaskItem
    {
        private int id;
        private string title;
        private bool isDone;
        private DateTime createdAtUtc;

        public TaskItem(int id, string title)
        {
            this.id = id;
            this.title = title;
            this.isDone = false;
            this.createdAtUtc = DateTime.UtcNow;
        }

        public int GetId() { return id; }
        public string GetTitle() { return title; }
        public bool GetIsDone() { return isDone; }
        public DateTime GetCreatedAtUtc() { return createdAtUtc; }

        public void SetIsDone(bool isDone) { this.isDone = isDone; }

        public override string ToString()
        {
            return $"Id: {id}, Title: {title}, IsDone: {isDone}, Creation Date in UTC: {createdAtUtc}";
        }
    }
}
