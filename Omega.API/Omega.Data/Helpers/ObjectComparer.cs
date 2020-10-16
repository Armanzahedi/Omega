using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Omega.Infrastructure.Helpers
{
    static class ObjectComparer
    {
		public static List<Variance> Compare<T>(this T val1, T val2)
		{
			var variances = new List<Variance>();
			var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
			foreach (var property in properties)
			{
				var v = new Variance
				{
					Prop = property.Name,
					valA = property.GetValue(val1),
					valB = property.GetValue(val2)
				};
				if (v.valA == null && v.valB == null)
				{
					continue;
				}
				if (
					(v.valA == null && v.valB != null)
					||
					(v.valA != null && v.valB == null)
				)
				{
					variances.Add(v);
					continue;
				}
				if (!v.valA.Equals(v.valB))
				{
					variances.Add(v);
				}
			}
			return variances;
		}


	}
    class Variance
    {
        public string Prop { get; set; }
        public object valA { get; set; }
        public object valB { get; set; }
    }
}
