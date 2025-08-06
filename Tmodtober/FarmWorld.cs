using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;

namespace Tmodtober
{
    public class FarmAnimal
    {
        public string name;
        public Vector2 pos;
        public int type;

        public int npcID;

        public FarmAnimal(string _name,Vector2 _position,int _type,int id)
        {
            name = _name;
            pos = _position;
            type = _type;
            npcID = id;
        }
    }


    public class FarmWorld:ModSystem
    {

        public static FarmWorld Instance;

        public override void Load()
        {
            Instance = this;
            _animals = new List<FarmAnimal>();
            base.Load();
        }

        public override void Unload()
        {
            Instance = null;
            base.Unload();
        }

        public const string ANIMAL_COUNT_SAVE_KEY = "farm_animals_count";
        public const string ANIMAL_POSITION_SAVE_KEY= "farm_animal_position_";
        public const string ANIMAL_TYPE_SAVE_KEY= "farm_animal_type_";

        public List<FarmAnimal> _animals;

        public override void SaveWorldData(TagCompound tag)
        {
            if(_animals!=null && _animals.Count > 0){
                tag.Add(ANIMAL_COUNT_SAVE_KEY, _animals.Count);

                for (int i = 0; i < _animals.Count; i++){
                    tag.Add(ANIMAL_POSITION_SAVE_KEY+i.ToString()+"_x", _animals[i].pos.X);
                    tag.Add(ANIMAL_POSITION_SAVE_KEY+i.ToString()+"_y", _animals[i].pos.Y);

                    tag.Add(ANIMAL_TYPE_SAVE_KEY+i.ToString(), _animals[i].type);
                }
            }
            base.SaveWorldData(tag);
        }
        private List<(Vector2, int)> __animalsToMake;

        public override void LoadWorldData(TagCompound tag)
        {
            __animalsToMake = new List<(Vector2, int)>();
            if (tag.ContainsKey(ANIMAL_COUNT_SAVE_KEY))
            {
                int _animalCount = tag.GetInt(ANIMAL_COUNT_SAVE_KEY);

                for(int i = 0; i < _animalCount; i++)
                {
                    Vector2 _pos = Vector2.Zero;
                    if(tag.ContainsKey(ANIMAL_POSITION_SAVE_KEY + i.ToString() + "_x")){
                        _pos.X = tag.GetFloat(ANIMAL_POSITION_SAVE_KEY + i.ToString() + "_x");
                    }
                    if (tag.ContainsKey(ANIMAL_POSITION_SAVE_KEY + i.ToString() + "_y")){
                        _pos.Y = tag.GetFloat(ANIMAL_POSITION_SAVE_KEY + i.ToString() + "_y");
                    }

                    int _type = NPCID.None;
                    if (tag.ContainsKey(ANIMAL_TYPE_SAVE_KEY + i.ToString())){
                        _type = tag.GetInt(ANIMAL_TYPE_SAVE_KEY + i.ToString());
                    }

                    __animalsToMake.Add(new(_pos, _type));

                }
            }
            base.LoadWorldData(tag);
        }

        public override void PostUpdateWorld()
        {
            if (__animalsToMake != null && __animalsToMake.Count>0)
            {
                for (int i = 0; i < __animalsToMake.Count; i++)
                {
                    EntitySource_Misc _s = new EntitySource_Misc("Was loaded");

                    NPC _checkNPC;
                    bool found = false;
                    for (int a = 0; a < Main.maxNPCs; a++)
                    {
                        _checkNPC = Main.npc[a];
                        if (_checkNPC.active && _checkNPC.CountsAsACritter && _checkNPC.type == __animalsToMake[i].Item2 && _checkNPC.Center == __animalsToMake[i].Item1)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found) { 
                        int _farmNPC = NPC.NewNPC(_s, (int)Main.player[Main.myPlayer].Center.X, (int)Main.player[Main.myPlayer].Center.Y, __animalsToMake[i].Item2);
                        NPC _npc = Main.npc[_farmNPC];
                        if (_npc != null)
                        {
                            FarmAnimalNPC _farmAnimal = _npc.GetGlobalNPC<FarmAnimalNPC>();
                            _farmAnimal.TransformIntoFarmAnimal(_npc);
                            _animals.Add(new FarmAnimal(_npc.FullName, __animalsToMake[i].Item1, __animalsToMake[i].Item2, _farmNPC));

                            _npc.Center = __animalsToMake[i].Item1;
                        }
                    }
                }
                __animalsToMake.Clear();
            }
            base.PostUpdateWorld();
        }

    }
}
