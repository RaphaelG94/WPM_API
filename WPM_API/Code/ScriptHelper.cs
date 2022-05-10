using System;
using WPM_API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AutoMapper;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using WPM_API.Data.DataContext;
using NLog;
using Token = WPM_API.Data.DataContext.Entities.Token;
using System.Management.Automation.Language;

namespace WPM_API.Code
{
    public static class ScriptHelper
    {       
        public static List<ParameterViewModel> GetParametersFromScript(string script, string osType)
        {
            var parameters = new List<ParameterViewModel>();
            if (osType == "Windows")
            {
                var ast = Parser.ParseInput(script, out System.Management.Automation.Language.Token[] tokens, out ParseError[] errors);
                if (errors.Length != 0)
                {

                    Console.WriteLine("Errors: {0}", errors.Length);
                    foreach (var error in errors)
                    {
                        Console.WriteLine(error);
                    }
                    return null;
                }

                if (ast.ParamBlock != null)
                {
                    foreach (var paramBlockParameter in ast.ParamBlock.Parameters)
                    {
                        parameters.Add(new ParameterViewModel()
                        {
                            DisplayName = paramBlockParameter.Name.ToString().Substring(1),
                            //                   Description = paramBlockParameter.Attributes.ToString(),
                            Key = paramBlockParameter.Name.ToString(),
                            Value = paramBlockParameter.DefaultValue.ToString().Replace("\"", String.Empty).Replace("'", String.Empty)
                        });
                    }
                }

                return parameters;
            } else
            {                
                // TODO: Fetch parameters from .sh script               
                int indexParamStart = script.IndexOf("###param(");
                int indexParamEnd = script.IndexOf("###)");
                string temp = script.Substring(indexParamStart, (indexParamEnd - indexParamStart));
                var paramArray = temp.Split("\n");
                foreach (string el in paramArray)
                {
                    if (el != "###param(" && el != "" && el != "###)")
                    {
                        var paramData = el.Split("=");
                        parameters.Add(new ParameterViewModel()
                        {
                            DisplayName = paramData[0],
                            Key = paramData[0],
                            Value = paramData[1].Replace("\"", String.Empty)
                        }) ;
                    }
                }
                return parameters;
            }     
        }

        public static List<ParameterViewModel> PrefillParameterWithDefaults(string script,
            Dictionary<string, string> defaults, List<ParameterViewModel> parametersPrefilled, string origin, string osType)
        {
            List<ParameterViewModel> parameters;
            parameters = parametersPrefilled == null || parametersPrefilled.Count == 0
                ? GetParametersFromScript(script, osType)
                : parametersPrefilled;
            foreach (var param in parameters)
            {
                if (defaults.TryGetValue(param.Key, out string val))
                {
                    if (!string.IsNullOrWhiteSpace(val))
                        param.Value = val;
                    param.Origin = origin;
                }
            }
            return parameters;
        }

        public static List<ParameterViewModel> PrefillParameterWithDefaults(string script,
            List<AdvancedProperty> defaults, List<ParameterViewModel> parametersPrefilled, string origin, string osType)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            defaults.ForEach(x => values.Add(x.Name, x.Value));
            return PrefillParameterWithDefaults(script, values, parametersPrefilled, origin, osType);
        }

        public static List<ParameterViewModel> PrefillParameterWithDefaults(string script,
            List<ClientParameter> defaults, List<ParameterViewModel> parametersPrefilled, string origin, string osType)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            try
            {
                defaults.ForEach(x => values.Add(x.ParameterName, x.Value));
            }
            catch
            {

            }
                return PrefillParameterWithDefaults(script, values, parametersPrefilled, origin, osType);
        }

        public static List<ParameterViewModel> PrefillParameterWithDefaults(string script, 
            List<Parameter> defaults, List<ParameterViewModel> parametersPrefilled, string origin, string osType)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            try
            {
                defaults.ForEach(x => values.Add(x.Key, x.Value));
            }
            catch
            {

            } 
            return PrefillParameterWithDefaults(script, values, parametersPrefilled, origin, osType);
        }
    }
}