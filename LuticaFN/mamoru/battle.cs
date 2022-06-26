using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuticaFN
{
    internal class mercenary
    {
        public string name;
        protected string sex;
        protected int fight;
        protected int hp;
        protected int destory;
        protected int move;
        protected int moti;
        protected int skill_id;
        protected int job;
        protected int rangetype;

        public mercenary(string name_, string sex_, int[] stats, int skillid_,int job_,int range_type)
        {
            this.name = name_;
            this.sex = sex_;
            fight = stats[0];
            hp = stats[1];
            destory = stats[2];
            move = stats[3];
            moti = stats[4];
            skill_id = skillid_;
            job = job_;
            rangetype = range_type;
        }
        public void lostHp(int damage)
        {
            hp -= damage;
        }
        public void defenceEffectHP(double percent) 
        {
            hp = (int) (this.hp * percent);
        }
        public void defencEffectAtk(double percent)
        { 
            fight = (int) (this.fight * percent);
        }
        public int intPower() 
        {
            return fight;
        }
        public void bufIntHp(int hp_) 
        {
            hp += hp_;
        }
        public int getRange() 
        {
            return rangetype;
        }

    }
    internal class building 
    {
        public string name;
        public int construction;
        public int opened;
        public int production;
        public int armer;
        public int handicap;
        public int defencehp = 0;
        public building(string _name,int[] stats) 
        {
            name = _name;
            construction = stats[0];
            production = stats[1];
            armer = stats[2];
            handicap = stats[3];
            opened = stats[4];
            buildingMakeDefHp();
        }
        public void buildingMakeDefHp() 
        {
            if(construction == 1) 
            {
                defencehp = 10;
            }
            else if (construction == 2) 
            {
                defencehp = 20;
            }
            else { defencehp = 0; }
            if(armer == 1) 
            {
                defencehp += 25;
            }
            else if(armer == 2) 
            {
                defencehp += 40;
            }
            else if(armer == 3) 
            {
                defencehp += 55;
            }
            else if (armer == 4)
            {
                defencehp += 65;
            }
        }
    }
    internal class battle
    {
        protected List<mercenary> mercenary_list_atk;
        protected List<mercenary> mercenary_list_def;
        protected building building;

        public battle(List<mercenary> mercenary_list_atk, List<mercenary> mercenary_list_def, building location)
        {
            this.mercenary_list_atk = mercenary_list_atk;
            this.mercenary_list_def = mercenary_list_def;
            this.building = location;
        }
        public void asemble() 
        {
            double fight_buf = 1.0;
            double hp_buf = 1.0;
            // 1단계 : 건물의 방어효과를 적용한다.
            if (building.armer == 1) 
            {
                hp_buf = 1.1;
            }
            else if(building.armer == 2)
            {
                fight_buf = 1.2;
                hp_buf = 1.2;
            }
            else if (building.armer == 3)
            {
                fight_buf = 1.25;
                hp_buf = 1.25;
            }
            else if (building.armer == 4)
            {
                fight_buf = 1.35;
                hp_buf = 1.35;
            }
            foreach(mercenary mercenary in mercenary_list_def) 
            {
                if (this.building.defencehp > 0) 
                {
                    mercenary.defenceEffectHP(hp_buf);
                    mercenary.defencEffectAtk(fight_buf); 
                }
            }
            //2단계 : 
            if(building.opened == 1) 
            {
                foreach(mercenary mercenary in mercenary_list_def) 
                {
                    if (mercenary.getRange() == 3) 
                    {
                        mercenary.bufIntHp(4);
                    }
                }
                foreach (mercenary mercenary in mercenary_list_atk)
                {
                    if (mercenary.getRange() == 3)
                    {
                        mercenary.bufIntHp(4);
                    }
                }
            }
            else if (building.opened == 2)
            {
                foreach (mercenary mercenary in mercenary_list_def)
                {
                    if (mercenary.getRange() == 1)
                    {
                        mercenary.bufIntHp(4);
                    }
                }
                foreach (mercenary mercenary in mercenary_list_atk)
                {
                    if (mercenary.getRange() == 1)
                    {
                        mercenary.bufIntHp(4);
                    }
                }
            }
            //3단계 : 데미지 함산
            int atk_power_long = 0; int atk_power_short = 0; int atk_power_middle = 0;
            int def_power_long = 0; int def_power_short = 0; int def_power_middle = 0;

            foreach (mercenary mercenary in mercenary_list_def) 
            {
                if (mercenary.getRange() == 1 || mercenary.getRange() == 0)
                { def_power_short += mercenary.intPower(); }
                else if (mercenary.getRange() == 2)
                { def_power_middle += mercenary.intPower(); }
                else if (mercenary.getRange() == 3)
                { def_power_long += mercenary.intPower(); }
            }
            foreach (mercenary mercenary in mercenary_list_atk)
            {
                if(mercenary.getRange() == 1 || mercenary.getRange()==0) 
                { atk_power_short += mercenary.intPower(); }
                else if(mercenary.getRange() == 2)
                { atk_power_middle += mercenary.intPower(); }
                else if (mercenary.getRange() == 3) 
                { atk_power_long += mercenary.intPower(); }
            }
            // 용병 사거리 = 최우선, 근접,중거리,장거리 0~3
            // 4단계 : 데미지 분배
            // 장거리데미지 -> 
            // 중거리데미지 -> 
            // 단거리데미지 ->
            List<mercenary> long_list_atk = new List<mercenary>();
            List<mercenary> middle_list_atk = new List<mercenary>();
            List<mercenary> short_list_atk = new List<mercenary>();
            List<mercenary> first_list_atk = new List<mercenary>();

            foreach (mercenary mercenary1 in mercenary_list_atk) 
            { 
                int m_range = mercenary1.getRange();
                if (m_range == 0) 
                {
                    long_list_atk.Add(mercenary1);
                }
                else if(m_range == 1) 
                {
                    middle_list_atk.Add(mercenary1);
                }
                else if (m_range == 2) 
                {
                    short_list_atk.Add((mercenary1));
                }
                else if (m_range == 3) 
                {
                    first_list_atk.Add(mercenary1);
                }

            }
            List<mercenary> long_list_def = new List<mercenary>();
            List<mercenary> middle_list_def = new List<mercenary>();
            List<mercenary> short_list_def = new List<mercenary>();
            List<mercenary> first_list_def = new List<mercenary>();

            foreach (mercenary mercenary1 in mercenary_list_def)
            {
                int m_range = mercenary1.getRange();
                if (m_range == 0)
                {
                    long_list_def.Add(mercenary1);
                }
                else if (m_range == 1)
                {
                    middle_list_def.Add(mercenary1);
                }
                else if (m_range == 2)
                {
                    short_list_def.Add(mercenary1);
                }
                else if (m_range == 3)
                {
                    first_list_def.Add(mercenary1);
                }

            }

        }
        public void mainbattle() 
        {
            
        }
    }

}
