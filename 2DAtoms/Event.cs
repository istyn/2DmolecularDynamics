using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DAtoms
{
    class Event : IComparable
    {
        private double time;            //invarient - used as priority by priority queue
        private Particle a, b;
        private int countA, countB;     //number of events since this was added

        public Event(double t,Particle a, Particle b)
        {
            this.time = t;
            this.a = a;
            this.b = b;
            if (a != null)
            {
                this.countA = a.getCollisionCount();
                
            }
            if (b != null)
            {
                this.countB = b.getCollisionCount();
            }
        }
        public double getTime()
        {
            return this.time;
        }
        public Particle getParticle1()
        {
            return this.a;
        }
        public Particle getParticle2()
        {
            return this.b;
        }
        int IComparable.CompareTo(Object obj)
        {
            Event j = (Event)obj;
            return this.time - j.time == 0 ? 0 : this.time < 0 ? -1 : 1;
        }
        public bool wasSuperveningEvent()
        {
            if (a == null && b == null)                         //redraw case
            {
                return false;
            }
            if (a != null && countA == a.getCollisionCount())   //if a exists and counts are equal
            {
                if (b != null)                                  //both a and b are defined 
                {
                    if (countB == b.getCollisionCount())        //and all counts are equal
                    {
                        return false;
                    }
                    else                                        //b defined but not equal counts
                    {
                        return true;
                    }
                    
                }
                else                                            //a defined and counts equal, b is null
                {
                    return false;

                }
            }
            else                                                //a defined but not equal counts
            {
                return true;
            }
        }
    }
}
