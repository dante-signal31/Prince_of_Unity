using System.Reflection;

namespace Tests.PlayTests.Tools.Lang
{
    public class AccessPrivateHelper
    {
        /// <summary>
        /// Helper to call private methods while testing.
        /// </summary>
        /// <param name="instance">Object whose method we want to call.</param>
        /// <param name="methodName">Method to call.</param>
        /// <param name="methodParameters">Parameters to pass to method.</param>
        /// <returns>Whatever called method returns.</returns>
        public static object AccessPrivateMethod(object instance, string methodName, params object[] methodParameters)
        {
            MethodInfo dynMethod = instance.GetType().GetMethod(methodName, 
                BindingFlags.NonPublic | BindingFlags.Instance);
            if (dynMethod != null)
            {
                return dynMethod.Invoke(instance, methodParameters);
            }
            return null;
        }

        /// <summary>
        /// Helper to get private properties values while testing.
        /// </summary>
        /// <param name="instance">Object whose property we want to see.</param>
        /// <param name="fieldName">Property to see</param>
        /// <returns>Whatever value that property has.</returns>
        public static T GetPrivateField<T>(object instance, string fieldName)
        {
            FieldInfo dynAttribute = instance.GetType().GetField(fieldName, 
                BindingFlags.NonPublic | BindingFlags.Instance);
            return (T) dynAttribute.GetValue(instance);
        }
        
        /// <summary>
        /// Helper to set private properties values while testing.
        /// </summary>
        /// <param name="instance">Object whose property we want to set.</param>
        /// <param name="fieldName">Property to set.</param>
        /// <param name="value">Whatever value that property is going to have.</param>
        /// <typeparam name="T">Private field type.</typeparam>
        public static void SetPrivateField<T>(object instance, string fieldName, T value)
        {
            FieldInfo dynAttribute = instance.GetType().GetField(fieldName, 
                BindingFlags.NonPublic | BindingFlags.Instance);
            dynAttribute.SetValue(instance, value);
        }
    }
}