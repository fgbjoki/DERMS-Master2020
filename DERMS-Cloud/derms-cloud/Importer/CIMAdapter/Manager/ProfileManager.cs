using System;
using System.Reflection;

namespace FTN.ESI.SIMES.CIM.CIMAdapter.Manager
{
	public enum SupportedProfiles : byte
	{
		DERMS
	};


	/// <summary>
	/// ProfileManager
	/// </summary>
	public static class ProfileManager
	{
		public const string Namespace = "DERMS";

		/// <summary>
		/// Method returns the name of CIM profile based on the defined enumeration.
		/// </summary>
		/// <param name="profile">supported CIM profile</param>
		/// <returns>name of profile + "CIMProfile"</returns>
		public static string GetProfileName(SupportedProfiles profile)
		{
			return string.Format("{0}CIMProfile", profile.ToString());
		}

		/// <summary>
		/// Method returns the name of the CIM profile DLL based on the defined enumeration.
		/// </summary>
		/// <param name="profile">supported CIM profile</param>
		/// <returns>name of profile + "CIMProfile.DLL"</returns>
		public static string GetProfileDLLName(SupportedProfiles profile)
		{
			return string.Format("{0}CIMProfile.dll", profile.ToString());
		}

		public static bool LoadAssembly(SupportedProfiles profile, out Assembly assembly)
		{
			try
			{
				assembly = Assembly.LoadFrom(string.Format(".\\{0}", ProfileManager.GetProfileDLLName(profile)));
			}
			catch (Exception e)
			{
				assembly = null;
				LogManager.Log(string.Format("Error during Assembly load. Profile: {0} ; Message: {1}", profile, e.Message), LogLevel.Error);
				return false;
			}
			return true;
		}

        public static bool LoadAssembly(string pathToDll, SupportedProfiles profile, out Assembly assembly)
        {
            if (String.IsNullOrEmpty(pathToDll))
            {
                pathToDll = ".";
            }

            try
            {
                assembly = Assembly.LoadFrom(string.Format("{0}\\{1}", pathToDll, ProfileManager.GetProfileDLLName(profile)));
            }
            catch (Exception e)
            {
                assembly = null;
                LogManager.Log(string.Format("Error during Assembly load. Profile: {0} ; Message: {1}", profile, e.Message), LogLevel.Error);
                return false;
            }
            return true;
        }

        public static bool LoadAssembly(string path, out Assembly assembly)
		{
			try
			{
				assembly = Assembly.LoadFrom(path);
			}
			catch (Exception e)
			{
				assembly = null;
				LogManager.Log(string.Format("Error during Assembly load. Path: {0} ; Message: {1}", path, e.Message), LogLevel.Error);
				return false;
			}
			return true;
		}
	}
}
