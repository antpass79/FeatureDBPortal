namespace FeatureDBPortal.Server.Utils
{
    public enum AllowMode
    {
        No = 0,
        Def = 1,
        A = 2
    }

	/// interface of the allower
	public interface IAllower
	{
		bool GetAllowance(AllowMode Allow);
	}

	/// abstract class of the allowance
	public abstract class Allower : IAllower
	{
		public abstract bool GetAllowance(AllowMode Allow);

		public static AllowMode GetMode(bool visible, bool available)
		{
			if (available)
			{
				if (visible)
				{
					return AllowMode.A;
				}
				else
				{
					return AllowMode.Def;
				}
			}
			return AllowMode.No;
		}
	}

	/// manager of the availability
	public class AvailableAllower : Allower
	{
		// ISAVAILABLE
		public override bool GetAllowance(AllowMode Allow)
		{
			return (Allow != AllowMode.No);
		}
	}

	/// manager of the visibility
	public class VisibleAllower : Allower
	{
		// ISVISIBLE
		public override bool GetAllowance(AllowMode Allow)
		{
			return (Allow == AllowMode.A);
		}
	}
}
