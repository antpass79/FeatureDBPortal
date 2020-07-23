using FeatureDBPortal.Server.Data.Models.RD;
using FeatureDBPortal.Server.Models;
using FeatureDBPortal.Shared;
using System.Collections.Generic;

namespace FeatureDBPortal.Server.Providers
{
    public interface IAllowModeProvider
	{
		AllowModeProperties Properties(IEnumerable<NormalRule> normalRules, int? probeId);

		bool IsVisible(AllowModeDTO allowMode);
		bool IsVisible(short allowMode);

		bool IsAvailable(AllowModeDTO allowMode);
		bool IsAvailable(short allowMode);

		AllowModeDTO GetMode(bool visible, bool available);
	}
}
