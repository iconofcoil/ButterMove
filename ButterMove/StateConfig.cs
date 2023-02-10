using System;
namespace ButterMove
{
	public struct StateConfig
	{
        public string ShortName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int NormalFee { get; set; } = 0;
        public int PremiumFee { get; set; } = 0;

        public StateConfig(string shortName, string name, int normalFee, int premiumFee)
		{
            this.ShortName = shortName;
            this.Name = name;
            this.NormalFee = normalFee;
            this.PremiumFee = premiumFee;
        }
    }
}

