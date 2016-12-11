using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DAtoms
{
    class PriorityQueue
    {
        // The items and priorities.
        List<Event> Values = new List<Event>();
        List<double> Priorities = new List<double>();

        // Return the number of items in the queue.
        public int NumItems
        {
            get
            {
                return Values.Count;
            }
        }

        // Add an item to the queue.
        public void Enqueue(Event new_value, double new_priority)
        {
            Values.Add(new_value);
            Priorities.Add(new_priority);
        }

        // Remove the item with the highest priority from the queue.
        public void Dequeue(out Event top_value, out double top_priority)
        {
            // Find the lowest number
            int best_index = 0;
            double best_priority = Priorities[0];
            for (int i = 1; i < Priorities.Count; i++)
            {
                if (best_priority > Priorities[i])
                {
                    best_priority = Priorities[i];
                    best_index = i;
                }
            }

            // Return the corresponding item.
            top_value = Values[best_index];
            top_priority = best_priority;

            // Remove the item from the lists.
            Values.RemoveAt(best_index);
            Priorities.RemoveAt(best_index);
        }
    }
}
