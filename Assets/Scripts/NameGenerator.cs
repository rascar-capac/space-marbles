using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameGenerator
{

    public class NameElements
    {
        private NameTemplate template;
        private string planetSynonym;
        private string concept;
        private string adjective;
        private string firstName;
        private string qualifyingTitle;
        private string nickname;
        private string matriculation;
        private string color;
    }

    public class NameTemplate
    {
        private string template;


    }

    public class Noun
    {
        private string word;
        private Genre genre;
        private Plurality plurality;
    }

    public class adjective
    {
        private string singularMFeminine;
        private string singularMasculine;
        private string pluralFeminine;
        private string pluralMasculine;
    }

    public enum Genre
    {
        FEMININE,
        MASCULINE
    }

    public enum Plurality
    {
        SINGULAR,
        PLURAL
    }
}


