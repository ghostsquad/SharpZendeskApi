namespace SharpZendeskApi.Test.Common
{
    #region

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using CredentialManagement;

    using FluentAssertions;

    using RestSharp;

    using Xunit;

    #endregion

    /// <summary>
    ///     The test helpers.
    /// </summary>
    public static class TestHelpers
    {
        #region Constants

        public const string CredentialTargetFormat = "ZendeskEnd2EndTests-{0}";

        public const string CredentialUserNameExample = "emailaddress@@http://mydomain.zendesk.com/api/v2";

        #endregion

        #region Static Fields

        public static Type[] AcceptablePrimitiveTypes = { typeof(int), typeof(bool), typeof(string) };

        /// <summary>
        ///     The random.
        /// </summary>
        private static readonly Random SystemRandom = new Random();

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the random.
        /// </summary>
        public static Random Random
        {
            get
            {
                return SystemRandom;
            }
        }

        #endregion

        #region Public Methods and Operators

        public static ZendeskClientBase GetClient(
            ZendeskAuthenticationMethod authenticationMethod,
            bool useGoodPasswordToken = true)
        {
            try
            {
                var credKeyValue = GetTestCredential(authenticationMethod);
                var password = useGoodPasswordToken ? credKeyValue.Value.Password : Guid.Empty.ToString();

                var client = new ZendeskClient(
                    credKeyValue.Key, 
                    credKeyValue.Value.Username, 
                    password, 
                    authenticationMethod);

                return client;
            }
            catch (Exception)
            {
                var expectedTarget = string.Format(CredentialTargetFormat, authenticationMethod);

                var assertMessage =
                    string.Format(
                        "Unable to run {0} authentication End2End tests.\n\nCreate a new Generic Credential in Credential Manager with:\n\nTarget Address as: \n\n[{1}] and\n\nUsername in this format: \n\n[{2}] ", 
                        authenticationMethod, 
                        expectedTarget, 
                        CredentialUserNameExample);

                Assert.False(true, assertMessage);
            }

            return null;
        }

        // public static void AssertModelsEqual(IZendeskApiModel expected, object actual)
        // {
        // var expectedType = expected.GetType();
        // var actualType = actual.GetType();
        // Assert.AreEqual(expectedType, actualType);

        // var expectedProperties = expectedType.GetProperties().OrderBy(x => x.Name).ToList();
        // var actualProperties = actualType.GetProperties().OrderBy(x => x.Name).ToList();

        // for (var i = 0; i < expectedProperties.Count; i++)
        // {
        // var expectedValue = expectedProperties[i].GetValue(expected);
        // var actualValue = actualProperties[i].GetValue(actual);

        // var expectedValueAsIList = expectedValue as IList;

        // if (expectedValueAsIList != null)
        // {
        // var actualValueAsIList = actualValue as IList;
        // if (actualValueAsIList != null)
        // {
        // var actualValueAsIListGenericType = actualValueAsIList.GetType().GetGenericArguments().Single();

        // if (actualValueAsIListGenericType.GetInterfaces().Contains(typeof(IZendeskApiModel)))
        // {
        // Assert.AreEqual(actualValueAsIList.Count, expectedValueAsIList.Count);
        // for (var ii = 0; ii < actualValueAsIList.Count; ii++)
        // {
        // AssertModelsEqual((IZendeskApiModel)expectedValueAsIList[ii], (IZendeskApiModel)actualValueAsIList[ii]);
        // }
        // return;
        // }

        // CollectionAssert.AreEqual(expectedValueAsIList, actualValueAsIList);
        // return;
        // }
        // }

        // var expectedValueAsIZendeskApiModel = expectedValue as IZendeskApiModel;
        // if (expectedValueAsIZendeskApiModel != null)
        // {
        // var actualValueAsIZendeskApiModel = actualValue as IZendeskApiModel;
        // AssertModelsEqual(expectedValueAsIZendeskApiModel, actualValueAsIZendeskApiModel);
        // return;
        // }

        // Assert.AreEqual(expectedValue, actualValue);
        // }
        // }

        /// <summary>
        ///     The get member info.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <typeparam name="TObject">
        /// </typeparam>
        /// <typeparam name="TProperty">
        /// </typeparam>
        /// <returns>
        ///     The <see cref="MemberInfo" />.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        public static MemberInfo GetMemberInfo<TObject, TProperty>(Expression<Func<TObject, TProperty>> expression)
        {
            var member = expression.Body as MemberExpression;
            if (member != null)
            {
                return member.Member;
            }

            throw new ArgumentException("expression");
        }

        /// <summary>
        ///     The get member name.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string GetMemberName(Expression<Func<object>> expression)
        {
            var body = expression.Body as MemberExpression;
            if (body != null)
            {
                return body.Member.Name;
            }

            var op = ((UnaryExpression)expression.Body).Operand;
            return ((MemberExpression)op).Member.Name;
        }

        /// <summary>
        ///     The get model property name from api key.
        /// </summary>
        /// <param name="apiKey">
        ///     The api key.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public static string GetModelPropertyNameFromApiKey(string apiKey)
        {
            // http://theburningmonk.com/2010/08/dotnet-tips-string-totitlecase-extension-methods/
            // http://stackoverflow.com/questions/3386609/function-to-make-pascal-case-c
            return apiKey.ToTitleCase().Replace("_", string.Empty);
        }

        public static dynamic GetNewModelWithValuesDerivedFromJson(Type modelType, JsonObject jsonObject)
        {
            // http://stackoverflow.com/questions/752/get-a-new-object-instance-from-a-type
            var newObject = Activator.CreateInstance(modelType);

            SetPropertiesFromJsonObject(jsonObject, newObject);

            return newObject;
        }

        public static KeyValuePair<string, Credential> GetTestCredential(
            ZendeskAuthenticationMethod authenticationMethod)
        {
            var credTarget = string.Format(CredentialTargetFormat, authenticationMethod);

            var credential = new Credential { Target = credTarget };
            var loadResult = credential.Load();

            var exceptionMsg = string.Format("No credential exists for given target {0}", credTarget);

            if (!loadResult)
            {
                throw new InvalidOperationException(exceptionMsg);
            }

            var indexOfDoubleAt = credential.Username.IndexOf("@@", StringComparison.Ordinal);
            if (indexOfDoubleAt < 0)
            {
                throw new InvalidOperationException(exceptionMsg);
            }

            var keyValue = new KeyValuePair<string, Credential>(
                credential.Username.Substring(indexOfDoubleAt + 2), 
                credential);

            credential.Username = credential.Username.Substring(0, indexOfDoubleAt);

            return keyValue;
        }

        /// <summary>
        ///     The list convert.
        /// </summary>
        /// <param name="inputList">
        ///     The input list.
        /// </param>
        /// <param name="targetType">
        ///     The target type.
        /// </param>
        /// <typeparam name="T">
        ///     The list type to convert to.
        /// </typeparam>
        /// <returns>
        ///     The <see cref="object" />.
        /// </returns>
        public static object ListConvert<T>(IList inputList, Type targetType)
        {
            if (targetType == typeof(T))
            {
                return inputList.Cast<T>().ToList();
            }

            return null;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     The set properties from json object.
        /// </summary>
        /// <param name="jsonObject">
        ///     The json object.
        /// </param>
        /// <param name="targetObject">
        ///     The target object.
        /// </param>
        private static void SetPropertiesFromJsonObject(JsonObject jsonObject, object targetObject)
        {
            foreach (string key in jsonObject.Keys)
            {
                PropertyInfo newPropertyInfo = targetObject.GetType().GetProperty(GetModelPropertyNameFromApiKey(key));
                object newValue = jsonObject[key];

                SetPropertyOnObject(newPropertyInfo, targetObject, newValue);
            }
        }

        /// <summary>
        ///     The set property on object.
        /// </summary>
        /// <param name="rootPropertyInfo">
        ///     The root property info.
        /// </param>
        /// <param name="rootTargetObject">
        ///     The root target object.
        /// </param>
        /// <param name="rootSourceValue">
        ///     The root source value.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        private static void SetPropertyOnObject(
            PropertyInfo rootPropertyInfo, 
            object rootTargetObject, 
            object rootSourceValue)
        {
            var targetPropertyType = rootPropertyInfo.PropertyType;

            // we accept jsonobjects to represent complex types
            var rootSourceValueAsJsonObject = rootSourceValue as JsonObject;
            if (rootSourceValueAsJsonObject != null)
            {
                var targetIsComplex =
                    !(targetPropertyType == typeof(IList) || AcceptablePrimitiveTypes.Contains(targetPropertyType));

                var assertMessage =
                    string.Format(
                        "Source is is a complex Json Object, and target is not. Actual type: [{0}]", 
                        targetPropertyType);

                targetIsComplex.Should().BeTrue(assertMessage);

                object newObject = Activator.CreateInstance(targetPropertyType);

                SetPropertiesFromJsonObject(rootSourceValueAsJsonObject, newObject);

                rootPropertyInfo.SetValue(rootTargetObject, newObject);
                return;
            }

            // deal with lists
            var rootSourceValueAsIList = rootSourceValue as IList;
            if (rootSourceValueAsIList != null)
            {
                // http://stackoverflow.com/questions/4452590/c-sharp-get-the-item-type-for-a-generic-list
                var newCollectionGenericType = rootSourceValue.GetType().GetGenericArguments().Single();

                if (newCollectionGenericType == typeof(JsonObject))
                {
                    var targetPropertyGenericCollectionType = targetPropertyType.GetGenericArguments().Single();

                    // http://stackoverflow.com/questions/5909144/how-to-create-listt-instance-in-c-sharp-file-using-reflection
                    var constructorInfo =
                        typeof(List<>).MakeGenericType(targetPropertyGenericCollectionType)
                            .GetConstructor(Type.EmptyTypes);
                    if (constructorInfo != null)
                    {
                        var newComplexCollection = (IList)constructorInfo.Invoke(null);

                        foreach (object item in rootSourceValueAsIList)
                        {
                            var newObject = Activator.CreateInstance(targetPropertyGenericCollectionType);
                            SetPropertiesFromJsonObject(item.As<JsonObject>(), newObject);
                            newComplexCollection.Add(newObject);
                        }

                        rootPropertyInfo.SetValue(rootTargetObject, newComplexCollection);
                    }

                    return;
                }

                object newPrimitiveCollection = ListConvert<int>(rootSourceValueAsIList, newCollectionGenericType)
                                                ?? ListConvert<string>(rootSourceValueAsIList, newCollectionGenericType)
                                                ?? ListConvert<bool>(rootSourceValueAsIList, newCollectionGenericType);

                if (newPrimitiveCollection != null)
                {
                    rootPropertyInfo.SetValue(rootTargetObject, newPrimitiveCollection);
                    return;
                }

                throw new InvalidOperationException(
                    string.Format("Collection type of {0} is not supported!", newCollectionGenericType));
            }

            // some of the model values can accept nulls
            if (rootSourceValue == null)
            {
                if (targetPropertyType.IsTypeNullable())
                {
                    rootPropertyInfo.SetValue(rootTargetObject, null);
                    return;
                }

                throw new InvalidOperationException(
                    string.Format(
                        "value is null, and property [{0}] of type [{1}] is not nullable.", 
                        rootPropertyInfo.Name, 
                        targetPropertyType));
            }

            // dates may look like strings on the JsonSide, but maybe map to DateTime on the Model side.
            if (targetPropertyType == typeof(DateTime) || targetPropertyType == typeof(DateTime?))
            {
                DateTime valueAsDate;
                var parseResult = DateTimeExtensions.TryParseExactIso8601DateTime(
                    rootSourceValue.ToString(), 
                    out valueAsDate);
                if (!parseResult)
                {
                    throw new InvalidOperationException(
                        string.Format(
                            "Attempt to parse [{0}] to [{1}] for model property[{2}] failed.", 
                            rootSourceValue, 
                            targetPropertyType, 
                            rootPropertyInfo.Name));
                }

                rootPropertyInfo.SetValue(rootTargetObject, valueAsDate);
                return;
            }

            // accept primitive types
            if ((targetPropertyType == typeof(string) && rootSourceValue is string)
                || (targetPropertyType == typeof(bool) && rootSourceValue is bool)
                || (rootSourceValue is int && (targetPropertyType == typeof(int) || targetPropertyType == typeof(int?))))
            {
                rootPropertyInfo.SetValue(rootTargetObject, rootSourceValue);
                return;
            }

            // if we've arrive here, it means that the json test object was not constructed properly.
            // it should only use JsonObjects, int, string, bool, datetime and collections thereof
            throw new InvalidOperationException(
                string.Format("cannot use custom types! type found: [{0}]", rootSourceValue.GetType()));
        }

        #endregion
    }
}