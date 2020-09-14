using Data.Database;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Tests.Services
{
    public class DataInitializer
    {
        public Language German, English, Polish;
        public SpeechPart[] GermanSpeechParts, EnglishSpeechParts, PolishSpeechParts;
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

            #region german speech parts
            GermanSpeechParts = new SpeechPart[]
            {
                new SpeechPart
                {
                    LanguageName = "german",
                    Name = "adjective",
                    Properties = new List<SpeechPartProperty>
                    {
                        new SpeechPartProperty
                        {
                            Name = "regularity",
                            PossibleValues = new HashSet<String>
                            {
                                "regular",
                                "irregular"
                            }
                        }
                    }
                },

                new SpeechPart
                {
                    LanguageName = "german",
                    Name = "noun",
                    Properties = new List<SpeechPartProperty>
                    {
                        new SpeechPartProperty
                        {
                            Name = "gender",
                            PossibleValues = new HashSet<String>
                            {
                                "masculine",
                                "feminine",
                                "neuter"
                            }
                        },

                        new SpeechPartProperty
                        {
                            Name = "declension",
                            PossibleValues = new HashSet<String>
                            {
                                "strong",
                                "weak",
                                "mixed" //das Herz for example
                            },

                        }
                    }
                },

                new SpeechPart
                {
                    LanguageName = "german",
                    Name = "verb",
                    Properties = new List<SpeechPartProperty>
                    {
                        new SpeechPartProperty
                        {
                            Name = "conjugation",
                            PossibleValues = new HashSet<String>
                            {
                                "strong",
                                "weak",
                                "mixed"
                            }
                        },

                        new SpeechPartProperty
                        {
                            Name = "transitivity",
                            PossibleValues = new HashSet<String>
                            {
                                "transitive",
                                "intransitive"
                            }
                        }
                    }
                },
            };

            #endregion

            #region english speech parts

            EnglishSpeechParts = new SpeechPart[]
            {
                new SpeechPart
                {
                    LanguageName = "english",
                    Name = "adjective"
                },

                new SpeechPart
                {
                    LanguageName = "english",
                    Name = "noun",
                    Properties =  new List<SpeechPartProperty>
                    {
                        new SpeechPartProperty
                        {
                            Name = "countability",
                            PossibleValues = new HashSet<String>
                            {
                                "countable",
                                "uncountable"
                            }
                        }
                    }
                },

                new SpeechPart
                {
                    LanguageName = "english",
                    Name = "verb",
                    Properties = new List<SpeechPartProperty>
                    {
                        new SpeechPartProperty
                        {
                            Name = "conjugation",
                            PossibleValues = new HashSet<String>
                            {
                                "regular",
                                "irregular"
                            }
                        },

                        new SpeechPartProperty
                        {
                            Name = "transitivity",
                            PossibleValues = new HashSet<String>
                            {
                                "transitive",
                                "intransitive"
                            }
                        }
                    }
                }
            };

            #endregion

            #region polish speech parts

            PolishSpeechParts = new SpeechPart[]
            {
                new SpeechPart
                {
                    LanguageName = "polish",
                    Name = "verb",
                    Properties = new List<SpeechPartProperty>
                    {
                        new SpeechPartProperty
                        {
                            Name = "conjugation",
                            PossibleValues = new HashSet<String>
                            {
                                "first conjugation",
                                "second conjugation",
                                "third conjugation group -a",
                                "third conjugation group -b",
                                "fully irregular"
                            }
                        },

                        new SpeechPartProperty
                        {
                            Name = "aspect",
                            PossibleValues = new HashSet<String>
                            {
                                "imperfective",
                                "perfective"
                            }
                        }
                    }
                },

                new SpeechPart
                {
                    Name = "participle",
                    Properties = new List<SpeechPartProperty>
                    {
                        new SpeechPartProperty
                        {
                            Name = "kind",
                            PossibleValues = new HashSet<String>
                            {
                                "active adjectival participle",
                                "passive adjectival participle",
                                "present adverbial participle",
                                "perfect adverbial participle"
                            }
                        }
                    }
                },

                new SpeechPart
                {
                    Name = "noun",
                    Properties = new List<SpeechPartProperty>
                    {
                        new SpeechPartProperty
                        {
                            Name = "declension",
                            PossibleValues = new HashSet<String>
                            {
                                "masculine declension",
                                "feminine declension",
                                "neuter declension"
                            }
                        },

                        new SpeechPartProperty
                        {
                            Name = "countablity",
                            PossibleValues = new HashSet<String>
                            {
                                "plural only",
                                "singular only",
                                "both singular and plural"
                            }
                        }
                    }
                }
            };

            #endregion

            #region german words

            GermanWords = new Word[]
            {
                new Word
                {
                    SourceLanguageName = "german",
                    SpeechPartName = "noun",
                    Value = "Stock",
                    Properties = new List<WordProperty>
                    {
                        new WordProperty
                        {
                            Name = "gender",
                            Value = "masculine"
                        },
                        new WordProperty
                        {
                            Name = "declension",
                            Value = "strong"
                        }
                    }
                },

                new Word
                {
                    SourceLanguageName = "german",
                    SpeechPartName = "verb",
                    Value = "schlafen",
                    Properties = new List<WordProperty>
                    {
                        new WordProperty
                        {
                            Name = "conjugation",
                            Value = "strong"
                        },
                        new WordProperty
                        {
                            Name = "transitivity",
                            Value = "intransitive"
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
                    SpeechPartName = "noun",
                    Value = "stick",
                    Properties = new List<WordProperty>
                    {
                        new WordProperty
                        {
                            Name = "countability",
                            Value = "countable"
                        }

                    }
                },

                new Word
                {
                    SourceLanguageName = "english",
                    SpeechPartName = "verb",
                    Value = "sleep",
                    Properties = new List<WordProperty>
                    {
                        new WordProperty
                        {
                            Name = "conjugation",
                            Value = "irregular"
                        },

                        new WordProperty
                        {
                            Name = "transitivity",
                            Value = "transitive"
                        }
                    }
                },

                new Word
                {
                    SourceLanguageName = "english",
                    SpeechPartName = "verb",
                    Value = "sleep",
                    Properties = new List<WordProperty>
                    {
                        new WordProperty
                        {
                            Name = "conjugation",
                            Value = "irregular"
                        },

                        new WordProperty
                        {
                            Name = "transitivity",
                            Value = "intransitive"
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
                    SpeechPartName = "noun",
                    Value = "patyk",
                    Properties = new List<WordProperty>
                    {
                        new WordProperty
                        {
                            Name = "declension",
                            Value = "masculine declension"
                        },

                        new WordProperty
                        {
                            Name = "countability",
                            Value = "both singular and plural"
                        }
                    }
                },

                new Word
                {
                    SourceLanguageName = "polish",
                    SpeechPartName = "verb",
                    Value = "spać",
                    Properties = new List<WordProperty>
                    {
                        new WordProperty
                        {
                            Name = "conjugation",
                            Value ="first conjugation"
                        },
                        new WordProperty
                        {
                            Name = "aspect",
                            Value = "imperfective"
                        }
                    }
                }
            };

            #endregion

        }


    }
}
