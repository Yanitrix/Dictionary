# Dictionary
A REST API made for adding and retrieving words/phrases to/from a dictionary. 

### _**Work in progress**_

##So how does it work?

* First off all, you need to create a [language](https://github.com/Yanitrix/Dictionary/blob/master/Data/Dto/LanguageDto.cs)<sup>*</sup>, which is basically an 
  entity holding a set of words. It is used later when creating dictionaries. (post on "/api/language" endpoint)
* Now, go on and add some [words](https://github.com/Yanitrix/Dictionary/blob/master/Data/Dto/WordDto.cs) to a language (post on /api/word). You can also include some 
  [properties](https://github.com/Yanitrix/Dictionary/blob/master/Data/Dto/WordPropertyDto.cs) (let's say a word has a grammatical gender and you want to note it) if you want.
* When the languages have some data you can create a [dictionary](https://github.com/Yanitrix/Dictionary/blob/master/Data/Dto/DictionaryDto.cs) (posting on "/api/dictionary").
  Dictionary has input and output language, so at least 2 need to exist. It also has a set of entries... 
* ...so now, let's post an [entry](https://github.com/Yanitrix/Dictionary/blob/master/Data/Dto/EntryDto.cs). You have to reference the newly created dictionary by providing its
  index. Also, you need to provide a word from the dictionary's input language. If the word's source language is different from the dictionary's input language, an error will be
  raised. Entry is basically a collection of [meanings](https://github.com/Yanitrix/Dictionary/blob/master/Data/Dto/MeaningDto.cs)
* A meaning has a reference to it's entry. It has a value - a translation of the word from the entry (basically, it's a meaning). It also includes some notes (maybe you want
  to mark that the meaning's value is pejorative). End, finally, you can include some [examples](https://github.com/Yanitrix/Dictionary/blob/master/Data/Dto/ExampleDto.cs)
 
###And now you're done!

>Wait, so you tell me that I have to do so many things just to add a simple translation to a dictionary?!
>Its better to just use google translate with its favorite option...
  
Well, no. That's why you can add a [free expression](https://github.com/Yanitrix/Dictionary/blob/master/Data/Dto/FreeExpressionDto.cs). Just reference a dictionary, put text
and its translation and you're good to go.

>What about querying translations?

Head to "api/translate/{dictionary}/{query}?{bidir}?<sup>#</sup>, include what you want to in path variables and voila! You should get your translations. <sup>*</sup>

<sup>*</sup> Well, you should be able to. But endpoints weren't tested yet so there's no certainty eveything will work as expected. As soon as tests are made and I'm sure everything
works, I'll remove this footnote.

<sup>#</sup>List of endpoints:
Endpoint | Usage
---------|------
_api/language_ | CRUD endpoint for [languages](https://github.com/Yanitrix/Dictionary/blob/master/Data/Dto/LanguageDto.cs)
_api/word_ | CRUD endpoint for [words](https://github.com/Yanitrix/Dictionary/blob/master/Data/Dto/WordDto.cs). You can also include some [properties](https://github.com/Yanitrix/Dictionary/blob/master/Data/Dto/WordProperty.cs)
_api/dictionary_ | for [dictionaries](https://github.com/Yanitrix/Dictionary/blob/master/Data/Dto/DictionaryDto.cs)
_api/entry_ | for [entries](https://github.com/Yanitrix/Dictionary/blob/master/Data/Dto/EntryDto.cs)
_api/meaning_ | for [meanings](https://github.com/Yanitrix/Dictionary/blob/master/Data/Dto/MeaningDto.cs). You can also include some [examples](https://github.com/Yanitrix/Dictionary/blob/master/Data/Dto/ExampleDto.cs)
_api/expression_ | for [free expressions](https://github.com/Yanitrix/Dictionary/blob/master/Data/Dto/FreeExpressionDto.cs)
_api/translate/{dictionaryName}/{queryString}?bidir_ | Here the magic happens. _dictionaryName_ is a combination of input and output languages' names of a dictionary. E.g. _polish-english_ for a dictionary with input language _polish_ and output language _english_. _queryString_ is the word you're looking for. Set _bidir_ to true if you want to search in both "parts" of a dictionary (_polish -> english_ and _english -> polish_)






  //add footnote
