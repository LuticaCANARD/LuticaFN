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

        
    }
    class Location 
    {
    
    }
    class Battle 
    {
        protected List<unit> attack;
        protected List<unit> defense;
        protected List<unit> attacker;
        protected List<unit> defenseer;
        protected 
        public void asemble(int code)
        {
            switch (code)
            {
                case 0: action0(); break;
                default: break;
            }
        }
        public void action0()
        {


        }
    }

}
