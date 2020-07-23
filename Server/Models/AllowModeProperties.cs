using FeatureDBPortal.Shared;

namespace FeatureDBPortal.Server.Models
{
	public class AllowModeProperties
    {
		public AllowModeProperties(bool visible, bool available, AllowModeDTO allowMode)
        {
			Visible = visible;
			Available = available;
			AllowMode = allowMode;
        }

		public bool Visible { get; }
		public bool Available { get; }
		public AllowModeDTO AllowMode { get; }
	}
}
