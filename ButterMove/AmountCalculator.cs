using System;
using System.Security.Cryptography.X509Certificates;

namespace ButterMove
{
	public class AmountCalculator
	{
        public enum EnumEstimateType { Normal, Premium };

        public string shortName { get; set; } = string.Empty;
        public EnumEstimateType estimateType { get; set; } = EnumEstimateType.Normal;
        public int kilometers { get; set; } = 0;
        public decimal baseAmount { get; set; } = 0;

        private List<StateConfig> states = new List<StateConfig>() {
			new StateConfig("NY", "New York", 25, 35),
            new StateConfig("CA", "California", 23, 33),
            new StateConfig("AZ", "Arizona", 20, 30),
            new StateConfig("TX", "Texas", 18, 28),
            new StateConfig("OH", "Ohio", 15, 25),
        };

        public AmountCalculator()
        {
        }

        public AmountCalculator(string shortName, EnumEstimateType estimateType, int kms, decimal baseAmount)
		{
            this.shortName = shortName;
            this.estimateType = estimateType;
            this.kilometers = kms;
            this.baseAmount = baseAmount;
		}

        public decimal GetAmount()
        {
            decimal result = 0;

            // Fetch State data
            if (!states.Exists(s => s.ShortName == shortName))
            {
                throw new Exception("Unsupported State");
            }

            var state = states.Where(s => s.ShortName == shortName).First();

            // Base calculation
            var fee = this.estimateType == EnumEstimateType.Normal ? state.NormalFee : state.PremiumFee;
            result = this.baseAmount + (this.baseAmount * ((decimal)fee / (decimal)100));

            // Specifics by State
            // New York
            if (state.ShortName == "NY")
            {
                result += (result * 0.21m);
            }
            // California and Arizona
            else if (state.ShortName == "CA" ||
                     state.ShortName == "AZ")
            {
                if (this.estimateType == EnumEstimateType.Normal &&
                    this.kilometers > 26)
                {
                    result -= result * (0.05m);
                }
            }
            // Texas and Ohio
            else if (state.ShortName == "TX" ||
                     state.ShortName == "OH")
            {
                if (this.estimateType == EnumEstimateType.Normal)
                {
                    if (this.kilometers >= 20 &&
                        this.kilometers <= 30)
                    {
                        result -= this.baseAmount * (0.03m);
                    }
                    else if (this.kilometers > 30)
                    {
                        result -= result * (0.05m);
                    }
                }
            }

            // Premium Fee
            if (this.estimateType == EnumEstimateType.Premium &&
                this.kilometers > 25)
            {
                result -= result * (0.05m);
            }

            return Math.Round(result, 2);
        }
	}
}

