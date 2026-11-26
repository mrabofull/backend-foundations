using System;
using System.Collections.Generic;
using System.Text;

namespace OrdersReport
{
    internal class Order
    {
        private int id;
        private int userId;
        private decimal amount;
        private DateTime createdAtUTC;

        public Order (int id, int userId, decimal amount)
        {
            this.id = id;
            this.userId = userId;
            this.amount = amount;
            this.createdAtUTC = DateTime.UtcNow; // UTC
            // DateTime.Now local time zone
        }

        public int getId() { return id; }
        public int getUserId() { return userId; }
        public decimal getAmount() { return amount; }
        public DateTime getCreatedAtUTC() { return createdAtUTC;  }

        public override string ToString()
        {
            return $"Id: {id}, UserId: {userId}, amount: {amount}, createdAtUTC: {createdAtUTC}";
        }
    }
}