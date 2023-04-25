using DataLayer.Entities;
using DataLayer.IRepos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class JapaneseWordNoteCardRepo : GenericRepo<JapaneseWordNoteCard>, IJapaneseWordNoteCardRepo
    {
        public JapaneseWordNoteCardRepo(KanjiDbContext context) : base(context) { }

        public async Task AddAsync(List<JapaneseWordNoteCard> cards)
        {

            //Didn'tWork put ill keep in
            //EF has trouble with Where and any???.  Need to make a "Built expression"https://stackoverflow.com/questions/68737681/the-linq-expression-could-not-be-translated-either-rewrite-the-query-in-a-form
            // EF do not supports complex predicates with local collections and here you need to build expression tree dynamically  https://stackoverflow.com/questions/71043307/find-in-entity-framework-multiple-or-parameters
            //I thihnk i need another library, but im lazy so im going to send 8 hours trying to figure out another way


            //something that i remembered during internship, and it helped me firgure it out
            //filter option? maybe https://stackoverflow.com/questions/33153932/filter-search-using-multiple-fields-asp-net-mvc

            //var query = _dbContext.JapaneseWordNoteCards.AsQueryable();
            //foreach (var card in cards)
            //{
                    //This filter the list by one, then returned only one.  then it filtered it by two, but since it was one, it got filtered otu
            //    query = query.Where(dbcard => dbcard.SentenceNoteCard.ItemQuestion == card.SentenceNoteCard.ItemQuestion);
                //query  =  query.Where(dbentry => ArrayOfMultipleDifferentFilterValues.Contains(dbentry.FilterOptionColumn)
                //query = query.Where(dbentry => SecondArrayOfMultipleDifferntFilterValues.Contains(debentry.DifferentColumn);
            //}
            //var qeurycards = query.ToList();


            var cardsTopName = cards.Select(c => c.SentenceNoteCard.ItemQuestion).ToList();
            //var addedCards = await _dbContext.JapaneseWordNoteCards.Where(dbcard => dbcard.ItemQuestion == "一体").ToListAsync();
            var addedCards = await _dbContext.JapaneseWordNoteCards.Where(dbcard => cardsTopName.Contains(dbcard.ItemQuestion)).ToListAsync();
            var cardsToAdd = cards.RemoveAll(c => addedCards.Any(added => added.ItemQuestion == c.SentenceNoteCard.ItemQuestion));

            Console.WriteLine("hi");
            foreach (var card in cards)
            {
                _dbContext.JapaneseWordNoteCards.Add(card);
            }
            await _dbContext.SaveChangesAsync();
        }

    }
}
