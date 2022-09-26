using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BooksSpot.Core.Enums
{
    public static class EnumExtensions
    {
        public static string? GetDisplayName(this Enum enumValue)
        {
            var displayName = enumValue.GetType()
              .GetMember(enumValue.ToString())
              .First()
              .GetCustomAttribute<DisplayAttribute>()
              ?.GetName();

            if (string.IsNullOrEmpty(displayName))
            {
                var enumName = enumValue.GetType().GetEnumName(enumValue);
                return enumName;
            }
            return displayName;
        }

     
    }

    public static class EnumExtensions<T>
    {
        public static List<T> GetEnumsInList()
        {
          return Enum.GetValues(typeof(T))
                            .Cast<T>()
                            .ToList();
        }
    }
}
