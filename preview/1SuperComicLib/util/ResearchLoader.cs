using System;
using System.Collections.Generic;
using System.Reflection;

namespace SuperComicLib.Oni
{
    public static class ResearchLoader
	{
		/// <summary>
		/// (string) ID static 필드를 찾고, 없으면 nameof(T)
		/// tech_key 는 ResearchKeys 에 다 있습니다
		/// 
		/// you can find 'tech_key' value in the ResearchKeys class
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public static void Smart<T>(string tech_key) => 
			Database.Techs.TECH_GROUPING[tech_key] = 
				new List<string>(Database.Techs.TECH_GROUPING[tech_key]) 
				{ 
					typeof(T).FindID()
				}
				.ToArray();

		public static void Add(string tech_key, params string[] ids)
		{
			List<string> item = new List<string>(Database.Techs.TECH_GROUPING[tech_key]);
			item.AddRange(ids);
			Database.Techs.TECH_GROUPING[tech_key] = item.ToArray();
		}
	}
}
