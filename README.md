# Dictionary
A REST API made for adding and retrieving words/phrases to/from a dictionary. 

### _**Work in progress, so some things are broken now**_


## So how does it work?

* First off all, you need to create a [language](https://github.com/Yanitrix/Dictionary/blob/master/Development/Domain/Dto/Language/CreateLanguage.cs)<sup>*</sup>, which is basically an entity holding a set of words. It is used later when creating dictionaries. Post it on _/api/language_ endpoint.
* Now, go on and add some [words](https://github.com/Yanitrix/Dictionary/blob/master/Development/Domain/Dto/Word/CreateWord.cs) to a language. Post on _/api/word_. You can also include some 
  [properties](https://github.com/Yanitrix/Dictionary/blob/master/Development/Domain/Dto/Word/WordPropertyDto.cs) (let's say a word has a grammatical gender and you want to note it) if you want.
* When the languages have some data you can create a [dictionary](https://github.com/Yanitrix/Dictionary/blob/master/Development/Domain/Dto/Dictionary/CreateDictionary.cs). Post on _/api/dictionary_.
  Dictionary has input and output language, so at least 2 need to exist. It also has a set of entries... 
* ...so now, let's post an [entry](https://github.com/Yanitrix/Dictionary/blob/master/Development/Domain/Dto/Entry/CreateEntry.cs). You have to reference the newly created dictionary by providing its
  index. Also, you need to provide a word from the dictionary's input language. If the word's source language is different from the dictionary's input language, an error will be
  raised. Entry is basically a collection of [meanings](https://github.com/Yanitrix/Dictionary/blob/master/Development/Domain/Dto/Meaning/CreateMeaning.cs). Go on, and add some meaning to an Entry. Post each one of them on _/api/meaning_.
* A meaning has a reference to it's entry. It has a value - a translation of the word from the Entry (basically, it's a meaning). It also includes some notes (maybe you want
  to mark that the meaning's value is pejorative). And, finally, you can include some [examples](https://github.com/Yanitrix/Dictionary/blob/master/Development/Domain/Dto/Meaning/ExampleDto.cs).
* Not that after cration not all values of an entity can be updated. (e.g. you cannot change a Word's source language after creating it). 
 
### And now you're done!

>Wait, so you tell me that I have to do so many things just to add a simple translation to a dictionary?!
>Its better to just use google translate with its favorite option...
  
Well, no. That's why you can add a [free expression](https://github.com/Yanitrix/Dictionary/blob/master/Development/Domain/Dto/Dictionary/CreateFreeExpression.cs). Just reference a dictionary, put text
and its translation and you're good to go.

>What about translating stuff?

Head to _api/translate/{dictionary}/{query}?{bidir}_. _dictionary_ is a combination of dictionary's input and output language in following format: _languageIn_-_languageOut_. _query_ is the word you're looking for and _bidir_ (_true_ or _false_, defaults to _false_) means that you want to search also in the opposite dictionary (e.g. both in _english -> polish_ and _polish -> english_).
Send a post request and you should get your translations.

Exact endpoint documentation available [here](https://app.swaggerhub.com/apis/yaninka/Dictionary/v1#/)


### Things that have been done:

* Server side valiation (using _FLuentValidation_)
* Database setup, storing entities (using _EF Core_, repository and UOW patterns)
* Unit tests (using _xUnit_ and _Moq_)
* Database integration tests (performed using MS SQL Local DB)

### Now working on:

* Polishing endpoint
* Finding bugs and fixing them
* Refactoring code to meet clean architecture standards
* Integration tests

### In future I want to implement:

* Authentication and authorization, user profiles, favourites, etc.
* Learning mode (working something similar to flashcards on [Quizlet](https://quizlet.com/))
* Front end (well, learning mode cannot function without frontend, unless you want to learn arabic using Postman)
