using Data.Models;
using System;
using System.Collections.Generic;

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
                    Properties = new WordPropertySet
                    {
                        new WordProperty
                        {
                            Name = "gender",
                            Values = new StringSet{ "masculine" }
                        },
                        new WordProperty
                        {
                            Name = "declension",
                            Values = new StringSet{"strong" }
                        }
                    }
                },

                new Word
                {
                    SourceLanguageName = "german",
                    Value = "schlafen",
                    Properties = new WordPropertySet
                    {
                        new WordProperty
                        {
                            Name = "conjugation",
                            Values = new StringSet{"strong" }
                        },
                        new WordProperty
                        {
                            Name = "transitivity",
                            Values = new StringSet{"intransitive" }
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
                    Properties = new WordPropertySet
                    {
                        new WordProperty
                        {
                            Name = "countability",
                            Values = new StringSet{"countable"}
                        }

                    }
                },

                new Word
                {
                    SourceLanguageName = "english",
                    Value = "sleep",
                    Properties = new WordPropertySet
                    {
                        new WordProperty
                        {
                            Name = "conjugation",
                            Values = new StringSet{"irregular"}
                        },

                        new WordProperty
                        {
                            Name = "transitivity",
                            Values = new StringSet{"transitive"}
                        }
                    }
                },

                new Word
                {
                    SourceLanguageName = "english",
                    Value = "sleep",
                    Properties = new WordPropertySet
                    {
                        new WordProperty
                        {
                            Name = "conjugation",
                            Values = new StringSet{"irregular"}
                        },

                        new WordProperty
                        {
                            Name = "transitivity",
                            Values = new StringSet{"intransitive"}
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
                    Properties = new WordPropertySet
                    {
                        new WordProperty
                        {
                            Name = "declension",
                            Values = new StringSet{"masculine declension"}
                        },

                        new WordProperty
                        {
                            Name = "countability",
                            Values = new StringSet{"both singular and plural"}
                        }
                    }
                },

                new Word
                {
                    SourceLanguageName = "polish",
                    Value = "spać",
                    Properties = new WordPropertySet
                    {
                        new WordProperty
                        {
                            Name = "conjugation",
                            Values = new StringSet{"first conjugation"}
                        },
                        new WordProperty
                        {
                            Name = "aspect",
                            Values = new StringSet{"imperfective"}
                        }
                    }
                }
            };

            #endregion

        }


    }
}
