using System;
using System.ComponentModel;
using System.Reflection;

namespace Mentoring.Web.Models
{
    public class EnumExtension
    {
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }

            return value.ToString();
        }
    }
}