using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuticaFN
{
    class unit
    {
        protected int bp;
        protected int hp;
        protected int speed;
        protected int sp;
        protected int[] special;
        protected int shield;
        protected string name;
        protected string type_name;
        protected string type;
       unit(int _hp,int _sp,int[] _special,string[] _type,int _speed,int _shield) 
        {
            hp = _hp;
            sp = _sp;
            special = _special;
            type = _type[0];
            type_name = _type[1];
            name = _type[2];
            speed = _speed;
            shield = _shield;
        }

        public void gotDamege(int damege) 
        {
            if (shield <= damege) { hp -= damege - shield; };
        }
        public int[] getSpecial() 
        {
            return special;
        }
        public void buffbp(int many)
        { bp += many; }

    }
    class Location 
    {
        protected string name;
        protected int code;
    }
    class Battle 
    {
        protected List<unit> attacker;
        protected List<unit> defenseer;
        protected Location location;
        public async void asemble(bool defence) 
        {
            List<unit> units = attacker;
            if (defence) 
            {
                units = defenseer;
            }
            foreach(unit a in units) 
            {
                List<Task> tasks = new List<Task>();
                foreach (int code in a.getSpecial()) 
                {
                    tasks.Add(asemble_task(a,code, defence));
                }
                await Task.WhenAll(tasks);
            }
        }
        public async Task asemble_task(unit unit_,int code,bool defence)
        {
            switch (code)
            {
                case 0: action0(defence); break;
                case 1: action1(unit_,defence); break;
                default: break;
            };
        }
        public void action0(bool defence)
        {
            List<unit> counter = attacker;
            if (defence) { counter = defenseer; }
            foreach(unit unit_ in counter) 
            {
                if (unit_.getSpecial().Contains(2)) 
                {
                    unit_.gotDamege(100);
                }
            }
        }
        public void action1(bool defence) 
        {
           
        }
        public void asmbleLocation(int code)
        {


        }
    }

}
