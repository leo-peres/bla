using System;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public static class ReflectionUtils {

    public static string[] GetPropertyNames<T>() {

        var type = typeof(T);

        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        return properties.Select(prop => prop.Name).ToArray();

    }

    public static Func<T, S>[] BuildSelectors<T, S>() {

        var typeT = typeof(T);
        var typeS = typeof(S);

        var properties = typeT.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => typeS.IsAssignableFrom(p.PropertyType));

        return properties.Select(prop => (Func<T, S>)(instance => (S)prop.GetValue(instance))).ToArray();

    }

}
