using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LibLoader.Helpers
{
    public class ParseArgs
    {
		// Retrieve a parameter value if it exists 
		// (overriding C# indexer property)
		public Dictionary<string, string> Arguments { get; set; }

		// Constructor
		public ParseArgs(string[] args)
        {
	        IList<Pair<string, string>> parameters = new List<Pair<string, string>>();

            Regex spliter = new Regex(@"^-{1,2}|^/|=|:", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            Regex remover = new Regex(@"^['""]?(.*?)['""]?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            string parameter = null;

	        // Valid parameters forms:
            // {-,/,--}param{ ,=,:}((",')value(",'))
            // Examples: 
            // -param1 value1 --param2 /param3:"Test-:-work" 
            //   /param4=happy -param5 '--=nice=--'

            foreach (string text in args)
            {
	            // Look for new parameters (-,/ or --) and a
                // possible enclosed value (=,:)
	            var parts = spliter.Split(text, 3);

	            switch (parts.Length)
                {
                    // Found a value (for the last parameter 
                    // found (space separator))
                    case 1:
                        if (parameter != null)
                        {
                            parts[0] = remover.Replace(parts[0], "$1");

                            if (!parameters.Contains(new Pair<string, string>(parameter, parts[0])))
                            {
                                parameters.Add(new Pair<string, string>(parameter, parts[0]));
                            }

                            parameter = null;
                        }
                        // else Error: no parameter waiting for a value (skipped)
                        break;

                    // Found just a parameter
                    case 2:
                        // The last parameter is still waiting. 
                        // With no value, set it to true.
                        if (parameter != null)
                        {
                            if (!parameters.Contains(new Pair<string, string>(parameter, "true"))) parameters.Add(new Pair<string, string>(parameter, "true"));
                        }
                        parameter = parts[1];
                        break;

                    // Parameter with enclosed value
                    case 3:
                        // The last parameter is still waiting. 
                        // With no value, set it to true.
                        if (parameter != null)
                        {
                            if (!parameters.Contains(new Pair<string, string>(parameter, "true"))) parameters.Add(new Pair<string, string>(parameter, "true"));
                        }

                        parameter = parts[1];

                        parts[2] = remover.Replace(parts[2], "$1");

                        // Remove possible enclosing characters (",')
                        if (!parameters.Contains(new Pair<string, string>(parameter, parts[2])))
                        {
                            parameters.Add(new Pair<string, string>(parameter, parts[2]));
                        }

                        parameter = null;
                        break;
                }
            }
	        // In case a parameter is still waiting
            if (parameter != null)
            {
                if (!parameters.Contains(new Pair<string, string>(parameter, "true"))) parameters.Add(new Pair<string, string>(parameter, "true"));
            }

            ReprocessParametersAsDictionary(parameters);
        }

        private void ReprocessParametersAsDictionary(IList<Pair<string, string>> parameters)
        {
            Arguments = new Dictionary<string, string>();

            if(parameters==null || parameters.Count < 1)
            {
                return;
            }

            foreach (var parameter in parameters)
            {
                if(!Arguments.ContainsKey(parameter.First))
                {
                    Arguments.Add(parameter.First.ToLower(), parameter.Second);
                }
            }

        }

    }

    public struct Pair<T, TK>
    {
        private T _first;
        private TK _second;


        public Pair(T first, TK second)
        {
            _first = first;
            _second = second;
        }


        public T First
        {
            get { return _first; }
            set { _first = value; }
        }

        public TK Second
        {
            get { return _second; }
            set { _second = value; }
        }        
    }
}