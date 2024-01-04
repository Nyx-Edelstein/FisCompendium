namespace FisCompendium.CharGen.Data
{
    public class GridData
    {
        public int Phys_Recovery { get; set; }
        public int Soc_Recovery { get; set; }
        public int Mag_Recovery { get; set; }
        public int Phys_MaxStress { get; set; }
        public int Soc_MaxStress { get; set; }
        public int Mag_MaxStress { get; set; }

        public int FPCap { get; set; }
        public double Luck { get; set; }

        public double RedAffinity { get; set; }
        public double GreenAffinity { get; set; }
        public double BlueAffinity { get; set; }
        public double PrismaticAffinity { get; set; }
        public double VoidAffinity { get; set; }

        public int HasEndowment { get; set; }
        public int HasWeakness { get; set; }
        public int NumBonuses { get; set; }
        public int NumMaluses { get; set; }
        public int NumNarrative { get; set; }
        public int NumProfessions { get; set; }

        public string Endowment { get; set; } = string.Empty;
        public string Weakness { get; set; } = string.Empty;
        public string Bonus1 { get; set; } = string.Empty;
        public string Bonus2 { get; set; } = string.Empty;
        public string Bonus3 { get; set; } = string.Empty;
        public string Malus1 { get; set; } = string.Empty;
        public string Malus2 { get; set; } = string.Empty;
        public string Malus3 { get; set; } = string.Empty;

        public int Phys_Forceful { get; set; }
        public int Phys_Decisive { get; set; }
        public int Phys_Flashy { get; set; }
        public int Phys_Intuitive { get; set; }
        public int Phys_Methodical { get; set; }
        public int Phys_Clever { get; set; }

        public int Soc_Forceful { get; set; }
        public int Soc_Decisive { get; set; }
        public int Soc_Flashy { get; set; }
        public int Soc_Intuitive { get; set; }
        public int Soc_Methodical { get; set; }
        public int Soc_Clever { get; set; }

        public int Mag_Forceful { get; set; }
        public int Mag_Decisive { get; set; }
        public int Mag_Flashy { get; set; }
        public int Mag_Intuitive { get; set; }
        public int Mag_Methodical { get; set; }
        public int Mag_Clever { get; set; }

        public int Forceful { get; set; }
        public int Decisive { get; set; }
        public int Flashy { get; set; }
        public int Intuitive { get; set; }
        public int Methodical { get; set; }
        public int Clever { get; set; }

        public int XPInitial { get; set; }

        public double Variance { get; set; }

        public override string ToString() =>
$@"{Forceful}	{Decisive}	{Flashy}	{Intuitive}	{Methodical}	{Clever}	{RedAffinity:F2}	{XPInitial} 
{Phys_Forceful}	{Phys_Decisive}	{Phys_Flashy}	{Phys_Intuitive}	{Phys_Methodical}	{Phys_Clever}	{GreenAffinity:F2}	{"-"}
{Soc_Forceful}	{Soc_Decisive}	{Soc_Flashy}	{Soc_Intuitive}	{Soc_Methodical}	{Soc_Clever}	{BlueAffinity:F2}	{"-"}
{Mag_Forceful}	{Mag_Decisive}	{Mag_Flashy}	{Mag_Intuitive}	{Mag_Methodical}	{Mag_Clever}	{PrismaticAffinity:F2}	{"-"}
{HasEndowment}	{HasWeakness}	{NumBonuses}	{NumMaluses}	{NumNarrative}	{NumProfessions}	{VoidAffinity:F2}	{"-"}
{Endowment}	{Weakness}	{Bonus1}	{Bonus2}	{Bonus3}	{Malus1}	{Malus2}	{Malus3}
{Phys_Recovery}	{Soc_Recovery}	{Mag_Recovery}	{Phys_MaxStress}	{Soc_MaxStress}	{Mag_MaxStress}	{FPCap}	{Luck:F2}";
    }
}
