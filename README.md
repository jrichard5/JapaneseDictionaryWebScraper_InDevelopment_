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
2. its kinda of a do...while loop... except it only has the while loop at the end.  I feel like I could set up the setup part inside the loop.
3. Figure out the repo Pattern. I don't recall me using context inside the repo....
4. Unit Tests!!!!!
