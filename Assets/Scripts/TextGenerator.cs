using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextGenerator
{
    public static Dictionary<string, string[]> FetchCategories(string text)
    {
        Dictionary<string, string[]> categoryElements = new Dictionary<string, string[]>();
        string[] categoryNames = FetchCategory("category", text);
        foreach(string categoryName in categoryNames)
        {
            categoryElements.Add(categoryName, FetchCategory(categoryName, text));
        }
        return categoryElements;
    }

    public static string Format(string rawString, Dictionary<IngredientData.IngredientType, IngredientInitializer> ingredients)
    {

        while(rawString.Contains("["))
        {
            string optionalElement = GetTextBetween("[", "]", rawString);
            rawString = ReplaceTextBetween("[", "]", Random.value > 0.5f ? optionalElement : "", rawString);
        }

        while(rawString.Contains("("))
        {
            string[] options = GetTextBetween("(", ")", rawString).Split('-');
            rawString = ReplaceTextBetween("(", ")", PickRandom(options), rawString);
        }

        List<IngredientData.Noun> matchingNouns = new List<IngredientData.Noun>();
        matchingNouns.Add(null);
        while(rawString.Contains("§"))
        {
            string placeholder = GetTextBetween("§", "§", rawString);

            int matchingIndex = int.Parse(placeholder[0].ToString());
            placeholder = placeholder.Substring(1);

            IngredientData.Noun noun;
            switch(placeholder)
            {
                case "planet_synonym" :
                    noun = ingredients[IngredientData.IngredientType.SOLID].Data.NamingElements.PlanetSynonym;
                    break;
                case "concept" :
                    noun = ingredients[IngredientData.IngredientType.SOLID].Data.NamingElements.Concept;
                    break;
                case "first_name" :
                    noun = ingredients[IngredientData.IngredientType.LIQUID].Data.NamingElements.FirstName;
                    break;
                case "nickname" :
                    noun = ingredients[IngredientData.IngredientType.SOLID].Data.NamingElements.Nickname;
                    break;
                case "matriculation" :
                    noun = ingredients[IngredientData.IngredientType.GASEOUS].Data.NamingElements.Matriculation;
                    break;
                default :
                    noun = null;
                    break;
            }
            matchingNouns.Insert(matchingIndex, noun);

            rawString = ReplaceTextBetween("§", "§", noun.Word, rawString);
        }

        while(rawString.Contains("+"))
        {
            string placeholder = GetTextBetween("+", "+", rawString);
            int matchingIndex = int.Parse(placeholder[0].ToString());
            placeholder = placeholder.Substring(1);

            IngredientData.Qualifier qualifier;
            switch(placeholder)
            {
                case "style_adjective" :
                    qualifier = ingredients[IngredientData.IngredientType.GASEOUS].Data.NamingElements.StyleAdjective;
                    break;
                case "personality_adjective" :
                    qualifier = ingredients[IngredientData.IngredientType.LIQUID].Data.NamingElements.PersonalityAdjective;
                    break;
                case "color" :
                    qualifier = ingredients[IngredientData.IngredientType.LIQUID].Data.NamingElements.Color;
                    break;
                default :
                    qualifier = null;
                    break;
            }

            placeholder = MatchQualifier(qualifier, matchingNouns[matchingIndex]);

            rawString = ReplaceTextBetween("+", "+", placeholder, rawString);
        }

        while(rawString.Contains("<"))
        {
            string article = GetTextBetween("<", ">", rawString);
            int matchingIndex = int.Parse(article[0].ToString());
            article = article.Substring(1);

            article = MatchArticle(article, matchingNouns[matchingIndex]);

            rawString = ReplaceTextBetween("<", ">", article, rawString);
        }

        // rawString = HandlePunctuation(rawString);
        rawString = rawString[0].ToString().ToUpper() + rawString.Substring(1);

        return rawString;
    }

    public static string PickRandom(string categoryName, Dictionary<string, string[]> categoryElements)
    {
        string[] elements = categoryElements[categoryName];
        return elements[Random.Range(0, elements.Length)];
    }

    public static string PickRandom(string[] options)
    {
        return options[Random.Range(0, options.Length)];
    }

    private static string[] FetchCategory(string categoryName, string context)
    {
        string categoryText = GetTextBetween($"#{categoryName}\n", "\n\n", context);
        return categoryText.Split('\n');
    }

    private static string GetTextBetween(string start, string end, string context)
    {
        int startIndex = context.IndexOf(start) + start.Length;
        int length = context.Substring(startIndex).IndexOf(end);
        return context.Substring(startIndex, length);
    }

    private static string ReplaceTextBetween(string start, string end, string replacement, string context)
    {
        int startIndex = context.IndexOf(start);
        int length = context.Substring(startIndex).IndexOf(end, start.Length) + end.Length;
        string textToReplace = context.Substring(startIndex, length);
        return context.Replace(textToReplace, replacement);
    }

    private static string MatchQualifier(IngredientData.Qualifier qualifier, IngredientData.Noun noun)
    {
        if(qualifier.Invariable != "")
        {
            return qualifier.Invariable;
        }

        if(noun == null)
        {
            return qualifier.SingularFeminine;
        }

        if(noun.Genre == IngredientData.Genre.FEMININE)
        {
            if(noun.Plurality == IngredientData.Plurality.SINGULAR)
            {
                return qualifier.SingularFeminine;
            }
            return qualifier.PluralFeminine;
        }

        if(noun.Plurality == IngredientData.Plurality.SINGULAR)
        {
            return qualifier.SingularMasculine;
        }
        return qualifier.PluralMasculine;
    }

    private static string MatchArticle(string article, IngredientData.Noun noun)
    {
        if(noun == null)
        {
            return "la";
        }

        bool isUndefined = article.Contains("u");
        bool hasPossessive = article.Contains("p");
        if(noun.Plurality == IngredientData.Plurality.SINGULAR)
        {
            if(noun.Genre == IngredientData.Genre.FEMININE)
            {
                if(isUndefined)
                {
                    return hasPossessive ? "d’une" : "une";
                }
                if(noun.HasContractedArticle)
                {
                    return hasPossessive ? "de l’" : "l’";
                }
                return hasPossessive ? "de la" : "la";
            }
            if(isUndefined)
            {
                return hasPossessive ? "d’un" : "un";
            }
            if(noun.HasContractedArticle)
            {
                return hasPossessive ? "de l’" : "l’";
            }
            return hasPossessive ? "du" : "le";
        }
        if(isUndefined)
        {
            return hasPossessive ? "de" : "des";
        }
        return hasPossessive ? "des" : "les";
    }

    private static string HandlePunctuation(string rawString)
    {
        char[] strongPunctuation = new char[] {'.', '!', '?'};
        int strongPunctuationIndex = rawString.IndexOfAny(strongPunctuation);
        bool isEndOfString = false;
        while(strongPunctuationIndex > 0 && !isEndOfString)
        {
            bool isCharFound = false;
            while(!isCharFound && !isEndOfString)
            {
                strongPunctuationIndex++;
                isEndOfString = strongPunctuationIndex >= rawString.Length;
                if(!isEndOfString && rawString[strongPunctuationIndex] != ' ')
                {
                    rawString =
                            rawString.Substring(0, strongPunctuationIndex) +
                            rawString[strongPunctuationIndex].ToString().ToUpper() +
                            rawString.Substring(strongPunctuationIndex + 1);
                    isCharFound = true;
                }
            }
            if(!isEndOfString)
            {
                strongPunctuationIndex = rawString.IndexOfAny(strongPunctuation, strongPunctuationIndex);
            }
        }
        return rawString;
    }
}
