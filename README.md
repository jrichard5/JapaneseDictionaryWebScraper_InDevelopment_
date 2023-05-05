### JishoWebScraper-in-Development-

# IN DEVELOPMENT
## I'm a new developer and this is a personal project, so I bet this will have bugs

Personal Project.  First Web scraper, so will try to put a timer betwen HTTP requests so I don't make too many calls.  Data will be used in a future notecard application.

Database is currently saved to user/AppData(HiddenFolder)/Local/zzNihongoDb/kbd.db

This is probably as much as I'm going to do besides cleaning it up a bit more.  It doesn't actually call a URL.  I just saved the pages, and then used those files to test parsing.  It tests the kanji page 1st, then a word page, then the next word page.  I hard coded the files names inside the "ParseKanjiHtmlFile" and "ParseWordsFromFile".



Bugs:  
* If I do the same page twice then save to database, get entity already tracked error
* The error message still shows when I try to add the same kanji twice.

TODOs:
1. fix bug (possibly by calling unique on the list)
* ~~furigana does not include hiragana, so gotta fix that.~~
* there is an issue where I do get __ when it is a unique reading.  but I could probably use that as a symbol it is a unique reading
3. Figure out the repo Pattern. I don't recall me using context inside the repo....
4. Unit Tests!!!!!
5. Be more extensisible.  I may want to add whether a verb in transitive or not (0 - not verb, 1 - verb, but not transitivie, and 2, ver and transitive.)
** https://www.devonblog.com/software-development/solid-violations-in-the-wild-the-open-closed-principle/
