using Data.Database;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Tests.Repositories
{
    public class DataInitializer
    {
        public Language German, English, Polish;
        public Word[] GermanWords, EnglishWords, PolishWords;

        public DataInitializer()
        {
            German = new Language
            {
                Name = "german",
            };

            English = new Language
            {
                Name = "english",
            };

            Polish = new Language
            {
                Name = "polish",
            };


            #region german words

            GermanWords = new Word[]
            {
                new Word
                {
                    SourceLanguageName = "german",
                    Value = "Stock",
                    Properties = new HashSet<WordProperty>
                    {
                        new WordProperty
                        {
                            Name = "gender",
                            Values = new HashSet<String>{ "masculine" }
                        },
                        new WordProperty
                        {
                            Name = "declension",
                            Values = new HashSet<String>{"strong" }
                        }
                    }
                },

                new Word
                {
                    SourceLanguageName = "german",
                    Value = "schlafen",
                    Properties = new HashSet<WordProperty>
                    {
                        new WordProperty
                        {
                            Name = "conjugation",
                            Values = new HashSet<String>{"strong" }
                        },
                        new WordProperty
                        {
                            Name = "transitivity",
                            Values = new HashSet<String>{"intransitive" }
                        }
                    }
                }
            };

            #endregion

            #region english words

            EnglishWords = new Word[]
            {
                new Word
                {
                    SourceLanguageName = "english",
                    Value = "stick",
                    Properties = new HashSet<WordProperty>
                    {
                        new WordProperty
                        {
                            Name = "countability",
                            Values = new HashSet<String>{"countable"}
                        }

                    }
                },

                new Word
                {
                    SourceLanguageName = "english",
                    Value = "sleep",
                    Properties = new HashSet<WordProperty>
                    {
                        new WordProperty
                        {
                            Name = "conjugation",
                            Values = new HashSet<String>{"irregular"}
                        },

                        new WordProperty
                        {
                            Name = "transitivity",
                            Values = new HashSet<String>{"transitive"}
                        }
                    }
                },

                new Word
                {
                    SourceLanguageName = "english",
                    Value = "sleep",
                    Properties = new HashSet<WordProperty>
                    {
                        new WordProperty
                        {
                            Name = "conjugation",
                            Values = new HashSet<String>{"irregular"}
                        },

                        new WordProperty
                        {
                            Name = "transitivity",
                            Values = new HashSet<String>{"intransitive"}
                        }
                    }
                }
            };

            #endregion

            #region polish words

            PolishWords = new Word[]
            {
                new Word
                {
                    SourceLanguageName = "polish",
                    Value = "patyk",
                    Properties = new HashSet<WordProperty>
                    {
                        new WordProperty
                        {
                            Name = "declension",
                            Values = new HashSet<String>{"masculine declension"}
                        },

                        new WordProperty
                        {
                            Name = "countability",
                            Values = new HashSet<String>{"both singular and plural"}
                        }
                    }
                },

                new Word
                {
                    SourceLanguageName = "polish",
                    Value = "spać",
                    Properties = new HashSet<WordProperty>
                    {
                        new WordProperty
                        {
                            Name = "conjugation",
                            Values = new HashSet<String>{"first conjugation"}
                        },
                        new WordProperty
                        {
                            Name = "aspect",
                            Values = new HashSet<String>{"imperfective"}
                        }
                    }
                }
            };

            #endregion

        }


    }
}
