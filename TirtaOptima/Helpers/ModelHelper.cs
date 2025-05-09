using System.Reflection;

namespace TirtaOptima.Helpers
{
	public static class ModelHelper
	{
		public static TDestination MapProperties<TSource, TDestination>(TSource source)
			where TDestination : new()
		{
			TDestination destination = new();

			var sourceProperties = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
			var destinationProperties = typeof(TDestination).GetProperties(BindingFlags.Public | BindingFlags.Instance);

			foreach (var sourceProperty in sourceProperties)
			{
				var destinationProperty = destinationProperties.FirstOrDefault(prop => prop.Name == sourceProperty.Name && prop.PropertyType == sourceProperty.PropertyType);
				if (destinationProperty != null && destinationProperty.CanWrite)
				{
					var value = sourceProperty.GetValue(source);
					destinationProperty.SetValue(destination, value);
				}
			}

			return destination;
		}

		public static void UpdateProperties<TSource, TTarget>(TSource source, TTarget target)
		{
			var sourceProperties = typeof(TSource).GetProperties();
			var targetProperties = typeof(TTarget).GetProperties();

			foreach (var sourceProperty in sourceProperties)
			{
				var targetProperty = targetProperties.FirstOrDefault(p => p.Name == sourceProperty.Name && p.PropertyType == sourceProperty.PropertyType);

				if (targetProperty != null)
				{
					var sourceValue = sourceProperty.GetValue(source);

					if (sourceValue is string strValue)
					{
						// Update hanya jika string tidak kosong atau whitespace
						if (!string.IsNullOrWhiteSpace(strValue))
						{
							targetProperty.SetValue(target, sourceValue);
						}
					}
					else if (sourceValue != null)
					{
						// Update jika nilai tidak null (untuk tipe non-string)
						targetProperty.SetValue(target, sourceValue);
					}
				}
			}
		}

		public static bool IsPropertyExist<T>(string propertyName)
		{
			return typeof(T).GetProperty(propertyName) != null;
		}
	}
}
