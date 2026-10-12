using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nyxpiri.ULTRAKILL.NyxLib.Reflection;

public static class MethodAccessExtensions
{
    public static ReturnType CallMethod<ReturnType>(this object instance, string methodName, object[] arguments = null, Type[] types = null) => (ReturnType)(CallMethod(instance.GetType(), instance, methodName, arguments, types));
    public static object CallMethod(this object instance, string methodName, object[] arguments = null, Type[] types = null) => CallMethod(instance.GetType(), instance, methodName, arguments, types);

    public static object CallMethod(Type type, object instance, string methodName, object[] arguments = null, Type[] types = null)
    {
        if (!_methods.TryGetValue(type, out var typeMethods))
        {
            typeMethods = [];
            _methods[type] = typeMethods;
        }

        if (!typeMethods.TryGetValue(methodName, out var nameMethods))
        {
            nameMethods = [];
            typeMethods[methodName] = nameMethods;
        }

        int typesLen = (types?.Length).GetValueOrDefault(0);
        int argumentsLength = (arguments?.Length).GetValueOrDefault(0);

        var method = nameMethods.FirstOrDefault((m) =>
        {
            if (m.GetParameters().Length != typesLen)
            {
                return false;
            }

            if (typesLen == 0)
            {
                return true;
            }

            var candidateParams = m.GetParameters();
            for (int i = 0; i < candidateParams.Length; i++)
            {
                ParameterInfo p = candidateParams[i];

                if (p.ParameterType != types[i])
                {
                    return false;
                }
            }

            return true;
        });

        if (method == null)
        {
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic;

            if (instance == null)
            {
                bindingFlags |= BindingFlags.Static;
            }
            else
            {
                bindingFlags |= BindingFlags.Instance;
            }

            if (types == null)
            {
                method = type.GetMethod(methodName, bindingFlags);
            }
            else
            {
                method = type.GetMethod(methodName, bindingFlags, Type.DefaultBinder, types, null);
            }

            nameMethods.Add(method);
        }

        Assert.IsNotNull(method);

        var methodParams = method.GetParameters();

        if (methodParams.Length > argumentsLength)
        {
            var prevArgLen = argumentsLength;
            arguments = new object[methodParams.Length];
            for (int i = prevArgLen; i < arguments.Length; i++)
            {
                arguments[i] = methodParams[i].DefaultValue;
            }
        }

        return method.Invoke(instance, arguments);
    }

    private static Dictionary<Type, Dictionary<string, List<MethodInfo>>> _methods = [];
}