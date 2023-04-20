using DataLayer.Entities;
using DataLayer.IRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class JapaneseWordNoteCardRepo : GenericRepo<JapaneseWordNoteCard>, IJapaneseWordNoteCardRepo
    {
        public JapaneseWordNoteCardRepo(KanjiDbContext context) : base(context) { }
    }
}
