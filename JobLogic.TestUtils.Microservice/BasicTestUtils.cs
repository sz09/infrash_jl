using AutoFixture;
using AutoFixture.Kernel;
using FluentAssertions;
using JobLogic.Infrastructure.Microservice.Server;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace JobLogic.TestUtils.Microservice
{
    public static class MicroserviceTestUtils
    {

        public static void TestAllContract_ShouldHaveSingleValidHandler(Type contractWithoutReturnType, Type contractWithReturnType, Assembly handlerAsm, Type baseContractType, Type baseEventType)
        {
            var contractYypeToGetAsm = contractWithoutReturnType ?? contractWithReturnType;
            var allTenancyContract = contractWithoutReturnType.Assembly.GetTypes().Where(x => !x.IsAbstract && (
                x.IsSubclassOf(contractWithoutReturnType) || x.IsSubclassOfRawGeneric(contractWithReturnType, out _)));

            var allHandler = handlerAsm.GetTypes().SelectMany(x => x.GetInterfaces())
                .Where(x => x.IsGenericType && (x.GetGenericTypeDefinition() == typeof(IHandler<>) || x.GetGenericTypeDefinition() == typeof(IHandler<,>)));

            foreach (var handlerType in allHandler)
            {
                var genericArguments = handlerType.GetGenericArguments();
                if (genericArguments.Any())
                {
                    var messageType = genericArguments.First();
                    (messageType.IsSubclassOf(baseContractType) || messageType.IsSubclassOf(baseEventType))
                        .Should()
                        .BeTrue($"the message type {messageType.FullName} should be a subclass of {baseContractType.Name} or {baseEventType.Name} in {handlerType.FullName}");
                }
            }

            foreach (var contractType in allTenancyContract)
            {
                Type handler = null;
                if (contractType.IsSubclassOfRawGeneric(contractWithReturnType, out Type[] genericTypeArguments))
                {
                    handler = typeof(IHandler<,>).MakeGenericType(contractType, genericTypeArguments.First());
                }
                else
                {
                    handler = typeof(IHandler<>).MakeGenericType(contractType);
                }
                Func<Type> assertFunc = () => allHandler.Single(x => x == handler);
                assertFunc.Should().NotThrow($"{handler} Must be single");
            }
        }

        static bool IsSubclassOfRawGeneric(this Type toCheck, Type generic, out Type[] genericTypeArguments)
        {
            genericTypeArguments = null;
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    genericTypeArguments = toCheck.GenericTypeArguments;
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }


        public static void TestAllContract_ShouldSerializedAndDeserializedCorrectly(Type contractWithoutReturnType, Type contractWithReturnType, bool isIgnoreRecursiveAutoFixture = false)
        {
            var typeToGetAsm = contractWithoutReturnType ?? contractWithReturnType;
            var allTenancyContract = typeToGetAsm.Assembly.GetTypes().Where(x => !x.IsAbstract && (
                x.IsSubclassOf(contractWithoutReturnType)
                || x.IsSubclassOfRawGeneric(contractWithReturnType, out _)));

            Fixture f = new Fixture();
            if (isIgnoreRecursiveAutoFixture) //TODO: temporary solution for circular reference 
            {
                f.Behaviors.Remove(new ThrowingRecursionBehavior());
                f.Behaviors.Add(new OmitOnRecursionBehavior());
            }
            foreach (var contractType in allTenancyContract)
            {

                if (contractType.GetProperties().Any())
                {
                    var contractObj = new SpecimenContext(f).Resolve(contractType);
                    var strSpecimen = JsonConvert.SerializeObject(contractObj);
                    var newSpecimen = JsonConvert.DeserializeObject(strSpecimen, contractType);
                    newSpecimen.Should().BeEquivalentTo(contractObj);
                }
            }
        }

        public static void TestFunction_ShouldHasDisableAttributeOnClassLevelOnly(Assembly assembly, string timerDisableSettingName = "JL_FUNCTION_TIMER_Disable",
            string busDisableSettingName = "JL_FUNCTION_BUS_Disable")
        {
            var functionMethods = assembly.GetTypes()
                    .SelectMany(x => x.GetMethods())
                    .Where(x => x.GetCustomAttributes(typeof(FunctionNameAttribute), true).Any());

            var timerFunctions = functionMethods.Where(x => x.GetParameters()
                .Any(x => x.GetCustomAttributes(typeof(TimerTriggerAttribute), true).Any()));

            foreach (var v in timerFunctions)
            {
                v.Should().NotBeDecoratedWith<DisableAttribute>();

                v.DeclaringType.Should()
                    .BeDecoratedWith<DisableAttribute>(x =>
                        x.SettingName == timerDisableSettingName);
            }


            var busFunctions = functionMethods.Where(x => x.GetParameters()
                .Any(x => x.GetCustomAttributes(typeof(ServiceBusTriggerAttribute), true).Any()));

            foreach (var v in busFunctions)
            {
                v.Should().NotBeDecoratedWith<DisableAttribute>();

                v.DeclaringType.Should()
                    .BeDecoratedWith<DisableAttribute>(x =>
                        x.SettingName == busDisableSettingName);
            }
        }

        public readonly static string[] ValidSettingKeys = new string[]
        {
            "AzureWebJobsStorage",
            "FUNCTIONS_WORKER_RUNTIME",
            "JL_FUNCTION_BUS_Disable",
            "JL_FUNCTION_TIMER_Disable",
            "APPINSIGHTS_INSTRUMENTATIONKEY",
            "ServiceBusNamespace",
            "JLAzureAppConfigurationConnStr",
            "JL_ENVIRONMENT",
            "CommandQueueNameSuffix",
            "SubscriptionNotationSuffix"
        };

        public static void TestLocalHostSetting_ShouldValid(params string[] moreValidSettingKeys)
        {
            var validSettingKeys = ValidSettingKeys.ToList();
            validSettingKeys.AddRange(moreValidSettingKeys);

            var localSettingFile = JsonConvert.DeserializeObject<JToken>(File.ReadAllText("local.settings.json"));
            var valueDict = localSettingFile["Values"].ToObject<Dictionary<string, string>>();

            valueDict["JL_FUNCTION_BUS_Disable"].Should().Be("1");
            valueDict["JL_FUNCTION_TIMER_Disable"].Should().Be("1");
            valueDict["JL_ENVIRONMENT"].Should().Be("Development");

            var allKeys = valueDict.Select(x => x.Key).Where(x => !x.StartsWith("AzureFunctionsJobHost__"));
            allKeys.Except(validSettingKeys).Should().BeEmpty();
        }
    }
}
