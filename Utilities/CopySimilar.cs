using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class CopySimilar
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        public static void CopyAll(object source, object target)
        {
            if ((null == source) || (null == target))
            {
                return;
            }
            SetProperties(source, target);
            SetFields(source, target);
        }

        public static void SetProperties(object source, object target)
        {
            if ((null == source)||(null == target))
            {
                return;
            }
            Type targetType = target.GetType();
            foreach (PropertyInfo prop in source.GetType().GetProperties())
            {
                PropertyInfo targetProp = targetType.GetProperty(prop.Name);
                if (null == targetProp)
                {
                    continue;
                }
                MethodInfo propGetter = prop.GetGetMethod();
                MethodInfo propSetter = targetProp.GetSetMethod();
                Type targetPropType = targetProp.GetType();
                var valueToSet = propGetter.Invoke(source, null);
                propSetter.Invoke(target, new[] { valueToSet });
            }
        }

        public static void SetFields(object source, object target)
        {
            if ((null == source) || (null == target))
            {
                return;
            }
            Type targetType = target.GetType();
            foreach (FieldInfo prop in source.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                try
                {
                    PropertyInfo targetProp = targetType.GetProperty(prop.Name);
                    if (null == targetProp)
                    {
                        continue;
                    }
                    MethodInfo propSetter = targetProp.GetSetMethod();
                    Type targetPropType = targetProp.GetType();
                    var valueToSet = prop.GetValue(source);
                    propSetter.Invoke(target, new[] { valueToSet });
                }
                catch (Exception e)
                {
                    logger.Error("Error copying parameter {0} from {1} to {2}", prop.Name, source.GetType().Name, target.GetType().Name);
                    logger.Error(e.Message);
                }
            }

        }

    }


}
