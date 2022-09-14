using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

public static class GroupCache {
    private delegate T ObjectActivator<T>(params object[] args);

    private static readonly Dictionary<Type, Delegate> _activators = new Dictionary<Type, Delegate>();

    private static readonly Dictionary<Type, ConstructorInfo> _constructor = new Dictionary<Type, ConstructorInfo>();

    private static ConstructorInfo GetConstructorCached(this Type type) {
        _constructor.TryGetValue(type, out var constructorInfos);
        if (constructorInfos == null) {
            constructorInfos = type.GetConstructors().First();
            _constructor[type] = constructorInfos;
        }

        return constructorInfos;
    }

    private static ObjectActivator<T> GetActivatorCached<T>() {
        _activators.TryGetValue(typeof(T), out var activator);
        if (activator == null) {
            activator = GetActivator<T>(typeof(T).GetConstructorCached());
            _activators[typeof(T)] = activator;
        }

        return activator as ObjectActivator<T>;
    }

    public static T CreateInstance<T>() where T : class, new() {

        //ObjectActivator<T> createdActivator = GetActivatorCached<T>();


        //T instance = createdActivator();
        return new T();
    }

    private static ObjectActivator<T> GetActivator<T>(ConstructorInfo ctor) {



        Type type = ctor.DeclaringType;
        ParameterInfo[] paramsInfo = ctor.GetParameters();

        //create a single param of type object[]
        ParameterExpression param =
            Expression.Parameter(typeof(object[]), "args");

        Expression[] argsExp =
            new Expression[paramsInfo.Length];

        //pick each arg from the params array 
        //and create a typed expression of them
        for (int i = 0; i < paramsInfo.Length; i++) {
            Expression index = Expression.Constant(i);
            Type paramType = paramsInfo[i].ParameterType;

            Expression paramAccessorExp =
                Expression.ArrayIndex(param, index);

            Expression paramCastExp =
                Expression.Convert(paramAccessorExp, paramType);

            argsExp[i] = paramCastExp;
        }

        //make a NewExpression that calls the
        //ctor with the args we just created
        NewExpression newExp = Expression.New(ctor, argsExp);

        //create a lambda with the New
        //Expression as body and our param object[] as arg
        LambdaExpression lambda =
            Expression.Lambda(typeof(ObjectActivator<T>), newExp, param);

        //compile it
        ObjectActivator<T> compiled = (ObjectActivator<T>)lambda.Compile();
        return compiled;
    }
}