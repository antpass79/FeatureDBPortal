using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Shared;
using System.Collections.Generic;
using System.Linq;

namespace FeatureDBPortal.Server.Providers
{
    public class AllowModeProvider : IAllowModeProvider
	{
        private readonly FeaturesContext _context;

        public AllowModeProvider(FeaturesContext context)
        {
            _context = context;
        }

		public AllowModeProperties Properties(IEnumerable<NormalRule> normalRules, int? probeId)
        {
			bool available = false;
			bool visible = false;
			AllowModeDTO allowMode = AllowModeDTO.No;

			if (probeId.HasValue)
			{
				// Probe 50: TLC 3-13 => 2 transducers
				var transducers = _context.ProbeTransducers
					.Where(item => item.ProbeId == probeId);

				foreach (var transducer in transducers)
				{
					var transducerNormalRules = normalRules.
						Where(normalRule => !normalRule.TransducerType.HasValue || transducer.TransducerType == normalRule.TransducerType);

					visible |= transducerNormalRules.All(normalRule => IsVisible(normalRule.Allow));
					available |= transducerNormalRules.All(normalRule => IsAvailable(normalRule.Allow));
				}
			}
			else
			{
				visible = normalRules.All(normalRule => IsVisible(normalRule.Allow));
				available = normalRules.All(normalRule => IsAvailable(normalRule.Allow));
			}

			allowMode = GetMode(visible, available);

			return new AllowModeProperties(visible, available, allowMode);
		}

		public bool IsVisible(AllowModeDTO allowMode)
        {
			return allowMode == AllowModeDTO.A;
		}
		public bool IsVisible(short allowMode)
		{
			return (AllowModeDTO)allowMode == AllowModeDTO.A;
		}

		public bool IsAvailable(AllowModeDTO allowMode)
        {
			return allowMode != AllowModeDTO.No;
		}
		public bool IsAvailable(short allowMode)
		{
			return (AllowModeDTO)allowMode != AllowModeDTO.No;
		}

		public AllowModeDTO GetMode(bool visible, bool available)
		{
			if (available)
			{
				if (visible)
				{
					return AllowModeDTO.A;
				}
				else
				{
					return AllowModeDTO.Def;
				}
			}
			return AllowModeDTO.No;
		}
	}
}
