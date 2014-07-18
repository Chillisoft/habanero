using System;
using System.Collections.Generic;

namespace Habanero.Base
{
    /// <summary>
    /// Abstract base class for Function Query Fields. The idea is that derivatives need to
    /// do as little of the heavy lifting as possible
    /// </summary>
    public abstract class FunctionQueryField : QueryField, ISelfFormattingField
    {
        protected string _functionName;
        protected object[] _parameters;


        /// <summary>
        /// Abstract constructor: you should only be interested if you're deriving from this class
        /// </summary>
        /// <param name="functionName">Name of the function to call in the output SQL</param>
        /// <param name="parameters">list of parameters to the function. Currently supports strings for column names and QueryFields</param>
        /// <exception cref="ArgumentException">Will throw argument exception if the function name is not supplied</exception>
        public FunctionQueryField(string functionName, params object[] parameters): base(GetPropertyNameFor(functionName), GetFieldNameFrom(parameters), null)
        {
            if (string.IsNullOrEmpty(functionName)) throw new ArgumentException("FunctionQueryField: function name not supplied", "functionName");
            _functionName = functionName;
            _parameters = parameters;
        }

        private static string GetFieldNameFrom(object[] parameters)
        {
            return string.Join(",", parameters);
        }
        private static string GetPropertyNameFor(string functionName)
        {
            return (functionName ?? "unknown").ToUpper() + "()";
        }

        public override string GetFormattedStringWith(ISqlFormatter formatter, IDictionary<string, string> aliases)
        {
            var parts = new List<string>();
            parts.Add(_functionName);
            parts.Add("(");
            AddParametersTo(parts, formatter, aliases);
            parts.Add(")");
            return string.Join("", parts);
        }

        private void AddParametersTo(List<string> parts, ISqlFormatter formatter, IDictionary<string, string> aliases)
        {
            var parameterParts = new List<string>();
            foreach (var parameter in _parameters)
            {
                if (AddStringParameter(parameter, parameterParts, formatter)) continue;
                AddQueryFieldParameter(parameter, parameterParts, formatter, aliases);
            }
            parts.Add(string.Join(",", parameterParts));
        }

        private bool AddQueryFieldParameter(object parameter, List<string> parts, ISqlFormatter formatter, IDictionary<string, string> aliases)
        {
            var queryField = parameter as QueryField;
            if (queryField == null) return false;
            var quotedSource = QuoteSource(queryField.Source, formatter, aliases);
            var quotedField = queryField.FieldName == "*" ? "*" : formatter.DelimitField(queryField.FieldName);
            var field = string.Join(string.Empty, new[] { quotedSource, quotedField });
            parts.Add(field);
            return true;
        }

        private string QuoteSource(Source source, ISqlFormatter withFormatter, IDictionary<string, string> aliases)
        {
            if (source == null) return string.Empty;
            var sourceName = aliases.ContainsKey(source.Name) ? aliases[source.Name] : source.Name;
            return withFormatter.DelimitTable(sourceName) + ".";
        }

        private bool AddStringParameter(object parameter, List<string> parts, ISqlFormatter formatter)
        {
            if (parameter is string)
            {
                var stringParameter = (parameter as string) ?? "";
                parts.Add("'" + stringParameter.Replace("'", "''") + "'");
                return true;
            }
            return false;
        }

    }
}