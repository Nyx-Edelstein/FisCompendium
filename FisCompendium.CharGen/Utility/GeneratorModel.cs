using FisCompendium.CharGen.Data;
using FisCompendium.CharGen.Enum;
using FisCompendium.CharGen.Interfaces;
using FisCompendium.CharGen.Models;
using FisCompendium.CharGen.Models.RacialVariables;

namespace FisCompendium.CharGen.Utility
{
    public static class CharacterGenerator
    {
        public static GridData Generate(Race race, int age, double xpCoeff)
        {
            var racialVariables = GetVariables(race);
            if (racialVariables == null) return new GridData();

            var data = GetGridData(racialVariables, race, age, xpCoeff);
            return data;
        }

        private static VariableCollection GetVariables(Race race)
        {
            IRacialVariablesSelector variablesSelector;
            switch (race)
            {
                case Race.Blin:
                    variablesSelector = new BlinVariables();
                    break;
                case Race.Deku:
                    variablesSelector = new DekuVariables();
                    break;
                case Race.Gerudo:
                    variablesSelector = new GerudoVariables();
                    break;
                case Race.Goron:
                    variablesSelector = new GoronVariables();
                    break;
                case Race.Hylian:
                    variablesSelector = new HylianVariables();
                    break;
                case Race.Lizalfos:
                    variablesSelector = new LizalfosVariables();
                    break;
                case Race.Lynel:
                    variablesSelector = new LynelVariables();
                    break;
                case Race.Zora:
                    variablesSelector = new ZoraVariables();
                    break;
                default:
                    return null;
            }

            return variablesSelector.Select();
        }

        private static GridData GetGridData(VariableCollection racialVariables, Race race, int age, double xpCoeff)
        {
            var sampler = new VariableSampler(racialVariables, race, age, xpCoeff);
            var data = sampler.GetSampleData();
            return data;
        }
    }
}
