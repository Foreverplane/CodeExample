using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

public static class SetterCache {
    private static readonly Dictionary<FieldInfo, Action<object, object>> _cacheActions = new Dictionary<FieldInfo, Action<object, object>>();
    public static void SetValueCached(this FieldInfo fieldInfo, object target, object value) {

        _cacheActions.TryGetValue(fieldInfo, out var action);
        if (action == null)
        {
            var obj = typeof(object);
            ParameterExpression targetExp = Expression.Parameter(obj, "target");
            ParameterExpression valueExp = Expression.Parameter(obj, "value");

            var convertedTarget = Expression.Convert(targetExp, fieldInfo.DeclaringType);
            var convertedValue = Expression.Convert(valueExp, fieldInfo.FieldType);

            MemberExpression fieldExp = Expression.Field(convertedTarget, fieldInfo);
            BinaryExpression assignExp = Expression.Assign(fieldExp, convertedValue);


            action = Expression.Lambda<Action<object, object>>(assignExp, targetExp, valueExp).Compile();


            _cacheActions[fieldInfo] = action;
        }

        action(target, value);
    }
}