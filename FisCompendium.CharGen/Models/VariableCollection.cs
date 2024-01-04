using FisCompendium.CharGen.Enum;
using FisCompendium.CharGen.Models.Variables;

namespace FisCompendium.CharGen.Models
{
    public class VariableCollection
    {
        public IntegerStatVariable Var_Phys_Recovery { get; set; }
        public IntegerStatVariable Var_Phys_MaxStress { get; set; }
        public IntegerStatVariable Var_Soc_Recovery { get; set; }
        public IntegerStatVariable Var_Soc_MaxStress { get; set; }
        public IntegerStatVariable Var_Mag_Recovery { get; set; }
        public IntegerStatVariable Var_Mag_MaxStress { get; set; }

        public IntegerStatVariable Var_FateCap { get; set; }
        public RealStatVariable Var_Luck { get; set; }

        public double RedAffinityRate { get; set; }
        public double GreenAffinityRate { get; set; }
        public double BlueAffinityRate { get; set; }
        public double PrismaticAffinityRate { get; set; }
        public double VoidAffinityRate { get; set; }

        public RealStatVariable Var_RedAffinity { get; set; }
        public RealStatVariable Var_GreenAffinity { get; set; }
        public RealStatVariable Var_BlueAffinity { get; set; }
        public RealStatVariable Var_PrismaticAffinity { get; set; }
        public RealStatVariable Var_VoidAffinity { get; set; }

        public DistributionVariable<CategoryModifier> Var_CategoryModDistribution { get; set; }
        public DistributionVariable<ApproachModifier> Var_ApproachModDistribution { get; set; }
    }
}
