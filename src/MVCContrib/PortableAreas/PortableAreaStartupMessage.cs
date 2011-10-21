namespace MvcContrib.PortableAreas
{
	public class PortableAreaStartupMessage : IEventMessage
	{
		private readonly string _areaName;

		public PortableAreaStartupMessage(string areaName)
		{
			_areaName = areaName;
		}

		public override string ToString()
		{
			return string.Format("The {0} Portable Area is regsitering", _areaName);
		}
	}
}