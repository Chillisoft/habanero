// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
using System.Collections.Specialized;
using System.Globalization;
//using System.Linq;//TODO brett 08 Jun 2010: For 2_0
using System.Text;
using System.Text.RegularExpressions;
using Habanero.Base.Exceptions;
using Habanero.Base.Util;

namespace Habanero.Util
{
    /// <summary>
    /// Provides a collection of utilities for strings
    /// </summary>
    public static class StringUtilities
    {
        private static readonly NameValueCollection myPluralRules;
        private static readonly string[] myUnaffectedPlural;
        private static readonly NameValueCollection myIrregularPlural;
        private static readonly NameValueCollection mySingularRules;
        private static readonly string[] myUnaffectedSingular;
        private static readonly NameValueCollection myIrregularSingular;
        static StringUtilities()
        {
            myPluralRules = new NameValueCollection();
            myPluralRules["(s)tatus$"] = "$1tatuses";
            myPluralRules["^(ox)$"] = "$1en"; // ox
            myPluralRules["([m|l])ouse$"] = "$1ice"; // mouse, louse
            myPluralRules["(matr|vert|ind)ix|ex$"] = "$1ices"; // matrix, vertex, index
            myPluralRules["(x|ch|ss|sh)$"] = "$1es"; // search, switch, fix, box, process, address
            myPluralRules["([^aeiouy]|qu)y$"] = "$1ies"; // query, ability, agency
            myPluralRules["(hive)$"] = "$1s"; // archive, hive
            myPluralRules["(?:([^f])fe|([lr])f)$"] = "$1$2ves"; // half, safe, wife
            myPluralRules["sis$"] = "ses"; // basis, diagnosis
            myPluralRules["([ti])um$"] = "$1a"; // datum, medium
            myPluralRules["(p)erson$"] = "$1eople"; // person, salesperson
            myPluralRules["(m)an$"] = "$1en"; // man, woman, spokesman
            myPluralRules["(c)hild$"] = "$1hildren"; // child
            myPluralRules["(buffal|tomat)o$"] = "$1oes"; // buffalo, tomato, (potato ?)
            myPluralRules["us$"] = "uses"; // us
            myPluralRules["(alias)"] = "$1es"; // alias
            myPluralRules["(octop|vir)us$"] = "$1i";
            // (never use ?)octopus, virus - virus has no defined plural (according to Latin/dictionary.com), but viri is better than viruses/viruss
            myPluralRules["(ax|cri|test)is$"] = "$1es"; // axis, crisis
            myPluralRules["s$"] = "s"; // no change (compatibility)
            myPluralRules["$"] = "s";

            myUnaffectedPlural = new[]
                                     {
                                         "^(.*[nrlm]ese)$", "^(.*deer)$", "^(.*fish)$", "^(.*measles)$", "^(.*ois)$",
                                         "^(.*pox)$", "^(.*sheep)$", "^(Amoyese)$", "^(bison)$", "^(Borghese)$",
                                         "^(bream)$", "^(breeches)$", "^(britches)$", "^(buffalo)$", "^(cantus)$",
                                         "^(carp)$", "^(chassis)$", "^(clippers)$", "^(cod)$", "^(coitus)$", "^(Congoese)$"
                                         , "^(contretemps)$", "^(corps)$", "^(debris)$", "^(diabetes)$", "^(djinn)$",
                                         "^(eland)$", "^(elk)$", "^(equipment)$", "^(Faroese)$", "^(flounder)$",
                                         "^(Foochowese)$", "^(gallows)$", "^(Genevese)$", "^(Genoese)$", "^(Gilbertese)$",
                                         "^(graffiti)$", "^(headquarters)$", "^(herpes)$", "^(hijinks)$",
                                         "^(Hottentotese)$", "^(information)$", "^(innings)$", "^(jackanapes)$",
                                         "^(Kiplingese)$", "^(Kongoese)$", "^(Lucchese)$", "^(mackerel)$", "^(Maltese)$",
                                         "^(mews)$", "^(moose)$", "^(mumps)$", "^(Nankingese)$", "^(news)$", "^(nexus)$",
                                         "^(Niasese)$", "^(Pekingese)$", "^(Piedmontese)$", "^(pincers)$", "^(Pistoiese)$",
                                         "^(pliers)$", "^(Portuguese)$", "^(proceedings)$", "^(rabies)$", "^(rice)$",
                                         "^(rhinoceros)$", "^(salmon)$", "^(Sarawakese)$", "^(scissors)$",
                                         "^(sea[- ]bass)$", "^(series)$", "^(Shavese)$", "^(shears)$", "^(siemens)$",
                                         "^(species)$", "^(swine)$", "^(testes)$", "^(trousers)$", "^(trout)$", "^(tuna)$",
                                         "^(Vermontese)$", "^(Wenchowese)$", "^(whiting)$", "^(wildebeest)$",
                                         "^(Yengeese)$"
                                     };

            myIrregularPlural = new NameValueCollection();
            myIrregularPlural[@"(.*)\b(atlas)$"] = "atlases";
            myIrregularPlural[@"(.*)\b(beef)$"] = "beefs";
            myIrregularPlural[@"(.*)\b(brother)$"] = "brothers";
            myIrregularPlural[@"(.*)\b(child)$"] = "children";
            myIrregularPlural[@"(.*)\b(corpus)$"] = "corpuses";
            myIrregularPlural[@"(.*)\b(cow)$"] = "cows";
            myIrregularPlural[@"(.*)\b(ganglion)$"] = "ganglions";
            myIrregularPlural[@"(.*)\b(genie)$"] = "genies";
            myIrregularPlural[@"(.*)\b(genus)$"] = "genera";
            myIrregularPlural[@"(.*)\b(graffito)$"] = "graffiti";
            myIrregularPlural[@"(.*)\b(hoof)$"] = "hoofs";
            myIrregularPlural[@"(.*)\b(loaf)$"] = "loaves";
            myIrregularPlural[@"(.*)\b(man)$"] = "men";
            myIrregularPlural[@"(.*)\b(money)$"] = "monies";
            myIrregularPlural[@"(.*)\b(mongoose)$"] = "mongooses";
            myIrregularPlural[@"(.*)\b(move)$"] = "moves";
            myIrregularPlural[@"(.*)\b(mythos)$"] = "mythoi";
            myIrregularPlural[@"(.*)\b(numen)$"] = "numina";
            myIrregularPlural[@"(.*)\b(occiput)$"] = "occiputs";
            myIrregularPlural[@"(.*)\b(octopus)$"] = "octopuses";
            myIrregularPlural[@"(.*)\b(opus)$"] = "opuses";
            myIrregularPlural[@"(.*)\b(ox)$"] = "oxen";
            myIrregularPlural[@"(.*)\b(penis)$"] = "penises";
            myIrregularPlural[@"(.*)\b(person)$"] = "people";
            myIrregularPlural[@"(.*)\b(sex)$"] = "sexes";
            myIrregularPlural[@"(.*)\b(soliloquy)$"] = "soliloquies";
            myIrregularPlural[@"(.*)\b(testis)$"] = "testes";
            myIrregularPlural[@"(.*)\b(trilby)$"] = "trilbys";
            myIrregularPlural[@"(.*)\b(turf)$"] = "turfs";

            mySingularRules = new NameValueCollection();
            mySingularRules["(s)tatuses$"] = "$1tatus";
            mySingularRules["(matr)ices$"] = "$1ix";
            mySingularRules["(vert|ind)ices$"] = "$1ex";
            mySingularRules["^(ox)en"] = "$1";
            mySingularRules["(alias)es$"] = "$1";
            mySingularRules["([octop|vir])i$"] = "$1us";
            mySingularRules["(cris|ax|test)es$"] = "$1is";
            mySingularRules["(shoe)s$"] = "$1";
            mySingularRules["(o)es$"] = "$1";
            mySingularRules["uses$"] = "us";
            mySingularRules["([m|l])ice$"] = "$1ouse";
            mySingularRules["(x|ch|ss|sh)es$"] = "$1";
            mySingularRules["(m)ovies$"] = "$1ovie";
            mySingularRules["(s)eries$"] = "$1eries";
            mySingularRules["([^aeiouy]|qu)ies$"] = "$1y";
            mySingularRules["([lr])ves$"] = "$1f"; // ?
            mySingularRules["(tive)s$"] = "$1";
            mySingularRules["(hive)s$"] = "$1";
            mySingularRules["(drive)s$"] = "$1";
            mySingularRules["([^f])ves$"] = "$1fe";
            mySingularRules["(^analy)ses$"] = "$1sis";
            mySingularRules["((a)naly|(b)a|(d)iagno|(p)arenthe|(p)rogno|(s)ynop|(t)he)ses$"] = "$1$2sis";
            mySingularRules["([ti])a$"] = "$1um";
            mySingularRules["(p)eople$"] = "$1erson";
            mySingularRules["(m)en$"] = "$1an";
            mySingularRules["(c)hildren$"] = "$1hild";
            mySingularRules["(n)ews$"] = "$1ews";
            mySingularRules["s$"] = "";

            myUnaffectedSingular = new string[]
                                       {
                                           "^(.*[nrlm]ese)$", "^(.*deer)$", "^(.*fish)$", "^(.*measles)$", "^(.*ois)$",
                                           "^(.*pox)$", "^(.*sheep)$", "^(.*us)$", "^(.*ss)$", "^(Amoyese)$", "^(bison)$",
                                           "^(Borghese)$", "^(bream)$", "^(breeches)$", "^(britches)$", "^(buffalo)$",
                                           "^(cantus)$", "^(carp)$", "^(chassis)$", "^(clippers)$", "^(cod)$", "^(coitus)$"
                                           , "^(Congoese)$", "^(contretemps)$", "^(corps)$", "^(debris)$", "^(diabetes)$",
                                           "^(djinn)$", "^(eland)$", "^(elk)$", "^(equipment)$", "^(Faroese)$",
                                           "^(flounder)$", "^(Foochowese)$", "^(gallows)$", "^(Genevese)$", "^(Genoese)$",
                                           "^(Gilbertese)$", "^(graffiti)$", "^(headquarters)$", "^(herpes)$",
                                           "^(hijinks)$", "^(Hottentotese)$", "^(information)$", "^(innings)$",
                                           "^(jackanapes)$", "^(Kiplingese)$", "^(Kongoese)$", "^(Lucchese)$",
                                           "^(mackerel)$", "^(Maltese)$", "^(mews)$", "^(moose)$", "^(mumps)$",
                                           "^(Nankingese)$", "^(news)$", "^(nexus)$", "^(Niasese)$", "^(Pekingese)$",
                                           "^(Piedmontese)$", "^(pincers)$", "^(Pistoiese)$", "^(pliers)$",
                                           "^(Portuguese)$", "^(proceedings)$", "^(rabies)$", "^(rice)$", "^(rhinoceros)$",
                                           "^(salmon)$", "^(Sarawakese)$", "^(scissors)$", "^(sea[- ]bass)$", "^(series)$",
                                           "^(Shavese)$", "^(shears)$", "^(siemens)$", "^(species)$", "^(swine)$",
                                           "^(testes)$", "^(trousers)$", "^(trout)$", "^(tuna)$", "^(Vermontese)$",
                                           "^(Wenchowese)$", "^(whiting)$", "^(wildebeest)$", "^(Yengeese)$"
                                       };

            myIrregularSingular = new NameValueCollection();
            myIrregularSingular[@"(.*)\b(atlases)$"] = "atlas";
            myIrregularSingular[@"(.*)\b(beefs)$"] = "beef";
            myIrregularSingular[@"(.*)\b(brothers)$"] = "brother";
            myIrregularSingular[@"(.*)\b(children)$"] = "child";
            myIrregularSingular[@"(.*)\b(corpuses)$"] = "corpus";
            myIrregularSingular[@"(.*)\b(cows)$"] = "cow";
            myIrregularSingular[@"(.*)\b(ganglions)$"] = "ganglion";
            myIrregularSingular[@"(.*)\b(genies)$"] = "genie";
            myIrregularSingular[@"(.*)\b(genera)$"] = "genus";
            myIrregularSingular[@"(.*)\b(graffiti)$"] = "graffito";
            myIrregularSingular[@"(.*)\b(hoofs)$"] = "hoof";
            myIrregularSingular[@"(.*)\b(loaves)$"] = "loaf";
            myIrregularSingular[@"(.*)\b(men)$"] = "man";
            myIrregularSingular[@"(.*)\b(monies)$"] = "money";
            myIrregularSingular[@"(.*)\b(mongooses)$"] = "mongoose";
            myIrregularSingular[@"(.*)\b(moves)$"] = "move";
            myIrregularSingular[@"(.*)\b(mythoi)$"] = "mythos";
            myIrregularSingular[@"(.*)\b(numina)$"] = "numen";
            myIrregularSingular[@"(.*)\b(occiputs)$"] = "occiput";
            myIrregularSingular[@"(.*)\b(octopuses)$"] = "octopus";
            myIrregularSingular[@"(.*)\b(opuses)$"] = "opus";
            myIrregularSingular[@"(.*)\b(oxen)$"] = "ox";
            myIrregularSingular[@"(.*)\b(penises)$"] = "penis";
            myIrregularSingular[@"(.*)\b(people)$"] = "person";
            myIrregularSingular[@"(.*)\b(sexes)$"] = "sex";
            myIrregularSingular[@"(.*)\b(soliloquies)$"] = "soliloquy";
            myIrregularSingular[@"(.*)\b(testes)$"] = "testis";
            myIrregularSingular[@"(.*)\b(trilbys)$"] = "trilby";
            myIrregularSingular[@"(.*)\b(turfs)$"] = "turf";
        }


        static readonly Regex _guidFormat = new Regex(
            "^[A-Fa-f0-9]{32}$|" + 
            "^({|\\()?[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}(}|\\))?$|" +
            "^({)?[0xA-Fa-f0-9]{3,10}(, {0,1}[0xA-Fa-f0-9]{3,6}){2}, {0,1}({)([0xA-Fa-f0-9]{3,4}, {0,1}){7}[0xA-Fa-f0-9]{3,4}(}})$");

        /// <summary>
        /// Replaces single quotes with two single quotes in the given string
        /// </summary>
        /// <param name="value">The string to amend</param>
        /// <returns>Returns the reformatted string</returns>
        public static string ReplaceSingleQuotesWithTwo(string value)
        {
            return value.Replace("'", "''");
        }

        /// <summary>
        /// Replaces double quotes with two double quotes in the given string
        /// </summary>
        /// <param name="value">The string to amend</param>
        /// <returns>Returns the reformatted string</returns>
        public static string ReplaceDoubleQuotesWithTwo(string value)
        {
            return value.Replace("\"", "\"\"");
        }

        /// <summary>
        /// Breaks up a Pascal-cased string into sections that are divided
        /// by the given delimiter.  For instance, an input string of
        /// "PascalCase" and a delimiter of " " will give "Pascal Case"
        /// </summary>
        /// <param name="inputString">The string to delimit</param>
        /// <param name="delimiter">The delimiter to insert between
        /// sections, such as a space or comma</param>
        /// <returns>Returns the delimited string</returns>
        public static string DelimitPascalCase(string inputString, string delimiter)
        {
            string formatted = "";
            int counter = 0;
            if (inputString == null) return null;
            foreach (char currentChar in inputString)
            {
                char prevChar = new char();
                if (counter > 0)
                {
                    prevChar = inputString[counter - 1];
                }
                char nextChar = new char();
                if (counter < inputString.Length - 1)
                {
                    nextChar = inputString[counter + 1];
                }

                bool isUpperCase = Char.IsUpper(currentChar);
                bool previousLetterIsUpperCase = Char.IsUpper(prevChar);
                bool nextLetterIsLowerCase = Char.IsLower(nextChar);
                bool isNotASpace = !Char.IsWhiteSpace(currentChar);
                bool previousLetterIsNotASpace = !Char.IsWhiteSpace(prevChar);
                bool isInteger = Char.IsNumber(currentChar);
                bool previousLetterIsInteger = Char.IsNumber(prevChar);
                bool isNotTheFirstLetter = counter > 0;

                bool addDelimiter = false;
                if (isNotTheFirstLetter && (isUpperCase || isInteger))
                {
                    if (isNotASpace && previousLetterIsNotASpace &&
                        (!isInteger ||(isInteger && !previousLetterIsInteger)) &&
                        (nextLetterIsLowerCase || !previousLetterIsUpperCase))
                    {
                        addDelimiter = true;
                    }
                }

                if (addDelimiter)
                {
                    formatted += delimiter + currentChar;
                }
                else
                {
                    formatted += currentChar;
                }
                counter++;
            }
            return formatted;
        }

        //TODO brett 08 Jun 2010: For 2_0
        /// <summary>
        /// Singularises the input string using heuristics and rule.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Singularize(string input)
        {
            if (input == String.Empty) return input;

            foreach (var unaffectedSingularRules in myUnaffectedSingular)
            {
                if (Regex.IsMatch(input, unaffectedSingularRules, RegexOptions.IgnoreCase)) return input;
            }
            /*//TODO_ brett 08 Jun 2010: For DotNet 2_0
                        if (myUnaffectedSingular.Any(rule => Regex.IsMatch(input, rule, RegexOptions.IgnoreCase)))
                        {
                            return input;
                        }*/
            var singularised = Singularise(input, myIrregularSingular);
            if (singularised != null) return singularised;

            singularised = Singularise(input, mySingularRules);
            return singularised ?? input;
        }

        private static string Singularise(string input, NameValueCollection singularisationRules)
        {
            string singularisationRule = null;
            foreach (var rule in singularisationRules)
            {
                var stringRule = (string)rule;
                if (!Regex.IsMatch(input, stringRule, RegexOptions.IgnoreCase)) continue;
                singularisationRule = stringRule;
                break;
            }
            if (singularisationRule != null)
            {
                return Regex.Replace(input, singularisationRule, singularisationRules[singularisationRule],
                                     RegexOptions.IgnoreCase);
            }
            return null;
            /*//TODO_ brett 08 Jun 2010: For DotNet 2_0
                        var singularisationRule = singularisationRules
                            .Cast<string>()
                            .Where(rule => Regex.IsMatch(input, rule, RegexOptions.IgnoreCase))
                            .FirstOrDefault();
                        if (singularisationRule != null)
                        {
                            return Regex.Replace(input, singularisationRule, singularisationRules[singularisationRule],
                                                 RegexOptions.IgnoreCase);
                        }
                        return null;*/
        }

        ///<summary>
        /// Pluralises a noun using standard rule e.g. Name => Names
        /// Pantry => Pantries.
        ///</summary>
        ///<param name="input"></param>
        ///<returns></returns>
        public static string Pluralize(string input)
        {
            if (input == String.Empty)
                return input;
            foreach (string rule in myUnaffectedPlural)
            {
                if (Regex.IsMatch(input, rule, RegexOptions.IgnoreCase))
                    return input;
            }
            foreach (string rule in myIrregularPlural)
            {
                if (rule.Equals(input, StringComparison.InvariantCultureIgnoreCase))
                    return myIrregularPlural[rule];
            }
            foreach (string rule in myPluralRules)
            {
                if (Regex.IsMatch(input, rule, RegexOptions.IgnoreCase))
                    return Regex.Replace(input, rule, myPluralRules[rule], RegexOptions.IgnoreCase);
            }
            return String.Empty;
        }


        public static string Camelize(string input)
        {
            var replace = Humanize(input);
            replace = replace.Replace(" ", "");
            return replace;
        }
        /// <summary>
        /// Replaces all _ with whites space
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Humanize(string input)
        {
            string replace = input.Replace("_", " ");
            replace = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(replace);
            return replace;
        }
        //TODO_ brett 08 Jun 2010: For DotNet 2_0  
        /// <summary>
        /// Create a classification of the input this is a Singular of the input
        ///   Camel cased.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Classify(string input)
        {
            return Camelize(Singularize(input));
        }

        /// <summary>
        /// Pascal Cases a table name that has as seperator '_', '-' or ' ' in it so that the 
        ///  the pascal casing will be done as  follows first letter is capitalised
        ///  the first letter that follows a seperator is capitalised. All other letters are left unchanged.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string PascalCaseTableName(string text)
        {
            if (text.Length > 1)
            {
                text = text[0].ToString().ToUpper() +
                       text.Substring(1);
                text = RemovePascalDelimiters(text, "_");
                text = RemovePascalDelimiters(text, " ");
                text = RemovePascalDelimiters(text, "-");
            }
            else
            {
                text = text.ToUpper();
            }
            return text;
        }
        private static string RemovePascalDelimiters(string text, string delimiter)
        {
            int pos = text.IndexOf(delimiter);
            while (pos != -1 && pos < text.Length - 1)
            {
                text = text.Substring(0, pos) + text[pos + 1].ToString().ToUpper()
                       + text.Substring(pos + 2);
                pos = text.IndexOf(delimiter, pos);
            }
            return text;
        }
        /// <summary>
        /// Determines whether the word is already pascal cased. 
        /// This will return true when.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsManyPascalWords(string text)
        {
            if (text.Length <= 1) return false;
            string firstLetter = text.Substring(0, 1);
            if (firstLetter == firstLetter.ToLowerInvariant()) return false;

            string textRemainderPart = text.Substring(1);
            return textRemainderPart != textRemainderPart.ToLower();
        }

        ///<summary>
        /// Removes a prefix defined by prefix from the text if it is found to be the leading characters of text.
        ///</summary>
        ///<param name="prefix"></param>
        ///<param name="text"></param>
        ///<returns></returns>
        public static string RemovePrefix(string prefix, string text)
        {
            if (!string.IsNullOrEmpty(prefix) && text.StartsWith(prefix, true, CultureInfo.CurrentCulture))
            {
                text = text.Substring(prefix.Length);
            }
            return text;
        }

        ///<summary>
        /// Changes the first character to a lowercase.
        ///</summary>
        ///<param name="input"></param>
        ///<returns></returns>
        public static string ToLowerFirstLetter(string input)
        {
            if (input.Length == 0) return "";
            char firstLetter = input[0];
            return Char.ToLower(firstLetter) + input.Substring(1);
        }
        /// <summary>
        /// Converts the string representation of a Guid to its Guid
        /// equivalent. A return value indicates whether the operation
        /// succeeded.
        /// </summary>
        /// <param name="s">A string containing a Guid to convert.</param>OwningClassDef
        /// <param name="result">
        /// When this method returns, contains the Guid value equivalent to
        /// the Guid contained in <paramref name="s"/>, if the conversion
        /// succeeded, or <see cref="Guid.Empty"/> if the conversion failed.
        /// The conversion fails if the <paramref name="s"/> parameter is a
        /// <see langword="null" /> reference (<see langword="Nothing" /> in
        /// Visual Basic), or is not of the correct format.
        /// </param>
        /// <value>
        /// <see langword="true" /> if <paramref name="s"/> was converted
        /// successfully; otherwise, <see langword="false" />.
        /// </value>
        /// <exception cref="ArgumentNullException">
        ///        Thrown if <pararef name="s"/> is <see langword="null"/>.
        /// </exception>
        public static bool GuidTryParse(string s, out Guid result)
        {
            if (s == null)
                throw new ArgumentNullException("s");
            
            Match match = _guidFormat.Match(s);

            if (match.Success)
            {
                result = new Guid(s);
                return true;
            }
            result = Guid.Empty;
            return false;
        }

        ///<summary>
        /// Converts a string representing a boolean in one of many formats.
        ///</summary>
        ///<param name="valueToParse">string to be parsed</param>
        ///<param name="result">the resultant parsed value if parsing was a success</param>
        ///<returns>true if string was parsed false otherwise</returns>
        ///<exception cref="ArgumentNullException">
        ///   Thrown if <pararef name="s"/> is <see langword="null"/>.
        /// </exception>
        public static bool BoolTryParse(object valueToParse, out bool result)
        {
            try
            {
                if (valueToParse is string)
                {
                    string stringValue = valueToParse as string;
                    switch (stringValue.ToUpper())
                    {
                        case "TRUE":
                        case "T":
                        case "YES":
                        case "Y":
                        case "1":
                        case "-1":
                        case "ON":
                            result = true;
                            return true;
                        case "FALSE":
                        case "F":
                        case "NO":
                        case "N":
                        case "0":
                        case "OFF":
                            result = false;
                            return true;
                        default:
                            result = Convert.ToBoolean(stringValue);
                            return true;
                    }
                }
                result = Convert.ToBoolean(valueToParse);
            }
            catch (Exception ex)
            {
                throw new HabaneroDeveloperException("There was an error parsing " + valueToParse + " to a boolean. Please contact your system administrator", "There was an error parsing " + valueToParse + " to a boolean",ex  );
            }
            return true;
        }
        /// <summary>
        /// Indicates the number of times a given string appears in a larger string
        /// </summary>
        /// <param name="fullText">The string to search within</param>
        /// <param name="searchText">The section to search for</param>
        /// <returns>Returns the number of occurrences</returns>
        public static int CountOccurrences(string fullText, string searchText)
        {
            return CountOccurrences(fullText, searchText, 0, fullText.Length);
        }

        /// <summary>
        /// Indicates the number of times a given string appears in a larger string
        /// </summary>
        /// <param name="fullText">The string to search within</param>
        /// <param name="searchText">The section to search for</param>
        /// <param name="startIndex">The index of the position to start counting occurences from.</param>
        /// <param name="length">The length of text to count occurences from.</param>
        /// <returns>Returns the number of occurrences</returns>
        public static int CountOccurrences(string fullText, string searchText, int startIndex, int length)
        {
            string text = fullText.Substring(startIndex, length);
            string[] parts = text.Split(new[] {searchText}, StringSplitOptions.None);
            return parts.Length - 1;
        }

        /// <summary>
        /// Indicates the number of times a given token appears in a string
        /// </summary>
        /// <param name="fullText">The string to search within</param>
        /// <param name="token">The token to search for</param>
        /// <returns>Returns the number of occurrences of the token</returns>
        public static int CountOccurrences(string fullText, char token)
        {
            return CountOccurrences(fullText, token, 0, fullText.Length);
        }

        /// <summary>
        /// Indicates the number of times a given token appears in a string
        /// </summary>
        /// <param name="fullText">The string to search within</param>
        /// <param name="token">The token to search for</param>
        /// <param name="startIndex">The index of the position to start counting occurences from.</param>
        /// <param name="length">The length of text to count occurences from.</param>
        /// <returns>Returns the number of occurrences of the token</returns>
        public static int CountOccurrences(string fullText, char token, int startIndex, int length)
        {
            int occurred = 0;
            for (int i = startIndex; i < length; i++)
            {
                if (token == fullText[i])
                {
                    occurred++;
                }
            }
            return occurred;
        }

        /// <summary>
        /// Returns the portion of the string that is left of the given
        /// search text
        /// </summary>
        /// <param name="fullText">The string to search within</param>
        /// <param name="searchText">The section to the right of the desired
        /// text</param>
        /// <returns>Returns the abbreviated string portion</returns>
        public static string GetLeftSection(string fullText, string searchText)
        {
            if (fullText.Contains(searchText))
            {
                return fullText.Substring(0, fullText.IndexOf(searchText));
            }
            return "";
        }

        /// <summary>
        /// Returns the portion of the string that is right of the given
        /// search text
        /// </summary>
        /// <param name="fullText">The string to search within</param>
        /// <param name="searchText">The section to the left of the desired
        /// text</param>
        /// <returns>Returns the abbreviated string portion</returns>
        public static string GetRightSection(string fullText, string searchText)
        {
            if (StringUtilitiesCE.Contains(searchText, fullText))
            {
                int startPos = fullText.IndexOf(searchText) + searchText.Length;
                return fullText.Substring(startPos, fullText.Length - startPos);
            }
            return "";
        }

        /// <summary>
        /// Appends a given message to an existing message, inserting
        /// a new line (carriage return) between the messages
        /// </summary>
        /// <param name="origMessage">The existing message (left part)</param>
        /// <param name="messageToAppend">The message to add on (right part)</param>
        /// <returns>Returns the combined string</returns>
        public static string AppendMessage(string origMessage, string messageToAppend)
        {
            if (!String.IsNullOrEmpty(origMessage)) origMessage += StringUtilitiesCE.NewLine;
            origMessage += messageToAppend;
            return origMessage;
        }

        /// <summary>
        /// Appends a given message to an existing message, using the given separator
        /// betweeen the two parts.
        /// Ensures that the seperator is only inserted between two appended messages.
        /// I.e. there will never be a trailing seperator.
        /// </summary>
        /// <param name="origMessage">The existing message (left part)</param>
        /// <param name="messageToAppend">The message to add on (right part)</param>
        /// <param name="separator">The separator to insert between the two
        /// parts</param>
        /// <returns>Returns the combined message</returns>
        public static string AppendMessage(string origMessage, string messageToAppend, string separator)
        {
            if (!String.IsNullOrEmpty(origMessage)) origMessage += separator;
            return origMessage + messageToAppend;
        }

        /// <summary>
        /// Appends a given message to an existing message contained in a string builder
        /// </summary>
        /// <param name="origStringBuilder">The string builder that contains the original
        /// message</param>
        /// <param name="appendedString">The message to add on (right part)</param>
        /// <param name="separator">The separator to insert between the two parts</param>
        /// <returns>Returns the combined message</returns>
        public static StringBuilder AppendMessage
            (StringBuilder origStringBuilder, string appendedString, string separator)
        {
            if (origStringBuilder.Length != 0)
                origStringBuilder.Append(separator);
            origStringBuilder.Append(appendedString);
            return origStringBuilder;
        }

        /// <summary>
        /// For a given name value pair e.g. a query string or cookie string that is formatted
        /// as name=value&amp;name2=value2&amp;name3=value3 etc this will return the value for a specified
        /// name e.g. for nameValuePairString = "name=value&amp;name2=value2&amp;name3=value3" and name = "name2"
        /// GetValueString will return value2.
        /// </summary>
        /// <param name="nameValuePairString">The name value pair to parse</param>
        /// <param name="name">The name of the name value pair for which you want the value</param>
        /// <returns></returns>
        public static string GetValueString(string nameValuePairString, string name)
        {
            NameValueCollection nameValueCollection = GetNameValueCollection(nameValuePairString);
            return nameValueCollection[name];
        }

        /// <summary>
        /// returns a NameValueCollection of nameValue Pairs for the nameValuePairString.
        /// e.g. nameValuePairString = "name=value&amp;name2=value2&amp;name3=value3" will return a 
        /// NameValueCollection with 3 items for name, name2 and name3.
        /// </summary>
        /// <param name="nameValuePairString">The name value pair to split.</param>
        /// <returns>The new collection containing the name value pair items.</returns>
        public static NameValueCollection GetNameValueCollection(string nameValuePairString)
        {
            NameValueCollection nameValueCollection = new NameValueCollection();
            if (!string.IsNullOrEmpty(nameValuePairString))
            {
                string[] pairs = nameValuePairString.Split(new[] {'&'});
                foreach (string pair in pairs)
                {
                    string[] values = pair.Split(new[] {'='});
                    nameValueCollection[values[0]] = values[1];
                }
            }
            return nameValueCollection;
        }

        ///<summary>
        /// Returns the guid as a string with a standard format of "B" and with all the characters
        ///   changed to upper.
        ///</summary>
        ///<param name="guid"></param>
        ///<returns></returns>
        public static string GuidToUpper(Guid guid)
        {
            return guid.ToString("B").ToUpperInvariant();
        }
    }
}