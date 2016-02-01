using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class CopySimilar
    {
        public static void SetProperties(object source, object target)
        {
            Type targetType = target.GetType();
            foreach (PropertyInfo prop in source.GetType().GetProperties())
            {
                PropertyInfo targetProp = targetType.GetProperty(prop.Name);
                MethodInfo propGetter = prop.GetGetMethod();
                MethodInfo propSetter = targetProp.GetSetMethod();
                Type targetPropType = targetProp.GetType();
                var valueToSet = propGetter.Invoke(source, null);
                propSetter.Invoke(target, new[] { valueToSet });
            }
        }

        public static void StructToProperties<T>(T source, object target) where T : struct
        {
            Type targetType = target.GetType();
            foreach (FieldInfo prop in source.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                PropertyInfo targetProp = targetType.GetProperty(prop.Name);
                MethodInfo propSetter = targetProp.GetSetMethod();
                Type targetPropType = targetProp.GetType();
                var valueToSet = prop.GetValue(source);
                propSetter.Invoke(target, new[] { valueToSet });
            }

        }

    }


}
